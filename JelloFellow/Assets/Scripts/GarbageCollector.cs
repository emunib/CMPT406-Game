using UnityEngine;

/// <summary>
/// Collected garbage every specified frame.
/// </summary>
public class GarbageCollector : MonoBehaviour {
	/* specified frame rate to call garbage collector */
	private const int CollectAtFrame = 120;
	private const int AverageFrameRate = 120;

	private void Awake() {
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = AverageFrameRate;
	}

	private void Update () {
		if (Time.frameCount % CollectAtFrame == 0) {
			System.GC.Collect();
		}
	}
}
