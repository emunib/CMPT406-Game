using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoosher : MonoBehaviour {

	[Range(1, 100)]
	public float upspeed;
	//[Range(1, 100)]
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

	[Range(1, 30)] public float repeatDelay;
	[Range(1, 5)] public int repeatN;
	public bool repeat;
	
	//public float ShakeRange;
	
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
	private GameObject kill_trigger;
	

	private int n;
	
	private void Start () {
		
		bar = transform.Find("SmooshBar").gameObject;
		rod = transform.Find("Rod").gameObject;
		playerInTrigger = false;
		goingback = false;
		initialSpeed = downspeed;
		speed = 0;
		doOnce = true;

		rodCollider = rod.GetComponent<BoxCollider2D>();
		n = 0;

		if (!repeat) {
			repeatN = 100000;
		}
	}
	
		
	private void Update() {

		bar.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
	
		if (doOnce) {
			n++;
			doOnce = false;
			Invoke("BeginSmooshing", StartDelay);

		}
		
		


		//At starting position. Need to start going back down. 
		if (goingback && Vector3.Distance(bar.transform.position, transform.position) < .5) {
			goingback = false;
			speed = 0;
			
			
			if (n < repeatN) {

				n++;
				Invoke("StartGoDown", StayUpDelay);
			
			}
			else {
				n = 1;
				Invoke("StartGoDown", repeatDelay);

			}
		}


		//Adjust the sprite and the collider
		float distance = Vector2.Distance(transform.position, bar.transform.position);
		SpriteRenderer rodsprite = rod.GetComponent<SpriteRenderer>();
		rodsprite.size = new Vector2(distance, rodsprite.size.y);
		Vector3 middlepos = bar.transform.position - transform.position;
		middlepos /= 2;
		rod.transform.position = middlepos + transform.position;
		rodCollider.size = new Vector2(rodsprite.size.y, rodsprite.size.x);
		
		rodCollider.size = rodsprite.size;

		if (!shake) {
			
			Vector3 down = -transform.up;
			bar.GetComponent<Rigidbody2D>().velocity = down*speed;
			
			

		}
		
		/*
		//Attempted a Shake effect but it looked lame
		if (shake) {
			if (shakedoonce) {
				lastPosition = bar.transform.position;
				shakedoonce = false;
			}
			//bar.transform.position = lastPosition;

			//lastPosition = transform.position;
      
			//bar.transform.position = (Vector2)bar.transform.position + Random.insideUnitCircle*ShakeRange;
		}*/
		SmooshCheck();




	}

	
	
	private void BeginSmooshing() {
		speed = initialSpeed;
	}

	
/// <summary>
/// Was doing raycast checking for the platform before. May still be usefull so don't want to get rid of it yet.
/// But right now does nothing
/// </summary>
	private void PlatformCheck() {
		Debug.DrawRay(bar.transform.position, -bar.transform.up * platformCheckRange);
		
		if (!goingback) {
			RaycastHit2D hit = Physics2D.Raycast(bar.transform.position, -bar.transform.up, platformCheckRange,
				LayerMask.GetMask("Ground"));
			if (hit.collider != null) {
				goingback = false;
				GoBack();
			}
		}

	}
	
	private void SmooshCheck() {

		if (playerInTrigger ) {
			RaycastHit2D hit = Physics2D.Raycast(bar.transform.position, -bar.transform.up, platformCheckRange,
				LayerMask.GetMask("Ground"));
			
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
		
		if (goingback) {
			return;
		}
		

		shake = true;
		Invoke("StartGoUp",StayDownDelay);
		goingback = true;

		speed = 0;

	}

	private void StartGoDown() {
		goingback = false;
		speed = downspeed;
	}
	
	private void StartGoUp() {
		speed = -upspeed;
		shake = false;
		//bar.transform.position = lastPosition;

	}

	
}
