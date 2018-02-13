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
	private Input2D 	_input;
	private int			level;
	private int			ammountOfLevels;

	public void Start()
	{
		InvokeRepeating("CheckForControllerInput", 0.0f, 0.07f);
		_input = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>().GetInput();
		scrollRect = GetComponent<ScrollRect>();
		buttonsArray = GetComponentsInChildren<Button>();
		buttonsArray[index].Select();
		level = 0;
		if(buttonsArray.Length%4 == 0)
		{
			ammountOfLevels = buttonsArray.Length / 4;
		}
		else 
		{
			ammountOfLevels = (buttonsArray.Length / 4) + 1;
		}
		yPos  = 1f - ((float)index / (buttonsArray.Length - 1));
		
	}

	public void Update(){
		select = _input.GetJumpButtonDown();

		if (select)
		{
			buttonsArray[index].onClick.Invoke();
		}
	}

	public void CheckForControllerInput()
	{
		upInput = _input.GetVerticalMovement() > 0;
		downInput = _input.GetVerticalMovement() < 0;
		leftInput = _input.GetHorizontalMovement() < 0;
		rightInput = _input.GetHorizontalMovement() > 0;

		if (upInput ^ downInput ^ leftInput ^ rightInput)
		{
			if (upInput)
			{
				if (index >= 4)
				{
					index = Mathf.Clamp(index - 4, 0, buttonsArray.Length - 1);
				}

				if (level > 0)
				{
					level--;
				}
			}
			else if (downInput)
			{
				if (index <= buttonsArray.Length - 5)
				{
					index = Mathf.Clamp(index + 4, 0, buttonsArray.Length - 1);
				}

				if (level < ammountOfLevels - 1)
				{
					level++;
				}
			}
			else if (leftInput)
			{
				if (index % 4 != 0)
				{
					index = Mathf.Clamp(index - 1, 0, buttonsArray.Length - 1);
				}
			}
			else if(rightInput)
			{
				if (index % 4 != 3)
				{
					index = Mathf.Clamp(index + 1, 0, buttonsArray.Length - 1);
				}
			}

			buttonsArray[index].Select();
			if (ammountOfLevels > 2)
			{
				yPos = 1f - ((float) level / (ammountOfLevels - 1));
			}

		}
		scrollRect.verticalNormalizedPosition = Mathf.Lerp(scrollRect.verticalNormalizedPosition, yPos, Time.deltaTime / lerpTime);	
	}
}
