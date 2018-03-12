using UnityEngine;

/// <inheritdoc />
/// <summary>
/// This class exposes joystick controls to the other classes.
/// </summary>
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

  public abstract float GetLeftTrigger();
  public abstract float GetRightTrigger();
  public abstract float GetHorizontalLeftStick();
  public abstract float GetVerticalLeftStick();
  public abstract float GetHorizontalRightStick();
  public abstract float GetVerticalRightStick();
  public abstract bool GetLeftBumperDown();
  public abstract bool GetLeftBumperUp();
  public abstract bool GetRightBumperDown();
  public abstract bool GetRightBumperUp();
  public abstract bool GetButton1Down();
  public abstract bool GetButton1Up();
  public abstract bool GetButton2Down();
  public abstract bool GetButton2Up();
  public abstract bool GetButton3Down();
  public abstract bool GetButton3Up();
  public abstract bool GetButton4Down();
  public abstract bool GetButton4Up();
  public abstract bool GetLeftStickDown();
  public abstract bool GetLeftStickUp();
  public abstract bool GetRightStickDown();
  public abstract bool GetRightStickUp();
  public abstract bool GetStartButtonUp();
  public abstract bool GetStartButtonDown();
}