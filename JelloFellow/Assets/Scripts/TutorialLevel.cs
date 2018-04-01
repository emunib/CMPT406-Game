using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class TutorialLevel : MonoBehaviour
{

	private bool first_tut_complete;
	private bool second_tut_complete;
	private bool third_tut_complete;
	private bool fourth_tut_complete;
	private bool fifth_tut_complete;
	
	
	// Use this for initialization
	void Start ()
	{
		first_tut_complete = false;
		second_tut_complete = false;
		third_tut_complete = false;
		fourth_tut_complete = false;
		fifth_tut_complete = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (transform.position.x > 20 && !first_tut_complete)
		{
			Time.timeScale = 0;
			FirstTutorial();
			Time.timeScale = 1;
			first_tut_complete = true;
		}
		
		if (transform.position.x > 80 && !second_tut_complete)
		{
			Time.timeScale = 0;
			SecondTutorial();
			Time.timeScale = 1;
			second_tut_complete = true;
		}
		
		if (transform.position.x > 170 && !third_tut_complete)
		{
			Time.timeScale = 0;
			ThirdTutorial();
			Time.timeScale = 1;
			third_tut_complete = true;
		}
		
		if (transform.position.x > 220 && !fourth_tut_complete)
		{
			Time.timeScale = 0;
			FourthTutorial();
			Time.timeScale = 1;
			fourth_tut_complete = true;
		}

		if (transform.position.x > 280 && !fifth_tut_complete)
		{
			Time.timeScale = 0;
			FifthTutorial();
			Time.timeScale = 1;
			fifth_tut_complete = true;
		}
	}


	private void FirstTutorial()
	{
		// Display prompt for minimum of 5 seconds
		StartCoroutine(Jump());
	}
	
	private void SecondTutorial()
	{
		StartCoroutine("ChangeGravity");
	}
	
	private void ThirdTutorial()
	{
		StartCoroutine("ExpandGravField");
	}
	
	private void FourthTutorial()
	{
		StartCoroutine("Cling");
	}

	private void FifthTutorial()
	{
		StartCoroutine("Explore");
	}

	private IEnumerator Jump() {
		PostProcessingBehaviour effects = Camera.main.GetComponent<PostProcessingBehaviour>();
		effects.profile.depthOfField.enabled = true;

		UnityJellySprite _jelly = GetComponent<UnityJellySprite>();
		_jelly.CentralPoint.GameObject.GetComponent<GenericPlayer>().AllowAnyMovement = false;
		
		Input2D _input = InputController.instance.GetInput();
		bool jump_button1 = false;
		bool jump_button2 = false;
		while (!jump_button1 && !jump_button2) {
			jump_button1 = _input.GetButton3Down();
			jump_button2 = _input.GetRightBumperDown();
			
			yield return null;
		}

		_jelly.CentralPoint.GameObject.GetComponent<GenericPlayer>().AllowAnyMovement = true;
		yield return null;
		if (_input.GetType() == typeof(SimpleInput)) {
			((SimpleInput)_input).SetButton3Down(true);
		}
		
		effects.profile.depthOfField.enabled = false;
		// while not pressing "x" (Jump) then constantly loop
		// break when pressed and exit coroutine

		// Be sure to actually jump
	}

	private void ChangeGravity()
	{
		// while not changing gravity upwards (Left stick) then constantly loop
		// break when pressed and exit coroutine
		
		// Be sure to actually change grav
	}

	private void ExpandGravField()
	{
		// while not expanding (R2) then constantly loop
		// break when pressed and exit coroutine
		
		// Be sure to actually expanding grav field
	}

	private void Cling()
	{
		// while not pressing towards the platform (should be coned) then constantly loop
		// break when pressed and exit coroutine
		
		// Be sure to actually cling
	}

	private void Explore()
	{
		// Prompt to explore the rest of the map (practice on different platforms, change gravity, cling, throw objects, etc)
		// break when "x" is pressed
	}
	
	
}
