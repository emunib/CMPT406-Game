using UnityEngine;
using UnityEngine.SceneManagement;

public class FellowPlayer : GenericPlayer {
	private const string splatsound_path = "Sounds/Splat";
	private const string deathsound_path = "Sounds/player_death";
	private const string gravityfieldsound_path = "Sounds/gravity_field";
	private const float highsplat_velocity = 25f;
	private const float lowsplat_velocity = 10f;
	
	private Input2D _input;
	private UnityJellySprite _jelly;
	private bool _deserves_lowsplat;
	private bool _deserves_highsplat;
	private bool _splat_cooldown;
	private bool _gravity_sound;

	private AudioSource _audio_source;
	private AudioClip _splat_sound;
	private AudioClip _death_sound;
	private AudioClip _gravityfield_sound;

	public Timer _timer;
	
	protected override void Start() {
		base.Start();
		
		_input = InputController.instance.GetInput();
		SetIgnoreFields(true);
		SetInput(_input);
		SetFieldRadius(2f);
		
		_jelly = GetComponent<JellySpriteReferencePoint>().ParentJellySprite.GetComponent<UnityJellySprite>();
		_audio_source = gameObject.AddComponent<AudioSource>();
		_audio_source.playOnAwake = false;

		_splat_sound = Resources.Load<AudioClip>(splatsound_path);
		_death_sound = Resources.Load<AudioClip>(deathsound_path);
		_gravityfield_sound = Resources.Load<AudioClip>(gravityfieldsound_path);
		
		_deserves_highsplat = false;
		_deserves_lowsplat = false;
		_splat_cooldown = false;
		
		GameObject timer_object = GameObject.Find("Timer");
		if (!timer_object) {
			Debug.LogError("Timer not present in the scene.");
		} else {
			_timer = timer_object.GetComponentInChildren<Timer>();
		}
		
		GameObject spawn_point = GameObject.Find("SpawnPoint");
		if (!spawn_point) {
			Debug.LogError("Spawn point not present in the scene.");
		} else {
			Vector2 dir = Quaternion.AngleAxis(spawn_point.transform.eulerAngles.z, Vector3.forward) * Vector3.down;
			SetGravity(dir);
		}
	}

	protected override void Update() {
		/* spawn spin */
		if (_jelly.gameObject.transform.localScale != (Vector3) Vector2.one) {
			float angle = _jelly.gameObject.transform.rotation.eulerAngles.z == 0f ? 360f : _jelly.gameObject.transform.rotation.eulerAngles.z;
			_jelly.gameObject.transform.rotation = Quaternion.Slerp(_jelly.gameObject.transform.rotation, Quaternion.Euler(0,0,_jelly.gameObject.transform.localScale.x * angle), 1f);
		} else {
			if(!_timer.Activate) _timer.Activate = true;
		  /* allow movement after we are done spawning */
			base.Update();

			/* we are allowed to move so grab velocity */
			Vector2 velocity = rigidbody.velocity;

			if (_input.GetHorizontalRightStick() != 0f || _input.GetVerticalRightStick() != 0f) {
				if (!_gravity_sound) {
					_gravity_sound = true;
					_audio_source.PlayOneShot(_gravityfield_sound, 1f);
				}
			} else {
				_gravity_sound = false;
			}
			
			/* make sure we haven't already decided that the player deserves a splat */
			if (!_deserves_highsplat && !_deserves_lowsplat && !is_grounded && !_splat_cooldown) {
				/* if velocity in either direction is greater then the low splat threshold */
				if (Mathf.Abs(velocity.x) >= lowsplat_velocity || Mathf.Abs(velocity.y) >= lowsplat_velocity) {
					/* if velocity in either direction is greater then the high splat threshold */
					if (Mathf.Abs(velocity.x) >= highsplat_velocity || Mathf.Abs(velocity.y) >= highsplat_velocity) {
						_deserves_highsplat = true;
					} else {
						_deserves_lowsplat = true;
					}

					_splat_cooldown = true;
				}
			} else {
				/* player deserves a splat so play the sound when we hit the ground */
				if (is_grounded && _splat_cooldown) {
					if (_deserves_highsplat) {
						_audio_source.PlayOneShot(_splat_sound, 1f);
						_deserves_highsplat = false;
						Invoke("ResetSplatCooldown", 0.5f);
					}

					if (_deserves_lowsplat) {
						_audio_source.PlayOneShot(_splat_sound, Random.Range(0f, 0.1f));
						_deserves_lowsplat = false;
						Invoke("ResetSplatCooldown", 0.5f);
					}
				}
			}
		}
	}

	private void ResetSplatCooldown() {
		_splat_cooldown = false;
	}
	
	protected override void Death() {
		_timer.Activate = false;
		_audio_source.PlayOneShot(_death_sound, Random.Range(0.5f, 1f));
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
