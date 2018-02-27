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
	public Sprite gray;

    private void OnCollisionEnter2D(Collision2D col) {
		

		if ((col.gameObject.CompareTag ("Blob") || col.gameObject.CompareTag ("Player")) && !collected) {
			
			script = GameObject.Find ("CollectedItems").GetComponent<CollectedItems> ();
			script.AddItem (gameObject.name, description);
			collected = true;
			gameObject.GetComponent<SpriteRenderer> ().sprite = gray;
			Destroy(gameObject.GetComponent<BoxCollider2D> ());

		}

    }

	/// <summary>
	/// Sets collected.
	/// </summary>
	/// <param name="x">If set to <c>true</c> x.</param>
	public void setCollected(bool x) {
		collected = x;
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

			// If the collectible is already in the list of collected items, set it as collected.
			if (script.isCollected (letters [i].name)) {
				letters [i].GetComponent<Collectable>().setCollected(true);		
			}
		}

	}
	
	// Update is called once per frame
	void Update () {

		// Should only animate if it isn't collected yet
		if (!collected) {
			
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
}
