using UnityEngine;

/// <summary>
/// Make individual player have gravity. The gravity is to change
/// when present in a gravity field and when it is not in a
/// gravity field it will go back to having its normal gravity.
/// Can be acted upon by other gravitional field and also effect it.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class GravityPlayer : MonoBehaviour, Gravity {
	private Vector2 gravity = new Vector2(0f, -9.8f);
	private Vector2 custom_gravity = new Vector2(0f, -9.8f);
	
	protected new Rigidbody2D rigidbody;
	private bool gravity_settable;
	private bool in_gravity_field;
	private bool ignore_other_fields;

	protected virtual void Awake() {
		/* get the rigidbody and make the component not be effected by Physics2D gravity */
		rigidbody = GetComponent<Rigidbody2D>();
		rigidbody.gravityScale = 0f;

		gravity_settable = false;
		in_gravity_field = false;
		ignore_other_fields = false;
	}

	protected virtual void FixedUpdate() {
		/* if it is in gravity field get affected by players gravity otherwise get effected by custom gravity */
		if (!in_gravity_field || ignore_other_fields) {
			rigidbody.velocity += gravity * Time.fixedDeltaTime;
		} else {
			rigidbody.velocity += custom_gravity * Time.fixedDeltaTime;
		}
	}

	public void InGravityField() {
		if (gravity_settable) {
			in_gravity_field = true;
		}
	}

	public void OutsideGravityField() {
		in_gravity_field = false;
	}

	public void SetCustomGravity(Vector2 _custom_gravity) {
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
	protected void SetGravity(Vector2 _gravity) {
		gravity = _gravity;
	}
	
	private void OnBecameInvisible() {
		gravity_settable = false;
	}

	private void OnBecameVisible() {
		gravity_settable = true;
	}
}
