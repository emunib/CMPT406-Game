using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZGooScript : MonoBehaviour {

	public string playerLayer = "SlimeEffector";
	public string gooLayer = "Goo";

	private void Start() {
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(playerLayer),LayerMask.NameToLayer(gooLayer));
		
	}

	
	
	
}
