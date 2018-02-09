using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnChk : MonoBehaviour {
	
	
	[Range(1f,10f)]
	public float Groundchkrange;


	[Range(1f, 10f)] 
	public float WallchkRange;
		
	private Enemy _enemyscript;
	private Transform parentTrans;
	private int dir = 1;

	
	
	private void Start () {
		_enemyscript = transform.parent.GetComponent<Enemy>();

		parentTrans = _enemyscript.transform;
	}
	
	private void Update () {
		
		
		//(start point, direction*direction facing * length, colour)
		Debug.DrawRay(transform.position,-transform.up*Groundchkrange);
		Debug.DrawRay(transform.position,transform.right*dir*WallchkRange, Color.blue);
		
		
		//(start, direction, range)
		RaycastHit2D groundhit = Physics2D.Raycast(transform.position, -transform.up, Groundchkrange);

		if (groundhit.collider == null) {
			Debug.Log("Turning");
			parentTrans.localScale = new Vector3(parentTrans.localScale.x *-1, parentTrans.localScale.y, parentTrans.localScale.z);	
			_enemyscript.movespeed = -_enemyscript.movespeed;
			dir = -dir;
		}
		
		RaycastHit2D wallhit = Physics2D.Raycast(transform.position, transform.right*dir, WallchkRange);

		if (wallhit.collider != null) {
			Debug.Log("Turning");
			parentTrans.localScale = new Vector3(parentTrans.localScale.x *-1, parentTrans.localScale.y, parentTrans.localScale.z);	
			_enemyscript.movespeed = -_enemyscript.movespeed;
			dir = -dir;
		}
		
		
	}
}
