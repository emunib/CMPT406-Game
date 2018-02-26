using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

	private GameObject collectables;
	private CollectedItems script;
	private float y_speed;
	private string direction;
	private bool collected;

	[TextArea]
	public string description;
	public GameObject collectPrefab;
	public float max_y_speed = 1;
	public float accel = 1;

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

		y_speed = max_y_speed;

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

		float dt = Time.deltaTime;

		// Decide whether y speed should increase or decrease
		if (y_speed >= max_y_speed) {
			direction = "down";
		} else if (y_speed <= -max_y_speed) {
			direction = "up";
		}

		// Apply acceleration in the proper direction
		if (direction == "up") {
			y_speed = y_speed + accel * dt;
		} else if (direction == "down") {
			y_speed = y_speed - accel * dt;
		}

		// Translate game object according to its speed
		transform.position = new Vector2 (transform.position.x, transform.position.y + y_speed * dt);
		
	}
}
