using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class WaypointFollowV2 : MonoBehaviour {


	public Vector3[] LocalWayPoints;

	
	public float speed; 
	private Transform Saw;
	private int fromWaypointIndex = 0;

	private float percentBetweenWaypoints = 0;

	private Vector3[] GlobalWaypoints;
	// Use this for initialization
	void Start () {
		Saw = transform.Find("Saw");

		GlobalWaypoints = new Vector3[LocalWayPoints.Length];
		for (int i = 0; i < LocalWayPoints.Length; i++) {
			GlobalWaypoints[i] = LocalWayPoints[i] + transform.position;
		}

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 velocity = CalculateSawMovement();

		Saw.Translate(velocity);
		
		

	}

	private Vector3 CalculateSawMovement() {
		int nextWaypointIndex = fromWaypointIndex + 1;
		float distance = Vector3.Distance(GlobalWaypoints[fromWaypointIndex], GlobalWaypoints[nextWaypointIndex]);
		percentBetweenWaypoints += Time.deltaTime * speed/distance;

		Vector3 newPos = Vector3.Lerp(GlobalWaypoints[fromWaypointIndex], GlobalWaypoints[nextWaypointIndex],
			percentBetweenWaypoints);

		if (percentBetweenWaypoints >= 1) {
			percentBetweenWaypoints = 0;
			fromWaypointIndex++;
			if (fromWaypointIndex >=GlobalWaypoints.Length) {
				fromWaypointIndex = 0;
				System.Array.Reverse(GlobalWaypoints);
			}
		}

		return newPos - Saw.transform.position;


	}

	private void OnDrawGizmos() {
		if (LocalWayPoints != null) {
			Gizmos.color = Color.green;
			float size = .3f;

			for (int i = 0; i < LocalWayPoints.Length; i++) {
				Vector3 globalWaypointPos = LocalWayPoints[i] + transform.position;
				Gizmos.DrawLine(globalWaypointPos-Vector3.up*size, globalWaypointPos+Vector3.up*size);
				Gizmos.DrawLine(globalWaypointPos-Vector3.right*size, globalWaypointPos+Vector3.right*size);

				
			}
			
		}
	}
}
