using UnityEngine;

public class EyeController : MonoBehaviour {
	private Input2D _input;
	public Transform Parent;
	private Vector2 _original;
	private Vector2 _vec;
	private Vector2 _velocity;

	private float _xScale;
	private float _yScale;

	public float SoftbodyScale = 1;

	private void Awake()
	{
		_xScale = Parent.transform.localScale.x * transform.localScale.x;
		_yScale = Parent.transform.localScale.y * transform.localScale.y;
	} 

	private void Start()
	{
		_input = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>().GetInput();
		_original = transform.localPosition;
	}

	private void Update ()
	{
		var dir = new Vector2(_input.GetHorizontalMovement(), _input.GetVerticalMovement());
		dir = dir.normalized ;
		dir = new Vector2(dir.x * _xScale * SoftbodyScale, dir.y * _yScale * SoftbodyScale);
		Debug.Log(dir.x);
		
		_vec = Vector2.SmoothDamp(_vec,
			new Vector2(dir.x, dir.y), ref _velocity, 0.2f,
			Mathf.Infinity, Time.deltaTime);

		transform.localPosition = new Vector2(_vec.x + _original.x, _vec.y + _original.y);
	}
}
