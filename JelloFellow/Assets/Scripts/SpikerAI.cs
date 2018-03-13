using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikerAI : GenericPlayer {
	private const string player_tag = "Player";
	private const float rotation_speed = 1f;
	private const float explode_time = 2f;
	
	private GenericEnemyInput _input;
	private UnityJellySprite jelly;
	private bool flip;
	private bool do_once;
	private bool stop_movement;
	private bool grounded_activated;
	private SpikerProjectile[] projectiles;
	private Color original_color;
	private Coroutine fuse_coroutine;
	
	protected override void Start() {
		base.Start();
		
		/* set default generic player stuff */
		_input = gameObject.AddComponent<GenericEnemyInput>();
		SetIgnoreFields(false);
		SetInput(_input);
		SetFieldRadius(4f);
		
		/* default jelly and AI stuff */
		jelly = GetComponent<JellySpriteReferencePoint>().ParentJellySprite.GetComponent<UnityJellySprite>();
		flip = jelly.m_FlipX;
		do_once = true;
		stop_movement = false;
		grounded_activated = false;
		projectiles = jelly.gameObject.GetComponentsInChildren<SpikerProjectile>();
		original_color = jelly.gameObject.GetComponent<Renderer>().material.color;

		foreach (SpikerProjectile projectile in projectiles) {
			projectile.gameObject.SetActive(false);
		}
	}

	protected override void Update() {
		/* reset all the values */
		_input.DefaultValues();
		
		/* only do it once */
		if (do_once) {
			/* change gravity direction to be point down of the sprite */
			_input.rightstickx = -transform.up.x;
			_input.rightsticky = -transform.up.y;
			
			do_once = false;
		}

		/* spawn spin */
		if (jelly.gameObject.transform.localScale != (Vector3) Vector2.one) {
			float angle = jelly.gameObject.transform.rotation.eulerAngles.z == 0f ? 360f : jelly.gameObject.transform.rotation.eulerAngles.z;
			jelly.gameObject.transform.rotation = Quaternion.Slerp(jelly.gameObject.transform.rotation, Quaternion.Euler(0,0,jelly.gameObject.transform.localScale.x * angle), 1f);
		} else {
			if (!is_grounded) {
				/* call handle movement after some time so we give the AI time to settle */
				if (!grounded_activated) {
					//Invoke("HandleNotGrounded", 4f);
					grounded_activated = true;
				}
			}
		}
		
		if (!stop_movement && is_grounded) {
			/* move in the direction of platform */
			float platform_walk_angle = platform_angle - 90;
			Vector2 movement_direction = new Vector2(Mathf.Sin(platform_walk_angle * Mathf.Deg2Rad), Mathf.Cos(platform_walk_angle * Mathf.Deg2Rad));

			int direction = flip ? -1 : 1;
			/* use the left control stick to move in direction */
			_input.leftstickx = movement_direction.x * direction;
			_input.leftsticky = movement_direction.y * direction;

			_input.rightstickclick_down = true;
			
			/* get the walking stick angle and if we leave the ground then handle that */
			float angle1 = platform_angle - 112f;
			float angle2 = platform_angle + 112f;
			float angle = flip ? Mathf.Max(angle1, angle2) : Mathf.Min(angle1, angle2);

			Vector2 forwardangle_direction = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
			HashSet<RaycastHit2D> leaving_ground = GetObjectsInView(forwardangle_direction, 1f, 0, 6f);
			if (leaving_ground.Count <= 0) {
				HandleLeavingGround();
			} else {
				foreach (RaycastHit2D hit in leaving_ground) {
					Vector2 hit_normal = hit.normal;
					/* if the object has children then use the parent's rotation to calculate the normal */
					if (hit.collider.gameObject.transform.childCount > 0) {
						hit_normal = Quaternion.AngleAxis(hit.collider.gameObject.transform.rotation.eulerAngles.z, Vector3.forward) * hit.normal;
					}

					/* get platform information we just hit */
					float platform_angle_update = GetAngle(hit_normal.y, hit_normal.x);
					transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, platform_angle_update != 0f ? platform_angle_update - 90 : 0f), Time.deltaTime * rotation_speed);
					break;
				}
			}
			
			HashSet<RaycastHit2D> forward_check = GetObjectsInView(flip ? transform.right : -transform.right, 1f, 0, 3.8f);
			if(forward_check.Count > 0) {
				HandleOtherInFront();
			}
			
			HashSet<RaycastHit2D> threat_check = GetObjectsInView(flip ? transform.right : -transform.right, 1f, 0, 6f, true);
			foreach (RaycastHit2D hit in threat_check) {
				/* player in front */
				if (hit.transform.CompareTag("Threat")) {
					HandleOtherInFront();
					break;
				}
			}
		}
		
		/* this allows us to have higher range to check for player */
		HashSet<RaycastHit2D> player_check = GetObjectsInView(flip ? transform.right : -transform.right, 45f, 6, 6f);
		foreach (RaycastHit2D hit in player_check) {
			/* player in front */
			if (hit.transform.CompareTag(player_tag)) {
				HandlePlayerInFront(hit.transform.gameObject);
				break;
			}
		}
		
		/* call this to run Update in the subclass */
		/* we call update after is because we want to change the input then call the update to handle the input changes
		   in the same frame rather to have to wait another frame */
		base.Update();
	}

	/// <summary>
	/// Should occur when this AI is thrown, landed on his head, or simply is not grounded
	/// for any reason.
	/// </summary>
	private void HandleNotGrounded() {
		/* just making sure that after some time it didn't just correct itself */
		if (!is_grounded) {
			//jelly.gameObject.AddComponent<DeathEffect>();
		} else {
			grounded_activated = false;
		}
	}
	
	/// <summary>
	/// The player is right in front of this AI do something, possibly apply damage to the player?
	/// </summary>
	/// <param name="_player"></param>
	private void HandlePlayerInFront(GameObject _player) {
		if (fuse_coroutine == null) {
			stop_movement = true;
			_input.button3_down = true;
			fuse_coroutine = StartCoroutine(StartFuse(0.1f));
			Invoke("Explode", explode_time);
		}

		//_player.transform.parent.GetComponentInChildren<GenericPlayer>().Damage(1);
	}

	private void Explode() {
		foreach (SpikerProjectile projectile in projectiles) {
			GameObject _projectile = Instantiate(projectile.gameObject, projectile.gameObject.transform.position, projectile.gameObject.transform.rotation);
			_projectile.gameObject.SetActive(true);
			_projectile.GetComponent<SpikerProjectile>().activate();
		}
		
		StopCoroutine(fuse_coroutine);
		fuse_coroutine = null;
		stop_movement = false;
		jelly.gameObject.GetComponent<Renderer>().material.color = original_color;
	}
	
	/// <summary>
	/// Starts the fuse visual by having the sprite slowly blink to its
	/// inverted/red colour.
	/// </summary>
	/// <param name="rate">Rate at which to cause the flicker</param>
	private IEnumerator StartFuse(float rate) {
		bool alt = false;
    
		Color start = original_color;
    
		/* if we wanted red colour from start */
		Color end = new Color(1f, start.g * 0.1f, start.b * 0.1f);
		const float duration = 0.5f;
		while (true) {
			for (float t = 0.0f; t < duration; t += Time.deltaTime) {
				Color changedColor = alt ? Color.Lerp(end, start, t / duration) : Color.Lerp(start, end, t / duration);
				jelly.gameObject.GetComponent<Renderer>().material.color = changedColor;
				yield return null;
			}
      
			alt = !alt;
			yield return new WaitForSeconds(rate);
		}
	}

	/// <summary>
	/// There is something in front that we cannot explain.
	/// </summary>
	private void HandleOtherInFront() {
		Flip();
	}

	/// <summary>
	/// We are about to fall off the platform, so handle that.
	/// </summary>
	private void HandleLeavingGround() {
		Flip();
	}

	/// <summary>
	/// Flip the sprite.
	/// </summary>
	private void Flip() {
		flip = !flip;
		jelly.SetFlipHorizontal(flip);
	}
}
