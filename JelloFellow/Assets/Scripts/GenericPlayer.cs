using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GenericPlayer : GravityField {
  private const float GravityForce = 9.8f;
  private const float MovementSpeed = 6f;
  private const float AirAccelerationTime = 0.4f;
  private const float GroundAccelerationTime = 0.1f;
  
  /// <summary>
  /// The input for this player.
  /// </summary>
  private Input2D input;

  private new Rigidbody2D rigidbody;
  private Vector2 gravity;
  private bool apply_constant_gravity;
  
  protected override void Awake() {
    base.Awake();
    
    /* get the input from the controller */
    InputController _inputcontroller = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>();
    input = _inputcontroller.GetInput();

    /* get the default values */
    rigidbody = GetComponent<Rigidbody2D>();
    apply_constant_gravity = true;
  }

  private void Update() {
    if (input != null) {
      float horizontal_movement = input.GetHorizontalMovement();
      float vertical_movement = input.GetVerticalMovement();
      float horizontal_gravity = input.GetHorizontalGravity();
      float vertical_gravity = input.GetVerticalGravity();

      if (horizontal_gravity != 0.0f || vertical_gravity != 0.0f) {
        if (apply_constant_gravity) {
          horizontal_gravity = Mathf.Sign(horizontal_gravity) * GravityForce;
          vertical_gravity = Mathf.Sign(vertical_gravity) * GravityForce;
        } else {
          horizontal_gravity *= GravityForce;
          vertical_gravity *= GravityForce;
        }

        Physics2D.gravity = new Vector2(horizontal_gravity, vertical_gravity);
        
        
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
  public void SetInput(Input2D _input) {
    input = _input;
  }

  private void ApplyGravity(Vector2 _gravity) {
    
  }
}