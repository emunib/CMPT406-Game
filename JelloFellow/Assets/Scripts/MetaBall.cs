using System.Collections;
using UnityEngine;

public class MetaBall : MonoBehaviour {
  private SpriteRenderer sprite_renderer;

  private void Awake() {
    sprite_renderer = GetComponent<SpriteRenderer>();
    sprite_renderer.color = Color.clear;
    StartCoroutine(Fade(1f, 10f));
  }

  private IEnumerator Fade(float alpha, float duration) {
    for (float t = 0.0f; t < duration; t += Time.deltaTime) {
      Color new_color = Color.Lerp(sprite_renderer.color, new Color(1f, 1f, 0f, alpha), t / duration);
      sprite_renderer.color = new_color;
      yield return null;
    }
  }
}
