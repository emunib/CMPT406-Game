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
	public CanvasGroup 	goldStar;
	public CanvasGroup 	silverStar;
	public CanvasGroup	bronzeStar;
	
	


	public void Start()
	{
		input = InputController.instance.GetInput();
		if (GameController.control.previousSceneName != "")
		{
			sceneName.text = GameController.control.previousSceneName;
		}

		if (Timer.timeToDisplay != 0f)
		{
			timeScore.text = "Time: " + Timer.timeToDisplay.ToString("0.00");
		}
		if(GameController.control.highScores.highScoreDictionary.ContainsKey(GameController.control.previousSceneName))
		{
			highScore.text = "Record: " + GameController.control.highScores.highScoreDictionary[GameController.control.previousSceneName];
		}

		if (100.00 > Timer.timeToDisplay)
		{
			bronzeStar.alpha = 1f;
			bronzeStar.GetComponentInParent<Image>().color = Color.green;
			if (40.00 > Timer.timeToDisplay)
			{
				silverStar.alpha = 1f;
				bronzeStar.GetComponentInParent<Image>().color = Color.gray;
				silverStar.GetComponentInParent<Image>().color = Color.gray;
				if (15.00 > Timer.timeToDisplay)
				{
					goldStar.alpha = 1f;
					bronzeStar.GetComponentInParent<Image>().color = Color.yellow;
					silverStar.GetComponentInParent<Image>().color = Color.yellow;
					goldStar.GetComponentInParent<Image>().color = Color.yellow;
				}
			}
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

			if (GameController.control.highScores.highScoreDictionary[GameController.control.previousSceneName].Equals("") ||
			    float.Parse(GameController.control.highScores.highScoreDictionary[GameController.control.previousSceneName]) >
			    Timer.timeToDisplay)
			{
				GameController.control.highScores.highScoreDictionary[GameController.control.previousSceneName] =
					"" + Timer.timeToDisplay.ToString("0.00");
			}

			GameController.control.Save();
			button.onClick.Invoke();
		}
	}
	
}
