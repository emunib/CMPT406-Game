using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;

public class BasicRotate : MonoBehaviour
{
	public float rate = 5f;
	public float ratez = 5f, ratex =5f;

	public bool x, y, z;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (x)
		{
			transform.Rotate (ratex*Time.deltaTime ,0,0);
		}
		if (y)
		{
			transform.Rotate (0,rate*Time.deltaTime ,0);
		}
		if (z)
		{
			transform.Rotate (0,0,ratez*Time.deltaTime);
		}
		
	}
}
