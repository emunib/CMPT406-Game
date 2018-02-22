using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;


public class ButtonComponent : MonoBehaviour {
	private Transform parent;
	public bool set = false;
	
	[Range(5,30)]
	public int timer;

	[Tooltip("The game object that will will handle what happens as this button is pressed")]
	public GameObject HandlerObject;
	[Tooltip("Put in the name of the function that this button triggers. Is Case Sensitive")]
	public string HandlerFunction;
	
	
	private void OnTriggerEnter2D(Collider2D other) {

		if (!set) {
			Vector3 scale = transform.localScale;
			scale.y = .1f;

			transform.localScale = scale;


			HandlerObject.SendMessage(HandlerFunction);
			set = true;
		}

	}
}
