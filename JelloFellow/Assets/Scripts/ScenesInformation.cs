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
		
		SceneInfo Lift = new SceneInfo();
		Lift.name = "Lift";
		Lift.creator = "Cody B.";
		Lift.total_collectables = 3;
		Lift.gold_boundary = new GoldBoundary { boundary = 19f};
		Lift.silver_boundary = new SilverBoundary { boundary = 30f};
		Lift.bronze_boundary = new BronzeBoundary { boundary = 40f};
		Lift.category = Category.World2;
		Lift.category_index = 2;
		Lift.image_name = "Lift";
		SceneInfos.Add(Lift.name, Lift);
		
		SceneInfo LivingBreezy = new SceneInfo();
		LivingBreezy.name = "Living Breezy";
		LivingBreezy.creator = "Sarah V.";
		LivingBreezy.total_collectables = 3;
		LivingBreezy.gold_boundary = new GoldBoundary { boundary = 4.5f};
		LivingBreezy.silver_boundary = new SilverBoundary { boundary = 6f};
		LivingBreezy.bronze_boundary = new BronzeBoundary { boundary = 10f};
		LivingBreezy.category = Category.World2;
		LivingBreezy.category_index = 3;
		LivingBreezy.image_name = "Living Breezy";
		SceneInfos.Add(LivingBreezy.name, LivingBreezy);
		
		SceneInfo YeeHaw = new SceneInfo();
		YeeHaw.name = "Yee Haw";
		YeeHaw.creator = "Nick B.";
		YeeHaw.total_collectables = 3;
		YeeHaw.gold_boundary = new GoldBoundary { boundary = 17f};
		YeeHaw.silver_boundary = new SilverBoundary { boundary = 22f};
		YeeHaw.bronze_boundary = new BronzeBoundary { boundary = 35f};
		YeeHaw.category = Category.World2;
		YeeHaw.category_index = 4;
		YeeHaw.image_name = "Yee Haw";
		SceneInfos.Add(YeeHaw.name, YeeHaw);
		
		SceneInfo Squeeeeze = new SceneInfo();
		Squeeeeze.name = "Squeeeeze";
		Squeeeeze.creator = "Sarah V.";
		Squeeeeze.total_collectables = 3;
		Squeeeeze.gold_boundary = new GoldBoundary { boundary = 5f};
		Squeeeeze.silver_boundary = new SilverBoundary { boundary = 10f};
		Squeeeeze.bronze_boundary = new BronzeBoundary { boundary = 15f};
		Squeeeeze.category = Category.World3;
		Squeeeeze.category_index = 1;
		Squeeeeze.image_name = "Squeeeeze";
		SceneInfos.Add(Squeeeeze.name, Squeeeeze);
		
		SceneInfo Jump = new SceneInfo();
		Jump.name = "Jump";
		Jump.creator = "Cody B.";
		Jump.total_collectables = 3;
		Jump.gold_boundary = new GoldBoundary { boundary = 15f};
		Jump.silver_boundary = new SilverBoundary { boundary = 18f};
		Jump.bronze_boundary = new BronzeBoundary { boundary = 25f};
		Jump.category = Category.World3;
		Jump.category_index = 2;
		Jump.image_name = "Jump";
		SceneInfos.Add(Jump.name, Jump);
		
		SceneInfo Breakout = new SceneInfo();
		Breakout.name = "Breakout";
		Breakout.creator = "Parker N.";
		Breakout.total_collectables = 3;
		Breakout.gold_boundary = new GoldBoundary { boundary = 20f};
		Breakout.silver_boundary = new SilverBoundary { boundary = 25f};
		Breakout.bronze_boundary = new BronzeBoundary { boundary = 35f};
		Breakout.category = Category.World3;
		Breakout.category_index = 3;
		Breakout.image_name = "Breakout";
		SceneInfos.Add(Breakout.name, Breakout);
		
		SceneInfo DeathHallway = new SceneInfo();
		DeathHallway.name = "Death Hallway";
		DeathHallway.creator = "Cody B.";
		DeathHallway.total_collectables = 3;
		DeathHallway.gold_boundary = new GoldBoundary { boundary = 25f};
		DeathHallway.silver_boundary = new SilverBoundary { boundary = 32f};
		DeathHallway.bronze_boundary = new BronzeBoundary { boundary = 42f};
		DeathHallway.category = Category.World3;
		DeathHallway.category_index = 4;
		DeathHallway.image_name = "Death Hallway";
		SceneInfos.Add(DeathHallway.name, DeathHallway);
		
		
		
		
		
		
		
		
		
		
		
		
	}

	public SceneInfo[] GetAllInfo() {
		return SceneInfos == null ? null : SceneInfos.Values.ToArray();
	}
}