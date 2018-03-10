using UnityEngine;

[ExecuteInEditMode]
public class AutoPlatform : MonoBehaviour {
  [Header("Platform Sprites")]
  [CustomLabel("Left Platform Sprite")] [Tooltip("Left cap of the platform.")] [SerializeField]
  private Sprite left_sprite;
  
  [CustomLabel("Middle Platform Sprite")] [Tooltip("Middle cap of the platform.")] [SerializeField]
  private Sprite mid_sprite;
  
  [CustomLabel("Right Platform Sprite")] [Tooltip("Right cap of the platform.")] [SerializeField]
  private Sprite right_sprite;
  
  [Header("Settings")]
  [CustomLabel("Platform Width")] [Tooltip("Width of the platform.")] [SerializeField]
  private float platform_width;

  private void Start() {
    if (!left_sprite) {
      Debug.LogError("Left sprite of the platform not set.");
    }
    
    if (!mid_sprite) {
      Debug.LogError("Middle sprite of the platform not set.");
    }
    
    if (!right_sprite) {
      Debug.LogError("Right sprite of the platform not set.");
    }
  }

  private void Update() {
    if (left_sprite && mid_sprite && right_sprite) {
      float left_width = left_sprite.bounds.size.x;
      float mid_width = mid_sprite.bounds.size.x;
      float right_width = right_sprite.bounds.size.x;
      float max_platform_width = left_width + mid_width + right_width;
      
      Debug.Log("Max Platform size: " + max_platform_width);

      if (platform_width < max_platform_width) {
        platform_width = max_platform_width;
      }
    }
  }

  private GameObject CreateGameObjectFromSprite(Sprite _sprite, string _name, Vector2 _size, bool _tiled = false) {
    GameObject _obj = new GameObject(_name);
    SpriteRenderer _renderer = _obj.AddComponent<SpriteRenderer>();
    _renderer.sprite = _sprite;
    _renderer.drawMode = SpriteDrawMode.Tiled;
    _renderer.tileMode = SpriteTileMode.Continuous;
    _renderer.size = _size;

    return _obj;
  }
}
