using UnityEngine;

public class InputDelay : SimpleInput {
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
    if (GetHorizontalRightStick() != 0f || GetVerticalRightStick() != 0f) {
      Time.timeScale = TimeScale;
      NormalTimeScale = 1f / TimeScale;
    } else {
      Time.timeScale = 1f;
      NormalTimeScale = 0f;
    }
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
