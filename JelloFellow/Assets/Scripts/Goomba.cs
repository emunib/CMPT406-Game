using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class Goomba : GenericPlayer {
  [Header("Stats")]
  [Range(-5f,5f)] public float movespeed = .01f;
  [Range(0.1f, 2f)] public float jumpCD = 0.7f;
  [Range(150, 400)] public int jumpForce = 150;


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
  private bool jumpOffCD = true;
  private HashSet<RaycastHit2D> agro_game_objects;
  private HashSet<RaycastHit2D> grounded_game_objects;
  private bool grounded;
  private bool attacking; 
  
  
  
  
  private void Start() {
    goomba_input = GetComponent<GoombaInput>();
    SetInput(goomba_input);
    SetIgnoreFields(false);
    root = gameObject.AddComponent<DecisionTree>();
    BuildDecisionTree();
    rb = GetComponent<Rigidbody2D>();
    base.Start();
  }

  private void FixedUpdate() {
    root.Search();
  }

  private void Attack() {
    Debug.Log("Im attacking");
    Transform target = GameObject.FindGameObjectWithTag("Player").transform;

    Vector2 targetray = target.position - transform.position;
    
    Debug.DrawRay(transform.position,targetray*5, Color.cyan);
    
    
    //Only Jump If I can
    if (grounded &&jumpOffCD) {
      jumpOffCD = false;
      
      /*TODO:Attack Attacking isn't great right now. Until I understand how enemies are moving on the side 
      using addforce for now
      rb.AddForce(targetray*jumpForce);   
      */
    }
    
    //Wait till goomba is grounded before hes able to attack again
    else if (grounded && !jumpOffCD) {
  
      
      jumpOffCD = true;
      
    }
    
  }
  private void ResetJumpCD() {
    jumpOffCD = true;
  }

  private void Walk() {
    
    Debug.DrawRay(transform.position,transform.right*10f);
    goomba_input.horizontal = transform.right.x;
    goomba_input.vertical = transform.right.y;
    
    
    Debug.Log("Grounded"+grounded);
    Debug.Log("turnOffCd"+turnOffCd);
    
    if (grounded && turnOffCd) {
      FwdCheck();
    }

  }
  private bool turnOffCd = true;
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

    Debug.Log("Turn: " +turn);
    if (turn) {
      rb.velocity = Vector2.zero;
      move_speed *= -1;
      turnOffCd = false;
      StartCoroutine(turnOnCdCoroutine());
    }
   /*
    if (fwdwallhit.collider != null || backwallhit.collider !=null || fwdgroundhit.collider ==null || backgroundhit.collider ==null) {
      if (LayerMask.LayerToName(fwdwallhit.transform.gameObject.layer)=="Ground") {
        
    //    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        move_speed *= -1;
        //rb.velocity = Vector2.zero;
        turnOffCd = false;
        StartCoroutine(turnOnCdCoroutine());

      }
    }
    

    if (fwdgroundhit.collider == null) {
      //transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
      move_speed*= -1;
      //rb.velocity = Vector2.zero;
      turnOffCd = false;
      StartCoroutine(turnOnCdCoroutine());


    }
    
    Debug.DrawRay(backCheck.transform.position,-backCheck.transform.right*fwdWallChkRange, Color.yellow);
	
    
    if (backwallhit.collider != null) {
      if (LayerMask.LayerToName(backwallhit.transform.gameObject.layer)=="Ground") {
        
        //transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        //move_speed *= -1;
        //rb.velocity = Vector2.zero;
        turnOffCd = false;
        StartCoroutine(turnOnCdCoroutine());

      }
    }

    Debug.Log("Gravity" + GetGravity());
    Debug.DrawRay(backCheck.transform.position,-transform.up*fwdGroundChkRange, Color.green);


    if (backgroundhit.collider == null) {
      //transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
      move_speed *= -1;
      //rb.velocity = Vector2.zero;
      //turnOffCd = false;
      StartCoroutine(turnOnCdCoroutine());
    }
*/


  }
  
  public IEnumerator turnOnCdCoroutine() {
    bool doneco = true;
    
    while (!turnOffCd) {

      if (grounded) {
        Vector2 fwd_angle = fwdCheck.transform.up + fwdCheck.transform.right;
        Vector2 back_angle = backCheck.transform.up - backCheck.transform.right; 
    
    
        RaycastHit2D fwdwallhit = Physics2D.Raycast(fwdCheck.transform.position, fwd_angle, fwdWallChkRange);
        RaycastHit2D fwdgroundhit = Physics2D.Raycast(fwdCheck.transform.position, -transform.up, fwdGroundChkRange);
        RaycastHit2D backwallhit = Physics2D.Raycast(backCheck.transform.position, back_angle, fwdWallChkRange);
        RaycastHit2D backgroundhit = Physics2D.Raycast(backCheck.transform.position, -transform.up, fwdGroundChkRange);
        /*
        Debug.DrawRay(fwdCheck.transform.position,fwdCheck.transform.right*fwdWallChkRange, Color.yellow);

        Debug.DrawRay(fwdCheck.transform.position,GetGravity().normalized*fwdGroundChkRange, Color.green);
        Debug.DrawRay(backCheck.transform.position,-transform.up*fwdGroundChkRange, Color.green);
        Debug.DrawRay(backCheck.transform.position,-backCheck.transform.right*fwdWallChkRange, Color.yellow);
*/
        if (fwdwallhit.collider == null && backwallhit.collider == null && fwdgroundhit.collider != null &&
            backgroundhit.collider != null) {
          yield return new WaitForSeconds(turnWait);

          turnOffCd = true;
        }

      }

      Debug.Log("IN coroutine");

      yield return null;

    }

    Debug.Log("Done coroutine");
  }
  
  

  private bool AgroCheck() {
   agro_game_objects = GetObjectsInView(transform.up, agro_ray_angle_fov, agro_ray_count, agro_ray_length, true);
    
    foreach (RaycastHit2D game_object in agro_game_objects) {
      if (LayerMask.LayerToName(game_object.transform.gameObject.layer) == "Player") {
        return true;
      }
    }
    return false;
    
  }

  private Vector2 groundedNormalVector;
  
  private bool UprightCheck() {
    grounded_game_objects = GetObjectsInView(GetGravity(), ground_fov_angle, ground_ray_count, ground_ray_length);
    
    foreach (RaycastHit2D gobject in grounded_game_objects) {
      RaycastHit2D groundhit = Physics2D.Raycast(transform.position,
      gobject.transform.position - transform.position * ground_ray_length);

      
      if (groundhit.normal == (Vector2)transform.up) {
        return true;
      }

      groundedNormalVector = groundhit.normal;

    }

    return false;
  }

  private void Upright() {

    transform.up = groundedNormalVector;

  }

  
  /*To prevent merge issues just going to put this here for now. Does the same thing as IsGrounded() in generic player, 
  but saves the game objects it gets. Also just sets a field. This will save computation rather than having to recheck 
  everything mutiple times*/
  private bool GroundedCheck() {
    return grounded = IsGrounded();
  }

  /* This checks if we may be in the air because we are already attacking as goombas are going to jump towards the player
   !!MAY CHANGE!!*/
  private bool AttackingCheck() {
    
    if (attacking) {
      return true;
    }
    
    return false;
  }

  //For now does nothing. Maybe like a funny panic animation in the air
  private void Panic() {
    Debug.Log("Panicking");
  }

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
