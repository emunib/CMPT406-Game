using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class ZCrawler : GenericPlayer {
	private ZCrawlerInput _input;
	private SpriteRenderer sprite_renderer;
	private bool do_once;
	private bool flip;
	private float direction;
	private bool wait_gravity;
	private Rigidbody2D rb;
	//public bool randomizeDirection = true;

	//private Quaternion glueRot = Quaternion.identity;
	// Use this for initialization
	protected override void Start () {
		base.Start();
		rb = GetComponent<Rigidbody2D>();
		_input = gameObject.AddComponent<ZCrawlerInput>();
		SetInput(_input);
		SetIgnoreFields(true);
		SetFieldRadius(2f);

		sprite_renderer = GetComponent<SpriteRenderer>();
		do_once = false;
		flip = false;
		if (UnityEngine.Random.Range(0f, 1f) >= 0.5f) direction = -1;
		else{
			direction = 1; }
		
		wait_gravity = false;
	}
	
	protected override void FixedUpdate() {
		base.FixedUpdate();
		/* reset all the values */
		_input.DefaultValues();
    	
		/* use find to see if theres players ahead or we need to flip */
		Find();
    
		/* move in the direction of platform */
		float platform_walk_angle = PlatformAngle() - 90;
		Vector2 movement_direction = new Vector2(Mathf.Sin(platform_walk_angle * Mathf.Deg2Rad), Mathf.Cos(platform_walk_angle * Mathf.Deg2Rad));
		
		/* use the left control stick to move in direction */
		_input.leftstickx = movement_direction.x * direction;
		_input.leftsticky = movement_direction.y * direction;
    	
		/* only do it once */
		if (do_once) {
			/* change gravity in direction of the platform */
			float platform_angle = PlatformAngle() + 180;
			Vector2 gravity_direction = new Vector2(Mathf.Sin(platform_angle * Mathf.Deg2Rad), Mathf.Cos(platform_angle * Mathf.Deg2Rad));
      
			/* use the right stick to set the gravity facing the platform */
			_input.rightstickx = gravity_direction.x;
			_input.rightsticky = gravity_direction.y;
      
			do_once = false;
		}
    
		/* call this to run Update in the subclass */
		/* we call update after is because we want to change the input then call the update to handle the input changes
		   in the same frame rather to have to wait another frame */
		base.Update();
	}
	
	/// <summary>
	/// Grab the angle of the platform or ground he is on at the moment.
	/// </summary>
	/// <returns>Angle of his ground.</returns>
	private float PlatformAngle() {
		float platform_angle = -1f;
		/* get platform information */
		HashSet<RaycastHit2D> hits = GetObjectsInView(-transform.up, config.ground_fov_angle, config.ground_ray_count, config.ground_ray_length);
		foreach (RaycastHit2D hit in hits) {
			if (hit.transform.gameObject.layer != gameObject.layer) {
				/* calculate angle of the platform we are on */
				platform_angle = Mathf.Atan2(hit.normal.x, hit.normal.y) * Mathf.Rad2Deg;

				/* get angle between 0 - 360, even handle negative signs with modulus */
				platform_angle = fmod(platform_angle, 360);
				if (platform_angle < 0) platform_angle += 360;
				break;
			}
		}

		return platform_angle;
	}

	private void Find() {
		/* which direction is our AI moving */
		//Vector2 direction_fov = flip ? transform.right : -transform.right;
		/* shoot a ray out in that direction and see if he is hitting player or if anythig else than change direction */
		//HashSet<RaycastHit2D> objects_in_view = GetObjectsInView(direction_fov, 1f, 0, 2.5f);
		//if(objects_in_view.Count > 0) Flip();
		
		/* as long as he is grounded make sure he does not fall off the platform edge and flip if he
		   is about to */
		
		if (!is_grounded && !wait_gravity)
		{
			
			float platform_angle = PlatformAngle();
			//float angle1 = platform_angle - 160;
			//float angle2 = platform_angle + 160f;

			//float angle = flip ? Mathf.Max(angle1, angle2) : Mathf.Min(angle1, angle2);
			
			//Vector2 forwardangle_direction = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
			Vector2 dir = -(Vector2)transform.up;
			
			const float velocityFactor = .50f;
			rb.AddTorque(direction*.5f);
			dir = new Vector2(dir.x, dir.y);
			//HashSet<RaycastHit2D> leaving_ground = GetObjectsInView(dir, 1f, 0, 8f,true);
			Vector2 startOffset = transform.position + new Vector3( rb.velocity.normalized.x *.7f, rb.velocity.normalized.y * .7f);
			RaycastHit2D daHit = Physics2D.Linecast(startOffset, startOffset + dir * rb.velocity.magnitude * velocityFactor);
			Debug.DrawLine(startOffset, startOffset + dir * rb.velocity.magnitude * velocityFactor, Color.red);
			
			//RaycastHit2D gravDirHit = Physics2D.Linecast(transform.position, (Vector2)transform.position  + this.GetGravity() * 9);
			//Debug.DrawLine(transform.position, (Vector2)transform.position  + this.GetGravity() * 9,Color.magenta);
			
			if (daHit && LayerMask.LayerToName(daHit.transform.gameObject.layer) != LayerMask.LayerToName(gameObject.layer))
			{
			
			/*if (daHit
			{
				Debug.Log("Leaving ground");
				wait_gravity = true;
				Invoke("ResetSetGravity",0.2f);
				return;

			}
			*/



				platform_angle = Mathf.Atan2(daHit.normal.x, daHit.normal.y) * Mathf.Rad2Deg;//* direction;

			Vector2 gravity_direction = new Vector2(Mathf.Sin((platform_angle + 180f * direction) * Mathf.Deg2Rad),
				Mathf.Cos((platform_angle + 180f * direction) * Mathf.Deg2Rad));

			Vector3 rotation_angle = transform.rotation.eulerAngles;
			Debug.Log("Platform angle is : " + platform_angle);
			rotation_angle.z = -platform_angle ;
			rb.angularVelocity = 0;
			transform.rotation = Quaternion.Euler(rotation_angle);


			/* use the right stick to set the gravity facing the platform */
			_input.rightstickx = gravity_direction.x;
			_input.rightsticky = gravity_direction.y;


		}
	}
		else
		{
			RaycastHit2D forwardHit = Physics2D.Linecast(transform.position, transform.position - transform.right * 1.3f * direction);
			Debug.DrawLine(transform.position, transform.position - transform.right * 1.3f * direction, Color.green);
			
			if (forwardHit && LayerMask.LayerToName(forwardHit.transform.gameObject.layer) != LayerMask.LayerToName(gameObject.layer))
			{
				if (forwardHit.collider.gameObject.tag == "Enemy")
				{
					Flip();
					return;
				}
				float platform_angle = Mathf.Atan2(forwardHit.normal.x, forwardHit.normal.y) * Mathf.Rad2Deg ;
				Debug.Log("Forward plat normal : " + forwardHit.normal);
				Vector2 gravity_direction =  new Vector2(Mathf.Sin((platform_angle + 180f * direction) * Mathf.Deg2Rad),
					Mathf.Cos((platform_angle + 180f * direction) * Mathf.Deg2Rad));
				Vector3 rotation_angle = transform.rotation.eulerAngles;
				Debug.Log("Platform angle is : " + platform_angle);
				rotation_angle.z = -platform_angle ;
				rb.angularVelocity = 0;
				transform.rotation = Quaternion.Euler(rotation_angle);


				/* use the right stick to set the gravity facing the platform */
				_input.rightstickx = gravity_direction.x;
				_input.rightsticky = gravity_direction.y;
				
			}
		}
	}

	
	private void ResetSetGravity() {
		Debug.Log("Enabling gravity change");
		wait_gravity = false;
	}
	
	/// <summary>
	/// Flip the sprite and direction.
	/// </summary>
	private void Flip() {
		flip = !flip;
		sprite_renderer.flipX = flip;
		direction = direction * -1;
	}
}


