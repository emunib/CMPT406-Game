using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

using UnityEngine;


public class GravLight : MonoBehaviour {

	[Header("Disable along which axis")] 
	public bool Horizontal = false;
	public bool Vertical = false;

	[Header("Amount of gravity to disable")]
	[Tooltip("amount is multiplied by the gravity that would otherwise be applied. " +
	         "So a value of 0 and disabling the x axis would disable gravity completely along that axis")]
	[Range(0.001f,1f)]
	public float amount = 0.001f;
	
	void OnTriggerEnter2D(Collider2D other) {

		Vector2 disableAlong = Vector2.one;
		if (Horizontal) {
			disableAlong.x = amount;
		}

		if (Vertical) {
			disableAlong.y = amount;
		}
		
		
		
		if (other.attachedRigidbody) {

			//Collider2D[] col = other.GetComponents<Collider2D>();
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


