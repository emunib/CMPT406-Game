using System;
using System.Collections.Generic;
using UnityEngine;

public class GenericPlayer : GravityField {
  /* name of the ground layer */
  private const string GroundLayerName = "Ground";
  
  /* gravity field values for angular and linear drags and for the trigger sensitivity
     at which effects the rate of increasing and decreasing the field */
  private const float LinearDragGravity = 3f;
  private const float AngularDragGravity = 0.5f;
  private const float LinearDragMovement = 0f;
  private const float AngularDragMovement = 0.5f;
  private const float TriggerSensitivity = 0.1f;

  /* max stamina for the gravity field */
  private const float MaxGravityStamina = 100f;
  /* the depletion rate for the gravity field (depletion rate / maxgravity -> per frame) */
  private const float GravityDepletionRate = 1f;
  
  private Input2D input;
  private Vector2 new_gravity;
  private bool apply_constant_gravity;
  private bool lock_movement;

  private float gravity_stamina;

  private float velocity_x_smoothing;
  private float velocity_y_smoothing;
  
  [Header("Grounded Settings")]
  /* RaycastOrigins name does not match other name convention but mainly changed to show up
     nicely in the inspector */
  [Tooltip("Origins to cast rays from.")]
  [SerializeField] private Transform[] RaycastOrigins;

  [CustomRangeLabel("Ray Length", 0f, 20f)] [Tooltip("Length of the ray.")]
  [SerializeField] protected float ray_length;

  [CustomRangeLabel("Ray Count", 0f, 20f)] [Tooltip("Number of rays to show in between main rays.")]
  [SerializeField] protected int ray_count;

  [CustomRangeLabel("Angle FOV", 0f, 180f)] [Tooltip("Padding for the angle.")]
  [SerializeField] protected float ray_angle_fov;

  [Header("Movement Settings")]
  [CustomRangeLabel("Angle Buffer", 0.01f, 1f)] [Tooltip("The angle at which to accept inputs (mainly used for angled platforms)")]
  [SerializeField] private float angle_buffer = 0.2f;
  
  [CustomLabel("Move Speed")] [Tooltip("The speed at which the player will move.")]
  [SerializeField] private float move_speed = 4f;
  
  [CustomLabel("Jump Force")] [Tooltip("Force to apply in order to jump.")]
  [SerializeField] private float jump_force = 6f;
  
  [CustomLabel("Air Acceleration")] [Tooltip("The rate at which to switch sides of velocity while in the air.")]
  [SerializeField] private float air_acceleration = 0.1f;
  
  [CustomLabel("Ground Acceleration")] [Tooltip("The rate at which to switch sides of velocity when on ground.")]
  [SerializeField] private float ground_Acceleration = 0.1f;

  [CustomLabel("Apply Transforms")]
  [Tooltip("Apply movement to all rigidbodies within the given transforms this excludes self as we already apply to it.")]
  [SerializeField] private bool apply_to_transforms;
  
  [Header("Debug Settings")]
  [CustomLabel("Show Movement Rays")] [Tooltip("Shows the movement rays while moving.")]
  [SerializeField] private bool movement_rays;

  [CustomLabel("Verbose Movement")] [Tooltip("Print out all the movement information.")]
  [SerializeField] private bool verbose_movement;

  
  protected override void Awake() {
    base.Awake();

    /* get the default values */
    new_gravity = Vector2.zero;
    apply_constant_gravity = true;
    lock_movement = false;
    gravity_stamina = MaxGravityStamina;
    
    if(apply_to_transforms) ApplyGravityTo(RaycastOrigins);
    
  }

