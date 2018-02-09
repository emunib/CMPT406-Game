using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplyDmg : MonoBehaviour {
	private void OnCollisionEnter2D(Collision2D other) {


		if (other.gameObject.CompareTag("Player")) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);

		}
		else if (other.gameObject.CompareTag("Enemy")) {
			Destroy(other.gameObject);
		}
		
	}
}
