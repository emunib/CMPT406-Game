using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooBounce : MonoBehaviour {

	[Range(0, 5)] public float bounciness = 2;
	
	
	public string playerLayer = "Player";
	private bool do_once;
	public bool debugReflectRay;

	private void Start() {
		do_once = false;
	}

	private void OnCollisionExit2D(Collision2D other) {
		if (other.gameObject.layer == LayerMask.NameToLayer(playerLayer)) {
			do_once = false;
		}
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.layer == LayerMask.NameToLayer(playerLayer) && !do_once) {

			if (other.rigidbody.velocity.magnitude >-1) {
				Debug.Log("Player came in");

				//Get Normal


				/*	
				RaycastHit2D ray = Physics2D.Raycast(other.transform.position, other.attachedRigidbody.velocity);
				Vector2 dir = Vector3.Reflect(other.attachedRigidbody.velocity, ray.normal);
	*/

				FellowPlayer center = other.transform.parent.GetComponentInChildren<FellowPlayer>();
				Vector2 v = center.gameObject.GetComponent<Rigidbody2D>().velocity;


				RaycastHit2D ray = Physics2D.Raycast(center.transform.position, v);
				Debug.DrawRay(center.transform.position, v, Color.green);
				Vector2 dir = Vector2.Reflect(v, ray.normal);

				Debug.DrawRay(center.transform.position, dir * 100);

				//center.SendMessage("AddVelocity",-center.GetGravity()*bounciness);
				center.SendMessage("AddVelocity", dir * bounciness);
				do_once = true;
			}
		}
	}


	
	private void OnCollisionStay2D(Collision2D other) {
		if (debugReflectRay) {
			if (other.gameObject.layer == LayerMask.NameToLayer(playerLayer)) {
				Debug.Log("Player came in");

				//Get Normal
				RaycastHit2D ray = Physics2D.Raycast(other.transform.position, other.collider.attachedRigidbody.velocity);
				Vector2 dir = Vector3.Reflect(other.collider.attachedRigidbody.velocity, ray.normal);

				Debug.DrawRay(other.transform.position, dir * 100);


			}
		}
	}

	}
	
	

