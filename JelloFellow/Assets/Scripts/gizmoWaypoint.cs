using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class gizmoWaypoint : MonoBehaviour {
	private void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(transform.position,.2F);
	}
}
