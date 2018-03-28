using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BasicRotate : MonoBehaviour
{
	public float rate = 5f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.Rotate (0,rate*Time.deltaTime,0); //rotates 50 degrees per second around z axis
	}
}
