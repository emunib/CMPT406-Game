using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : Singleton<SceneLoader> {
  private const string loadingscreen_path = "Prefabs/UI/LoadingCanvas";
  private const string loading_string = "Loading...";
  private const float fade_duration = 0.8f;
  
  private GameObject loading_canvas;
  private CanvasGroup loading_canvasgroup;
  private Text text;
  private AsyncOperation async_operation;
  public delegate void LoadCompleteDelegate(string scene_name);
  
  private void Awake() {
    loading_canvas = Resources.Load<GameObject>(loadingscreen_path);
    loading_canvas = Instantiate(loading_canvas, transform);
    loading_canvasgroup = loading_canvas.GetComponent<CanvasGroup>();
    GameObject loading_gameobject = loading_canvas.transform.Find("LoadingScreen").gameObject;
    text = loading_gameobject.GetComponentInChildren<Text>();
    loading_canvasgroup.alpha = 0f;
  }

  public void LoadSceneWithName(string _name, LoadCompleteDelegate _loaddelegate) {
    text.text = loading_string;
    StartCoroutine(Fade(loading_canvasgroup, FadeType.IN, _name, _loaddelegate));
  }

  private IEnumerator LoadLevel(string _name, LoadCompleteDelegate _loaddelegate) {
    async_operation = SceneManager.LoadSceneAsync(_name, LoadSceneMode.Single);    
    while (!async_operation.isDone) {
      yield return null;
    }

    _loaddelegate(_name);
    ResetLoadingScreen();
  }
  
  private IEnumerator Fade(CanvasGroup _group, FadeType _fade, string _name, LoadCompleteDelegate _loaddelegate) {
    float alpha = 0f;
    if (_fade == FadeType.IN) alpha = 1.0f;
    
    for (float t = 0.0f; t < fade_duration; t += Time.deltaTime) {
      _group.alpha = Mathf.Lerp(_group.alpha, alpha, t / fade_duration);
      yield return null;
    }
    
    if (_fade == FadeType.IN) StartCoroutine(LoadLevel(_name, _loaddelegate));
  }

  private void ResetLoadingScreen() {
    StartCoroutine(Fade(loading_canvasgroup, FadeType.OUT, null, null));
  }
  
  private enum FadeType { IN, OUT }
}
