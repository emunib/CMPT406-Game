using Rewired;
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
  
  /// <summary>
  /// These constant names defined are the mapping names for the controller
  /// used in ReWired.
  /// </summary>
  private const string GravityName_X = "GravitySetX";
  private const string GravityName_Y = "GravitySetY";
  private const string MoveName_X = "Move_H";
  private const string MoveName_Y = "Move_V";
  private const string JumpName = "Jump";
  
  /// <summary>
  /// The player which to get the input for, this way we can
  /// handle multiple inputs if need to be.
  /// </summary>
  private Player _player;
  
  private void Start() {
    /* Get the player from rewired */
    _player = ReInput.players.GetPlayer(FellowPlayer.PlayerID);
  }

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
    return _player.GetAxis(GravityName_X);
  }
  
  public override float GetVerticalGravity() {
    return _player.GetAxis(GravityName_Y);
  }
  
  public override float GetHorizontalMovement() {
    return _player.GetAxis(MoveName_X);
  }
  
  public override float GetVerticalMovement() {
    return _player.GetAxis(MoveName_Y);
  }
  
  public override bool GetJumpButtonDown() {
    return _player.GetButtonDown(JumpName);
  }
  
  public override bool GetJumpButtonUp() {
    return _player.GetButtonUp(JumpName);
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
