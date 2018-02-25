using UnityEngine;

public abstract class Gravity : MonoBehaviour {
  /* Force of the gravity to apply in which ever direction */
  protected float GravityForce() {
    Debug.Log("Height"+JumpHeight());
    Debug.Log("Apex"+JumpApexTime());
    float i = JumpHeight();
    float j = JumpApexTime();
    
    return 2 * JumpHeight() / Mathf.Pow(JumpApexTime(), 2);
  }
  
  /* Default gravity set when not in gravitation field */
  protected Vector2 DefaultGravity() {
   return new Vector2(0f, -GravityForce());
  }
  
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
  
  /// <summary>
  /// Height of the jump.
  /// </summary>
  /// <returns>Height this object jumps.</returns>
  public abstract float JumpHeight();
  
  /// <summary>
  /// Time to reach the apex of the jump.
  /// </summary>
  /// <returns>Time to reach apex of the jump.</returns>
  public abstract float JumpApexTime();

  /// <summary>
  /// Light restrictions when entering a light.
  /// The restrictions vector is multiplied to the gravity of the object.
  /// Restrictions (0,1) would have gravity.x be zero (removing gravity x component).
  /// </summary>
  /// <param name="_restrictions">Restrictions to apply to the gravity.</param>
  public abstract void SetGravityLightRestrictions(Vector2 _restrictions);
}
