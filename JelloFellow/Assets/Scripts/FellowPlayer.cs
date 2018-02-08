using System.Collections.Generic;
using UnityEngine;

public class FellowPlayer : MonoBehaviour {
  private const float GravityFieldRadius = 2f;
  /// <summary>
  /// The input for this player.
  /// </summary>
  private Input2D input;

  private HashSet<GameObject> objects_in_gravityfield;
  
  private void Start() {
    objects_in_gravityfield = new HashSet<GameObject>();
    
    /* get the input from the controller */
    InputController _inputcontroller = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>();
    input = _inputcontroller.GetInput();
  }

  private void Update() {
    /* next step is to know when the object us outside its radius */
    
    /* if the gameobject is within the GravityFieldRadius add it to the HashSet */
    /* Hashset only stores only unique values so it doesnt matter if it registers again */
    foreach (Collider2D object_in_radius in Physics2D.OverlapCircleAll(transform.position, GravityFieldRadius)) {
      if (object_in_radius.gameObject.Equals(gameObject)) {
        GravityComponent gravity_component = object_in_radius.gameObject.GetComponent<GravityComponent>();
        if (gravity_component != null) {
          gravity_component.InGravityField();
          objects_in_gravityfield.Add(object_in_radius.gameObject);
          Debug.Log(object_in_radius.gameObject.name);
        }
      }
    }
  }

  private void OnDrawGizmos() {
    Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
    Gizmos.DrawSphere(transform.position, GravityFieldRadius);
  }
}
