using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericPlayer : GravityField {
  /* the config variables for the player */
  public PlayerConfigurator configurator;

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

  /* smoothing velocity of moving in direction of x or y */
  private float velocity_x_smoothing;
  private float velocity_y_smoothing;

  /* contains the last grounded platforms angle and hit normal vector */
  protected float platform_angle { get; private set; }
  private Vector2 platform_hit_normal;

  /* lock and unlock for the gravity stick */
  private bool set_fixed_gravity = false;
  /* previous input accepted from the gravity stick */
  private Vector2 prevInput = Vector2.zero;
  
  /* all the accepted inputs */
  private float horizontal_gravity;
  private float vertical_gravity;
  private float left_trigger;
  private float right_trigger;
  private float horizontal_movement;
  private float vertical_movement;
  private bool jump_button_down;
  private bool right_stick_clicked;

  /* store the value of the ground check every update as its more efficient to not use raycast and calculations more than once
     in the same frame */
  protected bool is_grounded { get; private set; }

  protected virtual void Start() {
    if (!configurator) configurator = gameObject.GetComponent<PlayerConfigurator>();

    /* init hashset raycast origins */
    raycast_origins = new HashSet<Transform>();
    /* if the main object is a raycast origin then add to the collection */
    if (configurator.is_raycast_origin) raycast_origins.Add(transform);

    /* make sure the child components array is not 0 otherwise dont so anything to it */
    if (configurator.ChildComponents.Length != 0) {
      child_transforms = new HashSet<Transform>();

      /* add to set of child transforms, and if they are raycast origins then add to that set as well */
      foreach (ChildComponent child_component in configurator.ChildComponents) {
        child_transforms.Add(child_component.Child);
        if (child_component.RaycastOrigin) raycast_origins.Add(child_component.Child);
      }
    }

    /* if allowed to apply gravity to each child component do it */
    if (configurator.apply_gravity_tochild) {
      ApplyGravityTo(child_transforms);
    }

    /* init */
    normal_movement_drags = new Drag();
    restored_drag = true;
    gravity_depletion_rate = (configurator.min_gravity_stamina - configurator.max_gravity_stamina) / configurator.gravity_depletion_time;
    gravity_stamina = configurator.max_gravity_stamina;
    gravity_field_transition_rate = (MinRadius - MaxRadius) / configurator.gravity_field_transition_time;

    if (configurator.verbose_gravity) Debug.Log("Gravity depletion rate: " + gravity_depletion_rate);
    if (configurator.verbose_gravity) Debug.Log("Gravity field transition rate: " + gravity_field_transition_rate);

    Physics2D.gravity = GetGravity();
  }

  protected override void Update() {
    /* make sure we have something to take input from */
    if (input != null) {
      /* check if player is grounded (store this check as its efficient to not use raycast system more than once
         in the same update loop */
      is_grounded = IsGrounded(configurator.visualize_ground_check);

      /* get the gravity inputs from joystick */
      horizontal_gravity = input.GetHorizontalRightStick();
      vertical_gravity = input.GetVerticalRightStick();

      /* create magnitude deadzone by limiting the range of stick from 0 to set
         gravity deadzone */
      Vector2 stick_input = new Vector2(horizontal_gravity, vertical_gravity);
      if (stick_input.magnitude < configurator.gravity_deadzone) {
        horizontal_gravity = 0;
        vertical_gravity = 0;
      }

      /* change only when the inputs are not 0 */
      if (gravity_stamina != 0 && (horizontal_gravity != 0 || vertical_gravity != 0)) {
        /* store the movement drags */
        if (restored_drag) {
          normal_movement_drags.SetDrags(rigidbody.angularDrag, rigidbody.drag);
          restored_drag = false;
        }

        /* change to gravity drags */
        rigidbody.angularDrag = configurator.gravity_angular_drag;
        rigidbody.drag = configurator.gravity_linear_drag;

        /* apply jump, movement, and drag to the child components if allowed */
        if (configurator.apply_movement_tochild) {
          foreach (Transform child in child_transforms) {
            Rigidbody2D child_rigidbody = child.gameObject.GetComponent<Rigidbody2D>();
            if (child_rigidbody) {
              child_rigidbody.drag = configurator.gravity_linear_drag;
              child_rigidbody.angularDrag = configurator.gravity_angular_drag;
            }
          }
        }

        /* applies constant force (due to it being normalized) */
        Vector2 new_gravity = new Vector2(horizontal_gravity, vertical_gravity).normalized;
        if (configurator.verbose_gravity) Debug.Log("Gravity Direction: " + new_gravity);

        /* set the gravity to the new selected gravity */
        if (!set_fixed_gravity) {
          SetGravity(new_gravity);
        }

        /* deplete gravity as its being manipulated */
        gravity_stamina = Mathf.Clamp(gravity_stamina + gravity_depletion_rate * Time.deltaTime, configurator.min_gravity_stamina, configurator.max_gravity_stamina);
        if (configurator.verbose_gravity) Debug.Log("Gravity Stamina: " + gravity_stamina);
      } else {
        rigidbody.angularDrag = 0f;
        rigidbody.drag = 0f;

        /* apply jump, movement, and drag to the child components if allowed */
        if (configurator.apply_movement_tochild) {
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
      left_trigger = input.GetLeftTrigger();
      right_trigger = input.GetRightTrigger();

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
        gravity_stamina = Mathf.Clamp(gravity_stamina - gravity_depletion_rate * Time.deltaTime, configurator.min_gravity_stamina, configurator.max_gravity_stamina);
        if (configurator.verbose_gravity) Debug.Log("Gravity Stamina: " + gravity_stamina);

        /* restore movement drags */
        if (!restored_drag) {
          rigidbody.angularDrag = normal_movement_drags.AngularDrag;
          rigidbody.drag = normal_movement_drags.LinearDrag;
          restored_drag = true;
        }
      }

      /* get the inputs for movement */
      if (horizontal_movement == 0f) horizontal_movement = input.GetHorizontalLeftStick();
      if (vertical_movement == 0f) vertical_movement = input.GetVerticalLeftStick();
      if (!jump_button_down) jump_button_down = input.GetButton3Down();
      if (!right_stick_clicked) right_stick_clicked = input.GetRightStickDown();
      if (!jump_button_down) jump_button_down = input.GetRightBumperDown();
    }

    if (configurator.show_gravity) Debug.DrawRay(transform.position, GetGravity(), configurator.gravity_ray_color);

    /* update the gravity field fill to represent the gravity stamina */
    if (ReleasedGravity()) gravity_stamina = Mathf.Clamp(gravity_stamina - 5, configurator.min_gravity_stamina, configurator.max_gravity_stamina);
    ChangeGravityFill(Mathf.Clamp01(gravity_stamina / configurator.max_gravity_stamina));

    /* run update in base class (applies gravity) */
    base.Update();
  }

  protected virtual void FixedUpdate() {
    HandleMovement();

    /* we must have handled the inputs */
    horizontal_movement = 0f;
    vertical_movement = 0f;
    if (jump_button_down) jump_button_down = false;
    if (right_stick_clicked) right_stick_clicked = false;

    /* clamp velocity */
    rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, configurator.max_velocity);

    if (configurator.apply_movement_tochild) {
      foreach (Transform child in child_transforms) {
        Rigidbody2D child_rigidbody = child.gameObject.GetComponent<Rigidbody2D>();
        if (child_rigidbody) {
          child_rigidbody.velocity = Vector2.ClampMagnitude(child_rigidbody.velocity, configurator.max_velocity);
        }
      }
    }
  }

  /// <summary>
  /// Modulo operator function.
  /// https://answers.unity.com/questions/380035/c-modulus-is-wrong-1.html
  /// </summary>
  protected static float fmod(float a, float b) {
    return a - b * Mathf.Floor(a / b);
  }

  /// <summary>
  /// Called after releasing the gravity stick.
  /// </summary>
  /// <returns>If the gravity stick was changed.</returns>
  private bool ReleasedGravity() {
    Vector2 stick_input = new Vector2(horizontal_gravity, vertical_gravity);
    if (stick_input.magnitude < configurator.gravity_deadzone) {
      stick_input = Vector2.zero;
    }

    if (is_grounded) {
      prevInput = Vector2.one;
      return false;
    }

    if (prevInput == Vector2.zero) {
      prevInput = stick_input;
      return stick_input != Vector2.zero;
    }

    prevInput = stick_input;
    return false;
  }

  /// <summary>
  /// Get the angle of a vector.
  /// </summary>
  /// <param name="x">X component of the vector.</param>
  /// <param name="y">Y component of the vector.</param>
  /// <returns>Angle of the vector.</returns>
  protected static float GetAngle(float x, float y) {
    float tmp_angle = Mathf.Atan2(x, y) * Mathf.Rad2Deg;
    /* get angle between 0 - 360, even handle negative signs with modulus */
    tmp_angle = fmod(tmp_angle, 360);
    if (tmp_angle < 0) tmp_angle += 360;

    return tmp_angle;
  }

  /// <summary>
  /// Handles all basic movement for this player.
  /// </summary>
  private void HandleMovement() {
    /* angle of the movement joystick */
    float movement_angle = GetAngle(horizontal_movement, vertical_movement);
    if (configurator.verbose_movement) Debug.Log("Movement angle: " + movement_angle);

    /* get the direction from the movement angle */
    Vector2 movement_direction = new Vector2(Mathf.Sin(movement_angle * Mathf.Deg2Rad), Mathf.Cos(movement_angle * Mathf.Deg2Rad));
    if (configurator.show_movement) Debug.DrawRay(transform.position, movement_direction * configurator.move_speed, configurator.movement_direction_color);

    Vector2 velocity = rigidbody.velocity;
    bool apply_stop_drag = true;

    /* if we are on valid platform to allow movement and jump */
    if (platform_angle != -1f) {
      /* set gravity in direction of the platform if we are on platform */
      if (right_stick_clicked) {
        Vector2 platform_direction = new Vector2(Mathf.Sin(platform_angle * Mathf.Deg2Rad), Mathf.Cos(platform_angle * Mathf.Deg2Rad));
        SetGravity(-platform_direction);
        set_fixed_gravity = true;
        Invoke("UnlockGravity", 0.2f);
      }

      /* the positive and negative angle (pos and neg just a diffrentiater no other importance) */
      float platform_positive_angle = platform_angle + 90f;
      float platform_negative_angle = platform_angle - 90f;

      /* get the platform directions */
      Vector2 platform_direction_positive = new Vector2(Mathf.Sin(platform_positive_angle * Mathf.Deg2Rad), Mathf.Cos(platform_positive_angle * Mathf.Deg2Rad));
      if (configurator.show_movement) Debug.DrawRay(transform.position, platform_direction_positive * configurator.move_speed, configurator.platform_direction_color);

      Vector2 platform_direction_negative = new Vector2(Mathf.Sin(platform_negative_angle * Mathf.Deg2Rad), Mathf.Cos(platform_negative_angle * Mathf.Deg2Rad));
      if (configurator.show_movement) Debug.DrawRay(transform.position, platform_direction_negative * configurator.move_speed, configurator.platform_direction_color);

      /* make sure player actually wants to apply movement forces */
      if (horizontal_movement != 0f || vertical_movement != 0f) {
        /* get the leniency directions (leniency 2 mainly for drawing ray) */
        Vector2 movement_leniency_positive = new Vector2(Mathf.Sin((platform_positive_angle + configurator.leniency_angle) * Mathf.Deg2Rad), Mathf.Cos((platform_positive_angle + configurator.leniency_angle) * Mathf.Deg2Rad));
        Vector2 movement_leniency_positive2 = new Vector2(Mathf.Sin((platform_positive_angle - configurator.leniency_angle) * Mathf.Deg2Rad), Mathf.Cos((platform_positive_angle - configurator.leniency_angle) * Mathf.Deg2Rad));
        if (configurator.show_movement) Debug.DrawRay(transform.position, movement_leniency_positive * configurator.move_speed, configurator.movement_leniency_color);
        if (configurator.show_movement) Debug.DrawRay(transform.position, movement_leniency_positive2 * configurator.move_speed, configurator.movement_leniency_color);

        Vector2 movement_leniency_negative = new Vector2(Mathf.Sin((platform_negative_angle + configurator.leniency_angle) * Mathf.Deg2Rad), Mathf.Cos((platform_negative_angle + configurator.leniency_angle) * Mathf.Deg2Rad));
        Vector2 movement_leniency_negative2 = new Vector2(Mathf.Sin((platform_negative_angle - configurator.leniency_angle) * Mathf.Deg2Rad), Mathf.Cos((platform_negative_angle - configurator.leniency_angle) * Mathf.Deg2Rad));
        if (configurator.show_movement) Debug.DrawRay(transform.position, movement_leniency_negative * configurator.move_speed, configurator.movement_leniency_color);
        if (configurator.show_movement) Debug.DrawRay(transform.position, movement_leniency_negative2 * configurator.move_speed, configurator.movement_leniency_color);

        /* compute the distance between leniency and platform */
        float leniency_distance_positive = Vector2.Distance(platform_direction_positive, movement_leniency_positive);
        float leniency_distance_negative = Vector2.Distance(platform_direction_negative, movement_leniency_negative);

        /* compute distance between movement direction and platform direction */
        float movement_distance_positive = Vector2.Distance(platform_direction_positive, movement_direction);
        float movement_distance_negative = Vector2.Distance(platform_direction_negative, movement_direction);

        /* move in direction of the positive platform */
        if (movement_distance_positive <= leniency_distance_positive) {
          velocity.x = Mathf.SmoothDamp(velocity.x, platform_direction_positive.x * configurator.move_speed, ref velocity_x_smoothing, configurator.ground_acceleration);
          velocity.y = Mathf.SmoothDamp(velocity.y, platform_direction_positive.y * configurator.move_speed, ref velocity_y_smoothing, configurator.ground_acceleration);
          apply_stop_drag = false;
        }

        /* move in direction of the negative platform */
        if (movement_distance_negative <= leniency_distance_negative) {          
          velocity.x = Mathf.SmoothDamp(velocity.x, platform_direction_negative.x * configurator.move_speed, ref velocity_x_smoothing, configurator.ground_acceleration);
          velocity.y = Mathf.SmoothDamp(velocity.y, platform_direction_negative.y * configurator.move_speed, ref velocity_y_smoothing, configurator.ground_acceleration);
          apply_stop_drag = false;
        }
      }

      /* jump direction */
      if (jump_button_down && is_grounded) {
        apply_stop_drag = false;
        Vector2 hybrid_jump = platform_hit_normal + movement_direction;
        velocity += hybrid_jump * configurator.jump_force;
      }
    } else { /* assume we are either not grounded or not on valid platform */
      if (!is_grounded) {
        /* we want to move in air t(-.-t) */
        if (horizontal_movement != 0f | vertical_movement != 0f) {
          /* angle of gravity */
          float gravity_angle = GetAngle(GetGravity().x, GetGravity().y);

          if (configurator.verbose_movement) Debug.Log("Gravity angle: " + gravity_angle);

          /* the positive and negative angle (pos and neg just a diffrentiater no other importance) */
          float gravity_positive_angle = gravity_angle + 90f;
          float gravity_negative_angle = gravity_angle - 90f;

          /* get the gravity directions */
          Vector2 gravity_direction_positive = new Vector2(Mathf.Sin(gravity_positive_angle * Mathf.Deg2Rad), Mathf.Cos(gravity_positive_angle * Mathf.Deg2Rad));
          if (configurator.show_movement) Debug.DrawRay(transform.position, gravity_direction_positive * configurator.move_speed, configurator.platform_direction_color);

          Vector2 gravity_direction_negative = new Vector2(Mathf.Sin(gravity_negative_angle * Mathf.Deg2Rad), Mathf.Cos(gravity_negative_angle * Mathf.Deg2Rad));
          if (configurator.show_movement) Debug.DrawRay(transform.position, gravity_direction_negative * configurator.move_speed, configurator.platform_direction_color);

          /* get the leniency directions (leniency 2 mainly for drawing ray) */
          Vector2 movement_leniency_positive = new Vector2(Mathf.Sin((gravity_positive_angle + configurator.leniency_angle) * Mathf.Deg2Rad), Mathf.Cos((gravity_positive_angle + configurator.leniency_angle) * Mathf.Deg2Rad));
          Vector2 movement_leniency_positive2 = new Vector2(Mathf.Sin((gravity_positive_angle - configurator.leniency_angle) * Mathf.Deg2Rad), Mathf.Cos((gravity_positive_angle - configurator.leniency_angle) * Mathf.Deg2Rad));
          if (configurator.show_movement) Debug.DrawRay(transform.position, movement_leniency_positive * configurator.move_speed, configurator.movement_leniency_color);
          if (configurator.show_movement) Debug.DrawRay(transform.position, movement_leniency_positive2 * configurator.move_speed, configurator.movement_leniency_color);

          Vector2 movement_leniency_negative = new Vector2(Mathf.Sin((gravity_negative_angle + configurator.leniency_angle) * Mathf.Deg2Rad), Mathf.Cos((gravity_negative_angle + configurator.leniency_angle) * Mathf.Deg2Rad));
          Vector2 movement_leniency_negative2 = new Vector2(Mathf.Sin((gravity_negative_angle - configurator.leniency_angle) * Mathf.Deg2Rad), Mathf.Cos((gravity_negative_angle - configurator.leniency_angle) * Mathf.Deg2Rad));
          if (configurator.show_movement) Debug.DrawRay(transform.position, movement_leniency_negative * configurator.move_speed, configurator.movement_leniency_color);
          if (configurator.show_movement) Debug.DrawRay(transform.position, movement_leniency_negative2 * configurator.move_speed, configurator.movement_leniency_color);

          /* compute the distance between leniency and platform */
          float leniency_distance_positive = Vector2.Distance(gravity_direction_positive, movement_leniency_positive);
          float leniency_distance_negative = Vector2.Distance(gravity_direction_negative, movement_leniency_negative);

          /* compute distance between movement direction and platform direction */
          float movement_distance_positive = Vector2.Distance(gravity_direction_positive, movement_direction);
          float movement_distance_negative = Vector2.Distance(gravity_direction_negative, movement_direction);

          /* move in direction of the positive platform */
          if (movement_distance_positive <= leniency_distance_positive) {
            //rigidbody.AddForce(gravity_direction_positive * configurator.air_speed, ForceMode2D.Force);
            velocity = Vector3.Lerp(velocity , gravity_direction_positive * configurator.air_speed, configurator.air_acceleration);
          }

          /* move in direction of the negative platform */
          if (movement_distance_negative <= leniency_distance_negative) {
            //rigidbody.AddForce(gravity_direction_negative * configurator.air_speed, ForceMode2D.Force);
            velocity = Vector3.Lerp(velocity , gravity_direction_negative * configurator.air_speed, configurator.air_acceleration);
          }
        }

        apply_stop_drag = false;
      } else { /* not on a valid platform to allow movement */
      }
    }

    /* apply the stop drag (does not let the player slide) */
    rigidbody.drag = apply_stop_drag ? configurator.movement_linear_drag : 0f;

    /* update velocity */
    rigidbody.velocity = velocity;

    /* apply jump, movement, and drag to the child components if allowed */
    if (configurator.apply_movement_tochild) {
      foreach (Transform child in child_transforms) {
        Rigidbody2D child_rigidbody = child.gameObject.GetComponent<Rigidbody2D>();
        if (child_rigidbody) {
          child_rigidbody.drag = apply_stop_drag ? configurator.movement_linear_drag : 0f;
          child_rigidbody.velocity = velocity;
        }
      }
    }
  }

  /// <summary>
  /// Check if the player is touching anything in direction of gravity.
  /// </summary>
  /// <returns>True if touching ground otherwise false.</returns>
  private bool IsGrounded(bool visualize = false) {
    HashSet<RaycastHit2D> hits = GetObjectsInView(GetGravity(), configurator.ground_fov_angle, configurator.ground_ray_count, configurator.ground_ray_length, visualize);
    foreach (RaycastHit2D hit in hits) {
      if (LayerMask.LayerToName(hit.transform.gameObject.layer) != LayerMask.LayerToName(gameObject.layer)) {
        Vector2 hit_normal = hit.normal;
        /* if the object has children then use the parent's rotation to calculate the normal */
        if (hit.collider.gameObject.transform.childCount > 0) {
          hit_normal = Quaternion.AngleAxis(hit.collider.gameObject.transform.rotation.eulerAngles.z, Vector3.forward) * hit.normal;
        }

        /* get platform information we just hit */
        float platform_angle_update = GetAngle(hit_normal.x, hit_normal.y);

        Vector2 platform_comparator = hit.transform.right;
        float angle_diff = fmod(Vector2.Angle(platform_comparator, GetGravity()), 180);

        /* make sure the platform is within the movement angle (avoids walking upwards on platform */
        if (angle_diff >= configurator.movement_leniency_angle) {
          platform_angle = platform_angle_update;
          platform_hit_normal = hit_normal;
        } else {
          platform_angle = -1f;
          platform_hit_normal = Vector2.negativeInfinity;
        }

        return true;
      }
    }

    platform_angle = -1f;
    platform_hit_normal = Vector2.negativeInfinity;
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
          if (hit.transform.gameObject != gameObject) game_objects.Add(hit);
        }

        if (visualize) Debug.DrawRay(start_position, angle_direction * _ray_length, configurator.visualize_fov_inbetween);
      }

      /* check if we hit something in the start direction */
      RaycastHit2D start_hit = Physics2D.Raycast(start_position, start_direction, _ray_length);
      if (start_hit) {
        /* add to the hashset */
        if (start_hit.transform.gameObject != gameObject) game_objects.Add(start_hit);
      }

      /* check if we hit something in the end direction */
      RaycastHit2D end_hit = Physics2D.Raycast(start_position, end_direction, _ray_length);
      if (end_hit) {
        /* add to the hashset */
        if (end_hit.transform.gameObject != gameObject) game_objects.Add(end_hit);
      }

      if (visualize) Debug.DrawRay(start_position, start_direction * _ray_length, configurator.visualize_fov_edge);
      if (visualize) Debug.DrawRay(start_position, end_direction * _ray_length, configurator.visualize_fov_edge);
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

  /// <summary>
  /// Apply damage to this player.
  /// </summary>
  /// <param name="amount">Amount of damage to apply.</param>
  public void Damage(int amount) {
    configurator.cur_hp -= amount;
    if (configurator.cur_hp <= 0) {
      Death();
    }
  }

  /// <summary>
  /// A pause before accepting values from the gravity stick.
  /// This just unlocks the "pause"
  /// </summary>
  private void UnlockGravity() {
    set_fixed_gravity = false;
  }

  /// <summary>
  /// Called when health is equal to or below 0.
  /// </summary>
  protected abstract void Death();
}

/// <summary>
/// It holds the transform of a child (efficient way to pass around gameobject), and
/// a ray cast origin bool denoting if the child should have a grounded raycast coming out of it.
/// </summary>
[Serializable]
public class ChildComponent {
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