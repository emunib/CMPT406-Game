using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpikeTrigger : MonoBehaviour {

	private GameObject parent;
	private FallingSpikeSystem fallingSystem;

	private void Start() {
		parent = this.transform.parent.gameObject;
		fallingSystem = parent.GetComponent<FallingSpikeSystem>();
	}
	
	private void OnTriggerEnter2D(Collider2D other) {
		if (!fallingSystem.IsFalling() &&other.gameObject.layer == LayerMask.NameToLayer(fallingSystem.PlayerLayer)) {
			
			fallingSystem.PlayerHasEntered();
		}
	}

}
