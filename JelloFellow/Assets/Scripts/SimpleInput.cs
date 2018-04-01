using System;
using UnityEngine;

public class SimpleInput : Input2D {
  private float left_trigger;
  private float right_trigger;
  private float horizontal_right_stick;
  private float vertical_right_stick;
  private bool button3_down;
  private bool right_bumper_down;
  private bool leftstick_down;
  private bool rightstick_down;
  
  public override float GetLeftTrigger() {
    if(left_trigger != 0f) {
      float tmp = left_trigger;
      left_trigger = 0f;
      return tmp;
    }
    
    return Input.GetAxis(ControllerInfo.LeftTrigger());
  }
  
  public override float GetRightTrigger() {
    if(right_trigger != 0f) {
      float tmp = right_trigger;
      right_trigger = 0f;
      return tmp;
    }
    
    return Input.GetAxis(ControllerInfo.RightTrigger());
  }
  
  public override float GetHorizontalLeftStick() {
    return Input.GetAxis(ControllerInfo.Horizontal_LStick());
  }
  
  public override float GetVerticalLeftStick() {
    return Input.GetAxis(ControllerInfo.Vertical_LStick());
  }
  
  public override float GetHorizontalRightStick() {
    if(horizontal_right_stick != 0f) {
      float tmp = horizontal_right_stick;
      horizontal_right_stick = 0f;
      return tmp;
    }
    
    return Input.GetAxis(ControllerInfo.Horizontal_RStick());
  }
  
  public override float GetVerticalRightStick() {
    if(vertical_right_stick != 0f) {
      float tmp = vertical_right_stick;
      vertical_right_stick = 0f;
      return tmp;
    }
    
    return Input.GetAxis(ControllerInfo.Vertical_RStick());
  }

  public override bool GetLeftBumperDown() {
    return Input.GetButtonDown(ControllerInfo.LeftBumper());
  }
  
  public override bool GetLeftBumperUp() {
    return Input.GetButtonUp(ControllerInfo.LeftBumper());
  }
  
  public override bool GetRightBumperDown() {
    if(right_bumper_down) {
      right_bumper_down = false;
      return true;
    }
    
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
    if(button3_down) {
      button3_down = false;
      return true;
    }
    
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
    if(leftstick_down) {
      leftstick_down = false;
      return true;
    }
    
    return Input.GetButtonDown(ControllerInfo.Button_LStick());
  }
  
  public override bool GetLeftStickUp() {
    return Input.GetButtonUp(ControllerInfo.Button_LStick());
  }
  
  public override bool GetRightStickDown() {
    if(rightstick_down) {
      rightstick_down = false;
      return true;
    }
    
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

  public void SetLeftTrigger(float _value) {
    left_trigger = _value;
  }
  
  public void SetRightTrigger(float _value) {
    right_trigger = _value;
  }
  
  public void SetHorizontalRightStick(float _value) {
    horizontal_right_stick = _value;
  }
  
  public void SetVerticalRightStick(float _value) {
    vertical_right_stick = _value;
  }
  
  public void SetRightBumperDown(bool _value) {
    right_bumper_down = _value;
  }
  
  public void SetButton3Down(bool _value) {
    button3_down = _value;
  }
  
  public void SetLeftStickDown(bool _value) {
    leftstick_down = _value;
  }
  
  public void SetRightStickDown(bool _value) {
    rightstick_down = _value;
  }
}