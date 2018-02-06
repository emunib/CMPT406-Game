using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{

    public Object theSceneLinkedByThisButton;
    
    public void LoadByName()
    {
        SceneManager.LoadScene(theSceneLinkedByThisButton.name);
    }
}
