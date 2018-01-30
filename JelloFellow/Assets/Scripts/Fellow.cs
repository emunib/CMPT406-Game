using UnityEngine;
using Rewired;
public class Fellow : MonoBehaviour {
	//Camera
	private Camera cam;

	//REWIRED
	private int playerID;
	private Rewired.Player player;

	private Rigidbody2D _rigidbody2D;
	private LineRenderer lr;

	[Header("Cooldowns")]
	public float jumpCd;
	public float curJumpCd;

	[Header("Gravity Settings")]
	[Range(0.5f,10f)]
	public float gravityChangePower;

	[Header("Movement Settings")]
	[Range(1f,1000f)]
	public float moveSpeed;
	[Range(1f,1000f)]
	public float controlCutoff; //Movement influence stops if moving faster than this speed
	[Range(1f,1000f)]
	public float jumpForce;
	public bool grounded;
	public float groundedDistance;
	private Vector2 groundCheckVector = new Vector2();
	public LayerMask theFloor;

	public bool auto0Vel = false;
	private void Start() {
		cam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
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
	//Returns the current angle that the force vector forms
	private float gravityAngle(){
		return Mathf.Atan2 (Physics2D.gravity.y, Physics2D.gravity.x);
	}
	//Simply returns whether the character can jump
	public bool IsGrounded() {
		
		float angle = gravityAngle ();

		groundCheckVector.x = transform.position.x + Mathf.Cos(angle);
		groundCheckVector.y = transform.position.y + Mathf.Sin(angle);
		Debug.DrawLine(transform.position, groundCheckVector, Color.red);
		Debug.DrawRay(transform.position,new Vector2(Mathf.Cos(angle),Mathf.Sin(angle)),Color.green);
		RaycastHit2D hit = Physics2D.Raycast(transform.position,new Vector2(Mathf.Cos(angle),Mathf.Sin(angle)), groundedDistance, theFloor);
		return hit.collider != null;
	}
	private void Update () {
		cam.transform.position = transform.position - new Vector3(0,0,1);
		CooldownTick();
		grounded = IsGrounded ();
		// constant gravity
		float gravity = 9.81f;

		// analog inputs
		float hor = player.GetAxis("GravitySetX");
		float ver = player.GetAxis ("GravitySetY");
		float hor_m = player.GetAxis ("Move_H");
		float ver_m = player.GetAxis ("Move_V");


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

			}
			// If the setting is enabled - before we change gravity stop the rigidbody.W
			if (auto0Vel) {
				_rigidbody2D.velocity = Vector3.zero;
			}

			Physics2D.gravity = gDir * gravity * gravityChangePower;

		}


		//Basic Movement Controls

		if (curJumpCd < 0 && jump && grounded) {
			_rigidbody2D.AddForce (-Physics2D.gravity * jumpForce);
			Debug.Log ("Jumping!");
			curJumpCd = jumpCd;
		}
		//WORK IN PROGRESS 
		if (Mathf.Abs (hor_m) > 0 || Mathf.Abs(ver_m)>0 ) {
			float moveToGAngle = Vector2.Angle (new Vector2 (hor_m, ver_m), Physics2D.gravity);
			Debug.Log("Movement angle relative to G : " + moveToGAngle);
			float airMod = 1;
			if (!grounded) {
				airMod = .5f;
			}
			if (_rigidbody2D.velocity.magnitude <= controlCutoff) {
				
				_rigidbody2D.AddForce (new Vector2 (hor_m * airMod,0) * moveSpeed);

			}
		}
		// draw a ray in the direction of the gravity
		lr.SetPositions (new Vector3[] { transform.position, transform.position + (Vector3)Physics2D.gravity  });


	}
}
