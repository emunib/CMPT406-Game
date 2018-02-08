using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Gravity : MonoBehaviour
{
	private new Rigidbody2D r_body;
	private Vector2 gravity;
	public bool in_radius;

	private void Awake()
	{
		r_body.GetComponent<Rigidbody2D>();
		r_body.gravityScale = 0f;
		
		gravity = new Vector2(0f, -9.8f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		r_body.velocity += gravity*Time.deltaTime;
	}
	
	// Change to jello fellows cur grav
	public void ChangeGravity(Vector2 jello_grav)
	{
		gravity = jello_grav;
	}
}
