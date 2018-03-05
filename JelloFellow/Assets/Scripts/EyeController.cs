using UnityEngine;

public class EyeController : MonoBehaviour
{
    public Transform Parent;
    public Vector2 OffsetFromEyeCentre;
    
    private Input2D _input;
    private Vector2 _position;
    private Vector2 _vel;
    private float _xScale;
    private float _yScale;

    private void Awake()
    {
        _xScale = Parent.transform.localScale.x * transform.localScale.x;
        _yScale = Parent.transform.localScale.y * transform.localScale.y;
    }

    private void Start()
    {
        _input = InputController.instance.GetInput();
    }

    private void Update()
    {
        var dir = new Vector2(_input.GetHorizontalLeftStick(), _input.GetVerticalLeftStick());
        dir = dir.normalized;
        dir = new Vector2(dir.x * _xScale, dir.y * _yScale);
        dir = transform.InverseTransformDirection(dir);
        
        var targetPos = OffsetFromEyeCentre + dir;

        _position = Vector2.SmoothDamp(_position, targetPos, ref _vel, 0.01f, Mathf.Infinity, Time.deltaTime);

        transform.localPosition = _position;
    }
}