using UnityEngine;
using Rewired;
public class InputTest : MonoBehaviour {

	//REWIRED
	private int playerID;
	private Rewired.Player player;

	private Rigidbody2D _rigidbody2D;
	private LineRenderer lr;
	[Header("Cooldowns")]
	//Cooldowns
	public float jumpCd;
	public float curJumpCd;
	[Header("Gravity Settings")]
	[Range(0.5f,10f)]
	public float gravityChangePower;
	[Header("Movement Settings")]
	[Range(1f,1000f)]
	public float jumpForce;

	public bool auto0Vel = false;
	private void Start() {
		lr = GetComponent<LineRenderer> ();
		_rigidbody2D = GetComponent<Rigidbody2D>();
		player = ReInput.players.GetPlayer(playerID);
		InitCooldowns ();
	}


		
	//Sets all curCDs to their full value
	private void InitCooldowns(){
		curJumpCd = jumpCd;

	}

	private void CooldownTick(){
		curJumpCd -= Time.deltaTime;

	}

	private void Update () {
		// Output of the right analog sticks
		//Debug.Log("H: " + Input.GetAxis("Horizontal_g") + ", V: " + Input.GetAxis("Vertical_g"));
		CooldownTick();
		// constant gravity
		float gravity = 9.81f;
		
		// analog inputs
		float hor = player.GetAxis("GravitySetX");
		float ver = player.GetAxis ("GravitySetY");
		bool jump = player.GetButton ("Jump");
		
		// if they arent 0
		// the velocity of the object pauses when we are selecting the gravity direction
		// because it goes in here as the hor and ver are not normalized...
		if (hor != 0 || ver != 0) {
			// create gravity vector and normalize
			Vector2 gDir = new Vector2(hor, ver).normalized;
			Debug.Log("Gravity: " + gDir);
			float angle;
			if (hor != 0.0f || ver != 0.0f) {
				angle = Mathf.Atan2(ver, hor) * Mathf.Rad2Deg;
				Debug.Log("Angle of stick: " +  angle);

				// Do something with the angle here.
			}
			// before we change gravity stop the rigidbody

			if (auto0Vel) {
				_rigidbody2D.velocity = Vector3.zero;
			}
			Physics2D.gravity = gDir * gravity * gravityChangePower;
			



		}

		if (curJumpCd < 0 && jump) {
			_rigidbody2D.AddForce (-Physics2D.gravity * jumpForce);
			Debug.Log ("Jumping!");
			curJumpCd = jumpCd;
		}
		// draw a ray in the direction of the gravity
		lr.SetPositions (new Vector3[] { transform.position, transform.position + (Vector3)Physics2D.gravity  });


		
	}
}
