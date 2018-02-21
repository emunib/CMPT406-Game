using UnityEngine;

/// <summary>
/// Make individual components have gravity. The gravity is to change
/// when present in a gravity field and when it is not in a
/// gravity field it will go back to having its normal gravity.
/// Component will only have gravity acting upon it.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class GravityComponent : Gravity {
  private new Rigidbody2D rigidbody;
  
  /* is the gravity settable (visible in screen
     useful to make sure not to waste computations on something
     that is not seen */
  private bool gravity_settable;
  
  /* is the component within a gravity field */
  private bool in_gravity_field;
  
  /* custom gravity when in gravity field */
  private Vector2 gravity;

  /* if component should remember gravity */
  private bool remember_gravity;

  private float jump_height;
  private float jump_apex_time;

  private void Awake() {
    /* get the rigidbody and make the component not be effected by Physics2D gravity */
    rigidbody = GetComponent<Rigidbody2D>();
    rigidbody.gravityScale = 0f;

    /* default values */
    gravity_settable = false;
    in_gravity_field = false;
    remember_gravity = true;
    gravity = DefaultGravity();

    /* get the same gravity as the player */
    Gravity player_gravity = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Gravity>();
    jump_apex_time = player_gravity.JumpApexTime();
    jump_height = player_gravity.JumpHeight();
  }

  private void Update() {
    /* if it is in gravity field get affected by players gravity otherwise get effected by custom gravity */
    if (!in_gravity_field && !remember_gravity) {
      rigidbody.velocity += DefaultGravity() * Time.deltaTime;
      Debug.DrawRay(transform.position, DefaultGravity(), Color.red);
    } else {
      rigidbody.velocity += gravity * Time.deltaTime;
      Debug.DrawRay(transform.position, gravity, Color.red);
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
    gravity = _custom_gravity;
  }

  private void OnBecameInvisible() {
    gravity_settable = false;
  }

  private void OnBecameVisible() {
    gravity_settable = true;
  }
  
  public override float JumpHeight() {
    return jump_height;
  }

  public override float JumpApexTime() {
    return jump_apex_time;
  }
}