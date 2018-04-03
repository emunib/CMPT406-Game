using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneSelector : MonoBehaviour {
  private const string category_path = "";
  private const string scene_path = "";
  
  private void Start() {
    ScenesInformation _scenes_information = MainScript.instance.GetScenesInformation();
    SceneInfo[] _scene_infos = _scenes_information.GetAllInfo();
    
    /* make sure we actually have scenes to organize */
    if (_scene_infos.Length > 0) {
      Array _categories = Enum.GetValues(typeof(Category));
      SortedDictionary<Category, SortedList<int, SceneInfo>> _scenes_categories = new SortedDictionary<Category, SortedList<int, SceneInfo>>();

      /* init each sorted list for each category */
      foreach (Category _category in _categories) {
        _scenes_categories.Add(_category, new SortedList<int, SceneInfo>());
      }

      /* set the scene in the categories for the set index */
      foreach (SceneInfo _info in _scene_infos) {
        SortedList<int, SceneInfo> value;
        _scenes_categories.TryGetValue(_info.category, out value);
        if (value != null) {
          value.Add(_info.category_index, _info);
        }
      }
      
      GameObject WorldsView = GameObject.Find("WorldsView");
      GameObject Categories = WorldsView.transform.Find("Categories").gameObject;
      
      // Scrollview
        // Categories
          // Category
           // Scenes
            // Scene
             // Image
             // Name
            // Title

      foreach (SortedList<int,SceneInfo> _infolist in _scenes_categories.Values) {
        foreach (KeyValuePair<int,SceneInfo> _infopair in _infolist) {
          print(_infopair.Value.category + ", " + _infopair.Value.category_index + ", " + _infopair.Value.name);
        }
      }
    }
  }
}