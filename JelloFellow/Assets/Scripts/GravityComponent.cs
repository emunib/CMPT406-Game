using UnityEngine;

/// <summary>
/// Make individual components have gravity. The gravity is to change
/// when present in a gravity field and when it is not in a
/// gravity field it will go back to having its normal gravity.
/// Component will only have gravity acting upon it.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class GravityComponent : MonoBehaviour, Gravity {
  /* Default gravity set when not in gravitation field */
  private readonly Vector2 default_gravity = new Vector2(0f, -9.8f);
  
  private new Rigidbody2D rigidbody;
  
  /* is the gravity settable (visible in screen
     useful to make sure not to waste computations on something
     that is not seen */
  private bool gravity_settable;
  
  /* is the component within a gravity field */
  private bool in_gravity_field;
  
  /* custom gravity when in gravity field */
  private Vector2 gravity;

  private void Awake() {
    /* get the rigidbody and make the component not be effected by Physics2D gravity */
    rigidbody = GetComponent<Rigidbody2D>();
    rigidbody.gravityScale = 0f;

    /* default values */
    gravity_settable = false;
    in_gravity_field = false;
    gravity = default_gravity;
  }

  private void Update() {
    /* if it is in gravity field get affected by players gravity otherwise get effected by custom gravity */
    if (!in_gravity_field) {
      rigidbody.velocity += default_gravity * Time.deltaTime;
      Debug.DrawRay(transform.position, default_gravity, Color.red);
    } else {
      rigidbody.velocity += gravity * Time.deltaTime;
      Debug.DrawRay(transform.position, gravity, Color.red);
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
    gravity = _custom_gravity;
  }
  
  public Vector2 GetGravity() {
    return !in_gravity_field ? default_gravity : gravity;
  }

  private void OnBecameInvisible() {
    gravity_settable = false;
  }

  private void OnBecameVisible() {
    gravity_settable = true;
  }
}