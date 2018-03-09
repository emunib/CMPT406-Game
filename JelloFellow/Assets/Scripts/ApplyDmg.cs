using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplyDmg : MonoBehaviour {
	private void OnTriggerEnter2D(Collider2D other) {

		if (other.gameObject.CompareTag("Player")) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		//Kill enemy
		else if (other.gameObject.CompareTag("Enemy")) {
			Destroy(other.gameObject);
		}	
		
	}

	
}
