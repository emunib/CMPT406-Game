using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CollectableItems : Singleton<CollectableItems> {
  private const string collectedui_path = "Prefabs/UI/CollectedCanvas";
  private const string showroom_path = "Prefabs/UI/CollectedShowroom";
  private const string controlimage_path = "Prefabs/UI/ControlCanvas";
  private const float fade_duration = 0.8f;
  
  private string current_scenename;
  private bool inmenu;
  
  private CanvasGroup collected_canvasgroup;
  private Text collected_text;
  private bool active_in_scene;
  private Coroutine collected_canvasfader;

  private CanvasGroup showroom_canvasgroup;
  private CollectedShowroomManager showroom_manager;
  private bool showroom_inscene;
  private Coroutine showroom_canvasfader;

  private CanvasGroup control_canvasgroup;
  
  
  private void Start() {
    current_scenename = SceneManager.GetActiveScene().name;
    MainScript.instance.OnSceneChange += SceneWasChanged;

    inmenu = current_scenename == "MainMenu" || current_scenename == "SceneSelector" || current_scenename == "LevelSummary" || current_scenename == "Credits";
    GameObject collected_canvas = Resources.Load<GameObject>(collectedui_path);
    collected_canvas = Instantiate(collected_canvas, transform);
    collected_text = collected_canvas.GetComponentInChildren<Text>();
    collected_canvasgroup = collected_canvas.GetComponent<CanvasGroup>();
    collected_canvasgroup.alpha = 0f;
    active_in_scene = false;
    collected_canvasfader = null;

    GameObject showroom_canvas = Resources.Load<GameObject>(showroom_path);
    showroom_canvas = Instantiate(showroom_canvas, transform);
    showroom_canvasgroup = showroom_canvas.GetComponent<CanvasGroup>();
    showroom_manager = showroom_canvas.GetComponent<CollectedShowroomManager>();
    showroom_canvasgroup.alpha = 0f;
    showroom_inscene = false;
    showroom_canvasfader = null;
    
    showroom_manager.RefreshDays();
    
    GameObject control_canvas = Resources.Load<GameObject>(controlimage_path);
    control_canvas = Instantiate(control_canvas, transform);
    control_canvasgroup = control_canvas.GetComponent<CanvasGroup>();
    control_canvasgroup.alpha = 0f;
  }
  
  private void SceneWasChanged(string scene_name, string prev_scene) {
    current_scenename = scene_name;
    inmenu = current_scenename == "MainMenu" || current_scenename == "SceneSelector" || current_scenename == "LevelSummary" || current_scenename == "Credits";
    if (inmenu && active_in_scene) {
      if (collected_canvasfader != null) {
        StopCoroutine(collected_canvasfader);
        collected_canvasfader = null;
      }

      collected_canvasfader = StartCoroutine(Fade(collected_canvasgroup, 0f));
      active_in_scene = false;
    } else if (inmenu && prev_scene == "MainMenu") {
      if (showroom_canvasfader != null) {
        StopCoroutine(showroom_canvasfader);
        showroom_canvasfader = null;
      }
      
      showroom_canvasfader = StartCoroutine(Fade(control_canvasgroup, 0f));
      showroom_inscene = false;
    } else if (inmenu && prev_scene == "SceneSelector") {
      if (showroom_canvasfader != null) {
        StopCoroutine(showroom_canvasfader);
        showroom_canvasfader = null;
      }
      
      showroom_canvasfader = StartCoroutine(Fade(showroom_canvasgroup, 0f));
      showroom_inscene = false;
    } else if (!inmenu && showroom_inscene) {
      if (showroom_canvasfader != null) {
        StopCoroutine(showroom_canvasfader);
        showroom_canvasfader = null;
      }
      
      showroom_canvasfader = StartCoroutine(Fade(showroom_canvasgroup, 0f));
      showroom_inscene = false;
    }
  }

  private void Update() {
    Input2D _input = InputController.instance.input;
    /* toggle collected out of in scene */
    if (_input.GetButton1Down()) {
      if (!inmenu) {
        if (collected_canvasfader != null) {
          StopCoroutine(collected_canvasfader);
          collected_canvasfader = null;
        }

        if (active_in_scene) {
          collected_canvasfader = StartCoroutine(Fade(collected_canvasgroup, 0f));
          active_in_scene = false;
        } else {
          collected_canvasfader = StartCoroutine(Fade(collected_canvasgroup, 1f));
          active_in_scene = true;
        }
      } else {
        if (showroom_canvasfader != null) {
          StopCoroutine(showroom_canvasfader);
          showroom_canvasfader = null;
        }

        if (showroom_inscene) {
          showroom_canvasfader = StartCoroutine(current_scenename == "MainMenu" ? Fade(control_canvasgroup, 0f) : Fade(showroom_canvasgroup, 0f));

//          showroom_canvasfader = StartCoroutine(Fade(showroom_canvasgroup, 0f));
          showroom_inscene = false;
        } else {
          showroom_canvasfader = StartCoroutine(current_scenename == "MainMenu" ? Fade(control_canvasgroup, 1f) : Fade(showroom_canvasgroup, 1f));

//          showroom_canvasfader = StartCoroutine(Fade(showroom_canvasgroup, 1f));
          showroom_inscene = true;
        }
      }
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
  
  private IEnumerator Fade(CanvasGroup _group, float alpha) {
    for (float t = 0.0f; t < fade_duration; t += Time.deltaTime) {
      _group.alpha = Mathf.Lerp(_group.alpha, alpha, t / fade_duration);
      yield return null;
    }
  }

  /// <summary>
  /// Collected the item.
  /// </summary>
  /// <param name="_day">The day which the item was created</param>
  /// <param name="_desc">The item to collect</param>
  public void CollectedItem(int _day, string _desc) {
    ScenesInformation _information = MainScript.instance.GetScenesInformation();
    /* if dictionary is not allocated then do so */
    //if(_information.Collectables == null) _information.Collectables = new Dictionary<int, string>();
    
    /* if the day is already in the dictionary then let developers know its an error */
    if (_information.Collectables.ContainsKey(_day)) {
      Debug.LogError("Note for day (" + _day + ") was already created.");
    } else {
      _information.Collectables.Add(_day, _desc); /* add to the dictionary */
      
      /* make sure the scene exists in the current scene otherwise don't udpate */
      if (_information.SceneInfos.ContainsKey(current_scenename)) {
        SceneInfo _info = _information.SceneInfos[current_scenename];
        _info.collected_collectables++;
      }

      /* save the game data as it was updated */
      MainScript.instance.Save();
      
      /* refresh the table */
      showroom_manager.RefreshDays();
    }
  }

  /// <summary>
  /// Has the description (collectable) been collected yet?
  /// </summary>
  /// <param name="_day">The unique day the note was created to check if its in the dictionary</param>
  /// <returns>If the description is collected or not</returns>
  public static bool IsCollected(int _day) {
    ScenesInformation _information = MainScript.instance.GetScenesInformation();
    return _information.Collectables != null && _information.Collectables.ContainsKey(_day);
  }
}
