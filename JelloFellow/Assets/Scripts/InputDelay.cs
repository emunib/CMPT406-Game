using UnityEngine;

public class InputDelay : Input2D {
  /// <summary>
  /// Scale of time when gravity is changing.
  /// </summary>
  private const float TimeScale = 0.1f;
  
  /// <summary>
  /// Normalized time when TimeScale is changed.
  /// This can help normalize everything that does not
  /// want to be effected by timeScale.
  /// </summary>
  private float NormalTimeScale = 0f;

  private void Update() {
    /* When gravity is shifting slow everything down */
    if (GetHorizontalGravity() != 0f || GetVerticalGravity() != 0f) {
      Time.timeScale = TimeScale;
      NormalTimeScale = 1f / TimeScale;
    } else {
      Time.timeScale = 1f;
      NormalTimeScale = 0f;
    }
  }

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

  /// <summary>
  /// Normalize the TimeScale. Useful to cancel out the
  /// freeze effect when gravity is shifting.
  /// </summary>
  /// <returns>Normalized time scale</returns>
  public float NormalizedTimeScale() {
    return NormalTimeScale;
  }
}
