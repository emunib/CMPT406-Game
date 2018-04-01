using UnityEngine;

public class PlayerConfigurator : MonoBehaviour {
  [Header("General Settings")]
  [CustomLabel("Raycast Origin")] [Tooltip("Should the main object be an origin of raycasting.")] [SerializeField]
  public bool is_raycast_origin = true;
  
  [CustomRangeLabel("Max Velocity", 0f, 100f)] [Tooltip("The max velocity this player is allowed to reach.")] [SerializeField]
  public float max_velocity = 50f;
  
  [Header("Gravity Field Settings")]
  [CustomLabel("Animate Field")] [Tooltip("Should the gravity field have a wave animation.")] [SerializeField]
  public bool animate;
  
  [CustomLabel("Field Fill Colour")] [Tooltip("Colour of the gravity field fill.")] [SerializeField]
  public Color field_color;
  
  [CustomLabel("Field Outline Colour")] [Tooltip("Colour of the gravity field outline and marker.")] [SerializeField]
  public Color outline_colour;

  [Header("Child Settings")]
  [CustomLabel("Apply Gravity")] [Tooltip("Apply gravity to the child objects.")] [SerializeField]
  public bool apply_gravity_tochild;

  [CustomLabel("Apply Movement")] [Tooltip("Apply movement to the child objects.")] [SerializeField]
  public bool apply_movement_tochild;

  [Tooltip("The child components of this object.")] [SerializeField]
  public ChildComponent[] ChildComponents;

  [Header("Gravity Settings")]
  [CustomRangeLabel("Gravity Deadzone", 0f, 1f)] [Tooltip("Deadzone for the gravity stick.")] [SerializeField]
  public float gravity_deadzone = 0.9f;
  
  [CustomLabel("Angular Drag")] [Tooltip("Angular drag applied while changing gravity.")] [SerializeField]
  public float gravity_angular_drag = 0.5f;

  [CustomLabel("Linear Drag")] [Tooltip("Linear drag applied while changing gravity.")] [SerializeField]
  public float gravity_linear_drag = 3f;

  [CustomRangeLabel("Max Stamina", 0f, 100f)] [Tooltip("Max possible stamina that can be obtained by the player.")] [SerializeField]
  public float max_gravity_stamina = 100f;

  [CustomRangeLabel("Min Stamina", 0f, 100f)] [Tooltip("Min possible stamina that can be obtained by the player.")] [SerializeField]
  public float min_gravity_stamina = 0f;

  [CustomRangeLabel("Stamina Depletion Time", 0f, 100f)] [Tooltip("In seconds the time to completely lose stamina.")] [SerializeField]
  public float gravity_depletion_time = 5f;

  [CustomRangeLabel("Field Transition Time", 0f, 100f)] [Tooltip("The time to increase or decrease completely in seconds.")] [SerializeField]
  public float gravity_field_transition_time = 0.6f;

  [Header("Movement Settings")]
  [CustomRangeLabel("Movement Deadzone", 0f, 1f)] [Tooltip("Deadzone for the movement stick.")] [SerializeField]
  public float movement_deadzone = 0.6f;
  
  [CustomLabel("Linear Drag")] [Tooltip("Linear drag applied while changing movement.")] [SerializeField]
  public float movement_linear_drag = 5f;

  [CustomRangeLabel("Move Speed", 0f, 100f)] [Tooltip("Speed at which to move the player.")] [SerializeField]
  public float move_speed = 10f;
  
  [CustomRangeLabel("Air Speed", 0f, 100f)] [Tooltip("Speed at which to move the player in air.")] [SerializeField]
  public float air_speed = 2f;

  [CustomRangeLabel("Jump Force", 0f, 100f)] [Tooltip("Force to apply in order to jump.")] [SerializeField]
  public float jump_force = 35f;

  [CustomRangeLabel("Jump Normalized Threshold", 0f, 100f)] [Tooltip("The threshold to normalize the hybrid jump.")] [SerializeField]
  public float jump_normalize_threshold = 1f;

