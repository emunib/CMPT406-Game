using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Added a way to fade music.
 * Referenced from https://blog.jwiese.eu/en/2017/06/22/unity-3d-cross-fade-two-audioclips-with-two-audiosources/
 */
[RequireComponent(typeof(AudioSource))]
// Object is to be editted in the browser 
public class AudioManager : Singleton<AudioManager> {
  private const string mainMenuThemesPath = "Music/MainMenu";
  private const string levelThemesPath = "Music/InGame";
  private const float fade_duration = 0.5f;

  public AudioClip[] mainMenuThemes;
  public AudioClip[] levelThemes;
  private AudioSource[] _player;
  private IEnumerator[] fader;
  private int ActivePlayer = 0;
  public Queue<AudioClip> mainMenuClipsQ;
  private string currentSceneName;
  public Queue<AudioClip> levelClipsQ;

  private void Awake() {
    if (instance != this) Destroy(gameObject);

    mainMenuThemes = Resources.LoadAll<AudioClip>(mainMenuThemesPath);
    levelThemes = Resources.LoadAll<AudioClip>(levelThemesPath);

    MainScript.instance.OnSceneChange += SceneWasChanged;
    //Generate the two AudioSources
    _player = new[] {gameObject.AddComponent<AudioSource>(), gameObject.AddComponent<AudioSource>()};

    fader = new IEnumerator[2];

    //Set default values
    foreach (AudioSource s in _player) {
      s.loop = true;
      s.playOnAwake = false;
      s.volume = 0.0f;
    }

    currentSceneName = SceneManager.GetActiveScene().name;
    InitQueues();
    DecideAndPlayClip();
  }

  private void InitQueues() {
    mainMenuClipsQ = new Queue<AudioClip>();
    levelClipsQ = new Queue<AudioClip>();

    for (int i = 0; i < levelThemes.Length; i++) {
      levelClipsQ.Enqueue(levelThemes[i]);
    }

    for (int i = 0; i < mainMenuThemes.Length; i++) {
      mainMenuClipsQ.Enqueue(mainMenuThemes[i]);
    }
  }

  public float volumeChangesPerSecond = 10;

  private IEnumerator FadeAudioSource(AudioSource player, float duration, float targetVolume, Action finishedCallback) {
    //Calculate the steps
    int Steps = (int) (volumeChangesPerSecond * duration);
    float StepTime = duration / Steps;
    float StepSize = (targetVolume - player.volume) / Steps;

    //Fade now
    for (int i = 1; i < Steps; i++) {
      player.volume += StepSize;
      yield return new WaitForSeconds(StepTime);
    }

    //Make sure the targetVolume is set
    player.volume = targetVolume;

    //Callback
    if (finishedCallback != null) {
      finishedCallback();
    }
  }

  private void Play(AudioClip clip) {
    //Prevent fading the same clip on both players 
    if (clip == _player[ActivePlayer].clip) {
      return;
    }

    //Kill all playing
    foreach (IEnumerator i in fader) {
      if (i != null) {
        StopCoroutine(i);
      }
    }

    //Fade-out the active play, if it is not silent (eg: first start)
    if (_player[ActivePlayer].volume > 0) {
      fader[0] = FadeAudioSource(_player[ActivePlayer], fade_duration, 0.0f, () => { fader[0] = null; });
      StartCoroutine(fader[0]);
    }

    //Fade-in the new clip
    int NextPlayer = (ActivePlayer + 1) % _player.Length;
    _player[NextPlayer].clip = clip;
    _player[NextPlayer].Play();
    fader[1] = FadeAudioSource(_player[NextPlayer], fade_duration, 0.75f, () => { fader[1] = null; });
    StartCoroutine(fader[1]);

    //Register new active player
    ActivePlayer = NextPlayer;
  }

  private void DecideAndPlayClip() {
    AudioClip clipToPlay;
    if (currentSceneName == "SceneSelector" || currentSceneName == "MainMenu") {
      clipToPlay = mainMenuClipsQ.Dequeue();
      levelClipsQ.Enqueue(clipToPlay);
    } else {
      clipToPlay = levelClipsQ.Dequeue();
      levelClipsQ.Enqueue(clipToPlay);
    }

    Play(clipToPlay);
  }

  private void SceneWasChanged(string scene_name, string prev_scene) {
    currentSceneName = scene_name;
    DecideAndPlayClip();
  }
}