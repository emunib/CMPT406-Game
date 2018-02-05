using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
	public float Power;
	public float Speed;
	private Rigidbody2D[] _children;
	// Use this for initialization
	private void Start ()
	{
		_children = GetComponentsInChildren<Rigidbody2D>();
	}
	
	// Update is called once per frame
	private void Update () {

		if (Input.GetButtonDown("Jump"))
		{
			foreach (var child in _children)
			{
				child.AddForce(Vector2.up * Power, ForceMode2D.Impulse);
			}
		}

		foreach (var child in _children)
		{
			child.AddForce(Input.GetAxis("Horizontal") * Vector2.right * Speed);
		}
	}
}
