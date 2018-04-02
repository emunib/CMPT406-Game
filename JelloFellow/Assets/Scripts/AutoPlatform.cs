using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//Nick added this line
[SelectionBase]

[ExecuteInEditMode]
public class AutoPlatform : MonoBehaviour {
  private const string platform_sortinglayer = "Platform";
  
  [Header("Platform Sprites")]
  [CustomLabel("Left Platform Sprite")] [Tooltip("Left cap of the platform.")] [SerializeField]
  private Sprite left_sprite;

  [CustomLabel("Middle Platform Sprite")] [Tooltip("Middle cap of the platform.")] [SerializeField]
  private Sprite mid_sprite;

  [CustomLabel("Right Platform Sprite")] [Tooltip("Right cap of the platform.")] [SerializeField]
  private Sprite right_sprite;

  [Header("Settings")]
  [CustomLabel("Platform Width")] [Tooltip("Width of the platform.")] [SerializeField]
  protected float platform_width;
  [CustomLabel("Use PolygonCollider2D")] [Tooltip("Use PolygonCollider2D for sprites.")] [SerializeField]
  private bool use_polygon;
  [CustomLabel("Order in Layer")] [Tooltip("Use PolygonCollider2D for sprites.")] [SerializeField]
  private int order_in_layer = 0;
  [CustomLabel("Color")] [Tooltip("Color of the platform.")] [SerializeField]
  private Color color = Color.white;

  [Header("Editing Settings")]
  [CustomLabel("Extend from left")] [Tooltip("Extend the platform from the left side.")] [SerializeField]
  protected bool extend_left;
  [CustomLabel("Extend from right")] [Tooltip("Extend the platform from the right side.")] [SerializeField]
  protected bool extend_right;
  [CustomLabel("Extend from center")] [Tooltip("Extend the platform from the center side.")] [SerializeField]
  protected bool extend_center;
  
  protected float platform_width_old;
  protected bool extend_left_old;
  protected bool extend_right_old;
  protected bool extend_center_old;

  protected void Start() {
    extend_center = extend_center_old = true;
    extend_left = extend_left_old = false;
    extend_right = extend_right_old = false;
    
    gameObject.layer = LayerMask.NameToLayer("Ground");
  }

  protected virtual void Update() {
    /* if all are false change center to true */
    if (!extend_center && !extend_left && !extend_right) extend_center = true;
    
    /* only have one boolean be true */
    if (extend_left_old != extend_left) {
      extend_center = extend_center_old = false;
      extend_right = extend_right_old = false;
      
      extend_left_old = extend_left;
    }
    
    if (extend_right_old != extend_right) {
      extend_center = extend_center_old = false;
      extend_left = extend_left_old = false;
      
      extend_right_old = extend_right;
    }
    
    if (extend_center_old != extend_center) {
      extend_left = extend_left_old = false;
      extend_right = extend_right_old = false;
      
      extend_center_old = extend_center;
    }
    
    /* platform width changed so update it */
    if (platform_width_old != platform_width) {
      List<Transform> tempList = transform.Cast<Transform>().ToList();
      /* remove all gameobjects in parent */
      foreach (Transform child in tempList) {
        DestroyImmediate(child.gameObject);
      }
      
      /* only create the platform when all the childs are destroyed */
      CreatePlatform();
      platform_width_old = platform_width;
    }
  }

   protected virtual void CreatePlatform() {
    /* make sure all sprites are set or do not create a platform */
    if (left_sprite && mid_sprite && right_sprite) {
      /* find out the minimum width of the platform */
      float left_width = left_sprite.bounds.size.x;
      float mid_width = mid_sprite.bounds.size.x;
      float right_width = right_sprite.bounds.size.x;
      float min_platform_width = left_width + mid_width + right_width;

      /* set minimum platform width if we go lower then that */
      if (platform_width < min_platform_width) {
        platform_width = min_platform_width;
      }

      /* sizes of each sprite */
      Vector2 left_size = left_sprite.bounds.size;
      Vector2 mid_size = new Vector2(platform_width - left_width - right_width, mid_sprite.bounds.size.y);
      Vector2 right_size = right_sprite.bounds.size;
      
      /* create gameobjects from sprite whilst considering the width */
      GameObject left_tile = CreateGameObjectFromSprite(left_sprite, "Left", left_size);
      GameObject mid_tile = CreateGameObjectFromSprite(mid_sprite, "Middle", mid_size, true);
      GameObject right_tile = CreateGameObjectFromSprite(right_sprite, "Right", right_size);
      left_tile.transform.parent = transform;
      mid_tile.transform.parent = transform;
      right_tile.transform.parent = transform;

      left_tile.layer = LayerMask.NameToLayer("Ground");
      mid_tile.layer = LayerMask.NameToLayer("Ground");
      right_tile.layer = LayerMask.NameToLayer("Ground");
      
      /* position the tiles */
      left_tile.transform.localPosition = new Vector2(-(platform_width - left_width)/2f, 0f);
      mid_tile.transform.localPosition = Vector2.zero;
      right_tile.transform.localPosition = new Vector2((platform_width - right_width)/2f, 0f);

      /* update the localrotation so it matches the parent */
      left_tile.transform.localRotation = Quaternion.identity;
      mid_tile.transform.localRotation = Quaternion.identity;
      right_tile.transform.localRotation = Quaternion.identity;

      if (!extend_center) {
        int direction = extend_left ? 1 : -1;
        transform.position += transform.right * (platform_width_old - platform_width)/2f * direction;
      }
      
      /* add a box collider or polygon collider */
      if (use_polygon) {
        /* add collider */
        PolygonCollider2D left_collider = left_tile.AddComponent<PolygonCollider2D>();
        PolygonCollider2D mid_collider = mid_tile.AddComponent<PolygonCollider2D>();
        PolygonCollider2D right_collider = right_tile.AddComponent<PolygonCollider2D>();

        /* auto tile the collider if size increases */
        left_collider.autoTiling = true;
        mid_collider.autoTiling = true;
        right_collider.autoTiling = true;
        
        /* offset to 0 so its smooth transition when changing in editor */
        left_collider.offset = Vector2.zero;
        mid_collider.offset = Vector2.zero;
        right_collider.offset = Vector2.zero;
      } else {
        /* add collider */
        BoxCollider2D left_collider = left_tile.AddComponent<BoxCollider2D>();
        BoxCollider2D mid_collider = mid_tile.AddComponent<BoxCollider2D>();
        BoxCollider2D right_collider = right_tile.AddComponent<BoxCollider2D>();

        /* auto tile the collider if size increases */
        left_collider.autoTiling = true;
        mid_collider.autoTiling = true;
        right_collider.autoTiling = true;
        
        /* offset to 0 so its smooth transition when changing in editor */
        left_collider.offset = Vector2.zero;
        mid_collider.offset = Vector2.zero;
        right_collider.offset = Vector2.zero;
      }
    } else {
      /* if any of the sprites have not been set, warn them */
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
  }

  protected GameObject CreateGameObjectFromSprite(Sprite _sprite, string _name, Vector2 _size, bool _tiled = false) {
    GameObject _obj = new GameObject(_name);
    SpriteRenderer _renderer = _obj.AddComponent<SpriteRenderer>();
    _renderer.sprite = _sprite;
    _renderer.color = color;
    if (_tiled) {
      _renderer.drawMode = SpriteDrawMode.Tiled;
      _renderer.tileMode = SpriteTileMode.Continuous;
      _renderer.size = _size;
    }

    _renderer.sortingLayerName = platform_sortinglayer;
    _renderer.sortingOrder = order_in_layer;
    return _obj;
  }
}