  [CustomRangeLabel("Leniency Angle", 0f, 90f)] [Tooltip("The angle to allow movement in direction of the platforms angle.")] [SerializeField]
  public float leniency_angle = 42f;
  
  [CustomRangeLabel("Jump Angle Coefficient", 0f, 2f)] [Tooltip("The angle to allow movement in direction of the platforms angle.")] [SerializeField]
  public float jump_angle_coefficient = 0.8f;
  
  [CustomRangeLabel("Ground Acceleration Time", 0f, 1f)] [Tooltip("The smooth time of increasing velocity, will effect change in direction.")] [SerializeField]
  public float ground_acceleration = 0.1f;

  [CustomRangeLabel("Air Acceleration Time", 0f, 1f)] [Tooltip("The smooth time of increasing velocity, will effect change in direction.")] [SerializeField]
  public float air_acceleration = 0.4f;
  
  [CustomRangeLabel("Movement Leniency Angle", 0f, 180f)] [Tooltip("The angle to allow movement according to gravity.")] [SerializeField]
  public float movement_leniency_angle = 20f;

  [Header("Grounded Settings")]
  [CustomRangeLabel("Field of View Angle", 0f, 360f)] [Tooltip("Field of view (angle/arc) to cover.")] [SerializeField]
  public float ground_fov_angle = 5f;

  [CustomRangeLabel("Number of Rays", 0, 20)] [Tooltip("Number of rays within the field of view arc.")] [SerializeField]
  public int ground_ray_count = 2;

  [CustomRangeLabel("Length of Ray", 0f, 20f)] [Tooltip("Length of the ray within the field of view arc.")] [SerializeField]
  public float ground_ray_length = 0.65f;

  [Header("Debug Settings")]
  [CustomLabel("Verbose Gravity")] [Tooltip("Verbose gravity for debugging.")] [SerializeField]
  public bool verbose_gravity;

  [CustomLabel("Show Gravity")] [Tooltip("Show gravity with a ray.")] [SerializeField]
  public bool show_gravity;

  [CustomLabel("Gravity Color")] [Tooltip("Color of the ray representing gravity.")] [SerializeField]
  public Color gravity_ray_color = Color.black;

  [CustomLabel("Visualize Ground Check")] [Tooltip("Visualize the grounded check.")] [SerializeField]
  public bool visualize_ground_check;

  [CustomLabel("FOV Edge Ray Color")] [Tooltip("Color of the two edge rays.")] [SerializeField]
  public Color visualize_fov_edge = Color.blue;

  [CustomLabel("FOV In-Between Ray Color")] [Tooltip("Color of the rays in between the two edges.")] [SerializeField]
  public Color visualize_fov_inbetween = Color.red;

  [CustomLabel("Verbose Movement")] [Tooltip("Verbose movement for debugging.")] [SerializeField]
  public bool verbose_movement;

  [CustomLabel("Show Movement")] [Tooltip("Show movement with rays for debugging.")] [SerializeField]
  public bool show_movement;

  [CustomLabel("Movement Direction Color")] [Tooltip("Color representing the direction of requested movement by the joystick.")] [SerializeField]
  public Color movement_direction_color = Color.yellow;

  [CustomLabel("Platform Direction Color")] [Tooltip("Color representing the direction of the platform.")] [SerializeField]
  public Color platform_direction_color = Color.magenta;

  [CustomLabel("Leniency Color")] [Tooltip("Color representing the Leniency of the movement.")] [SerializeField]
  public Color movement_leniency_color = Color.blue;

  public bool IsRaycastOrigin { get; set; }
  
  [Header("Health Settings")]
  [CustomRangeLabel("Start HP", 1f, 1000f)] [Tooltip("HP to start with for the player.")] [SerializeField]
  public int cur_hp;

  [CustomRangeLabel("Max HP", 1f, 1000f)] [Tooltip("Max HP of the player.")] [SerializeField]
  public int max_hp;
}