using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class Goomba : GenericPlayer {
  [Header("Stats")]
  [Range(-1f,1f)] public float movespeed = .01f;
  [Range(0.1f, 2f)] public float jumpCD = 0.7f;
  [Range(150, 400)] public int jumpForce = 150;


  [Header("ForwardCheck")] public Transform fwdCheck;
  [Range(.01f, 2)] public float fwdGroundChkRange = 1f;
  [Range(.01f, 2)] public float fwdWallChkRange = 1f;
  
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
  }

  private void FixedUpdate() {
    root.Search();
  }

  private void Attack() {
    Debug.Log("Im attacking");
    // Presumably use with this input.GetJumpButtonDown();
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
      //Dont know if it should be instant or not
      Invoke("ResetJumpCD",.7f);
      
      //jumpOffCD = true;
      
    }
    
  }
  private void ResetJumpCD() {
    jumpOffCD = true;
  }

  private void Walk() {
    goomba_input.horizontal = movespeed;
    Debug.Log("Im walking");

    if (grounded) {
      FwdCheck();
    }

  }

  private void FwdCheck() {
    float facing = Mathf.Sign(goomba_input.horizontal);
    Debug.DrawRay(fwdCheck.transform.position,facing*fwdCheck.transform.right*fwdWallChkRange, Color.yellow);
	
    
    RaycastHit2D wallhit = Physics2D.Raycast(fwdCheck.transform.position, facing*fwdCheck.transform.right, fwdWallChkRange);
    if (wallhit.collider != null) {
      if (LayerMask.LayerToName(wallhit.transform.gameObject.layer)=="Ground") {
        
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        movespeed *= -1;
        rb.velocity = Vector2.zero;
      }
    }

    Debug.DrawRay(fwdCheck.transform.position,GetGravity()*fwdGroundChkRange, Color.green);

    RaycastHit2D groundhit = Physics2D.Raycast(fwdCheck.transform.position, GetGravity(), fwdGroundChkRange);

    if (groundhit.collider == null) {
      transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
      movespeed *= -1;
      rb.velocity = Vector2.zero;

    }
    
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
    grounded_game_objects = GetObjectsInView(GetGravity(), ray_angle_fov, ray_count, ray_length, true);
    
    foreach (RaycastHit2D gobject in grounded_game_objects) {
      RaycastHit2D groundhit = Physics2D.Raycast(transform.position,
      gobject.transform.position - transform.position * ray_length);

      
      if (groundhit.normal == (Vector2)transform.up) {
        groundedNormalVector = groundhit.normal;
        return true;
      }

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
    return IsGrounded();
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
