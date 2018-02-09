using UnityEngine;

public interface Gravity {
  /// <summary>
  /// Set custom gravity to the object.
  /// </summary>
  /// <param name="_custom_gravity">Gravity to effect the object.</param>
  void SetCustomGravity(Vector2 _custom_gravity);
  
  /// <summary>
  /// This object is in a gravity field.
  /// </summary>
  void InGravityField();
  
  /// <summary>
  /// This object has just left a gravity field.
  /// </summary>
  void OutsideGravityField();
}
