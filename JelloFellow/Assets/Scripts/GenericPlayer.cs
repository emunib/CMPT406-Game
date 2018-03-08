using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenericPlayer : GravityField {
  public PlayerConfigurator config;

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

  /* true if we just manipulated gravity (locks movement until we are grounded)
     ensures we still don't use the old platform angle to influence movement */
  private bool just_changed_gravity;

  /* contains the last grounded platforms angle and hit normal vector */
  private float platform_angle = 0f;
  private Vector2 platform_hit_normal;
  
  private bool set_fixed_gravity = false;


  /* store the value of the ground check every update as its more efficient to not use raycast and calculations more than once
     in the same frame */
  protected bool is_grounded { get; private set; }

  protected virtual void Start() {
    if (!config) config = gameObject.GetComponent<PlayerConfigurator>();

    /* init hashset raycast origins */
    raycast_origins = new HashSet<Transform>();
    /* if the main object is a raycast origin then add to the collection */
    if (config.is_raycast_origin) raycast_origins.Add(transform);

    /* make sure the child components array is not 0 otherwise dont so anything to it */
    if (config.ChildComponents.Length != 0) {
      child_transforms = new HashSet<Transform>();

      /* add to set of child transforms, and if they are raycast origins then add to that set as well */
      foreach (ChildComponent child_component in config.ChildComponents) {
        child_transforms.Add(child_component.Child);
        if (child_component.RaycastOrigin) raycast_origins.Add(child_component.Child);
      }
    }

    /* if allowed to apply gravity to each child component do it */
    if (config.apply_gravity_tochild) {
      ApplyGravityTo(child_transforms);
    }

    /* init */
    normal_movement_drags = new Drag();
    restored_drag = true;
    gravity_depletion_rate = (config.min_gravity_stamina - config.max_gravity_stamina) / config.gravity_depletion_time;
    gravity_stamina = config.max_gravity_stamina;
    gravity_field_transition_rate = (MinRadius - MaxRadius) / config.gravity_field_transition_time;
    just_changed_gravity = false;

    if (config.verbose_gravity) Debug.Log("Gravity depletion rate: " + gravity_depletion_rate);
    if (config.verbose_gravity) Debug.Log("Gravity field transition rate: " + gravity_field_transition_rate);

    Physics2D.gravity = GetGravity();
  }

  protected override void Update() {
    /* make sure we have something to take input from */
    if (input != null) {
      /* check if player is grounded (store this check as its efficient to not use raycast system more than once
         in the same update loop */
      is_grounded = IsGrounded(config.visualize_ground_check);

      /* get the gravity inputs from joystick */
      float horizontal_gravity = input.GetHorizontalRightStick();
      float vertical_gravity = input.GetVerticalRightStick();
      
      Vector2 stickInput = new Vector2(horizontal_gravity, vertical_gravity);
      if (stickInput.magnitude < config.gravity_deadzone)
      {
        horizontal_gravity = 0;
        vertical_gravity = 0;
      }

      /* change only when the inputs are not 0 */
      if (gravity_stamina != 0 && (horizontal_gravity != 0 || vertical_gravity != 0)) {
        just_changed_gravity = true;

        /* store the movement drags */
        if (restored_drag) {
          normal_movement_drags.SetDrags(rigidbody.angularDrag, rigidbody.drag);
          restored_drag = false;
        }

        /* change to gravity drags */
        rigidbody.angularDrag = config.gravity_angular_drag;
        rigidbody.drag = config.gravity_linear_drag;

        /* apply jump, movement, and drag to the child components if allowed */
        if (config.apply_movement_tochild) {
          foreach (Transform child in child_transforms) {
            Rigidbody2D child_rigidbody = child.gameObject.GetComponent<Rigidbody2D>();
            if (child_rigidbody) {
              child_rigidbody.drag = config.gravity_linear_drag;
              child_rigidbody.angularDrag = config.gravity_angular_drag;
            }
          }
        }

        /* applies constant force (due to it being normalized) */
        Vector2 new_gravity = new Vector2(horizontal_gravity, vertical_gravity).normalized;
        if (config.verbose_gravity) Debug.Log("Gravity Direction: " + new_gravity);

        /* set the gravity to the new selected gravity */
        if (!set_fixed_gravity) {
          SetGravity(new_gravity);
        }

        /* deplete gravity as its being manipulated */
        gravity_stamina = Mathf.Clamp(gravity_stamina + gravity_depletion_rate * Time.deltaTime, config.min_gravity_stamina, config.max_gravity_stamina);
        if (config.verbose_gravity) Debug.Log("Gravity Stamina: " + gravity_stamina);
      } else {
        rigidbody.angularDrag = 0f;
        rigidbody.drag = 0f;

        /* apply jump, movement, and drag to the child components if allowed */
        if (config.apply_movement_tochild) {
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
        gravity_stamina = Mathf.Clamp(gravity_stamina - gravity_depletion_rate * Time.deltaTime, config.min_gravity_stamina, config.max_gravity_stamina);
        if (config.verbose_gravity) Debug.Log("Gravity Stamina: " + gravity_stamina);

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
        //HandleMovement();
      }
      
      HandleMovement2();
    }

    if (config.show_gravity) Debug.DrawRay(transform.position, GetGravity(), config.gravity_ray_color);

    /* update the gravity field alpha to represent the gravity stamina */
    ChangeGravityAlpha(Mathf.Clamp01(gravity_stamina / config.max_gravity_stamina));
    
    /* run update in base class (applies gravity) */
    base.Update();
  }

  protected virtual void FixedUpdate() {
    rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, config.max_velocity);

    if (config.apply_movement_tochild)
    {
      foreach (var node in config.ChildComponents)
      {
        var body2D = node.Child.GetComponent<Rigidbody2D>();
        body2D.velocity = Vector2.ClampMagnitude(body2D.velocity, config.max_velocity);
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

  private void HandleMovement() {
    if (config.verbose_movement) Debug.Log("Handling Movement");

    if (input.GetRightStickDown()) {
      SetGravity(-platform_hit_normal);
      set_fixed_gravity = true;
      Invoke("UnlockGravity", 0.2f);
    }
    
    if (config.verbose_movement) Debug.Log("Platform angle: " + platform_angle);
    
    /* make sure there is a known platform before we apply movement */
    if (platform_angle != -1f) {
      Vector2 velocity = rigidbody.velocity;

      /* get the movement inputs from the left stick */
      float horizontal_movement = input.GetHorizontalLeftStick();
      float vertical_movement = input.GetVerticalLeftStick();

      /* angle of the movement joystick */
      float movement_angle = (Mathf.Atan2(horizontal_movement, vertical_movement) * Mathf.Rad2Deg + 360) % 360;
      if (config.verbose_movement) Debug.Log("Movement angle: " + movement_angle);

      /* get the direction from the movement angle */
      Vector2 movement_direction = new Vector2(Mathf.Sin(movement_angle * Mathf.Deg2Rad), Mathf.Cos(movement_angle * Mathf.Deg2Rad));
      if (config.show_movement) Debug.DrawRay(transform.position, movement_direction * config.move_speed, config.movement_direction_color);

      /* the positive and negative angle (pos and neg just a diffrentiater no other importance) */
      float platform_positive_angle = platform_angle + 90f;
      float platform_negative_angle = platform_angle - 90f;

      /* get the platform directions */
      Vector2 platform_direction_positive = new Vector2(Mathf.Sin(platform_positive_angle * Mathf.Deg2Rad), Mathf.Cos(platform_positive_angle * Mathf.Deg2Rad));
      if (config.show_movement) Debug.DrawRay(transform.position, platform_direction_positive * config.move_speed, config.platform_direction_color);

      Vector2 platform_direction_negative = new Vector2(Mathf.Sin(platform_negative_angle * Mathf.Deg2Rad), Mathf.Cos(platform_negative_angle * Mathf.Deg2Rad));
      if (config.show_movement) Debug.DrawRay(transform.position, platform_direction_negative * config.move_speed, config.platform_direction_color);

      /* make sure player actually wants to apply movement forces */
      bool apply_stop_drag;
      if (horizontal_movement != 0 || vertical_movement != 0) {
        /* get the leniency directions (leniency 2 mainly for drawing ray) */
        Vector2 movement_leniency_positive = new Vector2(Mathf.Sin((platform_positive_angle + config.leniency_angle) * Mathf.Deg2Rad), Mathf.Cos((platform_positive_angle + config.leniency_angle) * Mathf.Deg2Rad));
        Vector2 movement_leniency_positive2 = new Vector2(Mathf.Sin((platform_positive_angle - config.leniency_angle) * Mathf.Deg2Rad), Mathf.Cos((platform_positive_angle - config.leniency_angle) * Mathf.Deg2Rad));
        if (config.show_movement) Debug.DrawRay(transform.position, movement_leniency_positive * config.move_speed, config.movement_leniency_color);
        if (config.show_movement) Debug.DrawRay(transform.position, movement_leniency_positive2 * config.move_speed, config.movement_leniency_color);

        Vector2 movement_leniency_negative = new Vector2(Mathf.Sin((platform_negative_angle + config.leniency_angle) * Mathf.Deg2Rad), Mathf.Cos((platform_negative_angle + config.leniency_angle) * Mathf.Deg2Rad));
        Vector2 movement_leniency_negative2 = new Vector2(Mathf.Sin((platform_negative_angle - config.leniency_angle) * Mathf.Deg2Rad), Mathf.Cos((platform_negative_angle - config.leniency_angle) * Mathf.Deg2Rad));
        if (config.show_movement) Debug.DrawRay(transform.position, movement_leniency_negative * config.move_speed, config.movement_leniency_color);
        if (config.show_movement) Debug.DrawRay(transform.position, movement_leniency_negative2 * config.move_speed, config.movement_leniency_color);

        /* compute the distance between leniency and platform */
        float leniency_distance_positive = Vector2.Distance(platform_direction_positive, movement_leniency_positive);
        float leniency_distance_negative = Vector2.Distance(platform_direction_negative, movement_leniency_negative);

        /* compute distance between movement direction and platform direction */
        float movement_distance_positive = Vector2.Distance(platform_direction_positive, movement_direction);
        float movement_distance_negative = Vector2.Distance(platform_direction_negative, movement_direction);

        /* move in direction of the positive platform */
        if (movement_distance_positive <= leniency_distance_positive) {
          velocity.x = Mathf.SmoothDamp(velocity.x, platform_direction_positive.x * config.move_speed, ref velocity_x_smoothing, is_grounded ? config.ground_acceleration : config.air_acceleration);
          velocity.y = Mathf.SmoothDamp(velocity.y, platform_direction_positive.y * config.move_speed, ref velocity_y_smoothing, is_grounded ? config.ground_acceleration : config.air_acceleration);
        }

        /* move in direction of the negative platform */
        if (movement_distance_negative <= leniency_distance_negative) {
          velocity.x = Mathf.SmoothDamp(velocity.x, platform_direction_negative.x * config.move_speed, ref velocity_x_smoothing, is_grounded ? config.ground_acceleration : config.air_acceleration);
          velocity.y = Mathf.SmoothDamp(velocity.y, platform_direction_negative.y * config.move_speed, ref velocity_y_smoothing, is_grounded ? config.ground_acceleration : config.air_acceleration);
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
          Vector2 hybrid_jump = platform_hit_normal + new Vector2(horizontal_movement, vertical_movement) * config.jump_angle_coefficient;

          if (hybrid_jump.magnitude > config.jump_normalize_threshold) {
            hybrid_jump.Normalize();
          }

          
          
          velocity += hybrid_jump * config.jump_force;
          //rigidbody.AddForce(hybrid_jump * config.jump_angle_force, ForceMode2D.Impulse);
        } else {
          velocity += platform_hit_normal * config.jump_force;
          //}
        }
      }

      /* apply the stop drag (does not let the player slide) */
      if (apply_stop_drag) {
        rigidbody.drag = config.movement_linear_drag;
        rigidbody.freezeRotation = true;
      } else {
        rigidbody.drag = 0f;
        rigidbody.freezeRotation = true;
      }

      /* update velocity */
      rigidbody.velocity = velocity;

      /* apply jump, movement, and drag to the child components if allowed */
      if (config.apply_movement_tochild) {
        foreach (Transform child in child_transforms) {
          Rigidbody2D child_rigidbody = child.gameObject.GetComponent<Rigidbody2D>();
          if (child_rigidbody) {
            if (apply_stop_drag) {
              child_rigidbody.drag = config.movement_linear_drag;
              child_rigidbody.freezeRotation = true;
            } else {
              child_rigidbody.drag = 0f;
              child_rigidbody.freezeRotation = true;
            }

            child_rigidbody.velocity = velocity;
          }
        }
      }
    }
  }
  
   private void HandleMovement2() {
    if (config.verbose_movement) Debug.Log("Handling Movement");

    /* get the movement inputs from the left stick */
    float horizontal_movement = input.GetHorizontalLeftStick();
    float vertical_movement = input.GetVerticalLeftStick();

    /* angle of the movement joystick */
    float movement_angle = (Mathf.Atan2(horizontal_movement, vertical_movement) * Mathf.Rad2Deg + 360) % 360;
    if (config.verbose_movement) Debug.Log("Movement angle: " + movement_angle);

    /* get the direction from the movement angle */
    Vector2 movement_direction = new Vector2(Mathf.Sin(movement_angle * Mathf.Deg2Rad), Mathf.Cos(movement_angle * Mathf.Deg2Rad));
    if (config.show_movement) Debug.DrawRay(transform.position, movement_direction * config.move_speed, config.movement_direction_color);

    bool apply_stop_drag = true;

    Vector2 velocity = rigidbody.velocity;

    if (is_grounded) {
      if (config.verbose_movement) Debug.Log("Platform angle: " + platform_angle);
      
      if (input.GetRightStickDown()) {
        SetGravity(-platform_hit_normal);
        set_fixed_gravity = true;
        Invoke("UnlockGravity", 0.2f);
      }

      /* make sure there is a known platform before we apply movement */
      if (platform_angle != -1f) {
        /* the positive and negative angle (pos and neg just a diffrentiater no other importance) */
        float platform_positive_angle = platform_angle + 90f;
        float platform_negative_angle = platform_angle - 90f;

        /* get the platform directions */
        Vector2 platform_direction_positive = new Vector2(Mathf.Sin(platform_positive_angle * Mathf.Deg2Rad), Mathf.Cos(platform_positive_angle * Mathf.Deg2Rad));
        if (config.show_movement) Debug.DrawRay(transform.position, platform_direction_positive * config.move_speed, config.platform_direction_color);

        Vector2 platform_direction_negative = new Vector2(Mathf.Sin(platform_negative_angle * Mathf.Deg2Rad), Mathf.Cos(platform_negative_angle * Mathf.Deg2Rad));
        if (config.show_movement) Debug.DrawRay(transform.position, platform_direction_negative * config.move_speed, config.platform_direction_color);

        /* make sure player actually wants to apply movement forces */
        if (horizontal_movement != 0 || vertical_movement != 0) {
          /* get the leniency directions (leniency 2 mainly for drawing ray) */
          Vector2 movement_leniency_positive = new Vector2(Mathf.Sin((platform_positive_angle + config.leniency_angle) * Mathf.Deg2Rad), Mathf.Cos((platform_positive_angle + config.leniency_angle) * Mathf.Deg2Rad));
          Vector2 movement_leniency_positive2 = new Vector2(Mathf.Sin((platform_positive_angle - config.leniency_angle) * Mathf.Deg2Rad), Mathf.Cos((platform_positive_angle - config.leniency_angle) * Mathf.Deg2Rad));
          if (config.show_movement) Debug.DrawRay(transform.position, movement_leniency_positive * config.move_speed, config.movement_leniency_color);
          if (config.show_movement) Debug.DrawRay(transform.position, movement_leniency_positive2 * config.move_speed, config.movement_leniency_color);

          Vector2 movement_leniency_negative = new Vector2(Mathf.Sin((platform_negative_angle + config.leniency_angle) * Mathf.Deg2Rad), Mathf.Cos((platform_negative_angle + config.leniency_angle) * Mathf.Deg2Rad));
          Vector2 movement_leniency_negative2 = new Vector2(Mathf.Sin((platform_negative_angle - config.leniency_angle) * Mathf.Deg2Rad), Mathf.Cos((platform_negative_angle - config.leniency_angle) * Mathf.Deg2Rad));
          if (config.show_movement) Debug.DrawRay(transform.position, movement_leniency_negative * config.move_speed, config.movement_leniency_color);
          if (config.show_movement) Debug.DrawRay(transform.position, movement_leniency_negative2 * config.move_speed, config.movement_leniency_color);

          /* compute the distance between leniency and platform */
          float leniency_distance_positive = Vector2.Distance(platform_direction_positive, movement_leniency_positive);
          float leniency_distance_negative = Vector2.Distance(platform_direction_negative, movement_leniency_negative);

          /* compute distance between movement direction and platform direction */
          float movement_distance_positive = Vector2.Distance(platform_direction_positive, movement_direction);
          float movement_distance_negative = Vector2.Distance(platform_direction_negative, movement_direction);

          /* move in direction of the positive platform */
          if (movement_distance_positive <= leniency_distance_positive) {
            velocity.x = Mathf.SmoothDamp(velocity.x, platform_direction_positive.x * config.move_speed, ref velocity_x_smoothing, config.ground_acceleration);
            velocity.y = Mathf.SmoothDamp(velocity.y, platform_direction_positive.y * config.move_speed, ref velocity_y_smoothing, config.ground_acceleration);
          }

          /* move in direction of the negative platform */
          if (movement_distance_negative <= leniency_distance_negative) {
            velocity.x = Mathf.SmoothDamp(velocity.x, platform_direction_negative.x * config.move_speed, ref velocity_x_smoothing, config.ground_acceleration);
            velocity.y = Mathf.SmoothDamp(velocity.y, platform_direction_negative.y * config.move_speed, ref velocity_y_smoothing, config.ground_acceleration);
          }

          apply_stop_drag = false;
        }

        /* jump direction */
        if (input.GetButton3Down() && is_grounded) {
          apply_stop_drag = false;
          /* if angle selected than shoot at an angle */
          if (horizontal_movement != 0 || vertical_movement != 0) {
            Vector2 hybrid_jump = platform_hit_normal + new Vector2(horizontal_movement, vertical_movement) * config.jump_angle_coefficient;

            if (hybrid_jump.magnitude > config.jump_normalize_threshold) {
              hybrid_jump.Normalize();
            }

            velocity += hybrid_jump * config.jump_force;
          } else {
            velocity += platform_hit_normal * config.jump_force;
          }
        }

        /* apply the stop drag (does not let the player slide) */
        if (apply_stop_drag) {
          rigidbody.drag = config.movement_linear_drag;
          rigidbody.freezeRotation = true;
        } else {
          rigidbody.drag = 0f;
          rigidbody.freezeRotation = true;
        }
      }
    } else {
      if (horizontal_movement != 0 || vertical_movement != 0) {
        /* angle of the movement joystick */
        float gravity_angle = Mathf.Atan2(GetGravity().x, GetGravity().y) * Mathf.Rad2Deg;
        gravity_angle = fmod(gravity_angle, 360);
        if (gravity_angle < 0) gravity_angle += 360;

        if (config.verbose_movement) Debug.Log("Gravity angle: " + gravity_angle);

        /* the positive and negative angle (pos and neg just a diffrentiater no other importance) */
        float gravity_positive_angle = gravity_angle + 90f;
        float gravity_negative_angle = gravity_angle - 90f;

        /* get the platform directions */
        Vector2 gravity_direction_positive = new Vector2(Mathf.Sin(gravity_positive_angle * Mathf.Deg2Rad), Mathf.Cos(gravity_positive_angle * Mathf.Deg2Rad));
        if (config.show_movement) Debug.DrawRay(transform.position, gravity_direction_positive * config.move_speed, config.platform_direction_color);

        Vector2 gravity_direction_negative = new Vector2(Mathf.Sin(gravity_negative_angle * Mathf.Deg2Rad), Mathf.Cos(gravity_negative_angle * Mathf.Deg2Rad));
        if (config.show_movement) Debug.DrawRay(transform.position, gravity_direction_negative * config.move_speed, config.platform_direction_color);

        /* get the leniency directions (leniency 2 mainly for drawing ray) */
        Vector2 movement_leniency_positive = new Vector2(Mathf.Sin((gravity_positive_angle + config.leniency_angle) * Mathf.Deg2Rad), Mathf.Cos((gravity_positive_angle + config.leniency_angle) * Mathf.Deg2Rad));
        Vector2 movement_leniency_positive2 = new Vector2(Mathf.Sin((gravity_positive_angle - config.leniency_angle) * Mathf.Deg2Rad), Mathf.Cos((gravity_positive_angle - config.leniency_angle) * Mathf.Deg2Rad));
        if (config.show_movement) Debug.DrawRay(transform.position, movement_leniency_positive * config.move_speed, config.movement_leniency_color);
        if (config.show_movement) Debug.DrawRay(transform.position, movement_leniency_positive2 * config.move_speed, config.movement_leniency_color);

        Vector2 movement_leniency_negative = new Vector2(Mathf.Sin((gravity_negative_angle + config.leniency_angle) * Mathf.Deg2Rad), Mathf.Cos((gravity_negative_angle + config.leniency_angle) * Mathf.Deg2Rad));
        Vector2 movement_leniency_negative2 = new Vector2(Mathf.Sin((gravity_negative_angle - config.leniency_angle) * Mathf.Deg2Rad), Mathf.Cos((gravity_negative_angle - config.leniency_angle) * Mathf.Deg2Rad));
        if (config.show_movement) Debug.DrawRay(transform.position, movement_leniency_negative * config.move_speed, config.movement_leniency_color);
        if (config.show_movement) Debug.DrawRay(transform.position, movement_leniency_negative2 * config.move_speed, config.movement_leniency_color);

        /* compute the distance between leniency and platform */
        float leniency_distance_positive = Vector2.Distance(gravity_direction_positive, movement_leniency_positive);
        float leniency_distance_negative = Vector2.Distance(gravity_direction_negative, movement_leniency_negative);

        /* compute distance between movement direction and platform direction */
        float movement_distance_positive = Vector2.Distance(gravity_direction_positive, movement_direction);
        float movement_distance_negative = Vector2.Distance(gravity_direction_negative, movement_direction);

        /* move in direction of the positive platform */
        if (movement_distance_positive <= leniency_distance_positive) {
          velocity.x = Mathf.SmoothDamp(velocity.x, gravity_direction_positive.x * config.move_speed, ref velocity_x_smoothing, config.air_acceleration);
          velocity.y = Mathf.SmoothDamp(velocity.y, gravity_direction_positive.y * config.move_speed, ref velocity_y_smoothing, config.air_acceleration);
        }

        /* move in direction of the negative platform */
        if (movement_distance_negative <= leniency_distance_negative) {
          velocity.x = Mathf.SmoothDamp(velocity.x, gravity_direction_negative.x * config.move_speed, ref velocity_x_smoothing, config.air_acceleration);
          velocity.y = Mathf.SmoothDamp(velocity.y, gravity_direction_negative.y * config.move_speed, ref velocity_y_smoothing, config.air_acceleration);
        }
      }
      
      apply_stop_drag = false;
    }

    /* update velocity */
    rigidbody.velocity = velocity;

    /* apply jump, movement, and drag to the child components if allowed */
    if (config.apply_movement_tochild) {
      foreach (Transform child in child_transforms) {
        Rigidbody2D child_rigidbody = child.gameObject.GetComponent<Rigidbody2D>();
        if (child_rigidbody) {
          if (apply_stop_drag) {
            child_rigidbody.drag = config.movement_linear_drag;
            child_rigidbody.freezeRotation = true;
          } else {
            child_rigidbody.drag = 0f;
            child_rigidbody.freezeRotation = true;
          }

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
    HashSet<RaycastHit2D> hits = GetObjectsInView(GetGravity(), config.ground_fov_angle, config.ground_ray_count, config.ground_ray_length, visualize);
    foreach (RaycastHit2D hit in hits) {
      if (LayerMask.LayerToName(hit.transform.gameObject.layer) != LayerMask.LayerToName(gameObject.layer)) {
        /* get platform information we just hit */
        platform_angle = Mathf.Atan2(hit.normal.x, hit.normal.y) * Mathf.Rad2Deg;
        /* get angle between 0 - 360, even handle negative signs with modulus */
        platform_angle = fmod(platform_angle, 360);
        if (platform_angle < 0) platform_angle += 360;
        
        platform_hit_normal = hit.normal;
        return true;
      }
    }

    platform_angle = -1f;
    
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

        if (visualize) Debug.DrawRay(start_position, angle_direction * _ray_length, config.visualize_fov_inbetween);
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

      if (visualize) Debug.DrawRay(start_position, start_direction * _ray_length, config.visualize_fov_edge);
      if (visualize) Debug.DrawRay(start_position, end_direction * _ray_length, config.visualize_fov_edge);
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

  //Damage Information
  private void Damage(int amount) {
    config.cur_hp -= amount;
    if (config.cur_hp < 0) {
      Debug.Log("Bleh I died.");
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
  }

  //Makes it so gravity is recieved from the right stick's angle
  private void UnlockGravity() {
    set_fixed_gravity = false;
  }
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