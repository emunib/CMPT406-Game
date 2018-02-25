using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemy : GenericPlayer {
  private const float explode_time = 2f;
  
  private ExplodingEnemyInput _input;
  private Coroutine fuse_coroutine;
  private Color original_color;
  private SpriteRenderer sprite_renderer;
  private int direction;
  private bool flip;
  private bool do_once;
  private ParticleSystem particles;
  private bool exploding;
  
  protected override void Start() {
    /* call this to run Awake in the subclass */
    base.Start();

    /* set the input for generic player, and its field radius */
    _input = GetComponent<ExplodingEnemyInput>();
    SetInput(_input);
    SetIgnoreFields(false);
    SetFieldRadius(12f);
    
    /* default values */
    sprite_renderer = GetComponent<SpriteRenderer>();
    original_color = sprite_renderer.color;
    fuse_coroutine = null;
    direction = 1;
    flip = false;
    do_once = true;
    particles = GetComponentInChildren<ParticleSystem>();
    exploding = false;
  }

  protected override void Update() {
    /* reset all the values */
    _input.DefaultValues();
    
    /* use find to see if theres players ahead or we need to flip */
    Find();
    
    /* move in the direction of platform */
    float platform_walk_angle = PlatformAngle() - 90;
    Vector2 movement_direction = new Vector2(Mathf.Sin(platform_walk_angle * Mathf.Deg2Rad), Mathf.Cos(platform_walk_angle * Mathf.Deg2Rad));
    
    /* use the left control stick to move in direction */
    _input.leftstickx = movement_direction.x * direction;
    _input.leftsticky = movement_direction.y * direction;
    
    /* only do it once */
    if (do_once) {
      /* change gravity in direction of the platform */
      float platform_angle = PlatformAngle() + 180;
      Vector2 gravity_direction = new Vector2(Mathf.Sin(platform_angle * Mathf.Deg2Rad), Mathf.Cos(platform_angle * Mathf.Deg2Rad));
      
      /* use the right stick to set the gravity facing the platform */
      _input.rightstickx = gravity_direction.x;
      _input.rightsticky = gravity_direction.y;
      
      do_once = false;
    }
    
    /* call this to run Update in the subclass */
    /* we call update after is because we want to change the input then call the update to handle the input changes
       in the same frame rather to have to wait another frame */
    base.Update();
  }

  private void Find() {
    /* which direction is our AI moving */
    Vector2 direction_fov = flip ? transform.right : -transform.right;
    /* shoot a ray out in that direction and see if he is hitting player or if anythig else than change direction */
    HashSet<RaycastHit2D> objects_in_view = GetObjectsInView(direction_fov, 1f, 0, 2.5f);
    foreach (RaycastHit2D hit in objects_in_view) {
      string tag_name = hit.transform.parent ? hit.transform.parent.tag : hit.transform.gameObject.tag;
      
      if (tag_name == "Player" && !exploding) {
        if(fuse_coroutine == null) fuse_coroutine = StartCoroutine(StartFuse(0.1f));
        exploding = true;
        Invoke("Explode", explode_time);
        break;
      }

      Flip();
      break;
    }

    /* as long as he is grounded make sure he does not fall off the platform edge and flip if he
       is about to */
    if (is_grounded) {
      float platform_angle = PlatformAngle();
      float angle1 = platform_angle - 100f;
      float angle2 = platform_angle + 100f;

      float angle = flip ? Mathf.Max(angle1, angle2) : Mathf.Min(angle1, angle2);

      Vector2 forwardangle_direction = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
      HashSet<RaycastHit2D> leaving_ground = GetObjectsInView(forwardangle_direction, 1f, 0, 5f);
      if (leaving_ground.Count <= 0) {
        Flip();
      }
    }
  }

  /// <summary>
  /// Grab the angle of the platform or ground he is on at the moment.
  /// </summary>
  /// <returns>Angle of his ground.</returns>
  private float PlatformAngle() {
    float platform_angle = 0f;
    /* get platform information */
    HashSet<RaycastHit2D> hits = GetObjectsInView(-transform.up, ground_fov_angle, ground_ray_count, ground_ray_length);
    foreach (RaycastHit2D hit in hits) {
      if (hit.transform.gameObject.layer != gameObject.layer) {
        /* calculate angle of the platform we are on */
        platform_angle = Mathf.Atan2(hit.normal.x, hit.normal.y) * Mathf.Rad2Deg;

        /* get angle between 0 - 360, even handle negative signs with modulus */
        platform_angle = fmod(platform_angle, 360);
        if (platform_angle < 0) platform_angle += 360;
        break;
      }
    }

    return platform_angle;
  }
  
  /// <summary>
  /// Flip the sprite and direction.
  /// </summary>
  private void Flip() {
    flip = !flip;
    sprite_renderer.flipX = flip;
    direction = direction * -1;
  }

  /// <summary>
  /// What to do when he exploding.
  /// </summary>
  private void Explode() {
    particles.Play();
    Invoke("StopExplosion", explode_time);
  }

  private void StopExplosion() {
    StopCoroutine(fuse_coroutine);
    fuse_coroutine = null;
    sprite_renderer.color = original_color;
    particles.Stop();
    particles.Clear();
    exploding = false;
  }
  
  /// <summary>
  /// Starts the fuse visual by having the sprite slowly blink to its
  /// inverted/red colour.
  /// </summary>
  /// <param name="rate">Rate at which to cause the flicker</param>
  private IEnumerator StartFuse(float rate) {
    bool alt = false;
    
    Color start = original_color;
    //Color end = InvertColor(start);
    
    /* if we wanted red colour from start */
    Color end = new Color(1f, start.g * 0.5f, start.b * 0.5f);
    const float duration = 0.5f;
    while (true) {
      for (float t = 0.0f; t < duration; t += Time.deltaTime) {
        Color changedColor = alt ? Color.Lerp(end, start, t / duration) : Color.Lerp(start, end, t / duration);
        sprite_renderer.color = changedColor;
        yield return null;
      }
      
      alt = !alt;
      yield return new WaitForSeconds(rate);
    }
  }
  
  /// <summary>
  /// Invert the given colour (negative of the given colour).
  /// </summary>
  private Color InvertColor(Color color) {
    return new Color(1.0f - color.r, 1.0f - color.g, 1.0f - color.b);
  }
}
