using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{

    // in Unity add desired scene to the button throught the inspector
    //public Object theSceneLinkedByThisButton;
    [CustomLabel("Linked scene name")]
    public string linked_scene_name;
    
    public void LoadByName()
    {
        GameController.control.previousSceneName = SceneManager.GetActiveScene().name;
        GameController.control.currSceneName = linked_scene_name;
        SceneLoader.instance.LoadSceneWithName(linked_scene_name);
        //SceneManager.LoadScene(theSceneLinkedByThisButton.name);
    }

    public void Quit() {
        Application.Quit();
    }
}
