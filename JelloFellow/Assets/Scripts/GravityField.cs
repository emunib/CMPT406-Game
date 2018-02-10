using System.Collections.Generic;
using UnityEngine;

/// <inheritdoc/>
/// <summary>
/// Creates a gravity field around a gameobject. Used to manipulate gravity
/// of objects within the field.
/// </summary>
public class GravityField : GravityPlayer {
  private const string gravityfield_sprite_path = "Prefabs/GravityField";
  private const float GravityFieldRadius = 2f;
  private const float GravityDrag = 0.85f;
  private const float MinRadius = 2f;
  private const float MaxRadius = 5f;

  private CircleCollider2D gravity_field;
  private HashSet<GameObject> in_field;
  private GameObject gravityfield_visualizer;
  private object _lock;

  protected override void Awake() {
    base.Awake();

    _lock = new object();

    lock (_lock) {
      in_field = new HashSet<GameObject>();
    }

    gravity_field = gameObject.AddComponent<CircleCollider2D>();
    gravity_field.isTrigger = true;
    gravity_field.radius = GravityFieldRadius;

    gravityfield_visualizer = Resources.Load(gravityfield_sprite_path) as GameObject;
    gravityfield_visualizer = Instantiate(gravityfield_visualizer);
    gravityfield_visualizer.transform.parent = transform;
    gravityfield_visualizer.transform.localPosition = Vector3.zero;

    SetFieldRadius(GravityFieldRadius);
  }

  private void OnTriggerStay2D(Collider2D other) {
    /* let the gravity object know its in our field */
    Gravity grav = other.gameObject.GetComponent<Gravity>();
    if (grav != null) {
      lock (_lock) {
        in_field.Add(other.gameObject);
        grav.SetCustomGravity(GetGravity() * GravityDrag);
        grav.InGravityField();
      }
    }
  }

  private void OnTriggerExit2D(Collider2D other) {
    /* let the gravity object know its leaving our field */
    Gravity grav = other.gameObject.GetComponent<Gravity>();
    if (grav != null) {
      lock (_lock) {
        in_field.Remove(other.gameObject);
        grav.OutsideGravityField();
      }
    }
  }

  protected override void SetGravity(Vector2 _gravity) {
    lock (_lock) {
      foreach (GameObject gameObj in in_field) {
        Gravity grav = gameObj.gameObject.GetComponent<Gravity>();
        grav.SetCustomGravity(_gravity * GravityDrag);
      }
    }

    base.SetGravity(_gravity);
  }

  /// <summary>
  /// Change the radius of the gravity field.
  /// </summary>
  /// <param name="radius">The radius to change the gravity field to.</param>
  protected void SetFieldRadius(float radius) {
    if (radius >= MinRadius && radius <= MaxRadius) {
      gravity_field.radius = radius;
      gravityfield_visualizer.transform.localScale = new Vector3(radius * 2, radius * 2, 1);
    } else {
      Debug.LogWarning("Radius exceeded Min or Max radius.");
    }
  }

  protected float GetFieldRadius() {
    return gravity_field.radius;
  }

  /* uncomment to visualize without starting the scene */
  /*
  private void OnDrawGizmos() {
    Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
    Gizmos.DrawSphere(transform.position, GravityFieldRadius);
  }
  */
}