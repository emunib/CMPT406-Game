using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class WaypointFollowV2 : MonoBehaviour {


	[Header("Waypoints")]
	public Vector3[] LocalWayPoints;
	[Tooltip("If true, waypoints act in circular fashion. ie 0,1,2,3,0,1,2,3,0....")]
	public bool CircularWaypoints;
	public bool DebugWaypoints;
	public bool PositionInfo;
	[Header("Stats")]
	public float SpinRatee;
	[Range(0,10)]
	public float Speed;

	
	
	private Transform Saw;
	
	private int fromWaypointIndex = 0;

	private float percentBetweenWaypoints = 0;

	//private Vector3[] GlobalWaypoints;

	private Vector2 _nextPos,_myPos;
	
	
	
	private bool _goingUp = true;
	private int posCounter;

	// Use this for initialization
	void Start () {
		Saw = transform.Find("Saw");

		posCounter = 0;
		
		/*
		GlobalWaypoints = new Vector3[LocalWayPoints.Length];
		for (int i = 0; i < LocalWayPoints.Length; i++) {
			GlobalWaypoints[i] = LocalWayPoints[i] + transform.position;
		}*/

	}
	
	// Update is called once per frame
	void Update () {
		
		MoveSaw();
		Spin();
		
		
		/*Vector3 velocity = CalculateSawMoveSawment();
		Saw.Translate(velocity);
		Vector3 rotation = Saw.transform.localEulerAngles;
		Saw.transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z + 10);
		*/

		
	}
	
	/// <summary>
	/// MoveSaws the saw to the next position
	/// </summary>
	private void MoveSaw() {
		Saw.localPosition= Vector3.MoveTowards(Saw.localPosition, _nextPos,Speed*Time.deltaTime);
		
		ChangeWaypoint();
	}

	private void Spin() {
		Saw.transform.Rotate (0,0,-SpinRatee);

	}
	
	
	/// <summary>
	/// Changes the waypointn to the next waypoint. If curcularwaypoints set to true go through waypoints array in
	/// circular fashion
	/// </summary>
	private void ChangeWaypoint() {
		if (Vector2.Distance(_nextPos, Saw.localPosition) <= 0.1) {

			if (_goingUp) {
				if (posCounter < LocalWayPoints.Length - 1) {
					posCounter++;
					
					
				}
				else {

					if (CircularWaypoints) {
						posCounter = 0;
					}
					else {
						_goingUp = false;

					}

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

			_nextPos = LocalWayPoints[posCounter];

		}
	}
	/*
	private Vector3 CalculateSawMoveSawment() {
		int nextWaypointIndex = fromWaypointIndex + 1;
		float distance = Vector3.Distance(GlobalWaypoints[fromWaypointIndex], GlobalWaypoints[nextWaypointIndex]);
		percentBetweenWaypoints += Time.deltaTime * Speed/distance;

		Vector3 newPos = Vector3.Lerp(GlobalWaypoints[fromWaypointIndex], GlobalWaypoints[nextWaypointIndex],
			percentBetweenWaypoints);

		if (percentBetweenWaypoints >= 1) {
			percentBetweenWaypoints = 0;
			fromWaypointIndex++;
			if (fromWaypointIndex >=GlobalWaypoints.Length-1) {
				fromWaypointIndex = 0;
				System.Array.Reverse(GlobalWaypoints);
			}
		}

		return newPos - Saw.transform.position;


	}*/
	
	
	/// <summary>
	/// To make visualization of waypoints easier. turndrawgizmos off in 
	/// </summary>
	private void OnDrawGizmos() {
		
		
		if (DebugWaypoints &&LocalWayPoints != null) {
			Gizmos.color = Color.green;
			float size = .3f;

			for (int i = 0; i < LocalWayPoints.Length; i++) {
				Vector3 globalWaypointPos = LocalWayPoints[i] + transform.position;
				Gizmos.DrawLine(globalWaypointPos-Vector3.up*size, globalWaypointPos+Vector3.up*size);
				Gizmos.DrawLine(globalWaypointPos-Vector3.right*size, globalWaypointPos+Vector3.right*size);

				if (PositionInfo) {
					Handles.Label(LocalWayPoints[i] + transform.position,
						"Element " + i.ToString() + "\nLocalCoordinates " + LocalWayPoints[i].ToString());
				}

			}
			
		}
	}
}
