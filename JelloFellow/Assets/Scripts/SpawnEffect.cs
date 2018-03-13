using UnityEngine;

[RequireComponent(typeof(JellySprite))]
public class SpawnEffect : MonoBehaviour
{
	private JellySprite _jelly;
	private Vector2 _vel;
	[Range(0, 0.5f)]public float GrowTime = 0.3f;
	private bool _doOnce = true;

	private void Start()
	{
		_jelly = GetComponent<JellySprite>();
	}

	private void LateUpdate () {
		if (_doOnce)
		{
			transform.localScale = Vector2.zero;
			_doOnce = false;
		}

		var scale = transform.localScale;

		if (scale.x < 0.98f && scale.y < 0.98f)
		{
			foreach (var point in _jelly.ReferencePoints)
			{
				point.Body2D.velocity = Vector2.zero;
			}
		}
		else Destroy(this);
		
		transform.localScale = Vector2.SmoothDamp(transform.localScale, Vector2.one, ref _vel, GrowTime, Mathf.Infinity, Time.deltaTime);
	}
}
