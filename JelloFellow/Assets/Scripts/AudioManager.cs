using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(AudioSource))]
// Object is to be editted in the browser 
public class AudioManager : Singleton<AudioManager> {
	private const string mainMenuThemesPath = "Music/MainMenu";
	private const string levelThemesPath = "Music/InGame";
	
	public AudioClip[]  mainMenuThemes;
	public AudioClip[] levelThemes;
	private AudioSource musicSource;
	public Scene lastScene;
	public Queue<AudioClip> mainMenuClipsQ;
	

	public Queue<AudioClip> levelClipsQ;
	// Use this for initialization
	void Awake () {
		if(instance != this) Destroy(gameObject);

		mainMenuThemes = Resources.LoadAll<AudioClip>(mainMenuThemesPath);
		levelThemes = Resources.LoadAll<AudioClip>(levelThemesPath);
		
		musicSource = gameObject.GetComponent<AudioSource>();
		if (!musicSource) musicSource = gameObject.AddComponent<AudioSource>();
		musicSource.volume = 0.75f;
		musicSource.loop = true;
		
		InitQueues();
		DecideAndPlayClip();
		//lastScene = SceneManager.GetActiveScene();
	}

	void InitQueues()
	{
		mainMenuClipsQ = new Queue<AudioClip>();
		levelClipsQ= new Queue<AudioClip>();
		
		for(int i=0;i<levelThemes.Length;i++){
		  levelClipsQ.Enqueue(levelThemes[i]);
		}
		
		for(int i=0;i<mainMenuThemes.Length;i++){
			mainMenuClipsQ.Enqueue(mainMenuThemes[i]);
		}
	}
	//If the audio stops or 
	void DecideAndPlayClip()
	{
		Scene currentScene = SceneManager.GetActiveScene();
		lastScene = currentScene;

		AudioClip clipToPlay;
		if (currentScene.name == "SceneSelector" || currentScene.name == "MainMenu")
		{
			
			//Debug.Log("Playing Main Menu Theme");
			clipToPlay= mainMenuClipsQ.Dequeue();
			//Debug.Log("Playing the next tune in the mainMenuClips queue : " + clipToPlay.name);

			levelClipsQ.Enqueue(clipToPlay);
		}
		else
		{
			
			//
			//int rand = (int) Random.Range(0, levelThemes.Length);
			//clipToPlay = levelThemes[rand];
			clipToPlay = levelClipsQ.Dequeue();
			//Debug.Log("Playing the next tune in the levelClips queue : " + clipToPlay.name);
			levelClipsQ.Enqueue(clipToPlay);
		}

		musicSource.clip = clipToPlay;
		musicSource.Play();

	}
	// Update is called once per frame
	void LateUpdate () {
		if (!musicSource.isPlaying || lastScene.name != SceneManager.GetActiveScene().name)
		{
			//Debug.Log("Scene name last : " + lastScene.name);
			//Debug.Log("Scene name now  : " + SceneManager.GetActiveScene().name);
			DecideAndPlayClip();
			
		}
	}
}
