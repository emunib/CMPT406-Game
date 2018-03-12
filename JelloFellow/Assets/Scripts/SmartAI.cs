using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartAI : GenericPlayer {
	private GenericEnemyInput _input;
	private UnityJellySprite jelly_parent;
	private bool flip;
	
	protected override void Start() {
		/* call this to run Awake in the subclass */
		base.Start();

		/* set the input for generic player, and its field radius */
		_input = gameObject.AddComponent<GenericEnemyInput>();
		SetInput(_input);
		SetIgnoreFields(false);
		SetFieldRadius(12f);

		jelly_parent = GetComponent<JellySpriteReferencePoint>().ParentJellySprite.GetComponent<UnityJellySprite>();
		flip = jelly_parent.m_FlipX;

	}

	protected override void Update() {
		/* reset all the values */
		_input.DefaultValues();
		
		/* use the left control stick to move in direction */
		_input.leftstickx = 0f;
		_input.leftsticky = 0f;
		if(is_grounded) print("Is on ground");
		/* call this to run Update in the subclass */
		/* we call update after is because we want to change the input then call the update to handle the input changes
		   in the same frame rather to have to wait another frame */
		base.Update();
	}
	
	/// <summary>
	/// Flip the sprite.
	/// </summary>
	private void Flip() {
		flip = !flip;
		jelly_parent.m_FlipX = flip;
	}
}
