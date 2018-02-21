using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

	private GameObject collectables;

	private CollectedItems script;

    private void OnCollisionEnter2D(Collision2D col) {
		
		script = GameObject.Find ("CollectedItems").GetComponent<CollectedItems> ();
		if (col.gameObject.CompareTag ("Player")) {
			//myObject.GetComponent<MyScript>().MyFunction();
			Destroy (gameObject);
			script.AddItem ("Green Thing", "It's green, bitch");

			Destroy (gameObject);
		}

    }

    // Use this for initialization
    void Start () {
		collectables = GameObject.Find ("CollectedItems");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
