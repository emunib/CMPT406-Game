using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Move : MonoBehaviour
{
	private Input2D _input;
	public float Power;
	public float Speed;
	private readonly List<Transform> _children = new List<Transform>();

	// Use this for initialization
	private void Start ()
	{
		_input = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>().GetInput();

		for (var i = 1; i <= 9; i++)
		{
			_children.Add(GameObject.Find("Softbody/O" + i).transform);
		}
	}
	
	// Update is called once per frame
	private void Update ()
	{
		if (_input.GetJumpButtonDown())
		{
			foreach (var child in _children)
			{
				child.GetComponent<Rigidbody2D>().AddForce(Vector2.up * Power, ForceMode2D.Impulse);
			}
		}

		var temp = _children.OrderBy(o => o.transform.position.y).ToArray();

		for (var i = 0; i < 6; i++)
		{
			temp[i].GetComponent<Rigidbody2D>().AddForce(_input.GetHorizontalMovement() * Vector2.right * Speed);
		}
	}
}
