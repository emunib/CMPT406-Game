using UnityEngine;

// simple camera follow script with smooth movement as well as smooth zoom in/out based on target's velocity
public class CameraController : MonoBehaviour
{
    [Header("Gameobjects")] public Transform Target = null; // the gameobject the camera will follow
 
    public JellySprite Jelly; // if no target is defined then the camera will use the jelly object as a target
 
    // camera movement
    [Header("Camera Movement")] [Range(0, 1f)]
    public float MovementSmoothTime = .15f; // approx time it takes to reach the target, smaller is faster
 
    [Range(0, 10f)] public float DeadZone = 3; // radius of zone where the camera doesn't follow the target
 
    private Vector2 _velocity = Vector2.zero; // current velocity of camera, do not modify
 
    // camera zoom
    [Header("Camera Zoom")] [Range(1, 40)] public float MinSize = 10;
    [Range(1, 40)] public float MaxSize = 20;
    [Range(0, 1f)] public float ZoomInSmoothTime = .2f; // approx time it takes to zoom in, smaller is faster
    [Range(0, 1f)] public float ZoomOutSmoothTime = .75f; // approx time it takes to zoom out, smaller is faster
 
    private float _zoomSpeed;
 
    // update camera after target movement has occurred in Update()
    private void LateUpdate()
    {
        if (!Target && Jelly)
        {
            // if the jelly is defined instead of the target then use the jelly's centre point as a target
            Target = Jelly.ReferencePoints[0].transform;
        }
 
        if (Target)
        {
            Validate();
            Move();
            Zoom();
        }
    }
 
    private void Move()
    {
        Vector2 cur = transform.position;
        Vector2 tar = Target.position;
 
        if ((cur - tar).magnitude <= DeadZone) return; // do nothing if target is within dead zone
 
        var targetPos = tar - (tar - cur).normalized * DeadZone; // target position is the edge of the dead zone
 
        transform.position = Vector2.SmoothDamp(transform.position, targetPos, ref _velocity, MovementSmoothTime,
            Mathf.Infinity, Time.deltaTime); // gradually move towards target
 
        transform.position = new Vector3(transform.position.x, transform.position.y, -10); // set z coordinate
    }
 
    private void Zoom()
    {
        var cam = GetComponent<Camera>();
        var vel = Target.GetComponent<Rigidbody2D>().velocity.magnitude;
 
        var targetSize = Mathf.Clamp(vel, MinSize, MaxSize); // use velocity as size, limit it
        // to be between minSize and maxSize
 
        var zoomSmoothTime =
            cam.orthographicSize < targetSize ? ZoomOutSmoothTime : ZoomInSmoothTime; // zoom at the appropriate rate
 
        cam.orthographicSize =
            Mathf.SmoothDamp(cam.orthographicSize, targetSize, ref _zoomSpeed,
                zoomSmoothTime); // gradually move towards target size
    }
 
    private void Validate()
    {
        if (MaxSize < MinSize) MaxSize = MinSize; // max should be >= min
    }
}