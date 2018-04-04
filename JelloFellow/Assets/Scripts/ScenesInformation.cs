using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ScenesInformation {
	public Dictionary<string, SceneInfo> SceneInfos;

	public ScenesInformation() {
		SceneInfos = new Dictionary<string, SceneInfo>();
		
		/* created test scene info (easy) */
		SceneInfo _test = new SceneInfo();
		_test.name = "Scene Easy";
		_test.creator = "Rutvik S.";
		_test.total_collectables = 3;
		_test.gold_boundary = new GoldBoundary { boundary = 20f};
		_test.silver_boundary = new SilverBoundary { boundary = 30f};
		_test.bronze_boundary = new BronzeBoundary { boundary = 40f};
		_test.category = Category.Easy;
		_test.category_index = 2;
		_test.image_name = "Name of Image";
		
		SceneInfos.Add(_test.name, _test);
		
		SceneInfo _test_2 = new SceneInfo();
		_test_2.name = "Scene Easy 2";
		_test_2.creator = "Rutvik S.";
		_test_2.total_collectables = 3;
		_test_2.gold_boundary = new GoldBoundary { boundary = 20f};
		_test_2.silver_boundary = new SilverBoundary { boundary = 30f};
		_test_2.bronze_boundary = new BronzeBoundary { boundary = 40f};
		_test_2.category = Category.Easy;
		_test_2.category_index = 1;
		_test_2.image_name = "Name of Image";
		
		SceneInfos.Add(_test_2.name, _test_2);
		
		SceneInfo _test_medium = new SceneInfo();
		_test_medium.name = "Scene Medium";
		_test_medium.creator = "Rutvik S.";
		_test_medium.total_collectables = 3;
		_test_medium.gold_boundary = new GoldBoundary { boundary = 20f};
		_test_medium.silver_boundary = new SilverBoundary { boundary = 30f};
		_test_medium.bronze_boundary = new BronzeBoundary { boundary = 40f};
		_test_medium.category = Category.Medium;
		_test_medium.category_index = 1;
		_test_medium.image_name = "Name of Image";
		
		SceneInfos.Add(_test_medium.name, _test_medium);
		
		SceneInfo _test_medium_2 = new SceneInfo();
		_test_medium_2.name = "Scene Medium 2";
		_test_medium_2.creator = "Rutvik S.";
		_test_medium_2.total_collectables = 3;
		_test_medium_2.gold_boundary = new GoldBoundary { boundary = 20f};
		_test_medium_2.silver_boundary = new SilverBoundary { boundary = 30f};
		_test_medium_2.bronze_boundary = new BronzeBoundary { boundary = 40f};
		_test_medium_2.category = Category.Medium;
		_test_medium_2.category_index = 2;
		_test_medium_2.image_name = "Name of Image";
		
		SceneInfos.Add(_test_medium_2.name, _test_medium_2);
		
		SceneInfo _test_medium_3 = new SceneInfo();
		_test_medium_3.name = "Scene Medium 3";
		_test_medium_3.creator = "Rutvik S.";
		_test_medium_3.total_collectables = 3;
		_test_medium_3.gold_boundary = new GoldBoundary { boundary = 20f};
		_test_medium_3.silver_boundary = new SilverBoundary { boundary = 30f};
		_test_medium_3.bronze_boundary = new BronzeBoundary { boundary = 40f};
		_test_medium_3.category = Category.Medium;
		_test_medium_3.category_index = 3;
		_test_medium_3.image_name = "Name of Image";
		
		SceneInfos.Add(_test_medium_3.name, _test_medium_3);
		
		SceneInfo _test_medium_4 = new SceneInfo();
		_test_medium_4.name = "Scene Medium 4";
		_test_medium_4.creator = "Rutvik S.";
		_test_medium_4.total_collectables = 3;
		_test_medium_4.gold_boundary = new GoldBoundary { boundary = 20f};
		_test_medium_4.silver_boundary = new SilverBoundary { boundary = 30f};
		_test_medium_4.bronze_boundary = new BronzeBoundary { boundary = 40f};
		_test_medium_4.category = Category.Medium;
		_test_medium_4.category_index = 4;
		_test_medium_4.image_name = "Name of Image";
		
		SceneInfos.Add(_test_medium_4.name, _test_medium_4);
		
		SceneInfo _test_hard = new SceneInfo();
		_test_hard.name = "Scene Hard";
		_test_hard.creator = "Rutvik S.";
		_test_hard.total_collectables = 3;
		_test_hard.gold_boundary = new GoldBoundary { boundary = 20f};
		_test_hard.silver_boundary = new SilverBoundary { boundary = 30f};
		_test_hard.bronze_boundary = new BronzeBoundary { boundary = 40f};
		_test_hard.category = Category.Hard;
		_test_hard.category_index = 1;
		_test_hard.image_name = "Name of Image";
		
		SceneInfos.Add(_test_hard.name, _test_hard);
	}

	public SceneInfo[] GetAllInfo() {
		return SceneInfos == null ? null : SceneInfos.Values.ToArray();
	}
}