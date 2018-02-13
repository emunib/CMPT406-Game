/*

    Based code on snipet from https://gamedev.stackexchange.com/questions/112662/unity-autoscroll-when-selecting-buttons-out-of-viewport

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class SceneSelectorScene : MonoBehaviour 
{
	[SerializeField]
	private float		lerpTime;
	private ScrollRect 	scrollRect;
	private Button[]	buttonsArray;
	private int			index;
	private float		yPos;
	private bool		upInput;
	private bool		downInput;
	private bool		leftInput;
	private bool		rightInput;
	private bool 		select;
	private Input2D 	input;
	private int			row;
	private int			ammountOfRows;


	public void Start()
	{
		InvokeRepeating("CheckForControllerInput", 0.0f, 0.1f);
		input = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>().GetInput();
		scrollRect = GetComponent<ScrollRect>();
		buttonsArray = GetComponentsInChildren<Button>();
		buttonsArray[index].Select();
		row = 0;
		lerpTime = 0.01f;
        //Checks to see how many rows of scenes there are
		if(buttonsArray.Length%4 == 0)
		{
			ammountOfRows = buttonsArray.Length / 4;
		}
		else 
		{
			ammountOfRows = (buttonsArray.Length / 4) + 1;
		}
		yPos  = 1f - ((float)index / (buttonsArray.Length - 1));
		
	}

	public void Update(){
		
		//If selected click the button
		select = input.GetJumpButtonDown();
		if (select)
		{
			buttonsArray[index].onClick.Invoke();
		}
	}

	public void CheckForControllerInput()
	{
		float hor_m = input.GetHorizontalMovement();
		float ver_m = input.GetVerticalMovement();

		if (Mathf.Abs (hor_m) > 0 || Mathf.Abs (ver_m) > 0) {
			//Move Up
			if (ver_m > 0)
			{
				if (index >= 4)
				{
					index = Mathf.Clamp(index - 4, 0, buttonsArray.Length - 1);
				}

				if (row > 0)
				{
					row--;
				}
			}
			//Move Down
			else if (ver_m < 0)
			{
				if (index <= buttonsArray.Length - 5)
				{
					index = Mathf.Clamp(index + 4, 0, buttonsArray.Length - 1);
				}

				if (row < ammountOfRows - 1)
				{
					row++;
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
            //Don't scroll the screen if there are 2 or less rows of scenes
			if (ammountOfRows > 2)
			{
				yPos = 1f - ((float) row / (ammountOfRows - 1));
			}
		}
		//Scroll the screen to the proper hight
		scrollRect.verticalNormalizedPosition = Mathf.Lerp(scrollRect.verticalNormalizedPosition, yPos, Time.deltaTime / lerpTime);
	}
}
