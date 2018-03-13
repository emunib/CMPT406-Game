using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
	public Transform Outer;
	[Range(0, 0.5f)] public float thickness;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.localScale = Outer.localScale - Vector3.one * thickness;
	}
}
