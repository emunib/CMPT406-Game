using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedItems : MonoBehaviour {

	public static CollectedItems items;

	// Use this for initialization, happens before start
	void Awake () {
		
		// if items doesn't exist, make this 
		if (items == null) {
			DontDestroyOnLoad (gameObject);
			items = this;
		} else if (items != this) {
			// Destroy this object if items already exists
			Destroy (gameObject);
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
