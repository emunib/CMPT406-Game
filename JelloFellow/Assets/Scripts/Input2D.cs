using UnityEngine;

public abstract class Input2D : MonoBehaviour {
  /// <summary>
  /// Gets the horizontal axis for the gravity.
  /// </summary>
  /// <returns>Horizontal axis</returns>
  public abstract float GetHorizontalGravity();
  
  /// <summary>
  /// Gets the vertical axis for the gravity.
  /// </summary>
  /// <returns>Vertical axis</returns>
  public abstract float GetVerticalGravity();
  
  /// <summary>
  /// Gets the horizontal axis for the movement.
  /// </summary>
  /// <returns>Horizontal axis</returns>
  public abstract float GetHorizontalMovement();
  
  /// <summary>
  /// Gets the vertical axis for the movement.
  /// </summary>
  /// <returns>Vertical axis</returns>
  public abstract float GetVerticalMovement();
  
  /// <summary>
  /// Gets status of the jump button.
  /// </summary>
  /// <returns>True when pressed, false otherwise</returns>
  public abstract bool GetJumpButtonDown();
  
  /// <summary>
  /// Gets status of the jump button.
  /// </summary>
  /// <returns>True when status changing form pressed to unpressed, false otherwise</returns>
  public abstract bool GetJumpButtonUp(); 
}
