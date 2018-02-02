using UnityEngine;

public class FellowPlayer : MonoBehaviour {
  /// <summary>
  /// Every Player must have a different ID. We will be able to handle
  /// multiplayer if need to be, otherwise for default keep (0).
  /// </summary>
  public const int PlayerID = 0;
  
  /// <summary>
  /// The input for this player.
  /// </summary>
  private Input2D input;

  private void Start() {
    /* assigning a simple input */
    input = gameObject.AddComponent<SimpleInput>();
  }
}
