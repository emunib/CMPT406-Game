using UnityEngine;

public class Fellow : MonoBehaviour {
	private Input2D input;
	
	
	//Camera
	private Camera cam;

	private Rigidbody2D _rigidbody2D;
	private LineRenderer lr;
	
	GameObject[] gravAffectedObjs;
	public float leftStickAngle;
	[Header("Cooldowns")]
	public float jumpCd;
	public float curJumpCd;

	[Header("Gravity Settings")]
	[Range(0.5f,10f)]
	public float gravityChangePower;

	[Header("Movement Settings")]
	[Range(0.01f,10f)]
	public float moveSpeed;
	public float airMod = 1;
	[Range(10f,80f)]
	public float angleLiniency = 30f;
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

		//Testing arcbetween function
		if (!OnShortestArcBetween (0, 355, 5) || !OnShortestArcBetween(5,4,10)) {
			Debug.Log ("WRONG");
		}
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



	public GameObject GetFirstPlat(){
		float angle = gravityAngle ();
		groundCheckVector.x = transform.position.x + Mathf.Cos(angle);
		groundCheckVector.y = transform.position.y + Mathf.Sin(angle);
	
		RaycastHit2D hit = Physics2D.Raycast(transform.position,new Vector2(Mathf.Cos(angle),Mathf.Sin(angle)), groundedDistance, theFloor);


		if (hit && hit.collider.gameObject.tag == "Platform") {
			Debug.Log ("Hit a platform.");
			return hit.collider.gameObject;
		} else {
			
			return null;
		}
	}

	//Returns true if a1 is on the shortest arc between a2 and a3
	public bool OnShortestArcBetween(float a1, float a2, float a3){
		a1 %= 360;
		a2 %= 360;
		a3 %= 360;

		float diff = a2 - a3;
		float smaller, bigger;
		if (a2 == a3) {
			return false;
		}
		if (a2 > a3) {
			bigger = a2;
			smaller = a3;
		
		} else {
			smaller = a2;
			bigger = a3;
		}
			
		if (diff > 180) {
			return a1 > bigger || a1 < smaller;
				
			
		}
		else{
			return a1 <bigger && a1> smaller;

		}
		


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

		leftStickAngle =  (Mathf.Atan2 (ver_m,hor_m)*Mathf.Rad2Deg + 360) %360;
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
<<<<<<< HEAD
		gravAffectedObjs = GameObject.FindGameObjectsWithTag("Movable");
=======
		 gravAffectedObjs = GameObject.FindGameObjectsWithTag("Movable");
>>>>>>> master
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
		Vector2 toApply;
		GameObject plat = GetFirstPlat ();
		float platAngle;


		if (plat) {
			platAngle = plat.transform.localEulerAngles.z;

			if (Mathf.Abs (hor_m) > 0 || Mathf.Abs(ver_m)>0 ) {
				
			

				if (!grounded) {
					airMod = .5f;
				}
				Debug.Log("To move forward put stick between angles : (" + (platAngle - angleLiniency + 360)%360 + ","+ (platAngle + angleLiniency + 360)%360 + ")"  );
				Debug.Log("To move backward put stick between angles : (" +(platAngle - angleLiniency + 180)%360 + ","+ (platAngle + angleLiniency + 180)%360 + ")" );

				//Moving forwards
				if (OnShortestArcBetween(leftStickAngle, (platAngle - angleLiniency + 360)%360, (platAngle + angleLiniency + 360)%360)) {
					toApply = new Vector2 (Mathf.Cos(platAngle*Mathf.Deg2Rad)* moveSpeed, Mathf.Sin(platAngle*Mathf.Deg2Rad)* moveSpeed);
					_rigidbody2D.velocity = _rigidbody2D.velocity + toApply;

				}
				//Moving backwards
				else if(OnShortestArcBetween(leftStickAngle, platAngle + angleLiniency + 180, platAngle - angleLiniency + 180)){
					toApply = new Vector2 (-Mathf.Cos(platAngle*Mathf.Deg2Rad) * moveSpeed, -Mathf.Sin(platAngle*Mathf.Deg2Rad)*moveSpeed);
					_rigidbody2D.velocity = _rigidbody2D.velocity + toApply;

					
				}

		}


		}
		// draw a ray in the direction of the gravity
		lr.SetPositions (new Vector3[] { transform.position, transform.position + (Vector3)Physics2D.gravity  });


	}
}
