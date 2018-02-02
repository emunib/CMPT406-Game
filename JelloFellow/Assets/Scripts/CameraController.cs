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
    
    // camera zoom
    [Header("Camera Zoom")]
    [Range(1, 40)] public float MinSize = 10;
    [Range(1, 40)] public float MaxSize = 30;
    [Range(0, 1f)] public float ZoomInSmoothTime = .1f; // approx time it takes to zoom in, smaller is faster
    [Range(0, 1f)] public float ZoomOutSmoothTime = .6f; // approx time it takes to zoom out, smaller is faster

    private float _zoomSpeed;

    // update camera after target movement has occurred in Update()
    private void LateUpdate()
    {
        Validate();
        Move();
        Zoom();
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
        var targetSize = Mathf.Clamp(_velocity.magnitude, MinSize, MaxSize); // use velocity as size, limit it
                                                                             // to be between minSize and maxSize

        var zoomSmoothTime = cam.orthographicSize < targetSize ? ZoomOutSmoothTime : ZoomInSmoothTime; // zoom at the appropriate rate
        
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetSize, ref _zoomSpeed, zoomSmoothTime); // gradually move towards target size
    }

    private void Validate()
    {
        if (MaxSize < MinSize) MaxSize = MinSize; // max should be >= min
    }
}