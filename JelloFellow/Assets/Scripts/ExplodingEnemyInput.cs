using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemyInput : Input2D {
//	[HideInInspector] public float lefttrigger;
//	[HideInInspector] public float righttrigger;
//	[HideInInspector] public float leftstickx;
//	[HideInInspector] public float leftsticky;
//	[HideInInspector] public float rightstickx;
//	[HideInInspector] public float rightsticky;
//	[HideInInspector] public bool leftbumper_down;
//	[HideInInspector] public bool leftbumper_up;
//	[HideInInspector] public bool rightbumper_down;
//	[HideInInspector] public bool rightbumper_up;
//	[HideInInspector] public bool button1_down;
//	[HideInInspector] public bool button1_up;
//	[HideInInspector] public bool button2_down;
//	[HideInInspector] public bool button2_up;
//	[HideInInspector] public bool button3_down;
//	[HideInInspector] public bool button3_up;
//	[HideInInspector] public bool button4_down;
//	[HideInInspector] public bool button4_up;
//	[HideInInspector] public bool leftstickclick_down;
//	[HideInInspector] public bool leftstickclick_up;
//	[HideInInspector] public bool rightstickclick_down;
//	[HideInInspector] public bool rightstickclick_up;
	
	public float lefttrigger;
	public float righttrigger;
	public float leftstickx;
	public float leftsticky;
	public float rightstickx;
	public float rightsticky;
	public bool leftbumper_down;
	public bool leftbumper_up;
	public bool rightbumper_down;
	public bool rightbumper_up;
	public bool button1_down;
	public bool button1_up;
	public bool button2_down;
	public bool button2_up;
	public bool button3_down;
	public bool button3_up;
	public bool button4_down;
	public bool button4_up;
	public bool leftstickclick_down;
	public bool leftstickclick_up;
	public bool rightstickclick_down;
	public bool rightstickclick_up;

	/// <summary>
	/// Changes all variables to its default values. Good
	/// to call every update after changing it.
	/// </summary>
	public void DefaultValues() {
		/* default values */
		lefttrigger = 0.0f;
		righttrigger = 0.0f;
		leftstickx = 0.0f;
		leftsticky = 0.0f;
		rightstickx = 0.0f;
		rightsticky = 0.0f;
		leftbumper_down = false;
		leftbumper_up = false;
		rightbumper_down = false;
		rightbumper_up = false;
		button1_down = false;
		button1_up = false;
		button2_down = false;
		button2_up = false;
		button3_down = false;
		button3_up = false;
		button4_down = false;
		button4_up = false;
		leftstickclick_down = false;
		leftstickclick_up = false;
		rightstickclick_down = false;
		rightstickclick_up = false;
	}

	public override float GetLeftTrigger() {
		return lefttrigger;
	}

	public override float GetRightTrigger() {
		return righttrigger;
	}

	public override float GetHorizontalLeftStick() {
		return leftstickx;
	}

	public override float GetVerticalLeftStick() {
		return leftsticky;
	}

	public override float GetHorizontalRightStick() {
		return rightstickx;
	}

	public override float GetVerticalRightStick() {
		return rightsticky;
	}

	public override bool GetLeftBumperDown() {
		return leftbumper_down;
	}

	public override bool GetLeftBumperUp() {
		return leftbumper_up;
	}

	public override bool GetRightBumperDown() {
		return rightbumper_down;
	}

	public override bool GetRightBumperUp() {
		return rightbumper_up;
	}

	public override bool GetButton1Down() {
		return button1_down;
	}

	public override bool GetButton1Up() {
		return button1_up;
	}

	public override bool GetButton2Down() {
		return button2_down;
	}

	public override bool GetButton2Up() {
		return button2_up;
	}

	public override bool GetButton3Down() {
		return button3_down;
	}

	public override bool GetButton3Up() {
		return button3_up;
	}

	public override bool GetButton4Down() {
		return button4_down;
	}

	public override bool GetButton4Up() {
		return button4_up;
	}

	public override bool GetLeftStickDown() {
		return leftstickclick_down;
	}

	public override bool GetLeftStickUp() {
		return leftstickclick_up;
	}

	public override bool GetRightStickDown() {
		return rightstickclick_down;
	}

	public override bool GetRightStickUp() {
		return rightstickclick_up;
	}
}
