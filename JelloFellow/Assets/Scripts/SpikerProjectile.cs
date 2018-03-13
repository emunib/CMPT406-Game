using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikerProjectile : MonoBehaviour {
	private const float speed = 10f;
	private bool movement = false;

	private void Update() {
		if(movement) transform.position += transform.up * Time.deltaTime * speed;
	}
	
	public void activate() {
		movement = true;
	}
}
