using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class ButtonComponent : MonoBehaviour {
	private Transform parent;
	public bool set = false;
	
	[Range(5,30)]
	public int timer;
	
	private void Start() {
		parent = transform.parent;
	}
	
	private void OnTriggerEnter2D(Collider2D other) {
		
		Vector3 scale = transform.localScale;
		scale.y = .1f;

		transform.localScale = scale;

		set = true;
		parent.SendMessage("OpenDoors");
		
	}
}
