using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class EndGoalPoint : MonoBehaviour
{
<<<<<<< HEAD
    private GameObject slime;

    private void Start()
    {
        slime = GameObject.Find("Jelly");
    }

    void OnTriggerEnter2D(Collider2D hit) {
		Debug.Log ("Entered");
		if (hit.gameObject.tag == "Blob")
=======
//    private GameObject slime;
//
//    private void Start()
//    {
//        slime = GameObject.Find("SlimePlayer");
//    }

    void OnTriggerEnter2D(Collider2D hit) {
		Debug.Log ("Entered");
		if (hit.gameObject.CompareTag("Blob"))
>>>>>>> master
        {
            SceneController.control.previousSceneName = SceneManager.GetActiveScene().name;
            SceneController.control.currSceneName = "LevelSummary";
            SceneManager.LoadScene("LevelSummary");
        }
    }
}
