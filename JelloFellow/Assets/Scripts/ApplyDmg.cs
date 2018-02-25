using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplyDmg : MonoBehaviour {
	public int damageAmt;
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("SlimeNode")) {
			GameObject.Find("Center").SendMessage ("Damage",damageAmt);
			Debug.Log (name + "Damaged something");
		} else if (other.gameObject.CompareTag("Enemy")) {
			Destroy(other.gameObject);
		}
	}
}