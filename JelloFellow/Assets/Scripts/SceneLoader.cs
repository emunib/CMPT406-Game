using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : Singleton<SceneLoader> {
  private const string loadingscreen_path = "Prefabs/UI/LoadingCanvas";
  private const float scaletransition_speed = 50f;
  private const string loading_string = "Loading...";
  
  private GameObject loading_gameobject;
  private Slider bar;
  private Text text;
  private AsyncOperation async_operation;

  private void Awake() {
    GameObject loading_canvas = Resources.Load<GameObject>(loadingscreen_path);
    loading_canvas = Instantiate(loading_canvas, transform);
    loading_gameobject = loading_canvas.transform.Find("LoadingScreen").gameObject;
    loading_gameobject.transform.localScale = Vector3.zero;
    bar = loading_gameobject.GetComponentInChildren<Slider>();
    text = loading_gameobject.GetComponentInChildren<Text>();
    loading_gameobject.SetActive(false);
  }

  public void LoadSceneWithName(string _name) {
    text.text = loading_string;
    loading_gameobject.SetActive(true);
    StartCoroutine(ScaleLoadScene(_name));
  }

  private IEnumerator ScaleLoadScene(string _name) {
    while (loading_gameobject.transform.localScale != Vector3.one) {
      loading_gameobject.transform.localScale = Vector3.Lerp(loading_gameobject.transform.localScale, Vector3.one, Time.deltaTime * scaletransition_speed);
      yield return null;
    }

    StartCoroutine(LoadLevel(_name));
  }

  private IEnumerator LoadLevel(string _name) {
    async_operation = SceneManager.LoadSceneAsync(_name);
    while (!async_operation.isDone) {
      bar.value = async_operation.progress;
      yield return null;
    }
    ResetLoadingScreen();
  }

  private void ResetLoadingScreen() {
    loading_gameobject.SetActive(false);
    loading_gameobject.transform.localScale = Vector3.zero;
    bar.value = 0f;
  }
}
