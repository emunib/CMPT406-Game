using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnChk : MonoBehaviour {
	
	
	[Range(1f,10f)]
	public float groundchkrange;
	
	private Enemy _enemyscript;
	private Transform parentTrans;
	private int dir = 1;
	// Use this for initialization
	void Start () {
		_enemyscript = transform.parent.GetComponent<Enemy>();

		parentTrans = _enemyscript.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
		Debug.DrawRay(transform.position,-transform.up*groundchkrange);
		Debug.DrawRay(transform.position,transform.right*dir, Color.blue);

		RaycastHit2D groundhit = Physics2D.Raycast(transform.position, -transform.up, groundchkrange);

		if (groundhit.collider == null) {
			Debug.Log("Turning");
			parentTrans.localScale = new Vector3(parentTrans.localScale.x *-1, parentTrans.localScale.y, parentTrans.localScale.z);	
			_enemyscript.movespeed = -_enemyscript.movespeed;
			dir = -dir;
		}
		
		RaycastHit2D wallhit = Physics2D.Raycast(transform.position, transform.right*dir, 1);

		if (wallhit.collider != null) {
			Debug.Log("Turning");
			parentTrans.localScale = new Vector3(parentTrans.localScale.x *-1, parentTrans.localScale.y, parentTrans.localScale.z);	
			_enemyscript.movespeed = -_enemyscript.movespeed;
			dir = -dir;
		}
		
		
	}
}
