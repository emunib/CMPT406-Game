using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	public string previousSceneName = "";
	public string currSceneName = "";
	public static SceneController control;
	
	// Use this for initialization
	void Awake () {
		if (control == null)
		{
			DontDestroyOnLoad(gameObject);
			control = this;
		}
		else if(control != this)
		{
			Destroy(gameObject);
		}

		currSceneName = SceneManager.GetActiveScene().name;
	}
}
