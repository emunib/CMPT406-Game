using System.Collections.Generic;
using UnityEngine;

public class FaceController : MonoBehaviour
{
	private List<Transform> _nodes = new List<Transform>();
	private Transform centre;
	float offset;

	
	// Use this for initialization
	private void Start () {
		for (var i = 1; i <= 5; i++)
		{
			_nodes.Add(GameObject.Find("Softbody/C" + i).transform);
		}
		for (var i = 1; i <= 3; i++)
		{
			_nodes.Add(GameObject.Find("Softbody/U" + i).transform);
		}
		for (var i = 1; i <= 3; i++)
		{
			_nodes.Add(GameObject.Find("Softbody/L" + i).transform);
		}
		for (var i = 1; i <= 3; i++)
		{
			_nodes.Add(GameObject.Find("Softbody/R" + i).transform);
		}

		centre = GameObject.Find("Softbody/Centre").transform;
	}
	
	// Update is called once per frame
	private void LateUpdate ()
	{
		var minX = _nodes[0].position.x;
		var maxX = _nodes[0].position.x;
		
		var minY = _nodes[0].position.y;
		var maxY = _nodes[0].position.y;
		
		foreach (var node in _nodes)
		{
			minX = Mathf.Min(minX, node.position.x);
			maxX = Mathf.Max(maxX, node.position.x);
			
			minY = Mathf.Min(minY, node.position.y);
			maxY = Mathf.Max(maxY, node.position.y);
		}
		
		Debug.Log(centre.GetComponent<Rigidbody2D>().velocity);

		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
		{
			offset = Input.GetAxis("Horizontal");
		}
		
		transform.position = new Vector2((maxX - minX) * 0.5f + minX + offset * 0.5f, (maxY - minY) * 0.8f + minY);
			
	}
}
