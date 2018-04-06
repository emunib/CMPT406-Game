using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {
	private const string collectedsprite_path = "Sprites/collected";
	private const float amplitude = 1f;
	private const float speed = 0.8f; 
	
	[CustomLabel("Day of note")] [Tooltip("The day the note was created.")] [SerializeField]
	private int day;
  
	[CustomLabel("Description")] [Tooltip("Note from the scientist for the user to read.")] [SerializeField] [TextArea]
	private string description;

	private bool commited;
	private bool collected;	
	private Vector3 tempPos;	
	private float tempVal;
	
	private void Start() {
		commited = false;
		collected = false;
		tempPos = transform.position;
		tempVal = tempPos.y;
	}

	private void Update() {
		/* check if we have been already collected */
		if (CollectableItems.IsCollected(day) && !collected) {
			/* destroy collider (trigger), change sprite to collected sprite */
			Destroy(gameObject.GetComponent<BoxCollider2D>());
			SpriteRenderer _renderer = GetComponent<SpriteRenderer>();
			_renderer.sprite = Resources.Load<Sprite>(collectedsprite_path);
			collected = true;
		}

		if (!collected) {
			tempPos.y = tempVal +  amplitude * Mathf.Sin(speed * Time.time);
			transform.position = tempPos;
		}

	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player") && !commited) {
			CollectableItems.instance.CollectedItem(day, description);
			commited = true;
		}
	}
}
