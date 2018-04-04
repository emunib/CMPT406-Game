using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ScenesInformation {
	public readonly Dictionary<string, SceneInfo> SceneInfos;

	public ScenesInformation() {
		SceneInfos = new Dictionary<string, SceneInfo>();
		
		SceneInfo easy_peezy = new SceneInfo();
		easy_peezy.name = "EasyPeezy";
		easy_peezy.creator = "Sarah V.";
		easy_peezy.total_collectables = 3;
		easy_peezy.gold_boundary = new GoldBoundary { boundary = 5f};
		easy_peezy.silver_boundary = new SilverBoundary { boundary = 7f};
		easy_peezy.bronze_boundary = new BronzeBoundary { boundary = 10f};
		easy_peezy.category = Category.Easy;
		easy_peezy.category_index = 1;
		easy_peezy.image_name = "EasyPeezy";
		SceneInfos.Add(easy_peezy.name, easy_peezy);
	}

	public SceneInfo[] GetAllInfo() {
		return SceneInfos == null ? null : SceneInfos.Values.ToArray();
	}
}