using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFall : MonoBehaviour {
	private Rigidbody2D rbody;
	//Use this for initialization
	void Start()
	{
		rbody = gameObject.GetComponent<Rigidbody2D>();
		DropTheButton ();
	}
		
	//Change Kenectic to Dynamic Rigidbody
	void DropTheButton(){
		if (rbody.isKinematic = true) {
			StartCoroutine (FT (10f));
		} else {
			//backtoRest ();
		}

	}
	// backtoRest(){
	//		rbody.gravityScale = 0f;
	//		rbody.isKinematic = true;
	//}
		
	//10Sec Delay on Start before adding Gravity
	IEnumerator FT(float time)
	{
		yield return new WaitForSeconds(time);
		rbody.isKinematic = false;
		rbody.gravityScale = 0.5f;
	}
}