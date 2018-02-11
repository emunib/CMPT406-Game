using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public int movespeed;
	[Range(0, 5)] 
	public float GroundedChkRange;
	
		
	[Range(1f,10f)]
	public float Groundchkrange;


	[Range(1f, 10f)] 
	public float WallchkRange;
		
	private int dir = 1;

	
	private Rigidbody2D rb;
	// Use this for initialization

	private DecisionTree _root;
	public Transform fwdchk;
	
	
	private void Start () {
		rb = GetComponent<Rigidbody2D> ();
		
		BuildDecisionTree();
	

	}
	
	// Update is called once per frame
	private void Update () {
		//rb.velocity = new Vector2 (movespeed, rb.velocity.y);
		//GroundedChk();
		

	}

	private void FixedUpdate() {
		_root.Search();
	}

	private void BuildDecisionTree() {
		
		DecisionTree groundChkNode = this.gameObject.AddComponent<DecisionTree>();
		groundChkNode.SetDecisionDelegate(GroundedChk);
		
		DecisionTree iamgroundednode = this.gameObject.AddComponent<DecisionTree>();
		iamgroundednode.SetActionDelegate(IamGrounded);
		
		
		DecisionTree iamnotgroundednode = this.gameObject.AddComponent<DecisionTree>();
		iamnotgroundednode.SetActionDelegate(IamnotGrounded);

		DecisionTree walkNode = this.gameObject.AddComponent<DecisionTree>();
		walkNode.SetActionDelegate(Walk);

		DecisionTree uprightCheckNode = gameObject.AddComponent<DecisionTree>();

		DecisionTree orientateSelfNode = gameObject.AddComponent<DecisionTree>();
		
		
		groundChkNode.SetLeftChild(walkNode);
		groundChkNode.SetRightChild(iamnotgroundednode);

		_root = groundChkNode;


		

	}

	bool GroundedChk() {
		Debug.DrawRay(transform.position, -transform.up*GroundedChkRange, Color.green);

		RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, GroundedChkRange);


		//If the ground check collides with something then we are grounded
		return hit.collider != null;
		

	}

	void IamGrounded() {
		//Debug.Log("I am grounded");
	}

	void IamnotGrounded() {
		//Debug.Log("I am in the air");

	}

	private void OrientateSelf() {
		Vector3 g = Physics2D.gravity;
		
		
		
	}
	
	private void Walk() {
		Vector3 g = Physics2D.gravity;
		Vector2 right = Vector3.Cross(g, -transform.forward);
		
		
		rb.AddForce(right*.8f*dir);
		if (rb.velocity.x > 5) {
			rb.velocity = new Vector2(5, rb.velocity.y);
		}
		else if (rb.velocity.x < -5) {
			rb.velocity = new Vector2(-5, rb.velocity.y);
		} 
		if (rb.velocity.y > 5) {
			rb.velocity = new Vector2(rb.velocity.x, 5);
		}
		else if (rb.velocity.y < -5) {
			rb.velocity = new Vector2(rb.velocity.x, -5);
		}
		
		//(start point, direction*direction facing * length, colour)
		Debug.DrawRay(fwdchk.transform.position,-fwdchk.transform.up*Groundchkrange);
		Debug.DrawRay(fwdchk.transform.position,fwdchk.transform.right*dir*WallchkRange, Color.blue);
		
		
		//(start, direction, range)
		RaycastHit2D groundhit = Physics2D.Raycast(fwdchk.transform.position, -fwdchk.transform.up, Groundchkrange);

		if (groundhit.collider == null) {
			Debug.Log("Turning");
			transform.localScale = new Vector3(transform.localScale.x *-1, transform.localScale.y, transform.localScale.z);	
			//movespeed = movespeed;
			dir = -dir;
			rb.velocity = Vector2.zero;
		}
		
		RaycastHit2D wallhit = Physics2D.Raycast(fwdchk.transform.position, fwdchk.transform.right*dir, WallchkRange);

		if (wallhit.collider != null) {
			//Debug.Log("Turning");
			transform.localScale = new Vector3(transform.localScale.x *-1, transform.localScale.y, transform.localScale.z);	
			///_enemyscript.movespeed = -_enemyscript.movespeed;
			dir = -dir;
			rb.velocity = Vector2.zero;

		}
		
	}

	private void OnDrawGizmos() {
		Vector3 g = Physics2D.gravity;
		Vector2 right = Vector3.Cross(g, -transform.forward);
		
	
		
		//Gizmos.DrawRay(transform.position, right);
		
	}
}
