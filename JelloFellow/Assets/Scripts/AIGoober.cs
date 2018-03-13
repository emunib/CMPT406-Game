using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGoober : GenericPlayer {

  

  //TODO:Maybe need another config so I can play with these settings
  [Header("AgroFOV Raycast Settings")]

  [CustomRangeLabel("Ray Length", 0f, 20f)] [Tooltip("Length of the ray.")]
  [SerializeField] private float agro_ray_length= 12;

  [CustomRangeLabel("Ray Count", 0f, 20f)] [Tooltip("Number of rays to show in between main rays.")]
  [SerializeField] private int agro_ray_count = 7;

  [CustomRangeLabel("Angle FOV", 0f, 180f)] [Tooltip("Padding for the angle.")]
  [SerializeField] private float agro_ray_angle_fov = 180;

  public bool VisualizeAgro = true;

  
  
  

  private DecisionTree root;

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
  private bool do_once;

  
  protected override void Start() {
    base.Start();

    _input = gameObject.AddComponent<GenericEnemyInput>();
    SetIgnoreFields(false);
    SetInput(_input);
    SetFieldRadius(2f);
    
    root = gameObject.AddComponent<DecisionTree>();
    BuildDecisionTree();
    flip = false;
    direction = 1;
    do_once = true;
  }

  protected override void FixedUpdate() {
    
    base.FixedUpdate();

    Vector2 g = new Vector2(_input.rightstickx,_input.rightsticky);
    
    //_input.DefaultValues();
    

    
    if (do_once) {
      _input.rightstickx = -transform.up.x;
      _input.rightsticky = -transform.up.y;
      do_once = false;
    }

    _input.button3_down = false;
    root.Search();
    
    DetectCollision();
  }



  public int maxAttacks =10;
  public float attackCdTime = .4f;
  public float attackNumCdTime = 5f;

  public int numAttacks = 0;
  
  private void Attack() {

    
    //Only Jump If I can
    if (grounded &&attackOffCd) {
      //attackOffCd = false;
      //Invoke("ResetAttackCD",attackCdTime);
      if (numAttacks < maxAttacks) {
     
        _input.button3_down = true;

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


  private void DetectCollision() {
    HashSet<RaycastHit2D> forward_check = GetObjectsInView(transform.right, 360, 10, 3f,true);
    foreach (RaycastHit2D hit in forward_check) {
      /* player in front */
      if (hit.transform.CompareTag("Player")) {
        hit.transform.parent.GetComponentInChildren<GenericPlayer>().Damage(1);
        
      }

     
    }
  }
  
  private void Walk() {

    float platform_walk_angle = PlatformAngle() - 90;
    Vector2 movement_direction = new Vector2(Mathf.Sin(platform_walk_angle * Mathf.Deg2Rad), Mathf.Cos(platform_walk_angle * Mathf.Deg2Rad));
    
    /* use the left control stick to move in direction */
    _input.leftstickx = movement_direction.x * direction;
    _input.leftsticky = movement_direction.y * direction;
    if (grounded) {
      Vector3 fwdangle = -transform.up - transform.right;
      Debug.DrawRay(transform.position,fwdangle,Color.magenta);

  
      float platform_angle = PlatformAngle();
      float angle1 = platform_angle - 100f;
      float angle2 = platform_angle + 100f;

      float angle = flip ? Mathf.Max(angle1, angle2) : Mathf.Min(angle1, angle2);

      
      Vector2 forwardangle_direction = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));

      HashSet<RaycastHit2D> leaving_ground = GetObjectsInView(forwardangle_direction, 1f, 0, 9f, true);
      
      
      if (leaving_ground.Count <= 0) {
        //Debug.Log("I should Turn");
        Flip();
        
      }
      else {
        platform_angle = PlatformAngle();
        angle1 = platform_angle - 90f;
        angle2 = platform_angle + 90f;

        angle = flip ? Mathf.Max(angle1, angle2) : Mathf.Min(angle1, angle2);


        forwardangle_direction = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
        leaving_ground = GetObjectsInView(forwardangle_direction, 1f, 0, 5f, true);
        if (leaving_ground.Count > 0) {
          //Debug.Log("I should Turn");
          Flip();
        }
      }
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
    direction = direction * -1;
  }
  
  

  /// <summary>
  /// Checks for if the player is in range to start attacking
  /// </summary>
  /// <returns></returns>
  private bool AgroCheck() {
   agro_game_objects = GetObjectsInView(transform.up, agro_ray_angle_fov, agro_ray_count, agro_ray_length, VisualizeAgro);
    
    foreach (RaycastHit2D game_object in agro_game_objects) {
      
      if (LayerMask.LayerToName(game_object.transform.gameObject.layer) == "Player") {
        Debug.DrawRay(transform.position,(game_object.transform.position-transform.position), Color.cyan);

        attackVector = game_object.transform.position - transform.position;

        float plat_angle = PlatformAngle();
        
        //TODO: FIX Bug on 225degree angled platforms
        if ((plat_angle > 45 && plat_angle < 135) || (plat_angle>=225 &&plat_angle<=315)) {
          if (direction == -1 && attackVector.y < 0) {
            Flip();
          }
          else if (direction == 1 && attackVector.y >= 0) {
            Flip();
          }
        }
        else {
          if (direction == -1 && attackVector.x < 0) {
            Flip();
          }
          else if (direction == 1 && attackVector.x >= 0) {
            Flip();
          }
        }

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

      Vector2 hit_normal = groundhit.normal;
      /* if the object has children then use the parent's rotation to calculate the normal */
      if (groundhit.collider!= null) {
        if (groundhit.collider.gameObject.transform.childCount > 0) {
          hit_normal =
            Quaternion.AngleAxis(groundhit.collider.gameObject.transform.rotation.eulerAngles.z, Vector3.forward) *
            groundhit.normal;
        }
      }
      

      float up = GetAngle(transform.up.x,transform.up.y);
      if ((Mathf.Abs(platform_angle-up))>1) {
        return false;
      }
      

      groundedNormalVector = groundhit.normal;

    }

    return true;
  }

  /// <summary>
  /// Put the player in an upright position. 
  /// </summary>
  private void Upright() {

    //Special case where we set the ai's gravity the first time it touches a platform. 
    
    foreach (RaycastHit2D hit in grounded_game_objects) {
      Vector2 hit_normal = hit.normal;
      /* if the object has children then use the parent's rotation to calculate the normal */
      if (hit.collider.gameObject.transform.childCount > 0) {
        hit_normal = Quaternion.AngleAxis(hit.collider.gameObject.transform.rotation.eulerAngles.z, Vector3.forward) * hit.normal;
      }

      /* get platform information we just hit */
      //TODO:Not sure why its y then x. This gives wrong angle information
      float platform_angle_update = GetAngle(hit_normal.y, hit_normal.x);
      
      transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, platform_angle_update != 0f ? platform_angle_update - 90 : 0f), 3* Time.deltaTime);
      _input.rightstickx = -transform.up.x;
      _input.rightsticky = -transform.up.y;

      break;
    }
    
    
  }

  
  /*To prevent merge issues just going to put this here for now. Does the same thing as IsGrounded() in generic player, 
  but saves the game objects it gets. Also just sets a field. This will save computation rather than having to recheck 
  everything mutiple times*/
  private bool GroundedCheck() {
    return grounded = is_grounded;
  }

 

  //For now does nothing. Maybe like a funny panic animation in the air
  private void Panic() {

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
