using UnityEngine;

/// <inheritdoc/>
/// <summary>
/// Make individual player have gravity. The gravity is to change
/// when present in a gravity field and when it is not in a
/// gravity field it will go back to having its normal gravity.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class GravityPlayer : MonoBehaviour {
	private Vector2 gravity = new Vector2(0f, -9.8f);
	
	private new Rigidbody2D rigidbody;
	private bool gravity_settable;
	private bool in_gravity_field;

	protected virtual void Awake() {
		/* get the rigidbody and make the component not be effected by Physics2D gravity */
		rigidbody = rigidbody.GetComponent<Rigidbody2D>();
		rigidbody.gravityScale = 0f;

		gravity_settable = false;
		in_gravity_field = false;
	}

	private void FixedUpdate() {
		/* if it is in gravity field get affected by players gravity otherwise get effected by custom gravity */
		if (!in_gravity_field) {
			rigidbody.velocity += gravity * Time.fixedDeltaTime;
		}
	}

	public void InGravityField() {
		if (gravity_settable) {
			in_gravity_field = true;
			rigidbody.gravityScale = 1f;
		}
	}

	public void OutsideGravityField() {
		in_gravity_field = false;
		rigidbody.gravityScale = 0f;
	}

	private void OnBecameInvisible() {
		gravity_settable = false;
	}

	private void OnBecameVisible() {
		gravity_settable = true;
	}
}
