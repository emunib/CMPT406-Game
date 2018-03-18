using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmooshCollider : MonoBehaviour {
	
	
	private void OnTriggerEnter2D(Collider2D other) {
		//print("Collision"+ LayerMask.LayerToName(other.gameObject.layer));

		if (other.gameObject.layer == LayerMask.NameToLayer("Platform")) {
			SendMessageUpwards("GoBack");
		}

		
	}
//	private void OnCollisionEnter2D(Collision2D other) {
//		//print("Collision"+ LayerMask.LayerToName(other.gameObject.layer));
//
//		if (other.gameObject.layer == LayerMask.NameToLayer("Platform")) {
//			SendMessageUpwards("GoBack");
//		}
//
//		
//	}
	
	
}
