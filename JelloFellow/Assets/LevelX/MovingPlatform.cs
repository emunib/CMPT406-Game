using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	public Transform[] axis;
	public int startposition;
	public int endposition;
	public float speed;
	// Use this for initialization
	void Start () {
		transform.position = axis[startposition].position;
	}

	// Update is called once per frame
	void Update () {
		transform.position = Vector2.MoveTowards(transform.position, axis[endposition].position, speed = Time.deltaTime);
		if (transform.position == axis [endposition].position) {
			endposition++;
			if (endposition == axis.Length) {
				endposition = 0;
			}
		}	

	}
}