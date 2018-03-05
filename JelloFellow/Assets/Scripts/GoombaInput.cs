using UnityEngine;

public class GoombaInput : Input2D {
  public float horizontal;
  public float vertical;

  public float horg;
  public float verg;
  public bool jumpbtndown;
  private void Start() {
    horizontal = 1f;
    jumpbtndown = false;
  }
  
  public override float GetLeftTrigger() {
    return 0f;
  }
  
  public override float GetRightTrigger() {
    return 0f;
  }
  
  public override float GetHorizontalLeftStick() {
    return horizontal;
  }
  
  public override float GetVerticalLeftStick() {
    return vertical;
  }
  
  public override float GetHorizontalRightStick() {
    return horg;
  }
  
  public override float GetVerticalRightStick() {
    return verg;
  }

  public override bool GetLeftBumperDown() {
    return false;
  }
  
  public override bool GetLeftBumperUp() {
    return false;
  }
  
  public override bool GetRightBumperDown() {
    return false;
  }
  
  public override bool GetRightBumperUp() {
    return false;
  }
  
  public override bool GetButton1Down() {
    return false;
  }
  
  public override bool GetButton1Up() {
    return false;
  }
  
  public override bool GetButton2Down() {
    return false;
  }
  
  public override bool GetButton2Up() {
    return false;
  }
  
  public override bool GetButton3Down() {
    return jumpbtndown;
  }
  
  public override bool GetButton3Up() {
    return false;
  }
  
  public override bool GetButton4Down() {
    return false;
  }
  
  public override bool GetButton4Up() {
    return false;
  }
  
  public override bool GetLeftStickDown() {
    return false;
  }
  
  public override bool GetLeftStickUp() {
    return false;
  }
  
  public override bool GetRightStickDown() {
    return false;
  }

  public override bool GetRightStickUp() {
    return false;
  }
}
