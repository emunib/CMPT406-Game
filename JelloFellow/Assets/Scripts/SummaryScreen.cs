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
	public Text  		collected;
	public CanvasGroup 	goldStar;
	public CanvasGroup 	silverStar;
	public CanvasGroup	bronzeStar;
	public Dictionary<string, MedalBoundary> scoreBoundaries = new Dictionary<string, MedalBoundary>();
	
	public void Start()
	{
		input = InputController.instance.GetInput();
		// If there was a previous scene write out the name of that scene
		if (GameController.instance.previousSceneName != "")
		{
			sceneName.text = GameController.instance.previousSceneName;
		}

		//if the time to complete the level was an actual time write out what the time was
		if (Timer.timeToDisplay != 0f)
		{
			timeScore.text = "Time: " + Timer.timeToDisplay.ToString("0.00");
		}
		
		//if there were collectibles on previous level
		if (GameController.instance.numCollecablesPrevScene != 0)
		{
			collected.text = "Collected: " + GameController.instance.numCollectedPrevScene + "/" + GameController.instance.numCollecablesPrevScene;
		}
		else
		{
			collected.text = "Collected:    ";
		}
		
		//if there was a previous high score write out that high score
		if(GameController.instance.highScores.highScoreDictionary.ContainsKey(GameController.instance.previousSceneName))
		{
			highScore.text = "Record: " + GameController.instance.highScores.highScoreDictionary[GameController.instance.previousSceneName];
		}

		//If there is not a specified set of boundaries to get medals for this level do checks with default values
		if (!scoreBoundaries.ContainsKey(GameController.instance.previousSceneName))
		{
			//check if the time is better than 100s if so make a bronze star appear
			if (100.00 > Timer.timeToDisplay)
			{
				bronzeStar.alpha = 1f;
				bronzeStar.GetComponentInParent<Image>().color = new Color(0.82f,0.41f,0.11f,1f);
				//check if the time is better than 50s if so make two silver stars appear
				if (50.00 > Timer.timeToDisplay)
				{
					silverStar.alpha = 1f;
					Color silverColor = new Color(0.76f,0.76f,0.76f,1f);
					bronzeStar.GetComponentInParent<Image>().color = silverColor;
					silverStar.GetComponentInParent<Image>().color = silverColor;
					//check if the time is better than 25s if so make three gold stars appear
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
		//there is a set of boundaries to get medals for this level do checks with set values
		else
		{
			//if better than the bronze boundary show a bronze star
			if (scoreBoundaries[GameController.instance.previousSceneName].boundaries[2] > Timer.timeToDisplay)
			{
				bronzeStar.alpha = 1f;
				bronzeStar.GetComponentInParent<Image>().color = new Color(0.82f, 0.41f, 0.11f, 1f);
				//if better than the silver boundary show two silver stars
				if (scoreBoundaries[GameController.instance.previousSceneName].boundaries[1] > Timer.timeToDisplay)
				{
					silverStar.alpha = 1f;
					Color silverColor = new Color(0.76f, 0.76f, 0.76f, 1f);
					bronzeStar.GetComponentInParent<Image>().color = silverColor;
					silverStar.GetComponentInParent<Image>().color = silverColor;
					//if better than the gold boundary show three gold stars
					if (scoreBoundaries[GameController.instance.previousSceneName].boundaries[0] > Timer.timeToDisplay)
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
		
		/* If selected:
		 		-check if there is a highscore saved for the played level
		 		- if there is not or the old high score is worse than the new score,
		 			add the score for the level as a highscore
		 		-save the highscore list to the binary file
		 		-click the button
		 */
		select = input.GetButton3Down();
		if (select)
		{
			if(GameController.instance.highScores.highScoreDictionary.ContainsKey(GameController.instance.previousSceneName) == false)
			{
				GameController.instance.highScores.highScoreDictionary.Add(GameController.instance.previousSceneName, "");
			}

			if (GameController.instance.highScores.highScoreDictionary[GameController.instance.previousSceneName].Equals("") ||
			    float.Parse(GameController.instance.highScores.highScoreDictionary[GameController.instance.previousSceneName]) >
			    Timer.timeToDisplay)
			{
				GameController.instance.highScores.highScoreDictionary[GameController.instance.previousSceneName] =
					"" + Timer.timeToDisplay.ToString("0.00");
			}

			GameController.instance.Save();
			button.onClick.Invoke();
		}
	}

	/* where you type out the boundaries for levels
	 
	 		Use copies of the commented out lines in the function. 
	 		Replace the three values in the first line with ther boundaries desired. In the order: {gold,silver,bronze}
	 		Add the name of the level you are setting the boundaries for between the quotes
	 */	
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
