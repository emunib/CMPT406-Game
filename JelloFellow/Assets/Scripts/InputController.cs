using UnityEngine;

public class InputController : MonoBehaviour {
  private Input2D input;
  
  private void Awake() {
    input = gameObject.AddComponent<SimpleInput>();
    InputControllerInfo info = gameObject.AddComponent<XBoxOneControllerInfo>();    
    input.Init(info);
    Debug.Log(info.controller_type());
  }

  public Input2D GetInput() {
    return input;
  }

  private void Update() {
    float hor = input.GetHorizontalGravity();
    float ver = input.GetVerticalGravity();
    float hor_m = input.GetHorizontalMovement();
    float ver_m = input.GetVerticalMovement();

    //if (hor > 0 || ver > 0) {
      Debug.Log("Horizontal Grav: " + hor + ", Vertical Grav: " + ver);
    //}
    
    //if (hor_m > 0 || ver_m > 0) {
      Debug.Log("Horizontal axis: " + hor_m + ", Vertical axis: " + ver_m);
    //}
    
    if (input.GetJumpButtonDown()) {
      Debug.Log("Jump button down.");
    }
    
    if (input.GetJumpButtonUp()) {
      Debug.Log("Jump button Up.");
    }
  }
}
