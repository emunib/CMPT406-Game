using UnityEngine;

/// <summary>
/// Make individual player have gravity. The gravity is to change
/// when present in a gravity field and when it is not in a
/// gravity field it will go back to having its normal gravity.
/// Can be acted upon by other gravitional field and also effect it.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class GravityPlayer : Gravity {
	private Vector2 gravity;
	private Vector2 custom_gravity;
	
	protected new Rigidbody2D rigidbody;
	private bool gravity_settable;
	private bool in_gravity_field;
	private bool ignore_other_fields;

	protected virtual void Awake() {
		/* get the rigidbody and make the component not be effected by Physics2D gravity */
		rigidbody = GetComponent<Rigidbody2D>();
		rigidbody.gravityScale = 0f;

		/* default values */
		gravity_settable = false;
		in_gravity_field = false;
		ignore_other_fields = false;

		gravity = DefaultGravity;
		custom_gravity = DefaultGravity;
	}

	protected virtual void Update() {
		/* if it is in gravity field get affected by players gravity otherwise get effected by custom gravity */
		if (!in_gravity_field || ignore_other_fields) {
			rigidbody.velocity += gravity * Time.deltaTime;
			//Debug.DrawRay(transform.position, gravity, Color.red);
		} else {
			rigidbody.velocity += custom_gravity * Time.deltaTime;
			//Debug.DrawRay(transform.position, custom_gravity, Color.red);
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
		gravity = _gravity;
	}

	protected Vector2 GetGravity() {
		return !in_gravity_field || ignore_other_fields ? gravity : custom_gravity;
	}
	
	private void OnBecameInvisible() {
		gravity_settable = false;
	}

	private void OnBecameVisible() {
		gravity_settable = true;
	}
}
