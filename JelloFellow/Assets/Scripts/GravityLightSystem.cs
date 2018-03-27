using UnityEngine;

[ExecuteInEditMode] [RequireComponent(typeof(BoxCollider2D))]
public class GravityLightSystem : MonoBehaviour {
	[Header("Disable along which axis")] 
	public bool Horizontal = false;
	public bool Vertical = false;

	[Header("Amount of gravity to disable")]
	[Tooltip("amount is multiplied by the gravity that would otherwise be applied. " +
	         "So a value of 0 and disabling the x axis would disable gravity completely along that axis")]
	[Range(0.001f,1f)]
	public float amount = 0.001f;
	
	[Header("Gravity Visuals")]
	[CustomRangeLabel("Light Width", 1f, 100f)] [Tooltip("Width of the light (mainly to resize the trigger, and particle collider")] [SerializeField]
	private float light_width;
	
	[CustomRangeLabel("Light Height", 1f, 100f)] [Tooltip("Height of the light (mainly to resize the trigger, and particle collider")] [SerializeField]
	private float light_height;

	private ParticleSystem[] particles;
	private BoxCollider2D box_collider;

	//Nick Change
	public float init_sim_speed=30;
	public float end_init_speed_time=.5f;
	
	private void Start() {
		if (Application.isPlaying) {
			ActivateSystem();
		}
	}

	private void Update() {
		Debug.DrawLine(new Vector2(transform.position.x - light_width, transform.position.y), new Vector2(transform.position.x + light_width, transform.position.y), Color.red);
		Debug.DrawLine(new Vector2(transform.position.x - light_width, transform.position.y - light_height), new Vector2(transform.position.x + light_width, transform.position.y - light_height), Color.red);
		Debug.DrawLine(new Vector2(transform.position.x + light_width, transform.position.y), new Vector2(transform.position.x + light_width, transform.position.y - light_height), Color.red);
		Debug.DrawLine(new Vector2(transform.position.x - light_width, transform.position.y), new Vector2(transform.position.x - light_width, transform.position.y - light_height), Color.red);
	}

	private void ActivateSystem() {
		/* get all the particle systems and box collider */
		if (particles == null) particles = GetComponentsInChildren<ParticleSystem>(false);
		if (!box_collider) box_collider = GetComponent<BoxCollider2D>();
		
		/* create an empty gameobject to put the collider for the particle systems so they dont "escape" */
		GameObject particle_gamecollider = new GameObject("Particle Collider");
		particle_gamecollider.transform.position = transform.position;
		particle_gamecollider.transform.parent = transform;
		particle_gamecollider.layer = LayerMask.NameToLayer("GravityLight");

		/* rigidbody of the new gameobject to static so it doesn't move */
		Rigidbody2D particle_rigidbody = particle_gamecollider.AddComponent<Rigidbody2D>();
		particle_rigidbody.bodyType = RigidbodyType2D.Static;
		
		/* create an "inverse" box collider so particles inside don't escape out */
		CompositeCollider2D particle_compositecollier = particle_gamecollider.AddComponent<CompositeCollider2D>();
		particle_compositecollier.edgeRadius = 0.05f;
		
		/* box collider defining the cage */
		BoxCollider2D particle_boxcollider = particle_gamecollider.AddComponent<BoxCollider2D>();
		particle_boxcollider.usedByComposite = true;

		/* creating the cage */
		const float particle_collider_padding = 1f;
		const float particle_padding_time = 1f;
		particle_boxcollider.size = new Vector2(light_width*2f + particle_collider_padding, light_height + particle_collider_padding);
		particle_boxcollider.offset = new Vector2(0f, -particle_boxcollider.size.y/2f + particle_collider_padding/2f);
		
		/* collider for the player */
		box_collider.size = new Vector2(light_width*2f, light_height);
		box_collider.offset = new Vector2(0f, -box_collider.size.y/2f);

		/* set width and height of each particle system */
		foreach (ParticleSystem particle_system in particles) {
			/* disable particle systems */
			if (Horizontal && Vertical) {
				particle_system.gameObject.SetActive(false);
				continue;
			}
			
			if (Horizontal) {
				ParticleSystem.MainModule mainModule = particle_system.main;
				mainModule.startLifetime = light_height / 2f + particle_padding_time;

				ParticleSystem.ShapeModule shape = particle_system.shape;
				shape.radius = light_width;
				continue;
			}

			if (Vertical) {
				particle_system.gameObject.transform.localPosition = new Vector2(light_width, -light_height / 2f);
				particle_system.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, -90f);

				ParticleSystem.MainModule mainModule = particle_system.main;
				mainModule.startLifetime = light_width + particle_padding_time;

				ParticleSystem.ShapeModule shape = particle_system.shape;
				shape.radius = light_height/2f;
			}
		}
		
		/* activate the entire screen particles */
		if (Horizontal && Vertical) {
			GameObject entire_particles = transform.Find("Entire Particles").gameObject;
			entire_particles.SetActive(true);

			ParticleSystem entire_particlesystem = entire_particles.GetComponent<ParticleSystem>();
			ParticleSystem.ShapeModule shape = entire_particlesystem.shape;
			shape.position += new Vector3(0f, -light_height/2f, 0f);
			shape.scale = new Vector3(light_width * 2f, light_height, 1f);
			shape.radius = light_width;
		}
		
		
		//Nick Change in ActivateSystem
		var m = particles[0].main;
		m.simulationSpeed = init_sim_speed;
		Invoke("EndInitSpeed", end_init_speed_time);
	}


	private void EndInitSpeed() {
		var m = particles[0].main;
		m.simulationSpeed= 1;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		Gravity gravity_component = other.GetComponent<Gravity>();
		if (gravity_component) {
			Vector2 disable_along = Vector2.one;
			if (Horizontal) {
				disable_along.x = amount;
			}

			if (Vertical) {
				disable_along.y = amount;
			}
			
			gravity_component.SetGravityLightRestrictions(disable_along);
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		Gravity gravity_component = other.GetComponent<Gravity>();
		if (gravity_component) {
			gravity_component.SetGravityLightRestrictions(Vector2.one);
		}
	}
}
