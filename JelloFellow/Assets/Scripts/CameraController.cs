using UnityEngine;

// simple camera follow script with smooth movement as well as smooth zoom in/out based on target's velocity
public class CameraController : MonoBehaviour
{
    [Header("Gameobjects")]
    public Transform Target; // the gameobject the camera will follow
    
    // camera movement
    [Header("Camera Movement")]
    [Range(0, 1f)] public float MovementSmoothTime = .15f; // approx time it takes to reach the target, smaller is faster
    [Range(0, 10f)] public float DeadZone = 3; // radius of zone where the camera doesn't follow the target

    private Vector2 _velocity = Vector2.zero; // current velocity of camera, do not modify

    private void LateUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 cur = transform.position;
        Vector2 tar = Target.position;

        if ((cur - tar).magnitude <= DeadZone) return; // do nothing if target is within dead zone

        var targetPos = tar - (tar - cur).normalized * DeadZone; // target position is the edge of the dead zone
        
        transform.position = Vector2.SmoothDamp(transform.position, targetPos, ref _velocity, MovementSmoothTime,
            Mathf.Infinity, Time.deltaTime);
        
        transform.position = new Vector3(transform.position.x, transform.position.y, -10); // set z coordinate
    }
}