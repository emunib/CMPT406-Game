using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGoober : GenericPlayer {

  [Header("ForwardCheck Information")] 
  //private Transform fwdCheck;
  //private Transform backCheck;
  [Range(.01f, 2)] public float fwdGroundChkRange = 5f;
  [Range(.01f, 2)] public float fwdWallChkRange = 5f;
  [Range(.01f,1f)]
  public float turnWait = .5f;

  
  [Header("AgroFOV Raycast Settings")]

  [CustomRangeLabel("Ray Length", 0f, 20f)] [Tooltip("Length of the ray.")]
  [SerializeField] private float agro_ray_length;

  [CustomRangeLabel("Ray Count", 0f, 20f)] [Tooltip("Number of rays to show in between main rays.")]
  [SerializeField] private int agro_ray_count;

  [CustomRangeLabel("Angle FOV", 0f, 180f)] [Tooltip("Padding for the angle.")]
  [SerializeField] private float agro_ray_angle_fov;

  
  
  

  private DecisionTree root;

  private Rigidbody2D rb;
  private bool attackOffCd = true;
  private HashSet<RaycastHit2D> agro_game_objects;
  private HashSet<RaycastHit2D> grounded_game_objects;

  private bool grounded;
  private bool firstPlat = true;
  private bool turnOffCd = true;
  private Vector2 attackVector;
  
  private GenericEnemyInput _input;
  private bool flip;
  private int direction = 1;
  
  private SpriteRenderer sprite_renderer;

  
  protected override void Start() {    
    _input = gameObject.AddComponent<GenericEnemyInput>();
    SetInput(_input);
    SetIgnoreFields(false);
    root = gameObject.AddComponent<DecisionTree>();
    BuildDecisionTree();
    rb = GetComponent<Rigidbody2D>();
    base.Start();
    flip = false;
    direction = 1;
    sprite_renderer = GetComponent<SpriteRenderer>();

    
    //TODO: Not sure where this goes? Ask rutvik

    _input.rightstickx = -transform.up.x;
    _input.rightsticky = -transform.up.y;

    
  }

  protected override void FixedUpdate() {
    base.FixedUpdate();
    //_input.DefaultValues();

    //goomba_input.jumpbtndown = false;
    //root.Search();
    _input.leftstickx = 1;
    if(is_grounded) print("Is on ground");
  }



  public int maxAttacks =3;
  public float attackCdTime = .4f;
  public float attackNumCdTime = 2f;

  public int numAttacks = 0;
  
  private void Attack() {
    Debug.Log("Im attacking");
    //Transform target = GameObject.FindGameObjectWithTag("Player").transform;

  
    
    //Only Jump If I can
    if (grounded &&attackOffCd) {
      attackOffCd = false;
      Invoke("ResetAttackCD",attackCdTime);
      if (numAttacks < maxAttacks) {
        //goomba_input.horizontal = attackVector.x;
        //goomba_input.vertical = attackVector.y;

      
        numAttacks++;
        //goomba_input.jumpbtndown = true;
        if (numAttacks >= maxAttacks) {
          Invoke("ResetNumAttacks",attackNumCdTime);
        }
      
      }

    }
    
    
    
  }

  private void ResetNumAttacks() {
    numAttacks = 0;

  }

  
  private void ResetAttackCD() {

    attackOffCd = true;
  }

  private int facing = -1;
  private void Walk() {
    //Debug.Log("Movespeed sign "+Mathf.Sign(config.move_speed));

    //Debug.Log("Grounded"+grounded);
    //Debug.Log("turnOffCd"+turnOffCd);
    float platform_walk_angle = PlatformAngle() - 90;
    Vector2 movement_direction = new Vector2(Mathf.Sin(platform_walk_angle * Mathf.Deg2Rad), Mathf.Cos(platform_walk_angle * Mathf.Deg2Rad));
    
    /* use the left control stick to move in direction */
    _input.leftstickx = movement_direction.x * direction;
    _input.leftsticky = movement_direction.y * direction;
    if (grounded) {
      Vector3 fwdangle = -transform.up - transform.right;
      Debug.DrawRay(transform.position,fwdangle,Color.magenta);

      //FwdCheck();

      /*
      float platform_angle = PlatformAngle();
      float angle1 = platform_angle - 100f;
      float angle2 = platform_angle + 100f;

      float angle = flip ? Mathf.Max(angle1, angle2) : Mathf.Min(angle1, angle2);

      
      Vector2 forwardangle_direction = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
      
     
      HashSet<RaycastHit2D> leaving_ground = GetObjectsInView(forwardangle_direction, 1f, 0, 5f, true);
      if (leaving_ground.Count <= 0) {
        Flip();
      }
      */
    }

  }
  
  /// <summary>
  /// Grab the angle of the platform or ground he is on at the moment.
  /// </summary>
  /// <returns>Angle of his ground.</returns>
  private float PlatformAngle() {
    float platform_angle = 0f;
    /* get platform information */
    HashSet<RaycastHit2D> hits = GetObjectsInView(-transform.up, configurator.ground_fov_angle, configurator.ground_ray_count, configurator.ground_ray_length);
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
  
  private void Flip() {
    flip = !flip;
    sprite_renderer.flipX = flip;
    direction = direction * -1;
  }
  
  

  /// <summary>
  /// Checks for if the player is in range to start attacking
  /// </summary>
  /// <returns></returns>
  private bool AgroCheck() {
   agro_game_objects = GetObjectsInView(transform.up, agro_ray_angle_fov, agro_ray_count, agro_ray_length, true);
    
    foreach (RaycastHit2D game_object in agro_game_objects) {
      
      //Todo: Figure out what the layer /tag should be
      if (LayerMask.LayerToName(game_object.transform.gameObject.layer) == "SlimeEffector") {
        Debug.DrawRay(transform.position,(game_object.transform.position-transform.position), Color.cyan);

        attackVector = game_object.transform.position - transform.position;
        return true;
      }
    }
    return false;
    
  }

  private Vector2 groundedNormalVector;
  
  /// <summary>
  /// Check if the player is in an "Upright" position
  /// </summary>
  /// <returns></returns>
  private bool UprightCheck() {
    grounded_game_objects = GetObjectsInView(GetGravity(), configurator.ground_fov_angle, configurator.ground_ray_count, configurator.ground_ray_length);
    
    foreach (RaycastHit2D gobject in grounded_game_objects) {
      RaycastHit2D groundhit = Physics2D.Raycast(transform.position,
      gobject.transform.position - transform.position * configurator.ground_ray_length);

      
      if (groundhit.normal == (Vector2)transform.up) {
        return true;
      }

      groundedNormalVector = groundhit.normal;

    }

    return false;
  }

  /// <summary>
  /// Put the player in an upright position. 
  /// </summary>
  private void Upright() {
    //TODO:FIX. CURRENTLY CAUSES WEIRD STRETCHING WITH SOFTBODY
    //transform.up = groundedNormalVector;

    //Special case where we set the ai's gravity the first time it touches a platform. 
    if (firstPlat) {
      firstPlat = false;
      //goomba_input.horg = -groundedNormalVector.x;
      //goomba_input.verg = -groundedNormalVector.y;
    }
    
  }

  
  /*To prevent merge issues just going to put this here for now. Does the same thing as IsGrounded() in generic player, 
  but saves the game objects it gets. Also just sets a field. This will save computation rather than having to recheck 
  everything mutiple times*/
  private bool GroundedCheck() {
    return grounded = is_grounded;
  }

  /* This checks if we may be in the air because we are already attacking as goombas are going to jump towards the player
   !!MAY CHANGE!!*/
  private bool AttackingCheck() {
    
//    if (attacking) {
//      return true;
//    }
    
    return false;
  }

  //For now does nothing. Maybe like a funny panic animation in the air
  private void Panic() {
    Debug.Log("Panicking");
  }

  
  /// <summary>
  /// Builds the Decision Tree for the ai. Designed as left branch is true, right branch is false for decision nodes. 
  /// </summary>
  private void BuildDecisionTree() {

    DecisionTree agroCheckNode = gameObject.AddComponent<DecisionTree>();
    agroCheckNode.SetDecisionDelegate(AgroCheck);

    DecisionTree walkNode = gameObject.AddComponent<DecisionTree>();
    walkNode.SetActionDelegate(Walk);

    DecisionTree attackNode = gameObject.AddComponent<DecisionTree>();
    attackNode.SetActionDelegate(Attack);

    DecisionTree uprightCheckNode = gameObject.AddComponent<DecisionTree>();
    uprightCheckNode.SetDecisionDelegate(UprightCheck);

    DecisionTree uprightNode = gameObject.AddComponent<DecisionTree>();
    uprightNode.SetActionDelegate(Upright);

    DecisionTree groundedCheckNode = gameObject.AddComponent<DecisionTree>();
    groundedCheckNode.SetDecisionDelegate(GroundedCheck);

    DecisionTree PanicNode = gameObject.AddComponent<DecisionTree>();
    PanicNode.SetActionDelegate(Panic);
    
    groundedCheckNode.SetLeftChild(uprightCheckNode);
    groundedCheckNode.SetRightChild(PanicNode);
    
    uprightCheckNode.SetLeftChild(agroCheckNode);
    uprightCheckNode.SetRightChild(uprightNode);
    
    
    agroCheckNode.SetLeftChild(attackNode);
    agroCheckNode.SetRightChild(walkNode);
    
    root = groundedCheckNode;
  }
}
