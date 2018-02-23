using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trackSawmovement : MonoBehaviour {

	private Vector2 posA,posB, nexPos;
	public float speed;

	public Transform childtrans;

	public Transform transformB;

	private void move(){
		childtrans.localPosition = Vector2.MoveTowards (childtrans.localPosition, nexPos, speed * Time.deltaTime);

		if (Vector2.Distance (childtrans.localPosition, nexPos) <= 0.1) {
			nexPos = nexPos!= posA ? posA: posB;

		}

	}

	// Use this for initializatio  n
	void Start () {
		posA = childtrans.localPosition;
		posB = transformB.localPosition;
		nexPos = posB;

		
	}
	
	// Update is called once per frame
	void Update () {
		move ();
		
		
	}
}
