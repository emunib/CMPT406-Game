using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateMask : MonoBehaviour
{
	private SpriteRenderer _sr;
	private SpriteMask _sm;

	// Use this for initialization
	void Start ()
	{
		_sr = GetComponent<SpriteRenderer>();
		_sm = GetComponent<SpriteMask>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		_sm.sprite = _sr.sprite;
	}
}
