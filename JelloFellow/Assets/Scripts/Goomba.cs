using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

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
  
  
  private GoombaInput input;

  private DecisionTree root;

  private Rigidbody2D rb;
  private bool jumpOffCD = true;
 
  
  
  
  
  
  private void Start() {
    input = GetComponent<GoombaInput>();
    SetInput(input);
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
    if (IsGrounded() &&jumpOffCD) {
      jumpOffCD = false;
      
      /*TODO:Attack Attacking isn't great right now. Until I understand how enemies are moving on the side 
      using addforce for now
      rb.AddForce(targetray*jumpForce);   
      */
    }
    
    //Wait till goomba is grounded before hes able to attack again
    else if (IsGrounded() && !jumpOffCD) {
      Invoke("ResetJumpCD",.7f);

      
    }
    
  }
  private void ResetJumpCD() {
    jumpOffCD = true;
  }

  private void Walk() {
    input.horizontal = movespeed;
    Debug.Log("Im walking");

    if (IsGrounded()) {
      FwdCheck();
    }

  }

  private void FwdCheck() {
    float facing = Mathf.Sign(input.horizontal);
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
    HashSet<GameObject> game_objects = GetObjectsInView(transform.up,agro_ray_angle_fov, agro_ray_count,agro_ray_length, true);
    
    foreach (GameObject game_object in game_objects) {
      if (LayerMask.LayerToName(game_object.layer) == "Player") {
        return true;
      }
    }
    return false;
    
  }
  
  private void BuildDecisionTree() {

    DecisionTree agroCheckNode = gameObject.AddComponent<DecisionTree>();
    agroCheckNode.SetDecisionDelegate(AgroCheck);

    DecisionTree walkNode = gameObject.AddComponent<DecisionTree>();
    walkNode.SetActionDelegate(Walk);

    DecisionTree attackNode = gameObject.AddComponent<DecisionTree>();
    attackNode.SetActionDelegate(Attack);
    
    
    agroCheckNode.SetLeftChild(attackNode);
    agroCheckNode.SetRightChild(walkNode);
    
    root = agroCheckNode;
    


  }

  
  protected override void Update() {
    base.Update();
    
    // if I want to move
    
    
  }
  
  
}
