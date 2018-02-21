using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class SummaryScreen : MonoBehaviour 
{
	public Button		button;
	private bool 		select;
	private Input2D 	input;
	public Text 		sceneName;
	public Text  		timeScore;


	public void Start()
	{
		input = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>().GetInput();
		if (SceneButton.previousSceneName != null)
		{
			sceneName.text = SceneButton.previousSceneName;
		}

		if (Timer.timeToDisplay != null)
		{
			timeScore.text = "" + Timer.timeToDisplay;
		}

	}

	public void Update(){
		
		//If selected click the button
		select = input.GetButton3Down();
		if (select)
		{
			button.onClick.Invoke();
		}
	}
}
