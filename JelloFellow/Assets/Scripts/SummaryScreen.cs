using System;
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
	public Text  		highScore;
	


	public void Start()
	{
		input = InputController.instance.GetInput();
		if (GameController.control.previousSceneName != "")
		{
			sceneName.text = GameController.control.previousSceneName;
		}

		if (Timer.timeToDisplay != 0f)
		{
			timeScore.text = "Time: " + Timer.timeToDisplay;
		}
		if(GameController.control.highScores.highScoreDictionary.ContainsKey(GameController.control.previousSceneName))
		{
			highScore.text = GameController.control.highScores.highScoreDictionary[GameController.control.previousSceneName];
		}

	}

	public void Update(){
		
		//If selected click the button
		select = input.GetButton3Down();
		if (select)
		{
			if(GameController.control.highScores.highScoreDictionary.ContainsKey(GameController.control.previousSceneName) == false)
			{
				GameController.control.highScores.highScoreDictionary.Add(GameController.control.previousSceneName, "");
			}

			if (float.Parse(GameController.control.highScores.highScoreDictionary[GameController.control.previousSceneName]) >
			    Timer.timeToDisplay)
			{
				GameController.control.highScores.highScoreDictionary[GameController.control.previousSceneName] =
					"" + Timer.timeToDisplay;
			}

			GameController.control.Save();
			button.onClick.Invoke();
		}
	}
	
}
