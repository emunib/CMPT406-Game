using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class SceneButton : MonoBehaviour
{

    // in Unity add desired scene to the button throught the inspector
    public Object theSceneLinkedByThisButton;
    static public string previousSceneName = "";

    public void LoadByName()
    {
        SceneManager.LoadScene(theSceneLinkedByThisButton.name);
    }

    public void LoadLevelSummary()
    {
        previousSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(theSceneLinkedByThisButton.name);
    }
    
}
   
