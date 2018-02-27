using UnityEngine;

public class MainScript : Singleton<MainScript> {
  [CustomLabel("Gravity Force")] [Tooltip("Force at which gravity is applied to objects.")] [SerializeField]
  private float gravity_force = 48f;
  private float gravity_force_updater = 0f;
  
  public delegate void OnGravityForceChangeDelegate(float _gravity_force);
  public event OnGravityForceChangeDelegate OnGravityForceChange;

  public float GravityForce() {
    return gravity_force;
  }

  private void Update() {
    if (gravity_force_updater != gravity_force && OnGravityForceChange != null) {
      gravity_force_updater = gravity_force;
      OnGravityForceChange(gravity_force);
    }
  }
}