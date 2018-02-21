using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class SceneButton : MonoBehaviour
{

    // in Unity add desired scene to the button throught the inspector
    public Object theSceneLinkedByThisButton;

    public void LoadByName()
    {
        SceneController.control.previousSceneName = SceneManager.GetActiveScene().name;
        SceneController.control.currSceneName = theSceneLinkedByThisButton.name;
        SceneManager.LoadScene(theSceneLinkedByThisButton.name);
    }   
}
   
