using Rewired;

public class SimpleInput : Input2D {
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
}
