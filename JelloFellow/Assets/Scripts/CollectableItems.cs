using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CollectableItems : Singleton<CollectableItems> {
  private const string collectedui_path = "Prefabs/UI/CollectedCanvas";
  private string current_scenename;
  private GameObject collected_canvas;
  private Text collected_text;
  private bool inmenu;
  
  private void Start() {
    current_scenename = SceneManager.GetActiveScene().name;
    MainScript.instance.OnSceneChange += SceneWasChanged;

    inmenu = false;
    collected_canvas = Resources.Load<GameObject>(collectedui_path);
    collected_canvas = Instantiate(collected_canvas, transform);
    collected_text = collected_canvas.GetComponentInChildren<Text>();
    collected_canvas.SetActive(false);
  }
  
  private void SceneWasChanged(string scene_name, string prev_scene) {
    current_scenename = scene_name;
    inmenu = current_scenename == "MainMenu" || current_scenename == "SceneSelector" || current_scenename == "LevelSummary";
    if(inmenu) collected_canvas.SetActive(false);
  }

  private void Update() {
    Input2D _input = InputController.instance.input;
    /* toggle collected out of in scene */
    if (_input.GetButton1Down() && !inmenu) {
      collected_canvas.SetActive(!collected_canvas.activeInHierarchy);
    }

    /* update the text only if you are in a scene */
    if (!inmenu) {
      ScenesInformation _information = MainScript.instance.GetScenesInformation();
      if (_information.SceneInfos.ContainsKey(current_scenename)) {
        SceneInfo _info = _information.SceneInfos[current_scenename];
        collected_text.text = "Collected\n" + _info.collected_collectables + " of " + _info.total_collectables;
      }
    }
  }

  /// <summary>
  /// Collected the item.
  /// </summary>
  /// <param name="_desc">The item to collect</param>
  public void CollectedItem(string _desc) {
    ScenesInformation _information = MainScript.instance.GetScenesInformation();
    /* if dictionary is not allocated then do so */
    if(_information.Collectables == null) _information.Collectables = new Dictionary<string, LinkedList<string>>();
    
    /* if the current scene is already in the dictionary (assuming the linked list is created and intialized) */
    if (_information.Collectables.ContainsKey(current_scenename)) {
      /* try to get the linked list (will never fail as we have confirmed the key exists otherwise dictionary
         is corrupted in which case this code contains bugs) */
      LinkedList<string> value;
      _information.Collectables.TryGetValue(current_scenename, out value);
      if (value != null) {
        /* add the new collectables item to the list and update the dictionary */
        value.AddLast(_desc);
        _information.Collectables[current_scenename] = value;
      }
    } else {      
      /* init a list for the key (scene name) and add it to the dictionary */
      LinkedList<string> collected_list = new LinkedList<string>();
      collected_list.AddLast(_desc); /* add to the list */
      _information.Collectables.Add(current_scenename, collected_list); /* add to the dictionary */
    }

    /* make sure the scene exists in the current scene otherwise don't udpate */
    if (_information.SceneInfos.ContainsKey(current_scenename)) {
      SceneInfo _info = _information.SceneInfos[current_scenename];
      _info.collected_collectables++;
    }

    /* save the game data as it was updated */
    MainScript.instance.Save();
  }

  /// <summary>
  /// Has sent description (collectable) been collected yet?
  /// </summary>
  /// <param name="_desc">The collectable to check against</param>
  /// <returns>If the description is collected or not</returns>
  public bool IsCollected(string _desc) {
    ScenesInformation _information = MainScript.instance.GetScenesInformation();
    if (_information.Collectables != null) {
      if (_information.Collectables.ContainsKey(current_scenename)) {
        LinkedList<string> value = _information.Collectables[current_scenename];
        return value.Contains(_desc);
      }
    }

    return false;
  }
}
