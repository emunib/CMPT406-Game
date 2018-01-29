using UnityEngine;

public class InputTest : MonoBehaviour {
	private Rigidbody2D _rigidbody2D;

	private void Start() {
		_rigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void Update () {
		// Output of the right analog sticks
		//Debug.Log("H: " + Input.GetAxis("Horizontal_g") + ", V: " + Input.GetAxis("Vertical_g"));

		// constant gravity
		float gravity = 9.81f;
		
		// analog inputs
		float hor = Input.GetAxis("Horizontal_g");
		float ver = Input.GetAxis("Vertical_g");
		
		
		// if they arent 0
		// the velocity of the object pauses when we are selecting the gravity direction
		// because it goes in here as the hor and ver are not normalized...
		if (hor != 0 || ver != 0) {
			// create gravity vector and normalize
			Vector2 gDir = new Vector2(hor, ver).normalized;
			Debug.Log("Gravity: " + gDir);
			
			// before we change gravity stop the rigidbody
			_rigidbody2D.velocity = Vector3.zero;
			Physics2D.gravity = gDir * gravity;
			
			// draw a ray in the direction of the gravity
			Debug.DrawRay(transform.position, gDir, Color.red);
		}

		
	}
}
