using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

	private GameObject collectables;
	private CollectedItems script;

	[TextArea]
	public string description;
	public GameObject collectPrefab;

    private void OnCollisionEnter2D(Collision2D col) {
		
		script = GameObject.Find ("CollectedItems").GetComponent<CollectedItems> ();
		if (col.gameObject.CompareTag ("Player")) {
			//myObject.GetComponent<MyScript>().MyFunction();
			//Destroy (gameObject);
			script.AddItem (gameObject.name, description);

			Destroy (gameObject);
		}

    }

    // Use this for initialization
    void Start () {

		// Find the collected items object, may not exist yet
		collectables = GameObject.Find ("CollectedItems");
		if (collectables == null) {
			collectables = collectPrefab;								
			Instantiate (collectables);									// Created item storage object from prefab
			collectables = GameObject.Find ("CollectedItems(Clone)");	// New instance will have clone in the name
			collectables.name = "CollectedItems";						// Change it back to normal
		}

		// Check if the collectible has already been found.
		script = GameObject.Find ("CollectedItems").GetComponent<CollectedItems> ();
		GameObject[] letters = GameObject.FindGameObjectsWithTag ("Collectable");
		for (int i = 0; i < letters.Length; i++) {

			// If the collectible is already in the list of collected items, destroy it.
			if (script.isCollected (letters [i].name)) {
				Destroy (letters [i]);		
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
