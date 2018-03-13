using UnityEngine;

public class DeathEffect : MonoBehaviour {
    private JellySprite _jelly;
    private Vector2 _vel;
    [Range(0, 0.5f)]public float ShrinkTime = 0.2f;

    private void Start()
    {
        _jelly = GetComponent<JellySprite>();
    }

    private void LateUpdate () {
        var scale = transform.localScale;
        if (scale.x > 0.02f && scale.y > 0.02f)
        {
            foreach (var point in _jelly.ReferencePoints)
            {
                point.Body2D.velocity = Vector2.zero;
            }
        }
        else Destroy(gameObject);

        transform.localScale = Vector2.SmoothDamp(transform.localScale, Vector2.zero, ref _vel, ShrinkTime, Mathf.Infinity, Time.deltaTime);
    }
}
