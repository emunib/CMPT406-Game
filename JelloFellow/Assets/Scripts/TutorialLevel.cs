using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		StartCoroutine("Jump");
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

	private void Jump()
	{
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
