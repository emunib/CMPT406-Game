using UnityEngine;

public abstract class Gravity : MonoBehaviour {
  /* Force of the gravity to apply in which ever direction */
  protected const float GravityForce = 9.81f;
  /* Default gravity set when not in gravitation field */
  protected readonly Vector2 DefaultGravity = new Vector2(0f, -GravityForce);
  
  /// <summary>
  /// Set custom gravity to the object.
  /// </summary>
  /// <param name="_custom_gravity">Gravity to effect the object.</param>
  public abstract void SetCustomGravity(Vector2 _custom_gravity);
  
  /// <summary>
  /// This object is in a gravity field.
  /// </summary>
  public abstract void InGravityField();
  
  /// <summary>
  /// This object has just left a gravity field.
  /// </summary>
  public abstract void OutsideGravityField();
}
