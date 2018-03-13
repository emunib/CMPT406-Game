﻿using System.Collections.Generic;
using UnityEngine;

public class SmartAI : GenericPlayer {
	private const string player_tag = "Player";
	private GenericEnemyInput _input;
	private UnityJellySprite jelly;
	private bool flip;
	private bool do_once;
	private bool stop_movement;
	
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

		if (jelly.gameObject.transform.localScale != (Vector3) Vector2.one) {
			float angle = jelly.gameObject.transform.rotation.eulerAngles.z == 0f ? 360f : jelly.gameObject.transform.rotation.eulerAngles.z;
			jelly.gameObject.transform.rotation = Quaternion.Slerp(jelly.gameObject.transform.rotation, Quaternion.Euler(0,0,jelly.gameObject.transform.localScale.x * angle), 1f);
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
			float angle1 = platform_angle - 120f;
			float angle2 = platform_angle + 120f;
			float angle = flip ? Mathf.Max(angle1, angle2) : Mathf.Min(angle1, angle2);

			Vector2 forwardangle_direction = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
			HashSet<RaycastHit2D> leaving_ground = GetObjectsInView(forwardangle_direction, 1f, 0, 4f, true);
			if (leaving_ground.Count <= 0) {
				HandleLeavingGround();
			} else {
				foreach (RaycastHit2D hit in leaving_ground) {
					transform.rotation = Quaternion.Slerp(jelly.gameObject.transform.rotation, hit.transform.rotation, Time.deltaTime);
					break;
				}
			}
			
			HashSet<RaycastHit2D> forward_check = GetObjectsInView(flip ? transform.right : -transform.right, 1f, 0, 1.7f, true);
			foreach (RaycastHit2D hit in forward_check) {
				/* player in front */
				if (hit.transform.CompareTag(player_tag)) {
					HandlePlayerInFront(hit.transform.gameObject);
					break;
				}

				HandleOtherInFront();
				break;
			}
		}
		
		/* call this to run Update in the subclass */
		/* we call update after is because we want to change the input then call the update to handle the input changes
		   in the same frame rather to have to wait another frame */
		base.Update();
	}

	private void HandlePlayerInFront(GameObject _player) {
		
	}

	private void HandleOtherInFront() {
		Flip();
	}

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
