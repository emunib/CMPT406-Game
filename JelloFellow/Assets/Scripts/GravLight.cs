using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

using UnityEngine;

public class GravLight : MonoBehaviour {


	public bool disableHorizontal = true;
	public bool disableVertical = true;
	void OnTriggerEnter2D(Collider2D other) {

		Vector2 disableAlong = Vector2.one;
		if (disableHorizontal) {
			disableAlong.x = 0;
		}

		if (disableVertical) {
			disableAlong.y = 0;
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

		if (other.attachedRigidbody){
			Gravity field = other.GetComponent<Gravity>();

			if (field != null) {
				field.SetGravityLightRestrictions(Vector2.one);
			}

		}
	}

	
	
}


