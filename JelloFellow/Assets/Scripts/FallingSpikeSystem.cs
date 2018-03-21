using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Make individual components have gravity. The gravity is to change
/// when present in a gravity field and when it is not in a
/// gravity field it will go back to having its normal gravity.
/// Component will only have gravity acting upon it.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class FallingSpikeSystem : Gravity {

  public string PlayerLayer;
  [Range(0,1)]
  public float shakeRange;
  public float FallStartTime;
  public float EndFallTime;


  
  protected new Rigidbody2D rigidbody;
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
  private bool shake;
  
  private Vector2 gravity_restrictions;

  
  private Vector3 lastPosition;
  private bool isfalling;
  
  protected override void Awake() {
    base.Awake();
    
    /* get the rigidbody and make the component not be effected by Physics2D gravity */
    rigidbody = GetComponent<Rigidbody2D>();
    rigidbody.gravityScale = 0f;

    /* default values */
    gravity_settable = false;
    in_gravity_field = false;
    remember_gravity = true;
    gravity = Vector2.zero;
    gravity_restrictions = Vector2.one;
    
    shake = false;
    isfalling = false;
    lastPosition = transform.position;

    rigidbody.freezeRotation = true;


  }

  protected virtual void Update() {
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

    if (shake) {
      transform.position = lastPosition;
      //lastPosition = transform.position;
      
      transform.position = (Vector2)transform.position + Random.insideUnitCircle*shakeRange;
    }
    
  }

  public void PlayerHasEntered() {
    shake = true;
    isfalling = true;
    Invoke("BeginFalling",FallStartTime);
    transform.Find("trigger").GetComponent<BoxCollider2D>().enabled = false;

  }

  private void BeginFalling() {
    shake = false;
    gravity = -transform.up;
    //Invoke("EndFall",EndFallTime);
    Destroy(gameObject, EndFallTime);
  }

  private void EndFall() {
    
    transform.position = lastPosition;
    rigidbody.velocity = Vector2.zero;
    gravity = Vector2.zero;
    isfalling = false;
 
  }

  public bool IsFalling() {
    return isfalling;
  }
  
  

  public override void InGravityField() {
    /*if (gravity_settable) {
      in_gravity_field = true;
    }
    */
  }

  public override void OutsideGravityField() {
    //in_gravity_field = false;
  }
  
  public override void SetCustomGravity(Vector2 _custom_gravity) {
    //gravity = _custom_gravity;
    
  }

  
  
  public override void SetGravityLightRestrictions(Vector2 _restrictions) {
    gravity_restrictions = _restrictions;
  }
}