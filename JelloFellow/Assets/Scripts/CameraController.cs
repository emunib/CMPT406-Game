using UnityEngine;

// simple camera follow script with smooth movement as well as smooth zoom in/out based on target's velocity
public class CameraController : MonoBehaviour
{
    [Header("Gameobjects")] public Transform Target; // the gameobject the camera will follow

    public JellySprite Jelly; // if no target is defined then the camera will use the jelly object as a target

    // camera movement
    [Header("Camera Movement")] [Range(0, 2f)]
    public float MovementSmoothTime = 1f; // approx time it takes to reach the target, smaller is faster

    [Range(0, 1f)] public float LookAhead = 0.5f; // radius of zone where the camera doesn't follow the target

    private Vector2 _velocity = Vector2.zero; // current velocity of camera, do not modify

    // update camera after target movement has occurred in Update()
    private void LateUpdate()
    {
        // if the jelly is defined instead of the target then use the jelly's centre point as a target
        if (!Target && Jelly) Target = Jelly.ReferencePoints[0].transform;

        if (Target) Move();
    }

    private void Move()
    {
        Vector2 targetPos = Target.position;

        targetPos += Target.gameObject.GetComponent<Rigidbody2D>().velocity * LookAhead;

        transform.position = Vector2.SmoothDamp(transform.position, targetPos, ref _velocity, MovementSmoothTime,
            Mathf.Infinity, Time.deltaTime); // gradually move towards target

        transform.position = new Vector3(transform.position.x, transform.position.y, -10); // set z coordinate
    }
}