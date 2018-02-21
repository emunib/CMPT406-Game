using System;
using System.Collections.Generic;
using UnityEngine;

public class GenericPlayer : GravityField {
  [Header("General Settings")]
  [CustomLabel("Raycast Origin")] [Tooltip("Should the main object be an origin of raycasting.")] [SerializeField]
  private bool is_raycast_origin = true;
  
  [Header("Child Settings")]
  [CustomLabel("Apply Gravity")] [Tooltip("Apply gravity to the child objects.")] [SerializeField]
  private bool apply_gravity_tochild;
  
  [CustomLabel("Apply Movement")] [Tooltip("Apply movement to the child objects.")] [SerializeField]
  private bool apply_movement_tochild;
  
  [Tooltip("The child components of this object.")] [SerializeField]
  private ChildComponent[] ChildComponents;

  [Header("Gravity Settings")]
  [CustomLabel("Angular Drag")] [Tooltip("Angular drag applied while changing gravity.")] [SerializeField]
  private float gravity_angular_drag = 0.5f;

  [CustomLabel("Linear Drag")] [Tooltip("Linear drag applied while changing gravity.")] [SerializeField]
  private float gravity_linear_drag = 3f;

  [CustomRangeLabel("Max Stamina", 0f, 100f)] [Tooltip("Max possible stamina that can be obtained by the player.")] [SerializeField]
  private float max_gravity_stamina = 100f;
  
  [CustomRangeLabel("Min Stamina", 0f, 100f)] [Tooltip("Min possible stamina that can be obtained by the player.")] [SerializeField]
  private float min_gravity_stamina = 0f;

  [CustomRangeLabel("Stamina Depletion Time", 0f, 100f)] [Tooltip("In seconds the time to completely lose stamina.")] [SerializeField]
  private float gravity_depletion_time = 5f;
  
  [CustomRangeLabel("Field Transition Time", 0f, 100f)] [Tooltip("The time to increase or decrease completely in seconds.")] [SerializeField]
  private float gravity_field_transition_time = 0.6f;

  [Header("Movement Settings")]
  [CustomLabel("Linear Drag")] [Tooltip("Linear drag applied while changing movement.")] [SerializeField]
  private float movement_linear_drag = 5f;
  
  [CustomRangeLabel("Move Speed", 0f, 100f)] [Tooltip("Speed at which to move the player.")] [SerializeField]
  private float move_speed = 10f;
  
  [CustomRangeLabel("Jump Height", 0f, 100f)] [Tooltip("The height of the jump (apex).")] [SerializeField]
  private float jump_height = 6f;
  
  [CustomRangeLabel("Jump Apex Time", 0f, 100f)] [Tooltip("Time to reach the apex of the jump.")] [SerializeField]
  private float jump_apex_time = 0.4f;
  
  [CustomRangeLabel("Jump Angle Force", 0f, 100f)] [Tooltip("Force to apply in order to jump at an angle.")] [SerializeField]
  private float jump_angle_force = 10f;

  [CustomRangeLabel("Leniency Angle", 0f, 90f)] [Tooltip("The angle to allow movement in direction of the platforms angle.")] [SerializeField]
  private float leniency_angle = 25f;
  
  [CustomRangeLabel("Ground Acceleration Time", 0f, 1f)] [Tooltip("The smooth time of increasing velocity, will effect change in direction.")] [SerializeField]
  private float ground_acceleration = 0.1f;
  
  [CustomRangeLabel("Air Acceleration Time", 0f, 1f)] [Tooltip("The smooth time of increasing velocity, will effect change in direction.")] [SerializeField]
  private float air_acceleration = 0.4f;
  
  [Header("Grounded Settings")]
  [CustomRangeLabel("Field of View Angle", 0f, 360f)] [Tooltip("Field of view (angle/arc) to cover.")] [SerializeField]
  protected float ground_fov_angle = 25f;

  [CustomRangeLabel("Number of Rays", 0, 20)] [Tooltip("Number of rays within the field of view arc.")] [SerializeField]
  protected int ground_ray_count = 2;

