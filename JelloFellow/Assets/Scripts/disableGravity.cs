using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class disableGravity : MonoBehaviour {

	
	void OnTriggerEnter2D(Collider2D other){
		

		if (other.attachedRigidbody) {
		
 			Debug.Log ("object entered field");
			//other.attachedRigidbody.gravityScale = 0f;

			
			
			//Vector2 vel = other.attachedRigidbody.velocity;
			//other.attachedRigidbody.velocity = new Vector2(vel.x,0);
			

		}
	}

	void OnTriggerStay2D(Collider2D other) {
		

		if (other.attachedRigidbody) {
			GravityPlayer generic = other.GetComponent<GravityPlayer>();
			Vector2 g = generic.GetGravity();
			
			
			
			Vector2 vel = other.attachedRigidbody.velocity;


			vel = Vector2.zero;
			
			
			
			other.attachedRigidbody.velocity = vel;




			//Vector2 g = new Vector2(Physics2D.gravity.x,0);
			//other.attachedRigidbody.AddForce(g);
		}
	}
		
	void OnTriggerExit2D(Collider2D other){
		if (other.attachedRigidbody) {
			//other.attachedRigidbody.gravityScale = 1f;
			Debug.Log ("leaving grav field");
		}
	}
	
}


