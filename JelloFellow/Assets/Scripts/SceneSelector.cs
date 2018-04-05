using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SceneSelector : MonoBehaviour {
  private const string menuchoosesound_path = "Sounds/menu_choose";
  private const string menuselectsound_path = "Sounds/menu_select2";
  private const string category_path = "Prefabs/UI/Category";
  private const string scene_path = "Prefabs/UI/Scene";
  private const string scenepreview_path = "LevelPreviews/";
  private const float scroll_speed = 6f;
  private const float scroll_smooth_time = 0.08f;
  
  private GameObject category_resource;
  private GameObject scene_resource;
  private Sprite test_image;
  private SortedDictionary<string, Sprite> preview_images;
  private SortedDictionary<Category, SortedList<int, SceneInfo>> sorted_sceneinfo;
  private LinkedList<SceneOrganizer> cursor_subject;
  private LinkedListNode<SceneOrganizer> cursor;
  private RectTransform scrolling_pane;
  private Vector3 vertical_position;
  private Input2D _input;
  private AudioSource _audio_source;
  private AudioClip _choose_sound;
  private AudioClip _scroll_sound;
  
  private void Start() {
    ScenesInformation _scenes_information = MainScript.instance.GetScenesInformation();
    SceneInfo[] _scene_infos = _scenes_information.GetAllInfo();
    
    _audio_source = gameObject.AddComponent<AudioSource>();
    _audio_source.playOnAwake = false;
    _choose_sound = Resources.Load<AudioClip>(menuchoosesound_path);
    _scroll_sound = Resources.Load<AudioClip>(menuselectsound_path);
    
    /* make sure we actually have scenes to organize */
    if (_scene_infos.Length > 0) {
      GameObject WorldsView = GameObject.Find("WorldsView");
      GameObject Categories = WorldsView.transform.Find("Categories").gameObject;

      scrolling_pane = Categories.GetComponent<RectTransform>();
      
      category_resource = Resources.Load<GameObject>(category_path);
      scene_resource = Resources.Load<GameObject>(scene_path);
      
      Sprite[] resources_preview_images = Resources.LoadAll<Sprite>(scenepreview_path);
      preview_images = new SortedDictionary<string, Sprite>();

      /* load all the preview images */
      foreach (Sprite _preview in resources_preview_images) {
        if (_preview.name == "Test") {
          test_image = _preview;
          continue;
        }
        
        preview_images.Add(_preview.name, _preview);
      }
      
      /* Category enum to array (sorted in way it was entered) */
      Array _categories = Enum.GetValues(typeof(Category));
      sorted_sceneinfo = new SortedDictionary<Category, SortedList<int, SceneInfo>>();
      
      /* loop max because 2 if statements is better than 2 loops */
      int loop_max = Mathf.Max(_categories.Length, _scene_infos.Length);
      for (int i = 0; i < loop_max; i++) {
        /* init each sorted list for each category and load the categories */
        if (i < _categories.Length) {
          /* get the category value for i and alloc sortedlist for its sorted_sceneinfo value */
          Category _category = (Category) _categories.GetValue(i);
          sorted_sceneinfo.Add(_category, new SortedList<int, SceneInfo>());
        }

        /* iterate through the scenes and save them to their respective categories (assuming the categories were already created) */
        /* the order to create sceneinfo will be restrictive due to this slight efficiencient method */
        if (i < _scene_infos.Length) {
          /* make sure we get value and store _info into the sortedlist for the category index (it is sorted by its category index) */
          SortedList<int, SceneInfo> value;
          SceneInfo _info = (SceneInfo) _scene_infos.GetValue(i);
          sorted_sceneinfo.TryGetValue(_info.category, out value);
          if (value != null) {            
            /* save info with category index being the key */
            value.Add(_info.category_index, _info);
          }
        }
      }

      Color text_color = new Color32(0x00, 0xBC, 0x66, 0xFF);
      Color background_color = new Color32(0x29, 0x2B, 0x2B, 0xFF);
      Color scene_backgroundcolor = new Color32(0x29, 0x2B, 0x2B, 0xFF);
      Color scene_selectedcolor = new Color32(0xFF, 0xCA, 0x3A, 0xFF);
      Color title_background_color = new Color32(0xFF, 0xFF, 0xFF, 0x0A);
      
      cursor_subject = new LinkedList<SceneOrganizer>();
      
      foreach (KeyValuePair<Category, SortedList<int, SceneInfo>> scenes_category in sorted_sceneinfo) {
        SortedList<int, SceneInfo> scenes_sorted_list = scenes_category.Value;
        if (scenes_sorted_list.Count > 0) {
          Category _category_enum = scenes_category.Key;
          /* instantiate the category */
          GameObject _category = Instantiate(category_resource, Categories.transform);
          GameObject _scenes = _category.transform.Find("Scenes").gameObject;
          
          /* update category settings */
          CategoryOrganizer category_org = _category.GetComponent<CategoryOrganizer>();
          category_org.SetTitle(VerticalText("W * " + (int)_category_enum), text_color);
          category_org.SetBackgroundColor(background_color);
          category_org.SetTitleBackgroundColor(title_background_color);

          foreach (KeyValuePair<int, SceneInfo> scenes_info in scenes_sorted_list) {
            SceneInfo _info = scenes_info.Value;

            /* instantiate the scene */
            GameObject _scene = Instantiate(scene_resource, _scenes.transform);

            /* update the scene settings */
            SceneOrganizer scene_org = _scene.GetComponent<SceneOrganizer>();
            scene_org.SetTitle(_info.name, text_color);
            if (preview_images.ContainsKey(_info.image_name)) {
              Sprite _preview;
              preview_images.TryGetValue(_info.image_name, out _preview);
              scene_org.SetSceneImage(_preview);
            } else {
              scene_org.SetSceneImage(test_image);
            }

            scene_org.SetBackgroundColor(scene_backgroundcolor);
            scene_org.SetSelectedColor(scene_selectedcolor);
            scene_org.SetSceneInfo(_info);
            /* add to the list of cursors */
            cursor_subject.AddLast(scene_org);
          }
        }
      }

      StartCoroutine(UpdateCursor());
    }
  }

  private IEnumerator UpdateCursor() {
    /* wait a frame for instantiat to complete so we can set the cursor */
    yield return null;
    
    /* set the first scene to be the cursor */
    cursor = cursor_subject.First;
    cursor.Value.Select();

    while (true) {
      _input = InputController.instance.input;

      float pos_y = (int) cursor.Value.GetSceneInfo().category * 450 - 450/2f;
      vertical_position = new Vector3(0f, scrolling_pane.sizeDelta.y / -2 + pos_y, 0f);
      
      float horizontal = _input.GetHorizontalLeftStick();
      float vertical = _input.GetVerticalLeftStick();
      
      /* go right (handle scenes in category) */
      if (horizontal == 1f) {
        if (cursor.Next != null) {
          if (cursor.Next.Value.GetSceneInfo().category == cursor.Value.GetSceneInfo().category) {
            cursor.Value.Deselect();
            cursor = cursor.Next;
            cursor.Value.Select();
            _audio_source.PlayOneShot(_scroll_sound, 1f);
          }
        }
      } else if (horizontal == -1f) { /* go left */
        if (cursor.Previous != null) {
          if (cursor.Previous.Value.GetSceneInfo().category == cursor.Value.GetSceneInfo().category) {
            cursor.Value.Deselect();
            cursor = cursor.Previous;
            cursor.Value.Select();
            _audio_source.PlayOneShot(_scroll_sound, 1f);
          }
        }
      }

      /* go up (handle categories themselves) */
      if (vertical == 1f) {
        LinkedListNode<SceneOrganizer> tmp = cursor;
        while (tmp != null) {
          if (tmp.Value.GetSceneInfo().category != cursor.Value.GetSceneInfo().category) {
            cursor.Value.Deselect();
            cursor = tmp;

            SortedList<int, SceneInfo> value;
            sorted_sceneinfo.TryGetValue(cursor.Value.GetSceneInfo().category, out value);
            if (value != null) {
              int x = value.Count - 1;
              int i = 0;
              LinkedListNode<SceneOrganizer> tmp1 = cursor;
              while (i != x && tmp1 != null) {
                tmp1 = tmp1.Previous;
                i++;
              }

              if(tmp1 != null) cursor = tmp1;
            }
            cursor.Value.Select();
            _audio_source.PlayOneShot(_scroll_sound, 1f);
            break;
          }
          tmp = tmp.Previous;
        }
      } else if (vertical == -1f) { /* go down */
        LinkedListNode<SceneOrganizer> tmp = cursor;
        while (tmp != null) {
          if (tmp.Value.GetSceneInfo().category != cursor.Value.GetSceneInfo().category) {
            cursor.Value.Deselect();
            cursor = tmp;
            cursor.Value.Select();
            _audio_source.PlayOneShot(_scroll_sound, 1f);
            break;
          }
          tmp = tmp.Next;
        }
      }
      
      yield return new WaitForSecondsRealtime(0.15f);
    }
  }

  private Vector3 scroll_velocity;
  private void Update() {
    _input = InputController.instance.input;
    
    /* if the button is pressed then load the scene */
    if (_input.GetButton3Down()) {
      _audio_source.PlayOneShot(_choose_sound, 1f);
      MainScript.instance.LoadSceneWithName(cursor.Value.GetSceneInfo().name);
    }

    /* go back to main menu */
    if (_input.GetButton2Down()) {
      _audio_source.PlayOneShot(_choose_sound, 1f);
      MainScript.instance.LoadSceneWithName("MainMenu");
    }
    
    /* smoothly scroll to the vertical position of the category */
    scrolling_pane.localPosition = Vector3.SmoothDamp(scrolling_pane.localPosition, vertical_position, ref scroll_velocity, scroll_smooth_time);
    //scrolling_pane.localPosition = Vector3.Lerp(scrolling_pane.localPosition, vertical_position, Time.deltaTime * scroll_speed);
  }


  /// <summary>
  /// Invert the given colour (negative of the given colour).
  /// </summary>
  private Color InvertColor(Color color) {
    return new Color(1.0f - color.r, 1.0f - color.g, 1.0f - color.b);
  }
  
  /// <summary>
  /// Newline after every character in string. This creates
  /// a string (GUI Text) that is vertical.
  /// </summary>
  /// <param name="__input"></param>
  /// <returns></returns>
  private string VerticalText(string __input) {
    StringBuilder vertical_text = new StringBuilder(__input.Length * 2);
    for (int i = 0; i < __input.Length; i++) {
      vertical_text.Append(__input[i]).Append("\n");
    }

    return vertical_text.ToString();
  }
}