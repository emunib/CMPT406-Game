using System;
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
	public Text  		highScore;
	public CanvasGroup 	goldStar;
	public CanvasGroup 	silverStar;
	public CanvasGroup	bronzeStar;
	public Dictionary<string, MedalBoundary> scoreBoundaries = new Dictionary<string, MedalBoundary>();
	
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

		if (!scoreBoundaries.ContainsKey(GameController.control.previousSceneName))
		{
			if (100.00 > Timer.timeToDisplay)
			{
				bronzeStar.alpha = 1f;
				bronzeStar.GetComponentInParent<Image>().color = new Color(0.82f,0.41f,0.11f,1f);
				if (50.00 > Timer.timeToDisplay)
				{
					silverStar.alpha = 1f;
					Color silverColor = new Color(0.76f,0.76f,0.76f,1f);
					bronzeStar.GetComponentInParent<Image>().color = silverColor;
					silverStar.GetComponentInParent<Image>().color = silverColor;
					if (25.00 > Timer.timeToDisplay)
					{
						goldStar.alpha = 1f;
						Color goldColor = new Color(0.83f,0.68f,0.21f,1f);
						bronzeStar.GetComponentInParent<Image>().color = goldColor;
						silverStar.GetComponentInParent<Image>().color = goldColor;
						goldStar.GetComponentInParent<Image>().color = goldColor;
					}
				}
			}
		}
		else
		{
			if (scoreBoundaries[GameController.control.previousSceneName].boundaries[2] > Timer.timeToDisplay)
			{
				bronzeStar.alpha = 1f;
				bronzeStar.GetComponentInParent<Image>().color = new Color(0.82f, 0.41f, 0.11f, 1f);
				if (scoreBoundaries[GameController.control.previousSceneName].boundaries[1] > Timer.timeToDisplay)
				{
					silverStar.alpha = 1f;
					Color silverColor = new Color(0.76f, 0.76f, 0.76f, 1f);
					bronzeStar.GetComponentInParent<Image>().color = silverColor;
					silverStar.GetComponentInParent<Image>().color = silverColor;
					if (scoreBoundaries[GameController.control.previousSceneName].boundaries[0] > Timer.timeToDisplay)
					{
						goldStar.alpha = 1f;
						Color goldColor = new Color(0.83f, 0.68f, 0.21f, 1f);
						bronzeStar.GetComponentInParent<Image>().color = goldColor;
						silverStar.GetComponentInParent<Image>().color = goldColor;
						goldStar.GetComponentInParent<Image>().color = goldColor;
					}
				}
			}
		}

		SetBoundaries();

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

	public void SetBoundaries()
	{
		MedalBoundary boundary = new MedalBoundary();
//		boundary.boundaries= new float[3] {15f,30f,45f};
//		scoreBoundaries.Add("",boundary);
	}
	
	public class MedalBoundary
	{
		public float[] boundaries = new float[3];
	}
	
}
