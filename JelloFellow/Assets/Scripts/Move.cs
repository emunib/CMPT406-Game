using System.Linq;
using UnityEngine;

public class Move : MonoBehaviour
{
	private Input2D _input;
	public float Power;
	public float Speed;
	private GameObject[] _children;
	private Transform _centre;

	private int _count;
	// Use this for initialization
	private void Start ()
	{
		_input = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>().GetInput();
		_children = GameObject.FindGameObjectsWithTag("Node");
		_centre = GameObject.Find("Softbody/Centre").transform;
	}
	
	// Update is called once per frame
	private void Update ()
	{
		GetComponent<CircleCollider2D>().offset = _centre.transform.position;

//		if (_count <= 0) return;
		
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

	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log(other.tag);
		if (other.CompareTag("Wall"))
		{
			_count++;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Wall"))
		{
			_count--;
		}
	}
}
