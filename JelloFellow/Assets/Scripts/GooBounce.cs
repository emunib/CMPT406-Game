using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooBounce : MonoBehaviour {

	public float force= 100;
	public float distance = 1;
	private void OnCollisionEnter2D(Collision2D other) {
		
		if (other.gameObject.CompareTag("SlimeNode")) {
			Vector2 dir;
			dir= other.transform.position - transform.position;
			Transform center = GameObject.Find("Center").GetComponent<Transform>();
			RaycastHit2D hit = Physics2D.Raycast(center.position, -dir, distance);

			foreach (Rigidbody2D childrb in other.transform.parent.GetComponentsInChildren<Rigidbody2D>()) {
				
				Debug.DrawRay(transform.position, dir*5,Color.blue);
				Debug.DrawRay(transform.position, dir*5,Color.green);

				other.rigidbody.AddForce(hit.normal*force);

				}
			}

		}

		
		

	}
	
	