  [CustomRangeLabel("Length of Ray", 0f, 20f)] [Tooltip("Length of the ray within the field of view arc.")] [SerializeField]
  protected float ground_ray_length = 0.65f;
  
  [Header("Debug Settings")]
  [CustomLabel("Verbose Gravity")] [Tooltip("Verbose gravity for debugging.")] [SerializeField]
  private bool verbose_gravity;

  [CustomLabel("Show Gravity")] [Tooltip("Show gravity with a ray.")] [SerializeField]
  private bool show_gravity;

  [CustomLabel("Gravity Color")] [Tooltip("Color of the ray representing gravity.")] [SerializeField]
  private Color gravity_ray_color = Color.black;

  [CustomLabel("Visualize Ground Check")] [Tooltip("Visualize the grounded check.")] [SerializeField]
  private bool visualize_ground_check;
  
  [CustomLabel("FOV Edge Ray Color")] [Tooltip("Color of the two edge rays.")] [SerializeField]
  private Color visualize_fov_edge = Color.blue;
  
  [CustomLabel("FOV In-Between Ray Color")] [Tooltip("Color of the rays in between the two edges.")] [SerializeField]
  private Color visualize_fov_inbetween = Color.red;

  [CustomLabel("Verbose Movement")] [Tooltip("Verbose movement for debugging.")] [SerializeField]
  private bool verbose_movement;

  [CustomLabel("Show Movement")] [Tooltip("Show movement with rays for debugging.")] [SerializeField]
  private bool show_movement;
  
  [CustomLabel("Movement Direction Color")] [Tooltip("Color representing the direction of requested movement by the joystick.")] [SerializeField]
  private Color movement_direction_color = Color.yellow;
  
  [CustomLabel("Platform Direction Color")] [Tooltip("Color representing the direction of the platform.")] [SerializeField]
  private Color platform_direction_color = Color.magenta;
  
  [CustomLabel("Leniency Color")] [Tooltip("Color representing the Leniency of the movement.")] [SerializeField]
  private Color movement_leniency_color = Color.blue;

  /* variables not exposed in the inspector */
  /* input controller for this player */
  private Input2D input;
  /* holds the transforms of the child */
  private HashSet<Transform> child_transforms = null;
  /* holds the origins for the raycast system */
  private HashSet<Transform> raycast_origins = null;
  /* will hold the movement drag information while gravity is being manipulated (meant to restore drags for movement) */
  private Drag normal_movement_drags;
  /* has the movement drag been restored yet */
  private bool restored_drag;
  /* the calculated depletion rate from the max/min and time given (min - max / time) */
  private float gravity_depletion_rate;
  /* the current state of gravity */
  private float gravity_stamina;
  /* transition rate of changing gravity field */
  private float gravity_field_transition_rate;
  /* old platform the player used to be on */
  private GameObject old_platform;
  /* smoothing velocity of moving in direction of x or y */
  private float velocity_x_smoothing;
  private float velocity_y_smoothing;
  /* true if we just manipulated gravity (locks movement until we are grounded)
     ensures we still don't use the old platform angle to influence movement */
  private bool just_changed_gravity;
  /* contains the last grounded platforms angle */
  private float platform_angle = 0f;
  
  /* store the value of the ground check every update as its more efficient to not use raycast and calculations more than once
     in the same frame */
  protected bool is_grounded { get; private set; }
  
