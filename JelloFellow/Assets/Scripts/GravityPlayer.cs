using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Make individual player have gravity. The gravity is to change
/// when present in a gravity field and when it is not in a
/// gravity field it will go back to having its normal gravity.
/// Can be acted upon by other gravitional field and also effect it.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public abstract class GravityPlayer : Gravity {
	private Vector2 gravity;
	private Vector2 custom_gravity;
	
	protected new Rigidbody2D rigidbody;
	private bool gravity_settable;
	private bool in_gravity_field;
	private bool ignore_other_fields;
	private HashSet<Transform> objects;
	private Vector2 gravity_restrictions;
	private Vector2 gravity_restoration;
	private bool restore_gravity;

	protected bool AffectSelfWithGravity;
	
	protected override void Awake() {
		base.Awake();
		
		/* get the rigidbody and make the component not be effected by Physics2D gravity */
		rigidbody = GetComponent<Rigidbody2D>();
		rigidbody.gravityScale = 0f;

		/* default values */
		gravity_settable = false;
		in_gravity_field = false;
		ignore_other_fields = false;
		gravity_restrictions = Vector2.one;
		restore_gravity = false;
		
		gravity = DefaultGravity();
		custom_gravity = DefaultGravity();
		AffectSelfWithGravity = true;
	}

	protected virtual void Update() {
		if (AffectSelfWithGravity) {
			/* if it is in gravity field get affected by players gravity otherwise get effected by custom gravity */
			if (!in_gravity_field && ignore_other_fields) {
				if (gravity_restrictions != Vector2.one) {
					if (!restore_gravity) {
						gravity_restoration = gravity;
						restore_gravity = true;
					}

					gravity = new Vector2(gravity.x * gravity_restrictions.x, gravity.y * gravity_restrictions.y);
				} else {
					if (restore_gravity) {
						gravity = gravity_restoration;
						restore_gravity = false;
					}
				}

				rigidbody.velocity += gravity * GravityForce() * Time.deltaTime;

				if (objects != null) {
					foreach (Transform node in objects) {
						Rigidbody2D rigidbody_node = node.gameObject.GetComponent<Rigidbody2D>();
						if (rigidbody_node) {
							rigidbody_node.velocity += gravity * GravityForce() * Time.deltaTime;
							//Debug.DrawRay(node.position, gravity, Color.red);
						}
					}
				}

				//Debug.DrawRay(transform.position, gravity, Color.red);
			} else {
				if (gravity_restrictions != Vector2.one) {
					if (!restore_gravity) {
						gravity_restoration = custom_gravity;
						restore_gravity = true;
					}

					custom_gravity = new Vector2(custom_gravity.x * gravity_restrictions.x, custom_gravity.y * gravity_restrictions.y);
				} else {
					if (restore_gravity) {
						custom_gravity = gravity_restoration;
						restore_gravity = false;
					}
				}

				rigidbody.velocity += custom_gravity * GravityForce() * Time.deltaTime;

				if (objects != null) {
					foreach (Transform node in objects) {
						Rigidbody2D rigidbody_node = node.gameObject.GetComponent<Rigidbody2D>();
						if (rigidbody_node) {
							rigidbody_node.velocity += custom_gravity * GravityForce() * Time.deltaTime;
						}
					}
				}

				//Debug.DrawRay(transform.position, custom_gravity, Color.red);
			}
		}
	}

	protected void ApplyGravityTo(HashSet<Transform> _objects) {
		objects = _objects;
		foreach (Transform node in objects) {
			Rigidbody2D rigidbody_node = node.gameObject.GetComponent<Rigidbody2D>();
			if (rigidbody_node) {
				rigidbody_node.gravityScale = 0f;
			}
		}
	}

	public override void InGravityField() {
		if (gravity_settable) {
			in_gravity_field = true;
		}
	}

	public override void OutsideGravityField() {
		in_gravity_field = false;
	}

	public override void SetCustomGravity(Vector2 _custom_gravity) {
		if (!ignore_other_fields) {
			custom_gravity = _custom_gravity;
		}
	}

	protected void SetIgnoreFields(bool _ignorefields) {
		ignore_other_fields = _ignorefields;
	}
	
	/// <summary>
	/// Set the gravity of the player.
	/// </summary>
	/// <param name="_gravity">Gravity to be effected by.</param>
	protected virtual void SetGravity(Vector2 _gravity) {
		if (ignore_other_fields) {
			gravity = _gravity;
		} else {
			custom_gravity = _gravity;
		}
	}

	public Vector2 GetGravity() {
		return !in_gravity_field && ignore_other_fields ? gravity : custom_gravity;
	}
	
	private void OnBecameInvisible() {
		gravity_settable = false;
	}

	private void OnBecameVisible() {
		gravity_settable = true;
	}

	public override void SetGravityLightRestrictions(Vector2 _restrictions) {
		gravity_restrictions = _restrictions;
	}
}
