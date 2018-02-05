using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
	public Transform Target;
	
	// Update is called once per frame
	void LateUpdate () {
		Camera.main.transform.position = new Vector3(Target.position.x, Target.position.y, -10);
	}
}
