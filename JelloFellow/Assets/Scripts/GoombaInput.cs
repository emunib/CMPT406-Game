using UnityEngine;

public class GoombaInput : Input2D {
  public float horizontal;
  public float vertical;

  public float horg;
  public float verg;
  
  private void Start() {
    horizontal = 1f;
  }

  public override float GetHorizontalGravity() {
    return horg;
  }
  
  public override float GetVerticalGravity() {
    return verg;
  }
  
  public override float GetHorizontalMovement() {
    return horizontal;
  }
  
  public override float GetVerticalMovement() {
    return vertical;
  }
  
  public override bool GetJumpButtonDown() {
    return true;
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
