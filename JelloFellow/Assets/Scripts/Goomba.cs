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
  
  
  private GoombaInput input;

  private DecisionTree root;

  private Rigidbody2D rb;
  private bool jumpOnCD;
 
  
  
  
  
  
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
    if (IsGrounded() &&jumpOnCD ==false) {
      jumpOnCD = true;
      //using addforce for now
      rb.AddForce(transform.up*jumpForce);   
      //Invoke("ResetJumpCD",.7f);
    }
    else if (IsGrounded() == false &&jumpOnCD == true) {
      jumpOnCD = false;
    }
    
  }


  private void Walk() {
    input.horizontal = movespeed;
    Debug.Log("Im walking");

    FwdCheck();
    
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
    HashSet<GameObject> game_objects = GetObjectsInView(transform.right, true);
    
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
