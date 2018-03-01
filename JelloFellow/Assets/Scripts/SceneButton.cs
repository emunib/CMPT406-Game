using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{

    // in Unity add desired scene to the button throught the inspector
    public Object theSceneLinkedByThisButton;
    
    public void LoadByName()
    {
        SceneManager.LoadScene(theSceneLinkedByThisButton.name);
    }

    public void Quit() {
        Application.Quit();
    }
}
