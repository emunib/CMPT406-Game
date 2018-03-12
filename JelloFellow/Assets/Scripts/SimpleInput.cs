using System;
using UnityEngine;

public class SimpleInput : Input2D {
  public override float GetLeftTrigger() {
    return Input.GetAxis(ControllerInfo.LeftTrigger());
  }
  
  public override float GetRightTrigger() {
    return Input.GetAxis(ControllerInfo.RightTrigger());
  }
  
  public override float GetHorizontalLeftStick() {
    return Input.GetAxis(ControllerInfo.Horizontal_LStick());
  }
  
  public override float GetVerticalLeftStick() {
    return Input.GetAxis(ControllerInfo.Vertical_LStick());
  }
  
  public override float GetHorizontalRightStick() {
    return Input.GetAxis(ControllerInfo.Horizontal_RStick());
  }
  
  public override float GetVerticalRightStick() {
    return Input.GetAxis(ControllerInfo.Vertical_RStick());
  }

  public override bool GetLeftBumperDown() {
    return Input.GetButtonDown(ControllerInfo.LeftBumper());
  }
  
  public override bool GetLeftBumperUp() {
    return Input.GetButtonUp(ControllerInfo.LeftBumper());
  }
  
  public override bool GetRightBumperDown() {
    return Input.GetButtonDown(ControllerInfo.RightBumper());
  }
  
  public override bool GetRightBumperUp() {
    return Input.GetButtonUp(ControllerInfo.RightBumper());
  }
  
  public override bool GetButton1Down() {
    return Input.GetButtonDown(ControllerInfo.Button1());
  }
  
  public override bool GetButton1Up() {
    return Input.GetButtonUp(ControllerInfo.Button1());
  }
  
  public override bool GetButton2Down() {
    return Input.GetButtonDown(ControllerInfo.Button2());
  }
  
  public override bool GetButton2Up() {
    return Input.GetButtonUp(ControllerInfo.Button2());
  }
  
  public override bool GetButton3Down() {
    return Input.GetButtonDown(ControllerInfo.Button3());
  }
  
  public override bool GetButton3Up() {
    return Input.GetButtonUp(ControllerInfo.Button3());
  }
  
  public override bool GetButton4Down() {
    return Input.GetButtonDown(ControllerInfo.Button4());
  }
  
  public override bool GetButton4Up() {
    return Input.GetButtonUp(ControllerInfo.Button4());
  }
  
  public override bool GetLeftStickDown() {
    return Input.GetButtonDown(ControllerInfo.Button_LStick());
  }
  
  public override bool GetLeftStickUp() {
    return Input.GetButtonUp(ControllerInfo.Button_LStick());
  }
  
  public override bool GetRightStickDown() {
    return Input.GetButtonDown(ControllerInfo.Button_RStick());
  }
  
  public override bool GetRightStickUp() {
    return Input.GetButtonUp(ControllerInfo.Button_RStick());
  }

  public override bool GetStartButtonDown() {
    return Input.GetButtonDown(ControllerInfo.StartButton());
  }
  
  public override bool GetStartButtonUp() {
    return Input.GetButtonUp(ControllerInfo.StartButton());
  }
}