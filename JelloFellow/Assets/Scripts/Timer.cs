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
	public bool Activate;
	
	private void Awake () {
		if (transform.parent) {
			Canvas _canvas = transform.parent.GetComponent<Canvas>();
			if (_canvas) {
				_canvas.renderMode = RenderMode.ScreenSpaceCamera;
				_canvas.pixelPerfect = true;
				_canvas.worldCamera = Camera.main;
				_canvas.sortingOrder = 100;
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Activate) {
			timeToDisplay = Time.unscaledTime - startTime;
			timeText = timeToDisplay.ToString("0.00");
			timer.text = timeText;
		} else {
			startTime = Time.unscaledTime;
		}
	}

	public float getTime()
	{
		return timeToDisplay;
	}
}
