using UnityEngine;

public class GenericPlayer : GravityField {
  private const float LinearDragGravity = 3f;
  private const float AngularDragGravity = 0.5f;
  private const float LinearDragMovement = 0f;
  private const float AngularDragMovement = 0.5f;
  
  /// <summary>
  /// The input for this player.
  /// </summary>
  private Input2D input;

  private Vector2 new_gravity;
  private bool apply_constant_gravity;
  private bool lock_movement;
  
  protected override void Awake() {
    base.Awake();

    /* get the default values */
    new_gravity = Vector2.zero;
    apply_constant_gravity = true;
    lock_movement = false;
  }

  private float velocity_x_smoothing;
  private float velocity_y_smoothing;
  protected override void Update() {
    base.Update();
    
    /* make sure input is not null */
    if (input != null) {
      /* update lock movement to false */
      lock_movement = false;
      
      /* get the gravity vectors */
      float horizontal_gravity = input.GetHorizontalGravity();
      float vertical_gravity = input.GetVerticalGravity();
      
      /* make sure gravity is not 0 or dont change */
      if (horizontal_gravity != 0.0f || vertical_gravity != 0.0f) {
        /* update lock movement to true as we are actually changing gravity */
        lock_movement = true;
        
        if (apply_constant_gravity) {
          new_gravity = new Vector2(horizontal_gravity, vertical_gravity).normalized * GravityForce;
        } else {
          /* apply gravity with variable force */
          new_gravity = new Vector2(horizontal_gravity, vertical_gravity) * GravityForce;
        }

        /* apply gravity when changed */
        if(!new_gravity.Equals(Vector2.zero)) ApplyGravity(new_gravity);
      }

      /* if the movement is locked change the angular and linear drag so we can easily throw components */
      if (lock_movement) {
        rigidbody.angularDrag = AngularDragGravity;
        rigidbody.drag = LinearDragGravity;
      } else {
        rigidbody.angularDrag = AngularDragMovement;
        rigidbody.drag = LinearDragMovement;
      }
    } else {
      Debug.LogWarning("Input has not been assigned for this player (" + gameObject.name + ")");
    }
  }

  /// <summary>
  /// Set the input for this player.
  /// Important if this were to be used by an AI where AI would be the
  /// "controller" which can replicate player moves if need to be.
  /// </summary>
  /// <param name="_input">Input of the player.</param>
  protected void SetInput(Input2D _input) {
    input = _input;
  }

  protected bool IsGround() {
    return false;
  }

  private void ApplyGravity(Vector2 _gravity) {
    SetGravity(_gravity);
  }
}