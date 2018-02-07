using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	private Enemy parent;

	public int movespeed;

	private Rigidbody2D rb;
	// Use this for initialization
	
	private void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	private void Update () {
		rb.velocity = new Vector2 (movespeed, rb.velocity.y);

		
	}

	
	
}
