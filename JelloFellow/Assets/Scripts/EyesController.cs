using UnityEngine;

public class EyesController : MonoBehaviour
{
    public Vector2 OffsetFromCentre;
    [Range(0, 1)]public float SmoothTime;
    private Input2D _input;
    private Vector2 _vel;
    private Vector2 _position;


    // Use this for initialization
    private void Start()
    {
        _input = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>().GetInput();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        var targetPos = OffsetFromCentre + new Vector2(_input.GetHorizontalLeftStick(), _input.GetVerticalLeftStick());
        _position = Vector2.SmoothDamp(_position, targetPos, ref _vel, SmoothTime, Mathf.Infinity, Time.deltaTime);
        transform.localPosition = _position;
    }
}