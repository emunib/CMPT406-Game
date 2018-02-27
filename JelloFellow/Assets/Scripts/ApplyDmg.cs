using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplyDmg : MonoBehaviour {
<<<<<<< HEAD
	public LayerMask playerLayer = -1;
	

	private void OnTriggerEnter2D(Collider2D other) {

		if (other.gameObject.CompareTag("Blob")) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
=======

	public int damageAmt;
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("SlimeNode")) {
			other.gameObject.SendMessage ("Damage",damageAmt);
			Debug.Log (name + "Damaged something");
		} else if (other.gameObject.CompareTag("Enemy")) {
			Destroy(other.gameObject);
>>>>>>> master
		}
		//Kill enemy
		else if (other.gameObject.CompareTag("Enemy")) {
			Destroy(other.gameObject);
		}	
		
	}

	
}
