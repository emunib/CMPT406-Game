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
    
    /* get the valid controller in the joystick names */
    foreach (string controller in Input.GetJoystickNames()) {
      string lower_controller = controller.ToLower();

      /* xbox support */
      if (lower_controller.Contains("xbox")) {
        info = gameObject.AddComponent<XBoxOneControllerInfo>();
        break;
      } 
      
      /* ps4 support */
      if(lower_controller.Contains("sony")) {
        info = gameObject.AddComponent<Ps4ControllerInfo>();
        break;
      }
    }

    if (info != null) {
      /* output controller type */
      Debug.Log("Controller: " + info.controller_type());
      input.Init(info);
    } else {
      Debug.LogError("Please plugin a valid controller, and restart the game.");
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
