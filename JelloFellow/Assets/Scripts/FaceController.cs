using System.Collections.Generic;
using UnityEngine;

public class FaceController : MonoBehaviour
{
    private Input2D _input;
    private List<Transform> _nodes = new List<Transform>();
    private Vector2 _vec;
    private Vector2 _velocity;
    private Vector2 _velocity2;
    private Vector2 _vec2;

    // Use this for initialization
    private void Start()
    {
        _input = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>().GetInput();

        for (var i = 1; i <= 9; i++)
        {
            _nodes.Add(GameObject.Find("Softbody/O" + i).transform);
        }
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        var minX = _nodes[0].position.x;
        var maxX = _nodes[0].position.x;

        var minY = _nodes[0].position.y;
        var maxY = _nodes[0].position.y;

        foreach (var node in _nodes)
        {
            minX = Mathf.Min(minX, node.position.x);
            maxX = Mathf.Max(maxX, node.position.x);

            minY = Mathf.Min(minY, node.position.y);
            maxY = Mathf.Max(maxY, node.position.y);
        }

        var gravityOpposite = -Physics2D.gravity.normalized * 0.7f;

        _vec2 = Vector2.SmoothDamp(_vec2, gravityOpposite, ref _velocity2, 0.1f,
            Mathf.Infinity, Time.deltaTime); // gradually move towards target;

        transform.up = _vec2;

        _vec = Vector2.SmoothDamp(_vec,
            new Vector2(_input.GetHorizontalMovement() * transform.localScale.x,
                _input.GetVerticalMovement() / 2f * transform.localScale.y), ref _velocity, 0.2f,
            Mathf.Infinity, Time.deltaTime); // gradually move towards target

        transform.position = new Vector2((maxX - minX) * 0.5f + minX + _vec.x + _vec2.x
            , (maxY - minY) * 0.5f + minY + _vec.y + _vec2.y);
        
        transform.position = new Vector3(transform.position.x, transform.position.y, -3);
    }
}