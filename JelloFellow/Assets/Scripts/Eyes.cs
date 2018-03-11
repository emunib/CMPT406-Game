using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour {
	private const float MinWait = 0.5f;
	private const float MaxWait = 4f;
	private const float BlinkDuration = 0.2f;
	
	[CustomLabel("Eyes Opened")] [Tooltip("Reference to the opened eyes gameobject")] [SerializeField]
	private GameObject opened_eyes;
	
	[CustomLabel("Eyes Closed")] [Tooltip("Reference to the closed eyes gameobject")] [SerializeField]
	private GameObject closed_eyes;

	private void Awake() {
		closed_eyes.SetActive(false);
		opened_eyes.SetActive(true);
		StartCoroutine(Blink());
	}

	private IEnumerator Blink() {
		while (true) {
			closed_eyes.SetActive(true);
			opened_eyes.SetActive(false);
			yield return new WaitForSeconds(BlinkDuration);
			closed_eyes.SetActive(false);
			opened_eyes.SetActive(true);
			
			yield return new WaitForSeconds(Random.Range(MinWait, MaxWait));
		}
	}
}