//		/* shoot a ray out in that direction and see if he is hitting player or if anythig else than change direction */
//		HashSet<RaycastHit2D> get_platform = GetObjectsInView(-transform.up, 1f, 0, 0.6f);
//		/* if we aren't hitting something in the down direction */
////		if (get_platform.Count < 0) {
////			float platform_angle = old_platform_angle - 90f;
////
////			float i = 0f;
////			while (true) {
////				Vector3 rotation_angle = transform.rotation.eulerAngles;
////				rotation_angle.z += i * -direction;
////				transform.rotation = Quaternion.Euler(rotation_angle);
////				
////				float angle1 = platform_angle - i;
////				float angle2 = platform_angle + i;
////				
////				float angle = flip ? Mathf.Max(angle1, angle2) : Mathf.Min(angle1, angle2);
////
////				Vector2 forwardangle_direction = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
////				HashSet<RaycastHit2D> leaving_ground = GetObjectsInView(forwardangle_direction, 1f, 0, 5f, true);
////
////				if (leaving_ground.Count > 0) break;
////				
////				i++;
////			}
////
////			//no_velocity = true;
////		} else {
//		if (get_platform.Count > 0) {
//			if (!gravity_lock) {
//				float platform_angle = PlatformAngle() + 180;
//				/* change gravity in direction of the platform */
//				Vector2 gravity_direction = new Vector2(Mathf.Sin(platform_angle * Mathf.Deg2Rad), Mathf.Cos(platform_angle * Mathf.Deg2Rad));
//
//				/* use the right stick to set the gravity facing the platform */
//				_input.rightstickx = gravity_direction.x;
//				_input.rightsticky = gravity_direction.y;
//				gravity_lock = true;
//				Invoke("ResetGravLock", 0.5f);
//				no_velocity = true;
//			}
//		}
