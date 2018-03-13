using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooComponents : MonoBehaviour {
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update () {
		Vector3 vel = rb.velocity;
		if (vel.x>5) {
			vel.x = 5;

			rb.velocity = vel;
		}

		if (vel.y > 5) {
			vel.y = 5;
			rb.velocity = vel;
		}
		
	}
}
