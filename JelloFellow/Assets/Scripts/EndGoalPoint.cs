using UnityEngine;

public class EndGoalPoint : MonoBehaviour {
  private const string completesound_path = "Sounds/level_complete";

  private AudioSource _audio_source;
  private AudioClip _complete_sound;
  private bool _playing;
  private Timer _timer;
  private bool _endgoal_hit;

  private void Awake() {
    _endgoal_hit = false;
  }

  private void OnTriggerEnter2D(Collider2D hit) {
    if (hit.gameObject.CompareTag("Player") && !_endgoal_hit) {
      FellowPlayer _player = hit.gameObject.transform.parent.GetComponentInChildren<FellowPlayer>();
      _player._timer.Stop = false;
      _player.Pause = true;
      if ((_audio_source = GetComponent<AudioSource>()) != null && !_playing) {
        _audio_source.Stop();
        _complete_sound = Resources.Load<AudioClip>(completesound_path);
        _audio_source.PlayOneShot(_complete_sound, 0.6f);
        _playing = true;
      }

      /* disable collider for jello so he doesn't die after hitting goal */
      _player.transform.parent.gameObject.SetActive(false);
      _timer = _player._timer;
      Invoke("LevelSummary", 1.5f);
      _endgoal_hit = true;
    }
  }

  private void LevelSummary() {
    _playing = false;
    _endgoal_hit = false;
    MainScript.instance.LoadSummary(_timer.getTime());
  }
}