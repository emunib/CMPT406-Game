using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ScenesInformation {
	public readonly Dictionary<string, SceneInfo> SceneInfos;

	public ScenesInformation() {
		SceneInfos = new Dictionary<string, SceneInfo>();
		
		SceneInfo welcome = new SceneInfo();
		welcome.name = "Welcome";
		welcome.creator = "Sarah V.";
		welcome.total_collectables = 5;
		welcome.gold_boundary = new GoldBoundary { boundary = 5f};
		welcome.silver_boundary = new SilverBoundary { boundary = 7f};
		welcome.bronze_boundary = new BronzeBoundary { boundary = 15f};
		welcome.category = Category.World1;
		welcome.category_index = 1;
		welcome.image_name = "Welcome";
		SceneInfos.Add(welcome.name, welcome);
		
		SceneInfo easy_peezy = new SceneInfo();
		easy_peezy.name = "Easy Peezy";
		easy_peezy.creator = "Sarah V.";
		easy_peezy.total_collectables = 3;
		easy_peezy.gold_boundary = new GoldBoundary { boundary = 5f};
		easy_peezy.silver_boundary = new SilverBoundary { boundary = 7f};
		easy_peezy.bronze_boundary = new BronzeBoundary { boundary = 10f};
		easy_peezy.category = Category.World1;
		easy_peezy.category_index = 2;
		easy_peezy.image_name = "Easy Peezy";
		SceneInfos.Add(easy_peezy.name, easy_peezy);
		
		SceneInfo TheSpikeyWay = new SceneInfo();
		TheSpikeyWay.name = "The Spikey Way";
		TheSpikeyWay.creator = "Qummar G.";
		TheSpikeyWay.total_collectables = 3;
		TheSpikeyWay.gold_boundary = new GoldBoundary { boundary = 8f};
		TheSpikeyWay.silver_boundary = new SilverBoundary { boundary = 10f};
		TheSpikeyWay.bronze_boundary = new BronzeBoundary { boundary = 12f};
		TheSpikeyWay.category = Category.World1;
		TheSpikeyWay.category_index = 3;
		TheSpikeyWay.image_name = "The Spikey Way";
		SceneInfos.Add(TheSpikeyWay.name, TheSpikeyWay);
		
		SceneInfo CloseQuarters = new SceneInfo();
		CloseQuarters.name = "Close Quarters";
		CloseQuarters.creator = "Parker N.";
		CloseQuarters.total_collectables = 3;
		CloseQuarters.gold_boundary = new GoldBoundary { boundary = 20f};
		CloseQuarters.silver_boundary = new SilverBoundary { boundary = 47f};
		CloseQuarters.bronze_boundary = new BronzeBoundary { boundary = 68f};
		CloseQuarters.category = Category.World1;
		CloseQuarters.category_index = 4;
		CloseQuarters.image_name = "Close Quarters";
		SceneInfos.Add(CloseQuarters.name, CloseQuarters);
		
		SceneInfo Push = new SceneInfo();
		Push.name = "Push";
		Push.creator = "Sarah V.";
		Push.total_collectables = 3;
		Push.gold_boundary = new GoldBoundary { boundary = 4f};
		Push.silver_boundary = new SilverBoundary { boundary = 8f};
		Push.bronze_boundary = new BronzeBoundary { boundary = 17f};
		Push.category = Category.World2;
		Push.category_index = 1;
		Push.image_name = "Push";
		SceneInfos.Add(Push.name, Push);
		
		
		
		
		
		
		
	}

	public SceneInfo[] GetAllInfo() {
		return SceneInfos == null ? null : SceneInfos.Values.ToArray();
	}
}