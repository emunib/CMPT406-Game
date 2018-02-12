using UnityEngine;

public class EyeController : MonoBehaviour {
	private Input2D _input;
	public Transform Parent;
	private Vector2 _original;
	private Vector2 _vec;
	private Vector2 _velocity;

	private void Start()
	{
		_input = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>().GetInput();
		_original = transform.localPosition;
	}

	private void Update ()
	{
		var dir = new Vector2(_input.GetHorizontalMovement(), _input.GetVerticalMovement());
		dir = dir.normalized ;
		dir = new Vector2(dir.x * Parent.transform.localScale.x, dir.y * Parent.transform.localScale.y);
		dir = new Vector2(dir.x * transform.localScale.x, dir.y * transform.localScale.y);
		
		_vec = Vector2.SmoothDamp(_vec,
			new Vector2(dir.x, dir.y), ref _velocity, 0.2f,
			Mathf.Infinity, Time.deltaTime);

		transform.localPosition = new Vector2(_vec.x + _original.x, _vec.y + _original.y);
	}
}
