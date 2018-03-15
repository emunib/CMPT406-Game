using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoosher : MonoBehaviour {

	public float speed;
	
	
	private GameObject bar;
	private GameObject rod;

	private bool goingback;

	private Vector3 startPos;
	// Use this for initialization
	void Start () {
		
		bar = GameObject.Find("SmooshBar");
		rod = GameObject.Find("Rod");

		
	}
	
	// Update is called once per frame
	void Update () {
		bar.GetComponent<Rigidbody2D>().velocity = -transform.up*speed;

		float distance = Vector2.Distance(transform.position, bar.transform.position);
		SpriteRenderer rodsprite = rod.GetComponent<SpriteRenderer>();

		rodsprite.size = new Vector2(distance,rodsprite.size.y);
		Vector3 middlepos =  bar.transform.position -transform.position;
		middlepos /= 2;
		rod.transform.position = middlepos+transform.position;

		if (goingback && Vector3.Distance(bar.transform.position, transform.position) < .2) {
			goingback = false;
			speed = -speed;
		}
		

	}

	private void GoBack() {
		if (goingback == false) {
			speed = -speed;
			goingback = true;
		}
		
	}
	
	
}
