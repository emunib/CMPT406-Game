using UnityEngine;

public class FellowPlayer : MonoBehaviour {
  /// <summary>
  /// The input for this player.
  /// </summary>
  private Input2D input;

  private void Start() {
    /* get the input from the controller */
    InputController _inputcontroller = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>();
    input = _inputcontroller.GetInput();
  }
}
