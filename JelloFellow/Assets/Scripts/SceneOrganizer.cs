using UnityEngine;
using UnityEngine.UI;

public class SceneOrganizer : MonoBehaviour {
  private SceneInfo scene_info;
  private Image scene_image;
  private Image scene_background;
  private Text scene_text;

  private string scene_name;
  private string scene_name_updated;
  
  private Color scene_name_color;
  private Color scene_name_color_updated;

  private Color background_color;
  private Color background_color_updated;

  private Sprite scene_sprite;
  private Sprite scene_sprite_updated;
  
  private Color selected_color;  

  private void Start() {
    scene_background = GetComponent<Image>();
    scene_image = transform.Find("Image").gameObject.GetComponent<Image>();
    scene_text = transform.Find("Name").gameObject.GetComponent<Text>();

    scene_name = scene_text.text;
    scene_name_color = scene_text.color;
    background_color = scene_background.color;
    scene_sprite = scene_image.sprite;
  }
  
  /// <summary>
  /// Set the scene name text and its color. The scene name color will automatically always be
  /// alpha 1f.
  /// </summary>
  /// <param name="_name">The string representing the scene name</param>
  /// <param name="_name_color">Color for the scene name</param>
  public void SetTitle(string _name, Color _name_color) {
    _name_color.a = 1.0f;
    scene_name_updated = _name;
    scene_name_color_updated = _name_color;
  }
  
  /// <summary>
  /// Sets the background color.
  /// </summary>
  /// <param name="_background_color">Color of the background</param>
  public void SetBackgroundColor(Color _background_color) {
    background_color_updated = _background_color;
  }

  /// <summary>
  /// Sets the image of the scene representing the level.
  /// </summary>
  /// <param name="_scene_sprite">Image of the scene</param>
  public void SetSceneImage(Sprite _scene_sprite) {
    scene_sprite_updated = _scene_sprite;
  }
  
  /// <summary>
  /// Sets the selected color.
  /// </summary>
  /// <param name="_selected_color">Color of the selected</param>
  public void SetSelectedColor(Color _selected_color) {
    selected_color = _selected_color;
  }

  /// <summary>
  /// Link the scene info to this scene panel.
  /// </summary>
  /// <param name="_info">The info to be linked</param>
  public void SetSceneInfo(SceneInfo _info) {
    scene_info = _info;
  }

  /// <summary>
  /// Get the linked scene info;
  /// </summary>
  /// <returns>Scene info</returns>
  public SceneInfo GetSceneInfo() {
    return scene_info;
  }
  
  private void Update() {
    if (scene_name != scene_name_updated) {
      scene_text.text = scene_name = scene_name_updated;
    }

    if (scene_name_color != scene_name_color_updated) {
      scene_text.color = scene_name_color = scene_name_color_updated;
    }

    if (background_color != background_color_updated) {
      scene_background.color = background_color = background_color_updated;
    }

    if (scene_sprite != scene_sprite_updated) {
      scene_image.sprite = scene_sprite = scene_sprite_updated;
    }    
  }

  /// <summary>
  /// Call when the scene is selected.
  /// </summary>
  public void Select() {
    scene_background.color = selected_color;
  }

  /// <summary>
  /// Call when the scene is to be deselected.
  /// </summary>
  public void Deselect() {
    scene_background.color = background_color;
  }
}
