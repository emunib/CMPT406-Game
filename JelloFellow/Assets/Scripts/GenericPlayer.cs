using System;
using System.Collections.Generic;
using UnityEngine;

public class GenericPlayer : GravityField {
  private const string GroundLayerName = "Ground";
  
  private const float LinearDragGravity = 3f;
  private const float AngularDragGravity = 0.5f;
  private const float LinearDragMovement = 0f;
  private const float AngularDragMovement = 0.5f;
  private const float TriggerSensitivity = 0.1f;

  private Input2D input;
  private Vector2 new_gravity;
  private bool apply_constant_gravity;
  private bool lock_movement;

  [Header("Raycast Settings")]
  /* RaycastOrigins name does not match other name convention but mainly changed to show up
     nicely in the inspector */
  [Tooltip("Origins to cast rays from.")]
  [SerializeField]
  private Transform[] RaycastOrigins;

  [CustomRangeLabel("Ray Length", 0f, 20f)] [Tooltip("Length of the ray.")] [SerializeField]
  private float ray_length;

  [CustomRangeLabel("Ray Count", 0f, 20f)] [Tooltip("Number of rays to show in between main rays.")] [SerializeField]
  private int ray_count;

  [CustomRangeLabel("Angle FOV", 0f, 180f)] [Tooltip("Padding for the angle.")] [SerializeField]
  private float ray_angle_fov;

  protected override void Awake() {
    base.Awake();

    /* get the default values */
    new_gravity = Vector2.zero;
    apply_constant_gravity = true;
    lock_movement = false;
  }

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
        ApplyGravity(new_gravity);
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

  /// <summary>
  /// Check if the player is touching Ground layer.
  /// </summary>
  /// <returns>True if touching ground otherwise false.</returns>
  protected bool IsGrounded() {
    HashSet<GameObject> game_objects = GetObjectsInView(GetGravity(), true);
    foreach (GameObject game_object in game_objects) {
      if (LayerMask.LayerToName(game_object.layer) == GroundLayerName) {
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
  /// <param name="visualize">If you want to see the cone shape in the editor.</param>
  /// <returns>Hashset containing all the game objects within FOV.</returns>
  private HashSet<GameObject> GetObjectsInView(Vector2 direction, bool visualize = false) {
    /* hashset is useful as it only stores unique values so repeated hits won't be registered */
    HashSet<GameObject> game_objects = new HashSet<GameObject>();
    
    foreach (Transform origins in RaycastOrigins) {
      /* the center of the transform */
      Vector3 start_position = origins.position;
      /* the angle of direction relative to the transforms 'up' position */
      float initial_angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
      /* cut the field of view angle in half to add to both sides of the initial angle */
      float angle_fov = ray_angle_fov / 2f * Mathf.Deg2Rad;
      /* calculate the distance between rays (plus 1 to match the number shown in inspector,
         one is actually hidden under the start direction) */
      float distance_between_rays = ray_angle_fov / (ray_count + 1) * Mathf.Deg2Rad;
      /* angles to start and end with */
      float start_angle = Mathf.Deg2Rad * initial_angle + angle_fov;
      float end_angle = Mathf.Deg2Rad * initial_angle - angle_fov;

      /* detect every layer but self */
      LayerMask everything = ~1 << gameObject.layer;
      
      /* get the start direction by adding FOV to the angle (left side when gravity is normal) */
      Vector2 start_direction = new Vector2(Mathf.Sin(start_angle), Mathf.Cos(start_angle));
      /* get the end direction by adding FOV to the angle (right side when gravity is normal) */
      Vector2 end_direction = new Vector2(Mathf.Sin(end_angle), Mathf.Cos(end_angle));
      
      for (; start_angle > end_angle; start_angle -= distance_between_rays) {
        /* get direction of the incremented angle */
        Vector2 angle_direction = new Vector2(Mathf.Sin(start_angle), Mathf.Cos(start_angle));

        /* check if we hit something in that direction by the set length in the inspector */
        RaycastHit2D hit = Physics2D.Raycast(start_position, angle_direction, ray_length, everything);
        if (hit) {
          /* add to the hashset */
          if(hit.transform.gameObject != gameObject) game_objects.Add(hit.transform.gameObject);
        }
        
        if(visualize) Debug.DrawRay(start_position, angle_direction * ray_length, Color.red);
      }

      /* check if we hit something in the start direction */
      RaycastHit2D start_hit = Physics2D.Raycast(start_position, start_direction, ray_length, everything);
      if (start_hit) {
        /* add to the hashset */
        if(start_hit.transform.gameObject != gameObject) game_objects.Add(start_hit.transform.gameObject);
      }
      
      /* check if we hit something in the end direction */
      RaycastHit2D end_hit = Physics2D.Raycast(start_position, end_direction, ray_length, everything);
      if (end_hit) {
        /* add to the hashset */
        if(end_hit.transform.gameObject != gameObject) game_objects.Add(end_hit.transform.gameObject);
      }
      
      if(visualize) Debug.DrawRay(start_position, start_direction * ray_length, Color.blue);
      if(visualize) Debug.DrawRay(start_position, end_direction * ray_length, Color.blue);
    }

    return game_objects;
  }
  
  /// <summary>
  /// Applies gravity by calling its subclass method 'SetGravity'
  /// </summary>
  /// <param name="_gravity">The gravity to set.</param>
  private void ApplyGravity(Vector2 _gravity) {
    SetGravity(_gravity);
  }
}