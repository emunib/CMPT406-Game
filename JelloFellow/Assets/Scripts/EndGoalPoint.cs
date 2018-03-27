using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class EndGoalPoint : MonoBehaviour
{
    private const string completesound_path = "Sounds/level_complete";
//    private GameObject slime;
//
//    private void Start()
//    {
//        slime = GameObject.Find("SlimePlayer");
//    }
    
    private AudioSource _audio_source;
    private AudioClip _complete_sound;
    private bool _playing;
    
    void OnTriggerEnter2D(Collider2D hit) {
		Debug.Log ("Entered");
		if (hit.gameObject.CompareTag("Player"))
        {
            FellowPlayer _player = hit.gameObject.transform.parent.GetComponentInChildren<FellowPlayer>();
            _player._timer.Stop = false;
            _player.Pause = true;
            if ((_audio_source = GetComponent<AudioSource>()) != null && !_playing) {
                _audio_source.Stop();
                _complete_sound = Resources.Load<AudioClip>(completesound_path);
                _audio_source.PlayOneShot(_complete_sound, 0.6f);
                _playing = true;
            }
            Invoke("LevelSummary", 1.5f);
        }
    }

    private void LevelSummary() {
        _playing = false;
        GameController.instance.previousSceneName = SceneManager.GetActiveScene().name;
        GameController.instance.currSceneName = "LevelSummary";
        SceneLoader.instance.LoadSceneWithName("LevelSummary");
    }
    
}
