using UnityEngine;
using UnityEngine.UI;

public class SceneOrganizer : MonoBehaviour {
  private Image scene_image;
  private Text scene_name;
  
  private void Init(Sprite _image, string _name) {
    scene_image = transform.Find("Image").gameObject.GetComponent<Image>();
    scene_image.sprite = _image;
    
    scene_name = transform.Find("Name").gameObject.GetComponent<Text>();
    scene_name.text = _name;
  }
}
