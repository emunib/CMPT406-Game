using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour {
	public Text speed;
	public Rigidbody2D rb;
	// Use this for initialization
	void Start () {
		//speed = GetComponent<Text> ();
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update () {
		speed.text = Mathf.RoundToInt(rb.velocity.magnitude) + " MPH";
	}
}
