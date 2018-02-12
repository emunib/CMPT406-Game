using UnityEngine;

public abstract class Input2D : MonoBehaviour {
  protected InputControllerInfo ControllerInfo;

  /// <summary>
  /// Initialize the Input2D class.
  /// Mainly here to pass in important variables.
  /// </summary>
  /// <param name="_controllerInfo">Information about the controller being used.</param>
  public void Init(InputControllerInfo _controllerInfo) {
    ControllerInfo = _controllerInfo;
  }
  
  /// <summary>
  /// Gets the horizontal axis for the gravity.
  /// </summary>
  /// <returns>Horizontal axis</returns>
  public abstract float GetHorizontalGravity();
  
  /// <summary>
  /// Gets the vertical axis for the gravity.
  /// </summary>
  /// <returns>Vertical axis</returns>
  public abstract float GetVerticalGravity();
  
  /// <summary>
  /// Gets the horizontal axis for the movement.
  /// </summary>
  /// <returns>Horizontal axis</returns>
  public abstract float GetHorizontalMovement();
  
  /// <summary>
  /// Gets the vertical axis for the movement.
  /// </summary>
  /// <returns>Vertical axis</returns>
  public abstract float GetVerticalMovement();
  
  /// <summary>
  /// Gets status of the jump button.
  /// </summary>
  /// <returns>True when pressed, false otherwise</returns>
  public abstract bool GetJumpButtonDown();
  
  /// <summary>
  /// Gets status of the jump button.
  /// </summary>
  /// <returns>True when status changing form pressed to unpressed, false otherwise</returns>
  public abstract bool GetJumpButtonUp();

  /// <summary>
  /// Get status of the left trigger.
  /// </summary>
  /// <returns>Trigger axis</returns>
  public abstract float GetLeftTrigger();

  /// <summary>
  /// Get status of the right trigger.
  /// </summary>
  /// <returns>Trigger axis</returns>
  public abstract float GetRightTrigger();
}
