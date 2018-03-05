using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	private float startTime;
	static public float timeToDisplay;
	private string timeText;

	public Text timer;
	
	// Use this for initialization
	void Awake ()
	{
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
	{
		timeToDisplay = Time.unscaledTime - startTime;
		timeText = timeToDisplay.ToString("0.00000");
		timer.text = timeText;
	}

	public float getTime()
	{
		return timeToDisplay;
	}
}
