using System;
using UnityEngine;

public class EyesController : MonoBehaviour
{
    [Range(0, 1)] public float YOffsetFromCentre;
    [Range(0, 0.2f)] public float SmoothTime;
    [Range(0, 1)] public float Distance;
    private Input2D _input;
    private Vector2 _vel;
    private Vector2 _position;
    private Vector2 _gravDir = Vector2.down;


    // Use this for initialization
    private void Start()
    {
        _input = InputController.instance.GetInput();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        var gravOpp = -_gravDir;
        transform.up = gravOpp;
        
        var yOffset = YOffsetFromCentre * gameObject.GetComponentInParent<JellySprite>().m_SpriteScale.y * 2;
        var offset = gravOpp * yOffset;
        var dir = new Vector2(_input.GetHorizontalLeftStick(), _input.GetVerticalLeftStick());
        
        var targetPos = offset + dir.normalized * Distance;
        
        _position = Vector2.SmoothDamp(_position, targetPos, ref _vel, SmoothTime, Mathf.Infinity, Time.deltaTime);
        transform.localPosition = _position;

    }

    private void SetGravityDirection(Vector2 gravDir)
    {
        _gravDir = gravDir.normalized;
    }
}