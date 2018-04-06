using UnityEngine;
using UnityEngine.UI;

public class LoaderModified : MonoBehaviour {
	public Texture multiSpriteTexture;
	public Sprite[] sprites;
	
	[Range(0.0f,0.99f)]
	public float animationSpeed = .9f;
	public bool reverse;
	public bool pingpong;

	public bool rotate;
	public float rotation = 15;
	public float rotationTick = .1f;


	int animateVariationsCounter;
	Image cacheRenderer;
	int countAdd = 1;
	bool pong;

	void Start() {
		if (!multiSpriteTexture) return;
		cacheRenderer = GetComponent<Image>();
		Invoke("Animate", 1f - animationSpeed);
		Invoke("RotateSprite", rotationTick);
	}

	void Animate() {
		if (sprites.Length == 0 || animationSpeed == 0) goto anim; 
		if (pingpong && (animateVariationsCounter == sprites.Length || animateVariationsCounter == 0)) {
			if (!pong) 	reverse = pong = true;
			else reverse = pong = false;		
		}
		if (reverse) {
			if (animateVariationsCounter == 0) animateVariationsCounter = sprites.Length;
			countAdd = -1;
		}  else countAdd = 1;
		Sprite s = sprites[animateVariationsCounter % sprites.Length];
		cacheRenderer.sprite = s;
		animateVariationsCounter += countAdd;
		anim: Invoke("Animate", 1f - animationSpeed);
	}

	void RotateSprite() {
		if (!rotate || rotation == 0) goto anim;
		transform.Rotate(Vector3.forward * rotation);
		anim: Invoke("RotateSprite", rotationTick);
	}

#if UNITY_EDITOR
	// Bug fix
	// Hides the SpriteRenderer, it's causing memory to build in Unity Editor.
	void Awake() {
		GetComponent<Image>().hideFlags = HideFlags.HideInInspector;
	}

	void OnApplicationQuit() {
		GetComponent<Image>().hideFlags = HideFlags.None;
	}
#endif
}