/*

    Based code on snipet from https://gamedev.stackexchange.com/questions/112662/unity-autoscroll-when-selecting-buttons-out-of-viewport

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSelector : MonoBehaviour 
{
	[SerializeField]
	private Button[]	buttonsArray;
	private int			index;
	private float		yPos;
	private bool		upInput;
	private bool		downInput;
	private bool		leftInput;
	private bool		rightInput;
	private bool 		select;
	private Input2D 	input;



	public void Start(){
		InvokeRepeating("CheckForControllerInput", 0.0f, 0.1f);
		input = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>().GetInput();
		buttonsArray = GetComponentsInChildren<Button>();
		buttonsArray[index].Select();
	}

	public void Update(){

		//If selected click the button
		select = input.GetButton3Down();
		if (select)
		{
			buttonsArray[index].onClick.Invoke();
		}
	}

	public void CheckForControllerInput()
	{
		float hor_m = input.GetHorizontalLeftStick();
		float ver_m = input.GetVerticalLeftStick();

		if (Mathf.Abs (hor_m) > 0 || Mathf.Abs (ver_m) > 0) {
			//Move Up
			if (ver_m > 0)
			{
				if (index >= 4)
				{
					index = Mathf.Clamp(index - 4, 0, buttonsArray.Length - 1);
				}
			}
			//Move Down
			else if (ver_m < 0)
			{
				if (index <= buttonsArray.Length - 5)
				{
					index = Mathf.Clamp(index + 4, 0, buttonsArray.Length - 1);
				}
			}
			//Move Left
			if (hor_m < 0)
			{
				if (index % 4 != 0)
				{
					index = Mathf.Clamp(index - 1, 0, buttonsArray.Length - 1);
				}
			}
			// Move Right
			else if(hor_m > 0)
			{
				if (index % 4 != 3)
				{
					index = Mathf.Clamp(index + 1, 0, buttonsArray.Length - 1);
				}
			}
			buttonsArray[index].Select();

		}
	}
}
