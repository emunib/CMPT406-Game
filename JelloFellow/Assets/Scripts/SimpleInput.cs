using UnityEngine;

public class SimpleInput : Input2D {
  public override float GetHorizontalGravity() {
    return Input.GetAxis(ControllerInfo.Horizontal_RStick());
  }
  
  public override float GetVerticalGravity() {
    return Input.GetAxis(ControllerInfo.Vertical_RStick());
  }
  
  public override float GetHorizontalMovement() {
    return Input.GetAxis(ControllerInfo.Horizontal_LStick());
  }
  
  public override float GetVerticalMovement() {
    return Input.GetAxis(ControllerInfo.Vertical_LStick());
  }
  
  public override bool GetJumpButtonDown() {
    return Input.GetButtonDown(ControllerInfo.Jump());
  }
  
  public override bool GetJumpButtonUp() {
    return Input.GetButtonUp(ControllerInfo.Jump());
  }
  
  public override float GetLeftTrigger() {
    return Input.GetAxis(ControllerInfo.LeftTrigger());
  }

  public override float GetRightTrigger() {
    return Input.GetAxis(ControllerInfo.RightTrigger());
  }
}
