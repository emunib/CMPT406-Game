using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooBounceInner : MonoBehaviour {
	public string playerLayer = "SlimeEffector";

	public bool debugReflectRay;
	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.layer == LayerMask.NameToLayer(playerLayer)) {
			Debug.Log("Player came in");

			//Get Normal
			RaycastHit2D ray = Physics2D.Raycast(other.transform.position, other.attachedRigidbody.velocity);
			Vector2 dir = Vector3.Reflect(other.attachedRigidbody.velocity, ray.normal);

			FellowPlayer center = other.transform.parent.GetComponentInChildren<FellowPlayer>();
			center.SendMessage("AddVelocity",dir*2);
			
			
		}
	}


	
	private void OnTriggerStay2D(Collider2D other) {
		if (debugReflectRay) {
			if (other.gameObject.layer == LayerMask.NameToLayer(playerLayer)) {
				Debug.Log("Player came in");

				//Get Normal
				RaycastHit2D ray = Physics2D.Raycast(other.transform.position, other.attachedRigidbody.velocity);
				Vector2 dir = Vector3.Reflect(other.attachedRigidbody.velocity, ray.normal);

				Debug.DrawRay(other.transform.position, dir * 100);


			}
		}
	}
}
