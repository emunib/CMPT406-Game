using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public LayerMask GroundLayer;
    [Range(1f, 50f)] public float Speed = 10f;
    private JellySprite _jelly;
    public float MaxStretchDist = 5f;
    private Input2D _input;


    void Start()
    {
        _jelly = GetComponent<JellySprite>();
        _input = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>().GetInput();
    }


    private void Update()
    {
        Move();

        FixPoints();


        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jelly.Reset(Vector3.zero, Vector3.zero);
        }
    }

    private void FixPoints()
    {
        for (var i = 1; i < _jelly.ReferencePoints.Count; i++)
        {
            var n = i + 1 == _jelly.ReferencePoints.Count ? 1 : i + 1;

            var cur = _jelly.ReferencePoints[i];
            var next = _jelly.ReferencePoints[n];

            var dist = Vector2.Distance(cur.transform.position, next.transform.position);

            if (dist > MaxStretchDist)
            {
                _jelly.ResetPoint(cur);
            }
        }

        for (var i = _jelly.ReferencePoints.Count - 1; i > 0; i--)
        {
            var n = i - 1 == 0 ? _jelly.ReferencePoints.Count - 1 : i - 1;

            var cur = _jelly.ReferencePoints[i];
            var next = _jelly.ReferencePoints[n];

            var dist = Vector2.Distance(cur.transform.position, next.transform.position);

            if (dist > MaxStretchDist)
            {
                _jelly.ResetPoint(cur);
            }
        }
    }


    private void Move()
    {
        var movementDir = Vector2.zero;
        movementDir.x = _input.GetHorizontalLeftStick();
        movementDir.y = _input.GetVerticalLeftStick();
        movementDir.Normalize();

        _jelly.AddForce(movementDir * Speed);
    }
}