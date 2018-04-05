using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScript : Singleton<MainScript> {
  private const string gamedata_filename = "/gamedata.sav";
  private const string resetmenu_path = "Prefabs/UI/ResetMenu";
  private GameObject reset_canvas;
  private Text reset_text;
  private bool reset_active;
  
  [CustomLabel("Gravity Force")] [Tooltip("Force at which gravity is applied to objects.")] [SerializeField]
  private float gravity_force = 42f;
  private float gravity_force_updater = 0f;
  
  private string previous_scene_name = "";
  private string current_scene_name = "";

  private ScenesInformation scene_informations;
  
  public delegate void OnGravityForceChangeDelegate(float _gravity_force);
  public event OnGravityForceChangeDelegate OnGravityForceChange;

  /// <summary>
  /// Called when scene was just changed.
  /// </summary>
  /// <param name="scene_name">Name of the current scene</param>
  /// <param name="prev_scene_name">Name of the previous scene</param>
  public delegate void OnSceneChangeDelegate(string scene_name, string prev_scene_name);
  /// <summary>
  /// Event which one can subscribe to recieve on scene change events.
  /// </summary>
  public event OnSceneChangeDelegate OnSceneChange;

  private SceneInfo current_scene_info;
  private Input2D _input;
  
  public float GravityForce() {
    return gravity_force;
  }

  private void Awake() {
    /* this is if we start from current scene to test in editor */
    if (current_scene_name.Length == 0) current_scene_name = SceneManager.GetActiveScene().name;
    LoadGame();
    
    reset_canvas = Resources.Load<GameObject>(resetmenu_path);
    reset_canvas = Instantiate(reset_canvas, transform);
    reset_text = reset_canvas.GetComponentInChildren<Text>();
    reset_canvas.SetActive(false);
    reset_active = false;
    if (current_scene_name != "MainMenu" || current_scene_name != "LevelSummary") {
      PostProcessingBehaviour effects = Camera.main.gameObject.GetComponent<PostProcessingBehaviour>();
      effects.profile.depthOfField.enabled = false;
    }
  }

  private void LoadGame() {
    /* if saved data exists then load from it otherwise create new scene information */
    if (File.Exists(Application.persistentDataPath + gamedata_filename)) {
      BinaryFormatter bf = new BinaryFormatter();
      FileStream file = File.Open(Application.persistentDataPath + gamedata_filename, FileMode.Open);
      scene_informations = (ScenesInformation) bf.Deserialize(file);
      ScenesInformation tmp_scene_informations = new ScenesInformation();

      bool updated = false;
      /* update the saved file if new level has been added */
      foreach (string scene_information_key in tmp_scene_informations.SceneInfos.Keys) {
        if (!scene_informations.SceneInfos.ContainsKey(scene_information_key)) {
          if (!updated) updated = true;
          Debug.Log("Updated saved gamedata (" + scene_information_key + ")...");
          SceneInfo _value;
          tmp_scene_informations.SceneInfos.TryGetValue(scene_information_key, out _value);
          scene_informations.SceneInfos.Add(scene_information_key, _value);
        }
      }
      file.Close();
      if(updated) Save();
    } else {
      scene_informations = new ScenesInformation();
    }
  }
  
  public void Save() {
    /* save gamedata to file, and if file exists overwrite it */
    BinaryFormatter bf = new BinaryFormatter();
    FileStream file = File.Create(Application.persistentDataPath + gamedata_filename);
    bf.Serialize(file, scene_informations);
    file.Close();
  }
  
  private void Update() {
    _input = InputController.instance.input;
    
    if (gravity_force_updater != gravity_force && OnGravityForceChange != null) {
      gravity_force_updater = gravity_force;
      OnGravityForceChange(gravity_force);
    }

    if (previous_scene_name != current_scene_name && OnSceneChange != null) {
      OnSceneChange(current_scene_name, previous_scene_name);
      previous_scene_name = current_scene_name;
    }

    if (_input.GetStartButtonDown() && current_scene_name != "MainMenu" && current_scene_name != "SceneSelector" 
        && current_scene_name != "LevelSummary" && !reset_active) {
      StartCoroutine(PlayerInput());
      reset_active = true;
    }

    #if UNITY_EDITOR
    if (_input.GetButton4Down()) {
      if (File.Exists(Application.persistentDataPath + gamedata_filename)) {
        File.Delete(Application.persistentDataPath + gamedata_filename);
        Debug.Log("Game data file deleted.");
        UnityEditor.EditorApplication.isPlaying = false;
      }
    }
    #endif
  }
  
  private IEnumerator PlayerInput() {
    reset_canvas.SetActive(true);

    if (InputController.instance.type() == ControllerType.XBOXONE) {
      reset_text.text = "Press B to Exit\nPress A to Continue...";
    } else if (InputController.instance.type() == ControllerType.PS4) {
      reset_text.text = "Press O to Exit\nPress X to Continue...";
    } else {
      reset_text.text = "Unknown Input...";
    }
    
    FellowPlayer _player = GameObject.Find("Jelly").GetComponent<UnityJellySprite>().CentralPoint.GameObject.GetComponent<FellowPlayer>();
    _player.Pause = true;
    PostProcessingBehaviour effects = Camera.main.gameObject.GetComponent<PostProcessingBehaviour>();
    effects.profile.depthOfField.enabled = true;
    
    Time.timeScale = 0f;
    
    /* show reset menu */
    bool a_button = false;
    bool b_button = false;
    while (!a_button && !b_button) {
      a_button = _input.GetButton3Down();
      b_button = _input.GetButton2Down();
      yield return null;
    }
    
    Time.timeScale = 1f;
    
    effects.profile.depthOfField.enabled = false;
    reset_canvas.SetActive(false);
    reset_active = false;
    
    if (a_button) {
      /* continue scene */
      _player.Pause = false;
    } else if (b_button) {
      /* go back to the scene selector */
      instance.LoadSceneWithName("SceneSelector");
    }
  }

  private void UpdateScenename(string _name) {
    current_scene_name = _name;
  }
  
  public void LoadSceneWithName(string _name) {
    SceneLoader.instance.LoadSceneWithName(_name, UpdateScenename);
  }
  
  public void ReloadCurrentScene() {
    SceneLoader.instance.LoadSceneWithName(current_scene_name, UpdateScenename);
  }
  
  public void LoadSummary(float _time) {
    SceneInfo value;
    if (scene_informations.SceneInfos.TryGetValue(current_scene_name, out value)) {
      /* update the previously played time */
      value.previous_attempt_score = _time;
      /* update highscore if time is lesser */
      if (value.highscore == 0f || value.highscore > value.previous_attempt_score) value.highscore = value.previous_attempt_score;
      
      /* update medal if better */
      if (_time < value.gold_boundary.boundary) {
        if(value.achieved_medal > Medal.Gold) value.achieved_medal = Medal.Gold;
      } else if (_time < value.silver_boundary.boundary) {
        if(value.achieved_medal > Medal.Silver) value.achieved_medal = Medal.Silver;
      } else if (_time < value.bronze_boundary.boundary) {
        if(value.achieved_medal > Medal.Bronze) value.achieved_medal = Medal.Bronze;
      }
    }
    
    /* save before going to the level summary */
    Save();
    
    current_scene_info = value;
    SceneLoader.instance.LoadSceneWithName("LevelSummary", UpdateScenename);
  }

  /// <summary>
  /// Gets the previous scenes SceneInfo.
  /// Only return these info to LevelSummary otherwise assume that info has
  /// not been updated and will just return null.
  /// </summary>
  /// <returns>SceneInfo for the previous scene</returns>
  public SceneInfo GetPrevSceneInfo() {
    return current_scene_name == "LevelSummary" ? current_scene_info : null;
  }

  /// <summary>
  /// Return all of the loaded or newly created scenes information.
  /// </summary>
  /// <returns>All of the SceneInfos of the scene</returns>
  public ScenesInformation GetScenesInformation() {
    return scene_informations;
  }
}