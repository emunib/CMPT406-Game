using System.Collections.Generic;
using UnityEngine;

public class FellowPlayer : MonoBehaviour {
  private const float GravityFieldRadius = 2f;
  /// <summary>
  /// The input for this player.
  /// </summary>
  private Input2D input;

  private void Start() {
    /* get the input from the controller */
    InputController _inputcontroller = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>();
    input = _inputcontroller.GetInput();
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    GravityComponent grav_component = other.gameObject.GetComponent<GravityComponent>();
    if (grav_component != null)
    {
      grav_component.InGravityField();
    }
  }

  private void OnCollisionExit2D(Collision2D other)
  {
    GravityComponent grav_component = other.gameObject.GetComponent<GravityComponent>();
    if (grav_component != null)
    {
      grav_component.OutsideGravityField();
    }
  }


  private void OnDrawGizmos() {
    Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
    Gizmos.DrawSphere(transform.position, GravityFieldRadius);
  }
}
