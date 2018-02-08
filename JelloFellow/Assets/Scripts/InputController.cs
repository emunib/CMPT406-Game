using UnityEngine;

/// <inheritdoc />
/// <summary>
/// Recognizes different controllers, and sees if they are supported.
/// </summary>
public class InputController : MonoBehaviour {
  private Input2D input;
  private InputControllerInfo info;
  
  private void Awake() {
    /* dont destroy this object when loaded */
    DontDestroyOnLoad(gameObject);
    
    /* default values */
    input = gameObject.AddComponent<SimpleInput>();
    info = null;
    
    /* get the the first controller in the joystick names (does not have
       to be the first connected controller */
    string controller = Input.GetJoystickNames().Length > 0 ? Input.GetJoystickNames()[0] : null;
    if (controller != null) {
      /* check if it contains the word Xbox or PS4 which will
         determine the controller type */
      if (controller.Contains("Xbox")) {
        info = gameObject.AddComponent<XBoxOneControllerInfo>();
      } else if(controller.Contains("PS4")) {
        info = gameObject.AddComponent<Ps4ControllerInfo>();
      } else {
        Debug.LogError("The controller is not supported by our game.");
      }
      
      if (info != null) {
        /* output controller type */
        Debug.Log("Controller: " + info.controller_type());
        input.Init(info);
      }
    } else {
      Debug.LogError("No controller connected.");
    }
  }
  
  /// <summary>
  /// Grabs the auto-assigned input based on the controller.
  /// </summary>
  /// <returns>Auto-assigned input.</returns>
  public Input2D GetInput() {
    return input;
  }
}
