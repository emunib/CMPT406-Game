using UnityEngine;

/// <inheritdoc />
/// <summary>
/// Input controller contains the basic controller mapping for
/// different controllers.
/// </summary>
public abstract class InputControllerInfo : MonoBehaviour {
  protected const string lefttrigger = "LeftTrigger";
  protected const string righttrigger = "RightTrigger";
  protected const string leftstickx = "LeftStickX";
  protected const string leftsticky = "LeftStickY";
  protected const string rightstickx = "RightStickX";
  protected const string rightsticky = "RightStickY";
  protected const string leftbumper = "LeftBumper";
  protected const string rightbumper = "RightBumper";
  protected const string button1 = "Button1";
  protected const string button2 = "Button2";
  protected const string button3 = "Button3";
  protected const string button4 = "Button4";
  protected const string leftstickclick = "LeftStickClick";
  protected const string rightstickclick = "RightStickClick";
  
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
  /// Button 1.
  /// </summary>
  /// <returns>Button name</returns>
  public abstract string Button1();

  /// <summary>
  /// Button 2.
  /// </summary>
  /// <returns>Button name</returns>
  public abstract string Button2();
  
  /// <summary>
  /// Button 3.
  /// </summary>
  /// <returns>Button name</returns>
  public abstract string Button3();
  
  /// <summary>
  /// Button 4.
  /// </summary>
  /// <returns>Button name</returns>
  public abstract string Button4();

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
  
  /// <summary>
  /// Left top button on a controller.
  /// </summary>
  /// <returns>Button axis</returns>
  public abstract string LeftBumper();
  
  /// <summary>
  /// Right top button on a controller.
  /// </summary>
  /// <returns>Button axis</returns>
  public abstract string RightBumper();

  /// <summary>
  /// Click of the left stick.
  /// </summary>
  /// <returns>Button name</returns>
  public abstract string Button_LStick();
  
  /// <summary>
  /// Click of the right stick.
  /// </summary>
  /// <returns>Button name</returns>
  public abstract string Button_RStick();
}
