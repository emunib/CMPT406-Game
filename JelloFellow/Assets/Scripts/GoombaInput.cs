using UnityEngine;

public class GoombaInput : Input2D {
  public float horizontal;
  
  private void Start() {
    horizontal = 1f;
  }

  public override float GetHorizontalGravity() {
    return 0f;
  }
  
  public override float GetVerticalGravity() {
    return 0f;
  }
  
  public override float GetHorizontalMovement() {
    return horizontal;
  }
  
  public override float GetVerticalMovement() {
    return 0f;
  }
  
  public override bool GetJumpButtonDown() {
    return false;
  }
  
  public override bool GetJumpButtonUp() {
    return false;
  }
  
  public override float GetLeftTrigger() {
    return 0f;
  }

  public override float GetRightTrigger() {
    return 0f;
  }
}
