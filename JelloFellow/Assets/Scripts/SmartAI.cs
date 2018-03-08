using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartAI : GenericPlayer {
	private GenericEnemyInput _input;
	
	protected override void Start() {
		/* call this to run Awake in the subclass */
		base.Start();

		/* set the input for generic player, and its field radius */
		_input = gameObject.AddComponent<GenericEnemyInput>();
		SetInput(_input);
		SetIgnoreFields(false);
		SetFieldRadius(12f);
	}

	protected override void Update() {
		/* reset all the values */
		_input.DefaultValues();
		
		/* use the left control stick to move in direction */
		_input.leftstickx = 0f;
		_input.leftsticky = 0f;
		_input.button3_down = true;
		
		/* call this to run Update in the subclass */
		/* we call update after is because we want to change the input then call the update to handle the input changes
		   in the same frame rather to have to wait another frame */
		base.Update();
	}
}
