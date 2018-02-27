using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplyDmg : MonoBehaviour {
	//public LayerMask playerLayer = -1;

	public string playerLayer = "SlimeEffector";

	private void OnTriggerEnter2D(Collider2D other) {
/*
		if (playerLayer == (playerLayer| (1<<other.gameObject.layer))) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}*/
		if (playerLayer == LayerMask.LayerToName(other.gameObject.layer)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);

		}
		
		//Kill enemy
		else if (other.gameObject.CompareTag("Enemy")) {
			Destroy(other.gameObject);
		}	
		
	}

	
}
