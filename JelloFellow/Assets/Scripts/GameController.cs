using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController> {

	public string previousSceneName = "";
	public string currSceneName = "";
	public int numCollecablesPrevScene = 0;
	public int numCollectedPrevScene = 0;
	public HighScores highScores = new HighScores();
	private Input2D 	input;
	
	// Use this for initialization
	void Awake () {
		if(instance != this) Destroy(gameObject);
		
		input = InputController.instance.GetInput();
		
		currSceneName = SceneManager.GetActiveScene().name;
		Load();
	}
	
	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/highScores.dat");
        
		
		if(!instance.highScores.highScoreDictionary.ContainsKey(instance.currSceneName))
		{
			instance.highScores.highScoreDictionary.Add(instance.currSceneName, "");
		}
		instance.highScores.highScoreDictionary[instance.currSceneName] = "" + Timer.timeToDisplay;
		
		HighScores scores = new HighScores();
		scores = instance.highScores;
		
		bf.Serialize(file,scores);
		file.Close();
	}

	public void Load()
	{
		if (File.Exists(Application.persistentDataPath + "/highScores.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/highScores.dat", FileMode.Open);
			HighScores scores = (HighScores)bf.Deserialize(file);

			instance.highScores = scores;
		}
	}
	
	[Serializable]
	public class HighScores
	{
		public Dictionary<String, String> highScoreDictionary = new Dictionary<string, string>();
	}
	
	void Update () {
		if (input.GetStartButtonDown() && currSceneName != "MainMenu" && 
		    currSceneName != "SceneSelector" && currSceneName != "LevelSummary")
		{
			instance.previousSceneName = SceneManager.GetActiveScene().name;
			instance.currSceneName = "MainMenu";
			SceneLoader.instance.LoadSceneWithName("MainMenu");
		}
	}
}
