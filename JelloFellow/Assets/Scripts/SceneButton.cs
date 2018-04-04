using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour {
  [CustomLabel("Linked scene name")] [Tooltip("Linked Scene to load when button clicked")]
  public string linked_scene_name;

  public void LoadByName() {
    MainScript.instance.LoadSceneWithName(linked_scene_name);
  }

  public void Quit() {
    Application.Quit();
  }
}