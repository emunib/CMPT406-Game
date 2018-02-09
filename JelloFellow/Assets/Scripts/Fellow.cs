using UnityEngine;

public class Fellow : MonoBehaviour {
	private Input2D input;
	
	
	//Camera
	private Camera cam;

	private Rigidbody2D _rigidbody2D;
	private LineRenderer lr;
	
	GameObject[] gravAffectedObjs;

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
		lr = GetComponent<LineRenderer> ();
		_rigidbody2D = GetComponent<Rigidbody2D>();
		input = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>().GetInput();
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
		CooldownTick();
		grounded = IsGrounded ();
		// constant gravity
		float gravity = 9.81f;

		// analog inputs
		float hor = input.GetHorizontalGravity();
		float ver = input.GetVerticalGravity();
		float hor_m = input.GetHorizontalMovement();
		float ver_m = input.GetVerticalMovement();

		// Just using to test with keyboard
		//float hor_m = Input.GetAxis ("Horizontal");
		//float ver_m = Input.GetAxis ("Vertical");
		

		bool jump = input.GetJumpButtonDown();
		
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

		// For jello fellow grav-effected objects
		/*
		gravAffectedObjs = GameObject.FindGameObjectsWithTag("Movable");
		foreach (GameObject g in gravAffectedObjs) {
			Gravity obj_grav = g.gameObject.GetComponent<Gravity>();
			if (Vector3.Distance(transform.position, g.transform.position) < 3)
			{
				obj_grav.ChangeGravity(new Vector2(hor, ver));
				obj_grav.in_radius = true;
			}
			else
			{
				obj_grav.in_radius = false;
			}
		}
		*/


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
