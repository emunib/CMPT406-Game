using UnityEngine;

public class InputController : MonoBehaviour {
  /* timeout for recognizing the controller */
  private const float timeout_time = 10f;
  
  private Input2D input;
  private InputControllerInfo info;
  
  /* if the controller has been found yet */
  private bool foundController;
  /* if there has been a timeout yet */
  private bool timeout;
  /* has the player been instructed on pressing the jump button to
     so we can find the controller */
  private bool notified;
  
  private void Awake() {
    /* dont destroy this object when loaded */
    DontDestroyOnLoad(gameObject);
    
    /* default values */
    input = gameObject.AddComponent<SimpleInput>();
    foundController = false;
    timeout = false;
    notified = false;
    info = null;
    
    /* let the player know we are trying to recognize the controller */
    Debug.Log("Recognizing Controller...");
    /* start the time to timeout */
    Invoke("Timeout", timeout_time);
  }

  private void Timeout() {
    timeout = true;
  }

  private void Update() {
    if (!foundController) {
      if (!notified) {
        Debug.Log("Please Press A or X as seen on the controller.");
        notified = true;
      }
      
      if (Input.GetButton("Jump_X") || Input.GetButton("Jump_X_PC")) {
        info = gameObject.AddComponent<XBoxOneControllerInfo>();
      } else if (Input.GetButton("Jump_P")) {
        info = gameObject.AddComponent<Ps4ControllerInfo>();
      }

      if (info != null) {
        Debug.Log("Controller: " + info.controller_type());
        input.Init(info);
        foundController = true;
      }
    }

    if (timeout && !foundController) {
      Debug.LogError("Controller not recognized please attach a supported controller and restart the game.");
      /* Exit application */
    }
    
    /*
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
    */
  }
  
  public Input2D GetInput() {
    return input;
  }
}
