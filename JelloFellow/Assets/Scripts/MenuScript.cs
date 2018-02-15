using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	public AudioSource backgroundMusic;	//Background Music For Menu

	void Start () { 
		backgroundMusic = GetComponent<AudioSource>();
		backgroundMusic.Play ();
	}
	// Start game from level 1
	public void PlayGame () {
		SceneManager.LoadScene(1);

	}
	//Level Selector Button
	public void Levels(){
		SceneManager.LoadScene("SceneSelector");
	}
		
	//Options Button
	public void Options(){
		//SceneManager.LoadScene(1);
	}

	//Exit the game
	public void Quit () {
		Application.Quit();	
	}
}