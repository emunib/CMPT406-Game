using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class waypointFollow : MonoBehaviour
{


	public Transform[] Waypoints;
	public float MoveSpeed;
	public Transform Saw;
	private int posCounter= 0;
	private Vector2 _nextPos,_myPos;
	

	private bool _goingUp = true;

	private void Start () {

		_nextPos = Waypoints[0].localPosition;
	}
	
	private void Update () {
		Move();
	}

	private void Move(){
		Saw.localPosition = Vector3.MoveTowards(Saw.localPosition, _nextPos,MoveSpeed*Time.deltaTime);
		
		ChangeWaypoint();
		
	}

	private void ChangeWaypoint() {
		if (Vector2.Distance(_nextPos,Saw.localPosition)<=0.1) {
			
			if (_goingUp) {
				if (posCounter < Waypoints.Length-1) {
					posCounter++;
				}
				else {
					_goingUp = false;
					
				}
			}
			else {
				if (posCounter > 0) {
					posCounter--;
					
				}
				else {
					
					_goingUp = true;
				}
			}

			_nextPos = Waypoints[posCounter].localPosition;

		}
	}

}
