using System.Collections.Generic;
using UnityEngine;

public class FaceController : MonoBehaviour
{
    private Input2D _input;
    private List<Transform> _nodes;
    private Vector2 _vec;
    private Vector2 _velocity;
    private Vector2 _velocity2;
    private Vector2 _vec2;
    
    [Tooltip("Outer nodes of the slime.")]
    [SerializeField] private Transform[] nodes;

    // Use this for initialization
    private void Start()
    {
        _input = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>().GetInput();

        _nodes = new List<Transform>(nodes);
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
            new Vector2(_input.GetHorizontalLeftStick() * transform.localScale.x, _input.GetVerticalLeftStick() / 2f * transform.localScale.y), ref _velocity, 0.2f,
            Mathf.Infinity, Time.deltaTime); // gradually move towards target

        transform.position = new Vector2((maxX - minX) * 0.5f + minX + _vec.x + _vec2.x
            , (maxY - minY) * 0.5f + minY + _vec.y + _vec2.y);
        
        transform.position = new Vector3(transform.position.x, transform.position.y);
    }
}