using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnsureAudio : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject am;
		if (GameObject.Find("AudioManager") == null) {
			 am = Resources.Load("Prefabs/AudioManager") as GameObject;
			Debug.Log("l");
			Instantiate(am);

		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
