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

  /// <summary>
  /// Get the gravity currently being applied to object.
  /// </summary>
  /// <returns>Current gravity being used.</returns>
  Vector2 GetGravity();
}
