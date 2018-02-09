using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Move : MonoBehaviour
{
	private Input2D _input;
	public float Power;
	public float Speed;
	private GameObject[] _children;
	// Use this for initialization
	private void Start ()
	{
		_input = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>().GetInput();
		_children = GameObject.FindGameObjectsWithTag("Node");
	}
	
	// Update is called once per frame
	private void Update () {

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