  protected virtual void Start() {
    /* init hashset raycast origins */
    raycast_origins = new HashSet<Transform>();
    /* if the main object is a raycast origin then add to the collection */
    if (is_raycast_origin) raycast_origins.Add(transform);
    
    /* make sure the child components array is not 0 otherwise dont so anything to it */
    if (ChildComponents.Length != 0) {
      child_transforms = new HashSet<Transform>();
      
      /* add to set of child transforms, and if they are raycast origins then add to that set as well */
      foreach (ChildComponent child_component in ChildComponents) {
        child_transforms.Add(child_component.Child);
        if (child_component.RaycastOrigin) raycast_origins.Add(child_component.Child);
      }
    }

    /* if allowed to apply gravity to each child component do it */
    if (apply_gravity_tochild) {
      ApplyGravityTo(child_transforms);
    }
    
    /* init */
    normal_movement_drags = new Drag();
    restored_drag = true;
    gravity_depletion_rate = (min_gravity_stamina - max_gravity_stamina) / gravity_depletion_time;
    gravity_stamina = max_gravity_stamina;
    gravity_field_transition_rate = (MinRadius - MaxRadius) / gravity_field_transition_time;
    just_changed_gravity = false;
    
    if(verbose_gravity) Debug.Log("Gravity depletion rate: " + gravity_depletion_rate);
    if (verbose_gravity) Debug.Log("Gravity field transition rate: " + gravity_field_transition_rate);
    
    Physics2D.gravity = GetGravity();
  }

  protected override void Update() {
    /* make sure we have something to take input from */
    if (input != null) {
      /* check if player is grounded (store this check as its efficient to not use raycast system more than once
         in the same update loop */
      is_grounded = IsGrounded(visualize_ground_check);
      
      /* get the gravity inputs from joystick */
      float horizontal_gravity = input.GetHorizontalRightStick();
      float vertical_gravity = input.GetVerticalRightStick();
      
      /* change only when the inputs are not 0 */
      if (gravity_stamina != 0 && (horizontal_gravity != 0 || vertical_gravity != 0)) {
        just_changed_gravity = true;
        
        /* store the movement drags */
        if (restored_drag) {
          normal_movement_drags.SetDrags(rigidbody.angularDrag, rigidbody.drag);
          restored_drag = false;
        }

        /* change to gravity drags */
        rigidbody.angularDrag = gravity_angular_drag;
        rigidbody.drag = gravity_linear_drag;
        
        /* apply jump, movement, and drag to the child components if allowed */
        if (apply_movement_tochild) {
          foreach (Transform child in child_transforms) {
            Rigidbody2D child_rigidbody = child.gameObject.GetComponent<Rigidbody2D>();
            if (child_rigidbody) {
              child_rigidbody.drag = gravity_linear_drag;
              child_rigidbody.angularDrag = gravity_angular_drag;
            }
          }
        }
        
        /* applies constant force (due to it being normalized) */
        Vector2 new_gravity = new Vector2(horizontal_gravity, vertical_gravity).normalized * GravityForce();
        if(verbose_gravity) Debug.Log("Gravity Force: " + new_gravity);
        
        /* set the gravity to the new selected gravity */
        SetGravity(new_gravity);

        /* deplete gravity as its being manipulated */
        gravity_stamina = Mathf.Clamp(gravity_stamina + gravity_depletion_rate * Time.deltaTime, min_gravity_stamina, max_gravity_stamina);
        if(verbose_gravity) Debug.Log("Gravity Stamina: " + gravity_stamina);
      } else {
        rigidbody.angularDrag = 0f;
        rigidbody.drag = 0f;
        
        /* apply jump, movement, and drag to the child components if allowed */
        if (apply_movement_tochild) {
          foreach (Transform child in child_transforms) {
            Rigidbody2D child_rigidbody = child.gameObject.GetComponent<Rigidbody2D>();
            if (child_rigidbody) {
              child_rigidbody.drag = 0f;
              child_rigidbody.angularDrag = 0f;
            }
          }
        }
      }

      /* left and right trigger axis */
      float left_trigger = input.GetLeftTrigger();
      float right_trigger = input.GetRightTrigger();

      /* we don't take negative numbers; Acts as a deadzone */
      /* change the gravity field radius */
      if (left_trigger > 0) {
        SetFieldRadius(Mathf.Clamp(GetFieldRadius() + gravity_field_transition_rate * Time.deltaTime, MinRadius, MaxRadius));
      }

      if (right_trigger > 0) {
        SetFieldRadius(Mathf.Clamp(GetFieldRadius() - gravity_field_transition_rate * Time.deltaTime, MinRadius, MaxRadius));
      }

      if (is_grounded) {
        /* recharge gravity as its not being manipulated and the player is grounded */
        gravity_stamina = Mathf.Clamp(gravity_stamina - gravity_depletion_rate * Time.deltaTime, min_gravity_stamina, max_gravity_stamina);
        if(verbose_gravity) Debug.Log("Gravity Stamina: " + gravity_stamina);

        /* restore movement drags */
        if (!restored_drag) {
          rigidbody.angularDrag = normal_movement_drags.AngularDrag;
          rigidbody.drag = normal_movement_drags.LinearDrag;
          restored_drag = true;
        }
        
        if (just_changed_gravity) {
          just_changed_gravity = false;
        }
      }

      if (!just_changed_gravity) {
        /* handle movement as drag is restored and the gravity is not being manipulated */
        HandleMovement();
      }
    }
    
    if(show_gravity) Debug.DrawRay(transform.position, GetGravity(), gravity_ray_color);
    
    /* update the gravity field alpha to represent the gravity stamina */
    ChangeGravityAlpha(Mathf.Clamp01(gravity_stamina/max_gravity_stamina));
    
    base.Update();
  }
  
