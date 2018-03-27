using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <inheritdoc/>
/// <summary>
/// Creates a gravity field around a gameobject. Used to manipulate gravity
/// of objects within the field.
/// </summary>
public abstract class GravityField : GravityPlayer {
  private const string gravityfield_sprite_path = "Prefabs/GravityField";
  private const float GravityDrag = 0.85f;
  protected const float MinRadius = 12f;
  protected const float MaxRadius = 40f;

  private CircleCollider2D gravity_field;
  private HashSet<GameObject> in_field;
  private GameObject gravityfield_visualizer;
  private object _lock;

  private Transform mask;
  private Transform marker;

  protected override void Awake() {
    base.Awake();

    _lock = new object();

    lock (_lock) {
      in_field = new HashSet<GameObject>();
    }
    
    gravityfield_visualizer = Resources.Load(gravityfield_sprite_path) as GameObject;
    gravityfield_visualizer = Instantiate(gravityfield_visualizer);
    gravityfield_visualizer.name = "GravityField";
    gravityfield_visualizer.transform.parent = transform;
    gravityfield_visualizer.transform.localPosition = new Vector3(0f, 0f, gravityfield_visualizer.transform.position.z);
    SetSorting();

    gravityfield_visualizer = gravityfield_visualizer.transform.Find("Field").gameObject;
    
    gravityfield_visualizer.layer = gameObject.layer;

    gravity_field = gravityfield_visualizer.AddComponent<CircleCollider2D>();
    gravity_field.isTrigger = true;
    //gravity_field.radius = MinRadius;
    mask = gravityfield_visualizer.GetComponentInChildren<SpriteMask>().transform;
    marker = transform.Find("GravityField/Marker");
    
    SetFieldRadius(MinRadius);
    
    /* intial position of the marker */
    Vector2 _gravity = GetGravity();
    marker.up = -_gravity;
    marker.localPosition = transform.InverseTransformDirection(_gravity.normalized * gravityfield_visualizer.transform.localScale.x / 2);
  }

  protected override void Update() {
    base.Update();

    /* rotate gravity field to point the marker towards gravity */
    Vector2 _gravity = GetGravity();
    if (_gravity != Vector2.zero) {
      marker.up = -_gravity;
      var pos = _gravity.normalized * gravityfield_visualizer.transform.localScale.x / 2;
      marker.localPosition = transform.InverseTransformDirection(pos);
    }
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
        if (gameObj) {
          Gravity grav = gameObj.gameObject.GetComponent<Gravity>();
          grav.SetCustomGravity(_gravity * GravityDrag);
        } else {
          StartCoroutine(RemoveObject(gameObj, 0.1f));
        }
      }
    }

    base.SetGravity(_gravity);
  }

  private IEnumerator RemoveObject(GameObject _removing, float delay) {
    yield return new WaitForSeconds(delay);
    in_field.Remove(_removing);
  }

  /// <summary>
  /// Change the radius of the gravity field.
  /// </summary>
  /// <param name="radius">The radius to change the gravity field to.</param>
  protected void SetFieldRadius(float radius) {
    if (radius >= MinRadius && radius <= MaxRadius) {
      //gravity_field.radius = radius;
      gravityfield_visualizer.transform.localScale = new Vector3(radius, radius, 1);
    } else {
      //Debug.LogWarning("Radius exceeded Min or Max radius.");
    }
  }

  protected float GetFieldRadius() {
    return gravityfield_visualizer.transform.localScale.x;
    //return gravity_field.radius;
  }

  private float vel;
  protected void ChangeGravityFill(float progress)
  {
    var y = Mathf.SmoothDamp(mask.localPosition.y, Mathf.Lerp(-1.8f, -.6f, progress), ref vel, 0.1f, Mathf.Infinity);
    mask.localPosition = new Vector2(0, y);
  }

  private void SetSorting()
  {
    var fill = gravityfield_visualizer.transform.Find("Field/Full").GetComponent<SpriteRenderer>();
    var fillMask = gravityfield_visualizer.transform.Find("Field/Mask").GetComponent<SpriteMask>();
    
    fill.sortingOrder = UniqueSorting.GetNextSorting();
    fillMask.frontSortingOrder = fill.sortingOrder;
    fillMask.backSortingOrder = UniqueSorting.GetNextSorting();

    var outline = gravityfield_visualizer.transform.Find("Field/Outline").GetComponent<SpriteRenderer>();
    var outlineMask =  gravityfield_visualizer.transform.Find("OutlineMask").GetComponent<SpriteMask>();
    
    outline.sortingOrder = UniqueSorting.GetNextSorting();
    outlineMask.frontSortingOrder = outline.sortingOrder;
    outlineMask.backSortingOrder = UniqueSorting.GetNextSorting();
  }
  
  /* uncomment to visualize without starting the scene */
  /*
  private void OnDrawGizmos() {
    Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
    Gizmos.DrawSphere(transform.position, GravityFieldRadius);
  }
  */
}