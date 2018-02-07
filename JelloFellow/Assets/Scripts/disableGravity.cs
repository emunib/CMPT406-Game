using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableGravity : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.attachedRigidbody) {
			Vector2 vel = new Vector2 (other.attachedRigidbody.velocity.x, 0f);
			other.attachedRigidbody.gravityScale= 0f;
 			Debug.Log ("object entered field");
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.attachedRigidbody) {
			Vector2 vel = new Vector2 (0.0f, 0f);
			other.attachedRigidbody.gravityScale= 0f;
			other.attachedRigidbody.velocity = vel;
			Debug.Log ("Object is at rest");
		}
	}
		/*
	void OnTriggerExit2D(Collider2D other){
		if (other.attachedRigidbody) {
			other.attachedRigidbody.gravityScale = 1f;
			Debug.Log ("leaving grav field");
		}
	}
	*/
}
