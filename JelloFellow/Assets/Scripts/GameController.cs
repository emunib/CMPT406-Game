using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public string previousSceneName = "";
	public string currSceneName = "";
	public static GameController control;
	public HighScores highScores = new HighScores();
	
	// Use this for initialization
	void Awake () {
		if (control == null)
		{
			DontDestroyOnLoad(gameObject);
			control = this;
		}
		else if(control != this)
		{
			Destroy(gameObject);
		}

		currSceneName = SceneManager.GetActiveScene().name;
		Load();
	}
	
	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/highScores.dat");
        
		
		if(!control.highScores.highScoreDictionary.ContainsKey(control.currSceneName))
		{
			control.highScores.highScoreDictionary.Add(control.currSceneName, "");
		}
		control.highScores.highScoreDictionary[control.currSceneName] = "" + Timer.timeToDisplay;
		
		HighScores scores = new HighScores();
		scores = control.highScores;
		
		bf.Serialize(file,scores);
		file.Close();
	}

	public void Load()
	{
		if (File.Exists(Application.persistentDataPath + "/highScores.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/highScores.dat",FileMode.Open);
			HighScores scores = (HighScores)bf.Deserialize(file);

			control.highScores = scores;
		}
	}
	
	[Serializable]
	public class HighScores
	{
		public Dictionary<String, String> highScoreDictionary = new Dictionary<string, string>();
	}
}
