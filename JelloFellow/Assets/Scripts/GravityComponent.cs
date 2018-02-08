using UnityEngine;

/// <inheritdoc/>
/// <summary>
/// Make individual components have gravity. The gravity is to change
/// when present in the players gravity field and when it is not in
/// players gravity field it will go back to having its normal gravity.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class GravityComponent : MonoBehaviour {
  /* Default gravity set when not in gravitation field of player */
  private readonly Vector2 default_gravity = new Vector2(0f, -9.8f);
  
  private new Rigidbody2D rigidbody2D;
  private bool gravity_settable;
  private bool in_gravity_field;

  private void Awake() {
    /* get the gravity and make the component not be effected by Physics2D gravity */
    rigidbody2D = rigidbody2D.GetComponent<Rigidbody2D>();
    rigidbody2D.gravityScale = 0f;

    gravity_settable = false;
    in_gravity_field = false;
  }

  private void Update() {
    /* if it is in gravity field get affected by players gravity otherwise get effected by custom gravity */
    if (!in_gravity_field) {
      rigidbody2D.velocity += default_gravity * Time.deltaTime;
    }
  }

  public void InGravityField() {
    if (gravity_settable) {
      in_gravity_field = true;
      rigidbody2D.gravityScale = 1f;
    }
  }

  public void OutsideGravityField() {
    in_gravity_field = false;
    rigidbody2D.gravityScale = 0f;
  }

  private void OnBecameInvisible() {
    gravity_settable = false;
  }

  private void OnBecameVisible() {
    gravity_settable = true;
  }
}