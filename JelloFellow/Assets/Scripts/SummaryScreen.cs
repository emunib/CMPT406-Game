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
		input = InputController.instance.GetInput();
		if (SceneController.control.previousSceneName != "")
		{
			sceneName.text = SceneController.control.previousSceneName;
		}

		if (Timer.timeToDisplay != 0f)
		{
			timeScore.text = "Time: " + Timer.timeToDisplay;
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
