using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

	private GameObject collectables;
	private CollectedItems script;

	public string description;
	public GameObject collectPrefab;

    private void OnCollisionEnter2D(Collision2D col) {
		
		script = GameObject.Find ("CollectedItems").GetComponent<CollectedItems> ();
		if (col.gameObject.CompareTag ("Player")) {
			//myObject.GetComponent<MyScript>().MyFunction();
			Destroy (gameObject);
			script.AddItem (gameObject.name, description);

			Destroy (gameObject);
		}

    }

    // Use this for initialization
    void Start () {
		
		collectables = GameObject.Find ("CollectedItems");
		if (collectables == null) {
			collectables = collectPrefab;
			Instantiate (collectables);
			collectables = GameObject.Find ("CollectedItems(Clone)");	// New instance will have clone in the name
			collectables.name = "CollectedItems";						// Change it back to normal
		}

		script = GameObject.Find ("CollectedItems").GetComponent<CollectedItems> ();
		GameObject[] letters = GameObject.FindGameObjectsWithTag ("Collectable");
		for (int i = 0; i < letters.Length; i++) {
			if (script.isCollected (letters [i].name)) {
				Destroy (letters [i]);
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
