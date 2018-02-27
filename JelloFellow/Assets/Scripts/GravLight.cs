using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

using UnityEngine;


public class GravLight : MonoBehaviour {

	[Header("Disable along which axis'")]
	public bool Horizontal = true;
	public bool Vertical = true;

	public float amount = 0f;
	
	void OnTriggerEnter2D(Collider2D other) {

		Vector2 disableAlong = Vector2.one;
		if (Horizontal) {
			disableAlong.x = amount;
		}

		if (Vertical) {
			disableAlong.y = amount;
		}
		
		
		
		if (other.attachedRigidbody) {

			Collider2D[] col = other.GetComponents<Collider2D>();
			/*foreach (Collider2D c in col) {
				if (c.isTrigger) {
					return;
				}
			}*/
			
			Debug.Log ("object entered field");
			Gravity field = other.GetComponent<Gravity>();
			if (field != null) {
				field.SetGravityLightRestrictions(disableAlong);
			}

		}
	}

	void OnTriggerExit2D(Collider2D other) {

		//REanable Gravity
		if (other.attachedRigidbody){
			Gravity field = other.GetComponent<Gravity>();

			if (field != null) {
				field.SetGravityLightRestrictions(Vector2.one);
			}

		}
	}

	
	
}