  protected override void Update() {
    /* make sure input is not null */
    if (input != null) {
      /* update lock movement to false */
      lock_movement = false;

      /* get the gravity vectors */
      float horizontal_gravity = input.GetHorizontalGravity();
      float vertical_gravity = input.GetVerticalGravity();

      if (gravity_stamina != 0f) {
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
          ApplyGravity(new_gravity);
        }
      }

      /* if changing gravity have more drag (useful to throw components and control gravity) */
      if (lock_movement) {
        rigidbody.angularDrag = AngularDragGravity;
        rigidbody.drag = LinearDragGravity;
      } else {
        rigidbody.angularDrag = AngularDragMovement;
        rigidbody.drag = LinearDragMovement;
      }

      /* left and right trigger axis */
      float left_trigger = input.GetLeftTrigger();
      float right_trigger = input.GetRightTrigger();

      /* we don't take negative numbers; Acts as a deadzone */
      if (left_trigger > 0) {
        SetFieldRadius(GetFieldRadius() - TriggerSensitivity * left_trigger);
      }

      if (right_trigger > 0) {
        SetFieldRadius(GetFieldRadius() + TriggerSensitivity * right_trigger);
      }

      Movement();
    } else {
      Debug.LogWarning("Input has not been assigned for this player (" + gameObject.name + ")");
    }
    
    
    base.Update();
  }

  private void FixedUpdate() {
    GravityStamina();
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

  /// <summary>
  /// Check if the player is touching Ground layer.
  /// </summary>
  /// <returns>True if touching ground otherwise false.</returns>
  protected bool IsGrounded(bool visualize = false) {
    HashSet<RaycastHit2D> hits = GetObjectsInView(GetGravity(), ray_angle_fov, ray_count, ray_length, visualize);
    foreach (RaycastHit2D hit in hits) {
      if (LayerMask.LayerToName(hit.transform.gameObject.layer) == GroundLayerName) {
        return true;
      }
    }
    
    return false;
  }

  /// <summary>
  /// Gets all the game objects within the set Field of View, given the direction. Utilizes Raycast to create a 2D cone
  /// starting from the center of the given Transforms to the provided length of the ray to check for collisions.
  /// </summary>
  /// <param name="direction">The direction to point the cone.</param>
  /// <param name="_ray_angle_fov">The field of view angle.</param>
  /// <param name="_ray_count">The ray count within the field of view.</param>
  /// <param name="_ray_length">The length of the rays.</param>
  /// <param name="visualize">If you want to see the cone shape in the editor.</param>
  /// <returns>Hashset containing all the game objects within FOV.</returns>
  protected HashSet<RaycastHit2D> GetObjectsInView(Vector2 direction, float _ray_angle_fov, int _ray_count, float _ray_length, bool visualize = false) {
    /* hashset is useful as it only stores unique values so repeated hits won't be registered */
    HashSet<RaycastHit2D> game_objects = new HashSet<RaycastHit2D>();
    
    foreach (Transform origins in RaycastOrigins) {
      /* the center of the transform */
      Vector3 start_position = origins.position;
      /* the angle of direction relative to the transforms 'up' position */
      float initial_angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
      /* cut the field of view angle in half to add to both sides of the initial angle */
      float angle_fov = _ray_angle_fov / 2f * Mathf.Deg2Rad;
      /* calculate the distance between rays (plus 1 to match the number shown in inspector,
         one is actually hidden under the start direction) */
      float distance_between_rays = _ray_angle_fov / (_ray_count + 1) * Mathf.Deg2Rad;
      /* angles to start and end with */
      float start_angle = Mathf.Deg2Rad * initial_angle + angle_fov;
      float end_angle = Mathf.Deg2Rad * initial_angle - angle_fov;
      
      /* get the start direction by adding FOV to the angle (left side when gravity is normal) */
      Vector2 start_direction = new Vector2(Mathf.Sin(start_angle), Mathf.Cos(start_angle));
      /* get the end direction by adding FOV to the angle (right side when gravity is normal) */
      Vector2 end_direction = new Vector2(Mathf.Sin(end_angle), Mathf.Cos(end_angle));
      
      for (; start_angle > end_angle; start_angle -= distance_between_rays) {
        /* get direction of the incremented angle */
        Vector2 angle_direction = new Vector2(Mathf.Sin(start_angle), Mathf.Cos(start_angle));

        /* check if we hit something in that direction by the set length in the inspector */
        RaycastHit2D hit = Physics2D.Raycast(start_position, angle_direction, _ray_length);
        if (hit) {
          /* add to the hashset */
          if(hit.transform.gameObject != gameObject) game_objects.Add(hit);
        }
        
        if(visualize) Debug.DrawRay(start_position, angle_direction * _ray_length, Color.red);
      }

      /* check if we hit something in the start direction */
      RaycastHit2D start_hit = Physics2D.Raycast(start_position, start_direction, _ray_length);
      if (start_hit) {
        /* add to the hashset */
        if(start_hit.transform.gameObject != gameObject) game_objects.Add(start_hit);
      }
      
      /* check if we hit something in the end direction */
      RaycastHit2D end_hit = Physics2D.Raycast(start_position, end_direction, _ray_length);
      if (end_hit) {
        /* add to the hashset */
        if(end_hit.transform.gameObject != gameObject) game_objects.Add(end_hit);
      }
      
      if(visualize) Debug.DrawRay(start_position, start_direction * _ray_length, Color.blue);
      if(visualize) Debug.DrawRay(start_position, end_direction * _ray_length, Color.blue);
    }

    return game_objects;
  }
  
  /// <summary>
  /// Applies gravity by calling its subclass method 'SetGravity'.
  /// </summary>
  /// <param name="_gravity">The gravity to set.</param>
  private void ApplyGravity(Vector2 _gravity) {
    SetGravity(_gravity);
  }
  
  /// <summary>
  /// Handles the depletion and gaining for the gravity stamina.
  /// </summary>
  private void GravityStamina() {
    if (lock_movement) {
      if (gravity_stamina < 0) {
        gravity_stamina = 0f;
      } else {
        gravity_stamina -= GravityDepletionRate;
      }
    } else {
      if (IsGrounded()) {
        if (gravity_stamina < MaxGravityStamina) {
          gravity_stamina += GravityDepletionRate;
        } else {
          gravity_stamina = MaxGravityStamina;
        }
      }
    }
    
    ChangeGravityAlpha(gravity_stamina/MaxGravityStamina);
  }

  /// <summary>
  /// Handles all movement done by the player.
  /// </summary>
  private void Movement() {
    /* get the platform */
    float platform_angle = 0f;
    GameObject current_platform = null;
    HashSet<RaycastHit2D> hits = GetObjectsInView(GetGravity(), ray_angle_fov, ray_count, ray_length);
    foreach (RaycastHit2D hit in hits) {
      if (LayerMask.LayerToName(hit.transform.gameObject.layer) == GroundLayerName) {
        Debug.Log("We did hit a platform");
        /* calculate angle of the platform we are on */
        current_platform = hit.transform.gameObject;
        platform_angle = (Mathf.Atan2(hit.normal.x, hit.normal.y) * Mathf.Rad2Deg + 360) % 360;
        break;
      }
    }
    
    /* if platform was found */
    if (current_platform != null) {
      Vector2 velocity = rigidbody.velocity;

      bool is_grounded = IsGrounded(true);
      
      /* if verbose mode is on */
      if(verbose_movement) Debug.Log("Platform found, Angle of Platform: " + platform_angle);
      
      float horizontal_movement = input.GetHorizontalMovement();      
      float vertical_movement = input.GetVerticalMovement();
      
      if (is_grounded && (horizontal_movement != 0f || vertical_movement != 0f)) {
        /* angle of the movement joystick */
        float movement_angle = (Mathf.Atan2(horizontal_movement, vertical_movement) * Mathf.Rad2Deg + 360) % 360;
        
        /* if verbose mode is on */
        if(verbose_movement) Debug.Log("Movement is requested, Movement Angle: " + movement_angle);

        /* direction of the movement angle */
        Vector2 direction_movement = new Vector2(Mathf.Sin(movement_angle * Mathf.Deg2Rad), Mathf.Cos(movement_angle * Mathf.Deg2Rad));
        if(movement_rays) Debug.DrawRay(transform.position, direction_movement * ray_length, Color.yellow); /* draw the ray for debugging */
        
        /* direction of the platform angle */
        Vector2 direction_platform = new Vector2(Mathf.Sin((platform_angle + 90) * Mathf.Deg2Rad), Mathf.Cos((platform_angle + 90) * Mathf.Deg2Rad));
        if(movement_rays) Debug.DrawRay(transform.position, direction_platform * ray_length, Color.magenta); /* draw the ray for debugging */

        /* the velocity to apply to the player */
        Vector2 velocity_updated = new Vector2(direction_movement.x * Mathf.Abs(direction_platform.x), direction_movement.y * Mathf.Abs(direction_platform.y));
        
        /* if verbose mode is on */
        if (verbose_movement) {
          Debug.Log("Movement Direction: " + direction_movement);
          Debug.Log("Platform Direction: " + direction_platform);
        }
        
        /* check if the platform the player is on is straight or at an angle */
        if (Mathf.Approximately(Mathf.Abs(direction_platform.x), 1f) || Mathf.Approximately(Mathf.Abs(direction_platform.y), 1f)) {
          /* if verbose mode is on */
          if(verbose_movement) Debug.Log("Platform is straight");
          
          velocity.x = Mathf.SmoothDamp(velocity.x, velocity_updated.x * move_speed, ref velocity_x_smoothing, is_grounded ? ground_Acceleration : air_acceleration);
          velocity.y = Mathf.SmoothDamp(velocity.y, velocity_updated.y * move_speed, ref velocity_y_smoothing, is_grounded ? ground_Acceleration : air_acceleration);
        } else {
          /* if verbose mode is on */
          if(verbose_movement) Debug.Log("Platform is at an angle");
          
          /* get the direction of the opposite angle of platform (seems redundunt as x and y are swapped but works so here for now) */
          Vector2 direction_platform2 = new Vector2(Mathf.Sin((platform_angle - 90) * Mathf.Deg2Rad), Mathf.Cos((platform_angle - 90) * Mathf.Deg2Rad));
          /* if verbose mode is on */
          if(verbose_movement) Debug.Log("Platform Direction 2: " + direction_platform2);
          
          /* get the distance of the movement direction to the platforms direction */
          float direction_distance = Vector2.Distance(direction_movement, direction_platform);
          float direction_distance2 = Vector2.Distance(direction_movement, direction_platform2);
          
          /* if verbose mode is on */
          if (verbose_movement) {
            Debug.Log("Distance Direction: " + direction_distance);
            Debug.Log("Distance Direction 2: " + direction_distance2);
          }

          /* this makes sure we only grab the angle with same direction sign as the 4 corners of an axis range from -1 to 1 */
          bool platform_sign = Mathf.Sign(direction_platform.x) == Math.Sign(direction_platform.y);

          if (platform_sign ? Mathf.Sign(direction_movement.x) == Mathf.Sign(direction_movement.y) : Mathf.Sign(direction_movement.x) != Mathf.Sign(direction_movement.y)) {
            /* make sure we are in the range of the angle buffer (still think direction distance 2 is not needed but it works so here for now) */
            if ((direction_distance < angle_buffer || direction_distance > 2 - angle_buffer) && (direction_distance2 < angle_buffer || direction_distance2 > 2 - angle_buffer)) {
              velocity.x = Mathf.SmoothDamp(velocity.x, velocity_updated.x * move_speed, ref velocity_x_smoothing, is_grounded ? ground_Acceleration : air_acceleration);
              velocity.y = Mathf.SmoothDamp(velocity.y, velocity_updated.y * move_speed, ref velocity_y_smoothing, is_grounded ? ground_Acceleration : air_acceleration); 
            }
          }
        }
      } else {
        /* if player in air apply this velocity */
        Vector2 in_air = new Vector2 (horizontal_movement * move_speed, vertical_movement * move_speed);
        in_air *= air_acceleration;
        velocity += in_air;
      }
      
      /* apply changed velocity */
      rigidbody.velocity = velocity;

      /* should the nodes jump too */
      bool node_jump = false;
      /* direction of the platform */
      Vector2 direction_jump = new Vector2(Mathf.Sin(platform_angle * Mathf.Deg2Rad), Mathf.Cos(platform_angle * Mathf.Deg2Rad));
      Vector2 hybrid_jump = Vector2.zero;
      /* vector of the movement */
      Vector2 movement_vector = new Vector2(horizontal_movement, vertical_movement);
      if (input.GetJumpButtonDown() && is_grounded) {
        hybrid_jump = direction_jump + movement_vector * 1.5f;
        if (hybrid_jump.magnitude > 1f) {
          hybrid_jump.Normalize();
        }

        if (verbose_movement) Debug.Log("Jump: " + hybrid_jump * jump_force);
        rigidbody.AddForce(hybrid_jump * jump_force, ForceMode2D.Impulse);
        node_jump = true;
      }

      /* apply velocity and jump to every node */
      if (apply_to_transforms) {
        foreach (Transform node in RaycastOrigins) {
          Rigidbody2D rigidbody_node = node.gameObject.GetComponent<Rigidbody2D>();
          if (rigidbody_node) {
            rigidbody_node.velocity = velocity;
            if (node_jump) rigidbody_node.AddForce(hybrid_jump * jump_force, ForceMode2D.Impulse);
          }
        }
      }
    }
  }
}