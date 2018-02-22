using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class EndGoalPoint : MonoBehaviour
{
    private GameObject slime;

    private void Start()
    {
        slime = GameObject.Find("SlimePlayer");
    }

    void OnTriggerEnter2D(Collider2D hit) {

        if (hit.transform.parent.gameObject == slime)
        {
            SceneController.control.previousSceneName = SceneManager.GetActiveScene().name;
            SceneController.control.currSceneName = "LevelSummary";
            SceneManager.LoadScene("LevelSummary");
        }
    }
}
