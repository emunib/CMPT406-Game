using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

	private GameObject collectables;
	private CollectedItems script;

	public string description;
	public GameObject collectPrefab;

    private void OnCollisionEnter2D(Collision2D col) {
		
		script = GameObject.Find ("CollectedItems(Clone)").GetComponent<CollectedItems> ();
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
			collectables.name = "CollectedItems";
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
