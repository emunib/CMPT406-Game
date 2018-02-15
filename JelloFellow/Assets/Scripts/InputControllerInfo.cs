using UnityEngine;

/// <inheritdoc />
/// <summary>
/// Input controller contains the basic controller mapping for
/// different controllers.
/// </summary>
public abstract class InputControllerInfo : MonoBehaviour {
  /// <summary>
  /// The controller type (mainly for debugging) currently being used to control the
  /// player.
  /// </summary>
  /// <returns>The type of controller being used</returns>
  public abstract string controller_type();
  
  /// <summary>
  /// Horizontal axis of the left joystick.
  /// </summary>
  /// <returns>Horizontal axis name</returns>
  public abstract string Horizontal_LStick();
  
  /// <summary>
  /// Vertical axis of the left joystick.
  /// </summary>
  /// <returns>Vertical axis name</returns>
  public abstract string Vertical_LStick();
  
  /// <summary>
  /// Horizontal axis of the right joystick.
  /// </summary>
  /// <returns>Horizontal axis name</returns>
  public abstract string Horizontal_RStick();
  
  /// <summary>
  /// Vertical axis of the right joystick.
  /// </summary>
  /// <returns>Vertical axis name</returns>
  public abstract string Vertical_RStick();

  /// <summary>
  /// Jump button.
  /// </summary>
  /// <returns>Jump button name</returns>
  public abstract string Jump();

  /// <summary>
  /// Left bottom button on a controller.
  /// </summary>
  /// <returns>Button axis</returns>
  public abstract string LeftTrigger();
  
  /// <summary>
  /// Right bottom button on a controller.
  /// </summary>
  /// <returns>Button axis</returns>
  public abstract string RightTrigger();
}
