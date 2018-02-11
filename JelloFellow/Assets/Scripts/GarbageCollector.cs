using UnityEngine;

/// <summary>
/// Collected garbage every specified frame.
/// </summary>
public class GarbageCollector : MonoBehaviour {
	/* specified frame rate to call garbage collector */
	private const int CollectAtFrame = 60;
	private void Update () {
		if (Time.frameCount % CollectAtFrame == 0) {
			System.GC.Collect();
		}
	}
}