  /// <summary>
  /// Modulo operator function.
  /// https://answers.unity.com/questions/380035/c-modulus-is-wrong-1.html
  /// </summary>
  private static float fmod(float a, float b) {
    return a - b * Mathf.Floor(a / b);
  }

  private void HandleMovement() {
    /* get platform information */
    GameObject current_platform = null;
    HashSet<RaycastHit2D> hits = GetObjectsInView(GetGravity(), ground_fov_angle, ground_ray_count, ground_ray_length);
    Vector2 platform_hit_normal = Vector2.zero;
    foreach (RaycastHit2D hit in hits) {
      if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "Ground") {
        /* calculate angle of the platform we are on */
        current_platform = hit.transform.gameObject;
        platform_angle = Mathf.Atan2(hit.normal.x, hit.normal.y) * Mathf.Rad2Deg;
        
        /* get angle between 0 - 360, even handle negative signs with modulus */
        platform_angle = fmod(platform_angle, 360);
        if (platform_angle < 0) platform_angle += 360;

        platform_hit_normal = hit.normal;
        if(verbose_movement) Debug.Log("Platform angle: " + platform_angle);
        break;
      }
    }

    /* try to replace current platform with old if no platforms found */
    if (current_platform == null) current_platform = old_platform;

    /* make sure there is a known platform before we apply movement */
    if (current_platform != null) {
      Vector2 velocity = rigidbody.velocity;

      /* get the movement inputs from the left stick */
      float horizontal_movement = input.GetHorizontalLeftStick();
      float vertical_movement = input.GetVerticalLeftStick();
      
      /* angle of the movement joystick */
      float movement_angle = (Mathf.Atan2(horizontal_movement, vertical_movement) * Mathf.Rad2Deg + 360) % 360;
      if (verbose_movement) Debug.Log("Movement angle: " + movement_angle);
        
      /* get the direction from the movement angle */
      Vector2 movement_direction = new Vector2(Mathf.Sin(movement_angle * Mathf.Deg2Rad), Mathf.Cos(movement_angle * Mathf.Deg2Rad));
      if(show_movement) Debug.DrawRay(transform.position, movement_direction * move_speed, movement_direction_color);
      
      /* the positive and negative angle (pos and neg just a diffrentiater no other importance) */
      float platform_positive_angle = platform_angle + 90f;
      float platform_negative_angle = platform_angle - 90f;
        
      /* get the platform directions */
      Vector2 platform_direction_positive = new Vector2(Mathf.Sin(platform_positive_angle * Mathf.Deg2Rad), Mathf.Cos(platform_positive_angle * Mathf.Deg2Rad));
      if(show_movement) Debug.DrawRay(transform.position, platform_direction_positive * move_speed, platform_direction_color);
        
      Vector2 platform_direction_negative = new Vector2(Mathf.Sin(platform_negative_angle * Mathf.Deg2Rad), Mathf.Cos(platform_negative_angle * Mathf.Deg2Rad));
      if(show_movement) Debug.DrawRay(transform.position, platform_direction_negative * move_speed, platform_direction_color);
      
      /* make sure player actually wants to apply movement forces */
      bool apply_stop_drag;
      if (horizontal_movement != 0 || vertical_movement != 0) {
        /* get the leniency directions (leniency 2 mainly for drawing ray) */
        Vector2 movement_leniency_positive = new Vector2(Mathf.Sin((platform_positive_angle + leniency_angle) * Mathf.Deg2Rad), Mathf.Cos((platform_positive_angle + leniency_angle) * Mathf.Deg2Rad));
        Vector2 movement_leniency_positive2 = new Vector2(Mathf.Sin((platform_positive_angle - leniency_angle) * Mathf.Deg2Rad), Mathf.Cos((platform_positive_angle - leniency_angle) * Mathf.Deg2Rad));
        if(show_movement) Debug.DrawRay(transform.position, movement_leniency_positive * move_speed, movement_leniency_color);
        if(show_movement) Debug.DrawRay(transform.position, movement_leniency_positive2 * move_speed, movement_leniency_color);
        
        Vector2 movement_leniency_negative = new Vector2(Mathf.Sin((platform_negative_angle + leniency_angle) * Mathf.Deg2Rad), Mathf.Cos((platform_negative_angle + leniency_angle) * Mathf.Deg2Rad));
        Vector2 movement_leniency_negative2 = new Vector2(Mathf.Sin((platform_negative_angle - leniency_angle) * Mathf.Deg2Rad), Mathf.Cos((platform_negative_angle - leniency_angle) * Mathf.Deg2Rad));
        if(show_movement) Debug.DrawRay(transform.position, movement_leniency_negative * move_speed, movement_leniency_color);
        if(show_movement) Debug.DrawRay(transform.position, movement_leniency_negative2 * move_speed, movement_leniency_color);

        /* compute the distance between leniency and platform */
        float leniency_distance_positive = Vector2.Distance(platform_direction_positive, movement_leniency_positive);
        float leniency_distance_negative = Vector2.Distance(platform_direction_negative, movement_leniency_negative);

        /* compute distance between movement direction and platform direction */
        float movement_distance_positive = Vector2.Distance(platform_direction_positive, movement_direction);
        float movement_distance_negative = Vector2.Distance(platform_direction_negative, movement_direction);

        /* move in direction of the positive platform */
        if (movement_distance_positive <= leniency_distance_positive) {
          velocity.x = Mathf.SmoothDamp(velocity.x, platform_direction_positive.x * move_speed, ref velocity_x_smoothing, is_grounded ? ground_acceleration : air_acceleration);
          velocity.y = Mathf.SmoothDamp(velocity.y, platform_direction_positive.y * move_speed, ref velocity_y_smoothing, is_grounded ? ground_acceleration : air_acceleration);
        }

        /* move in direction of the negative platform */
        if (movement_distance_negative <= leniency_distance_negative) {
          velocity.x = Mathf.SmoothDamp(velocity.x, platform_direction_negative.x * move_speed, ref velocity_x_smoothing, is_grounded ? ground_acceleration : air_acceleration);
          velocity.y = Mathf.SmoothDamp(velocity.y, platform_direction_negative.y * move_speed, ref velocity_y_smoothing, is_grounded ? ground_acceleration : air_acceleration);
        }

        apply_stop_drag = false;
      } else {
        apply_stop_drag = is_grounded;
      }

      /* jump direction */
      if (input.GetButton3Down() && is_grounded) {
        apply_stop_drag = false;
        /* if angle selected than shoot at an angle */
        if (horizontal_movement != 0 || vertical_movement != 0) {
          velocity += (platform_hit_normal + new Vector2(horizontal_movement, vertical_movement)) * jump_angle_force;
        } else {
          velocity += platform_hit_normal * jump_angle_force;
        //  velocity += new Vector2(jump_direction.x + MaxJumpVelocity().x, jump_direction.y + MaxJumpVelocity().y);
        }
      }
      
      /* apply the stop drag (does not let the player slide) */
      if (apply_stop_drag) {
        rigidbody.drag = movement_linear_drag;
        rigidbody.freezeRotation = true;
      } else {
        rigidbody.drag = 0f;
        rigidbody.freezeRotation = false;
      }
      
      /* update velocity */
      rigidbody.velocity = velocity;

      /* apply jump, movement, and drag to the child components if allowed */
      if (apply_movement_tochild) {
        foreach (Transform child in child_transforms) {
          Rigidbody2D child_rigidbody = child.gameObject.GetComponent<Rigidbody2D>();
          if (child_rigidbody) {
            if (apply_stop_drag) {
              child_rigidbody.drag = movement_linear_drag;
              child_rigidbody.freezeRotation = true;
            } else {
              child_rigidbody.drag = 0f;
              child_rigidbody.freezeRotation = false;
            }
            
            child_rigidbody.velocity = velocity;
          }
        }
      }
      
      /* update old platform */
      old_platform = current_platform;
    }
  }

  public override float JumpHeight() {
    return jump_height;
  }

  public override float JumpApexTime() {
    return jump_apex_time;
  }

  /// <summary>
  /// Calculates max jump velocty from preset jump height and calculated gravity.
  /// </summary>
  /// <returns>Calculated max jump velocity.</returns>
  private Vector2 MaxJumpVelocity() {
    return new Vector2(Mathf.Abs(GetGravity().x), Mathf.Abs(GetGravity().y)) * jump_apex_time;
  }
  
  /// <summary>
  /// Check if the player is touching anything in direction of gravity.
  /// </summary>
  /// <returns>True if touching ground otherwise false.</returns>
  private bool IsGrounded(bool visualize = false) {
    HashSet<RaycastHit2D> hits = GetObjectsInView(GetGravity(), ground_fov_angle, ground_ray_count, ground_ray_length, visualize);
    foreach (RaycastHit2D hit in hits) {
      if (LayerMask.LayerToName(hit.transform.gameObject.layer) != LayerMask.LayerToName(gameObject.layer)) {
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
    
    foreach (Transform origins in raycast_origins) {
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
        
        if(visualize) Debug.DrawRay(start_position, angle_direction * _ray_length, visualize_fov_inbetween);
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
      
      if(visualize) Debug.DrawRay(start_position, start_direction * _ray_length, visualize_fov_edge);
      if(visualize) Debug.DrawRay(start_position, end_direction * _ray_length, visualize_fov_edge);
    }

    return game_objects;
  }
  
  /// <summary>
  /// Set the input for this player.
  /// Important: this can be used by an AI where AI would be the
  /// "controller" which can replicate player moves if need to be.
  /// </summary>
  /// <param name="_input">Input of the player.</param>
  protected void SetInput(Input2D _input) {
    input = _input;
  }
}

/// <summary>
/// It holds the transform of a child (efficient way to pass around gameobject), and
/// a ray cast origin bool denoting if the child should have a grounded raycast coming out of it.
/// </summary>
[Serializable]
internal class ChildComponent {
  [Tooltip("The transform of the child component.")] [SerializeField]
  public Transform Child;
  [Tooltip("Should the child object be an origin of raycasting.")] [SerializeField]
  public bool RaycastOrigin;
}

/// <summary>
/// Used to store the movement drags to restore at a later time.
/// </summary>
internal class Drag {
  public float AngularDrag { get; private set; }
  public float LinearDrag { get; private set; }
  
  public void SetDrags(float _angular_drag, float _linear_drag) {
    AngularDrag = _angular_drag;
    LinearDrag = _linear_drag;
  }
}