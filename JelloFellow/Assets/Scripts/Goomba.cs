using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.UI;

public class Goomba : GenericPlayer {
  
  [Header("ForwardCheck Information")] public Transform fwdCheck;
  public Transform backCheck;
  [Range(.01f, 2)] public float fwdGroundChkRange = 1f;
  [Range(.01f, 2)] public float fwdWallChkRange = 1f;
  [Range(.01f,1f)]
  public float turnWait = .5f;

  
  [Header("AgroFOV Raycast Settings")]

  [CustomRangeLabel("Ray Length", 0f, 20f)] [Tooltip("Length of the ray.")]
  [SerializeField] private float agro_ray_length;

  [CustomRangeLabel("Ray Count", 0f, 20f)] [Tooltip("Number of rays to show in between main rays.")]
  [SerializeField] private int agro_ray_count;

  [CustomRangeLabel("Angle FOV", 0f, 180f)] [Tooltip("Padding for the angle.")]
  [SerializeField] private float agro_ray_angle_fov;

  
  
  
  private GoombaInput goomba_input;

  private DecisionTree root;

  private Rigidbody2D rb;
  private bool attackOffCd = true;
  private HashSet<RaycastHit2D> agro_game_objects;
  private HashSet<RaycastHit2D> grounded_game_objects;
  private bool grounded;
  //private bool attacking; 
  private bool firstPlat = true;
  private bool turnOffCd = true;
  private Vector2 attackVector;
  
  
  protected override void Start() {    
    goomba_input = GetComponent<GoombaInput>();
    SetInput(goomba_input);
    SetIgnoreFields(false);
    root = gameObject.AddComponent<DecisionTree>();
    BuildDecisionTree();
    rb = GetComponent<Rigidbody2D>();
    base.Start();

    goomba_input.horg = -transform.up.x;
    goomba_input.verg = -transform.up.y;


  }

  
  
  protected override void Update() {
    base.Update();
    goomba_input.jumpbtndown = false;
    root.Search();
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
        goomba_input.horizontal = attackVector.x;
        goomba_input.vertical = attackVector.y;

      
        numAttacks++;
        goomba_input.jumpbtndown = true;
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

  private int facing = 1;
  private void Walk() {
    //Debug.Log("Movespeed sign "+Mathf.Sign(config.move_speed));
    goomba_input.horizontal = transform.right.x*facing;
    goomba_input.vertical = transform.right.y*facing;

    
    
    //goomba_input.jumpbtndown = true;
    
    //Debug.Log("Grounded"+grounded);
    //Debug.Log("turnOffCd"+turnOffCd);
    
    if (grounded && turnOffCd) {
      FwdCheck();
    }

  }
  
  /// <summary>
  /// Check forward AND BACK for a wall. Backwards check is because sometimes the ai lands weird. Uses raycasts to check
  /// range determined by public parameters. 
  /// </summary>
  private void FwdCheck() {

    bool turn = false;
    Vector2 fwd_angle = fwdCheck.transform.up + fwdCheck.transform.right;
    Vector2 back_angle = backCheck.transform.up - backCheck.transform.right; 
    
    
    RaycastHit2D fwdwallhit = Physics2D.Raycast(transform.position, fwd_angle, fwdWallChkRange);
    RaycastHit2D fwdgroundhit = Physics2D.Raycast(fwdCheck.transform.position, -transform.up, fwdGroundChkRange);
    RaycastHit2D backwallhit = Physics2D.Raycast(transform.position, back_angle, fwdWallChkRange);
    RaycastHit2D backgroundhit = Physics2D.Raycast(backCheck.transform.position, -transform.up, fwdGroundChkRange);

    Debug.DrawRay(transform.position,(fwd_angle)*fwdWallChkRange, Color.yellow);
    Debug.DrawRay(fwdCheck.transform.position,-transform.up*fwdGroundChkRange, Color.green);
    
    Debug.DrawRay(transform.position,(back_angle)*fwdWallChkRange, Color.yellow);
    Debug.DrawRay(backCheck.transform.position,-backCheck.transform.up*fwdGroundChkRange, Color.green);


    if (fwdwallhit.collider != null && LayerMask.LayerToName(fwdwallhit.transform.gameObject.layer)=="Ground") {

      turn = true;
    }
    else if (backwallhit.collider != null&& LayerMask.LayerToName(backwallhit.transform.gameObject.layer)=="Ground") {

      turn = true;
    }

    else if (fwdgroundhit.collider == null) {
      turn = true;
    }
    else if(backgroundhit.collider == null) {
      turn = true;
      
    }

    //Debug.Log("Turn: " +turn);
    if (turn) {
      rb.velocity = Vector2.zero;
      //move_speed *= -1;
      turnOffCd = false;
      facing *= -1;
      StartCoroutine(turnOnCdCoroutine());
    }
  

  }
  
  /// <summary>
  /// This Makes it so the ai waits till all the checks the ai uses to detect a wall or lack of ground only restarts when
  /// the failed checks are cleared after turning the ai. 
  /// </summary>
  /// <returns></returns>
  public IEnumerator turnOnCdCoroutine() {
    while (!turnOffCd) {

      if (grounded) {
        Vector2 fwd_angle = fwdCheck.transform.up + fwdCheck.transform.right;
        Vector2 back_angle = backCheck.transform.up - backCheck.transform.right; 
    
        //could possibly use same raycasts as before as they're the exact same and pass them into this function.
        //May save some computation     
        RaycastHit2D fwdwallhit = Physics2D.Raycast(transform.position, fwd_angle, fwdWallChkRange);
        RaycastHit2D fwdgroundhit = Physics2D.Raycast(fwdCheck.transform.position, -transform.up, fwdGroundChkRange);
        RaycastHit2D backwallhit = Physics2D.Raycast(transform.position, back_angle, fwdWallChkRange);
        RaycastHit2D backgroundhit = Physics2D.Raycast(backCheck.transform.position, -transform.up, fwdGroundChkRange);
   
        
        //Keep redoing until this check clears
        if (fwdwallhit.collider == null && backwallhit.collider == null && fwdgroundhit.collider != null &&
            backgroundhit.collider != null) {
   
          //Found it made better having a few miliseconds of wait before we actually allow turning again. 
          yield return new WaitForSeconds(turnWait);

          turnOffCd = true;
        }

      }

      //Debug.Log("IN coroutine");

      yield return null;

    }

    //Debug.Log("Done coroutine");
  }
  
  

  /// <summary>
  /// Checks for if the player is in range to start attacking
  /// </summary>
  /// <returns></returns>
  private bool AgroCheck() {
   agro_game_objects = GetObjectsInView(transform.up, agro_ray_angle_fov, agro_ray_count, agro_ray_length, false);
    
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

    transform.up = groundedNormalVector;

    //Special case where we set the ai's gravity the first time it touches a platform. 
    if (firstPlat) {
      firstPlat = false;
      goomba_input.horg = -groundedNormalVector.x;
      goomba_input.verg = -groundedNormalVector.y;
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
  
  protected override void Death() {
    Destroy(gameObject);
  }
}
