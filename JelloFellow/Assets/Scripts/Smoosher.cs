using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoosher : MonoBehaviour {

	[Range(1, 100)]
	public float upspeed;
	[Range(1, 100)]
	public float downspeed;
	[Range(1, 10)]
	public float platformCheckRange;
	[Range(0, 5)]
	public float StartDelay;
	[Range(0, 5)]
	public float StayDownDelay;
	[Range(0, 5)]
	public float StayUpDelay;
	[Range(1, 30)]
	public int AmountJellyColliders;

	public float ShakeRange;
	
	private bool doOnce;
	private bool playerInTrigger;
	private float initialSpeed;
	private GameObject bar;
	private GameObject rod;
	private GenericPlayer player;
	private bool goingback;
	private BoxCollider2D rodCollider; 
	private float speed;
	private Vector3 startPos;

	private Vector3 lastPosition;
	private bool shake;
	private bool shakedoonce;
	
	
	void Start () {
		
		bar = transform.Find("SmooshBar").gameObject;
		rod = transform.Find("Rod").gameObject;
		playerInTrigger = false;
		goingback = false;
		initialSpeed = downspeed;
		speed = 0;
		doOnce = true;

		rodCollider = rod.GetComponent<BoxCollider2D>();
		shakedoonce = true;

	}
	
	
	
	// Update is called once per frame
	void Update() {


		if (doOnce) {
			doOnce = false;
			Invoke("BeginSmooshing", StartDelay);
		}



		if (goingback && Vector3.Distance(bar.transform.position, transform.position) < .5) {
			goingback = false;
			speed = 0;
			Invoke("StartGoDown", StayUpDelay);
		}





		if (!shake) {
			
			Vector3 A = -transform.up;
			bar.transform.position += A * Time.deltaTime * speed;
			//bar.transform.position += A * speed;
			
			float distance = Vector2.Distance(transform.position, bar.transform.position);
			SpriteRenderer rodsprite = rod.GetComponent<SpriteRenderer>();

			rodsprite.size = new Vector2(distance, rodsprite.size.y);
			Vector3 middlepos = bar.transform.position - transform.position;
			middlepos /= 2;
			rod.transform.position = middlepos + transform.position;
			//rodCollider.size = new Vector2(rodsprite.size.y, rodsprite.size.x);
			rodCollider.size = rodsprite.size;

		
		}
		if (shake) {
			if (shakedoonce) {
				lastPosition = bar.transform.position;
				shakedoonce = false;
			}
			bar.transform.position = lastPosition;

			//lastPosition = transform.position;
      
			bar.transform.position = (Vector2)bar.transform.position + Random.insideUnitCircle*ShakeRange;
		}
		SmooshCheck();

		PlatformCheck();


	}

	void BeginSmooshing() {
		speed = initialSpeed;
	}

	

	private void PlatformCheck() {
		Debug.DrawRay(bar.transform.position, -bar.transform.up * platformCheckRange);
		
		if (!goingback) {
			RaycastHit2D hit = Physics2D.Raycast(bar.transform.position, -bar.transform.up, platformCheckRange,
				LayerMask.GetMask("Platform"));
			if (hit.collider != null) {
				goingback = false;
				GoBack();
			}
		}

	}
	
	void SmooshCheck() {

		if (playerInTrigger && !goingback) {
			RaycastHit2D hit = Physics2D.Raycast(bar.transform.position, -bar.transform.up, platformCheckRange,
				LayerMask.GetMask("Platform"));

			if (hit.collider != null) {
				goingback = false;
				//GoBack();
				print("SMOOSHED!!!!");
				Invoke("Smoosh",.1f);
			}
		}


	}
	
	private void Smoosh() {
		player.Damage(1);

	}

	private void PlayerEnteredTrigger(GenericPlayer p) {
		playerInTrigger = true;
		player = p;
	}
	private void PlayerLeftTrigger() {
		playerInTrigger = false;
	}
	private void GoBack() {
		if (goingback == false) {
			shake = true;
			Invoke("StartGoUp",StayDownDelay);
			goingback = true;

			speed = 0;
		}
		
	}

	private void StartGoDown() {
		goingback = false;
		speed = downspeed;
	}
	
	private void StartGoUp() {
		speed = -upspeed;
		shake = false;
		bar.transform.position = lastPosition;

	}
	
	
}
