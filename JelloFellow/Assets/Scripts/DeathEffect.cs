using System.Collections;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

[RequireComponent(typeof(JellySprite))]
[DisallowMultipleComponent]
public class DeathEffect : MonoBehaviour {
  private const string resetmenu_path = "Prefabs/UI/ResetMenu";
  private JellySprite _jelly;
  private Vector2 _vel;
  [Range(0, 0.5f)] public float ShrinkTime = 0.2f;
  private GameObject reset_canvas;
  private Text reset_text;
  private bool destroyed;
  
  private void Start() {
    _jelly = GetComponent<JellySprite>();
    //_jelly.CentralPoint.transform.parent.gameObject.SetActive(false);
    
    reset_canvas = Resources.Load<GameObject>(resetmenu_path);
    reset_canvas = Instantiate(reset_canvas);
    reset_canvas.GetComponent<Canvas>().sortingOrder = 101;
    reset_text = reset_canvas.GetComponentInChildren<Text>();
    reset_canvas.SetActive(false);
    destroyed = false;
  }

  private void LateUpdate() {
    var scale = transform.localScale;
    if ((scale.x < 0.02f || scale.y < 0.02f) && !destroyed) {
      /* call reset menu */
      if (CompareTag("Player")) {
        StartCoroutine(PlayerDeath());
        //SceneLoader.instance.LoadSceneWithName(SceneManager.GetActiveScene().name);
      } else {
        _jelly.CentralPoint.transform.parent.gameObject.SetActive(false);
        Destroy(_jelly.gameObject);
      }

      destroyed = true;
    }

    transform.localScale = Vector2.SmoothDamp(transform.localScale, Vector2.zero, ref _vel, ShrinkTime, Mathf.Infinity, Time.deltaTime);
  }

  private IEnumerator PlayerDeath() {
    reset_canvas.SetActive(true);
    
    if (InputController.instance.type() == ControllerType.XBOXONE) {
      reset_text.text = "Press B to Exit\nPress A to Try Again?";
    } else if (InputController.instance.type() == ControllerType.PS4) {
      reset_text.text = "Press O to Exit\nPress X to Try Again?";
    } else {
      reset_text.text = "Unknown Input...";
    }

    FellowPlayer _player = _jelly.CentralPoint.GameObject.GetComponent<FellowPlayer>();
    _player._timer.Stop = false;
    _player.Pause = true;

    /* disable collider for jello so he doesn't die after hitting goal */
    _player.transform.parent.gameObject.SetActive(false);

    PostProcessingBehaviour effects = Camera.main.gameObject.GetComponent<PostProcessingBehaviour>();
    effects.profile.depthOfField.enabled = true;
    
    /* show reset menu */
    bool a_button = false;
    bool b_button = false;
    while (!a_button && !b_button) {
      Input2D _input = InputController.instance.input;
      a_button = _input.GetButton3Down();
      b_button = _input.GetButton2Down();
      yield return null;
    }
    
    effects.profile.depthOfField.enabled = false;
    reset_canvas.SetActive(false);
    
    if (a_button) {
      /* reload scene */
      MainScript.instance.ReloadCurrentScene();
    } else if (b_button) {
      /* go back to the scene selector */
      MainScript.instance.LoadSceneWithName("SceneSelector");
    }
    
    Destroy(_jelly.gameObject);
  }
}