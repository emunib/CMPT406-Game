using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Move : MonoBehaviour
{
	public float Power;
	public float Speed;
	private GameObject[] _children;
	// Use this for initialization
	private void Start ()
	{
		_children = GameObject.FindGameObjectsWithTag("Node");
	}
	
	// Update is called once per frame
	private void Update () {

		if (Input.GetButtonDown("Jump"))
		{
			foreach (var child in _children)
			{
				child.GetComponent<Rigidbody2D>().AddForce(Vector2.up * Power, ForceMode2D.Impulse);
			}
		}

		var temp = _children.OrderBy(o => o.transform.position.y).ToArray();

		for (var i = 0; i < 6; i++)
		{
			temp[i].GetComponent<Rigidbody2D>().AddForce(Input.GetAxis("Horizontal") * Vector2.right * Speed);
		}
	}
}
