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
  protected Vector2 gravity;

  /* if component should remember gravity */
  private bool remember_gravity;
  
  private Vector2 gravity_restrictions;
  
  protected override void Awake() {
    base.Awake();
    
    /* get the rigidbody and make the component not be effected by Physics2D gravity */
    rigidbody = GetComponent<Rigidbody2D>();
    rigidbody.gravityScale = 0f;

    /* default values */
    gravity_settable = false;
    in_gravity_field = false;
    remember_gravity = true;
    gravity = DefaultGravity();
    gravity_restrictions = Vector2.one;
  }

  private void Update() {
    /* if it is in gravity field get affected by players gravity otherwise get effected by custom gravity */
    if (!in_gravity_field && !remember_gravity) {
      Vector2 default_gravity = new Vector2(DefaultGravity().x * gravity_restrictions.x, DefaultGravity().y * gravity_restrictions.y);
      rigidbody.velocity += default_gravity * GravityForce() * Time.deltaTime;
      Debug.DrawRay(transform.position, default_gravity, Color.red);
    } else {
      gravity = new Vector2(gravity.x * gravity_restrictions.x, gravity.y * gravity_restrictions.y);
      rigidbody.velocity += gravity * GravityForce() * Time.deltaTime;
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
  
  public override void SetGravityLightRestrictions(Vector2 _restrictions) {
    gravity_restrictions = _restrictions;
  }
}