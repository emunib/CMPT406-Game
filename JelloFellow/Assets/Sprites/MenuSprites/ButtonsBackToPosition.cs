using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsBackToPosition : MonoBehaviour {

	public GameObject Position;
	public GameObject Button;
	public float speed;
	private Input2D input;


	void Start(){
		//save first location
		Position.transform.position = Button.transform.position;
		//read Input from controller
		input = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>().GetInput();

	}

	void Update() {
		float hor_m = input.GetHorizontalLeftStick();
		float ver_m = input.GetVerticalLeftStick();

		if (Mathf.Abs (hor_m) > 0 || Mathf.Abs (ver_m) > 0 ){
			StartCoroutine(ReturnToPosition());
	}
	}
	//Transition Back to First Postion		
	IEnumerator ReturnToPosition() {

		while(Button.transform.position != Position.transform.position) {

			Button.transform.position = Vector3.MoveTowards(Button.transform.position, Position.transform.position, Time.deltaTime * speed);
			yield return null;
		}
	}
}
