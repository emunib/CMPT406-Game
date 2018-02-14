using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trackSawspin : MonoBehaviour {

	public float spin;

	void Start(){
	}
	// Update is called once per frame
	void Update () {
		transform.Rotate (0,0,spin);
	}
}
