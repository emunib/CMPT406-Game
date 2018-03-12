using UnityEngine;

public class SmartAI : GravityComponent {
	private UnityJellySprite jelly;
	private bool flip;
	
	private void Start() {
		jelly = GetComponent<JellySpriteReferencePoint>().ParentJellySprite.GetComponent<UnityJellySprite>();
		flip = jelly.m_FlipX;
	}

	protected override void Update() {
		base.Update();
		rigidbody.AddForce(-transform.right);
	}

	/// <summary>
	/// Flip the sprite.
	/// </summary>
	private void Flip() {
		flip = !flip;
		jelly.m_FlipX = flip;
	}
}
