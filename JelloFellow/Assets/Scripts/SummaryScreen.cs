using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryScreen : MonoBehaviour 
{
	[SerializeField]
	public Button		button;
	private bool 		select;
	private Input2D 	input;


	public void Start()
	{
		input = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>().GetInput();
     
	}

	public void Update(){
		
		//If selected click the button
		select = input.GetJumpButtonDown();
		if (select)
		{
			button.onClick.Invoke();
		}
	}
}
