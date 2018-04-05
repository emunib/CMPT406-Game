using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ScenesInformation {
	public readonly Dictionary<string, SceneInfo> SceneInfos;
	public Dictionary<int, string> Collectables;

	public ScenesInformation() {
		Collectables = new Dictionary<int, string>();
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
		easy_peezy.total_collectables = 2;
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
		
		SceneInfo Ram = new SceneInfo();
		Ram.name = "Ram";
		Ram.creator = "Sarah V.";
		Ram.total_collectables = 3;
		Ram.gold_boundary = new GoldBoundary { boundary = 5};
		Ram.silver_boundary = new SilverBoundary { boundary = 8f};
		Ram.bronze_boundary = new BronzeBoundary { boundary = 10f};
		Ram.category = Category.World4;
		Ram.category_index = 1;
		Ram.image_name = "Ram";
		SceneInfos.Add(Ram.name, Ram);
		
		SceneInfo TinyTest = new SceneInfo();
		TinyTest.name = "Tiny Test";
		TinyTest.creator = "Parker N.";
		TinyTest.total_collectables = 3;
		TinyTest.gold_boundary = new GoldBoundary { boundary = 2.5f};
		TinyTest.silver_boundary = new SilverBoundary { boundary = 5f};
		TinyTest.bronze_boundary = new BronzeBoundary { boundary = 10f};
		TinyTest.category = Category.World4;
		TinyTest.category_index = 2;
		TinyTest.image_name = "Tiny Test";
		SceneInfos.Add(TinyTest.name, TinyTest);
		
		SceneInfo Ascension = new SceneInfo();
		Ascension.name = "Ascension";
		Ascension.creator = "Cody B.";
		Ascension.total_collectables = 3;
		Ascension.gold_boundary = new GoldBoundary { boundary = 2.5f};
		Ascension.silver_boundary = new SilverBoundary { boundary = 5f};
		Ascension.bronze_boundary = new BronzeBoundary { boundary = 10f};
		Ascension.category = Category.World4;
		Ascension.category_index = 3;
		Ascension.image_name = "Ascension";
		SceneInfos.Add(Ascension.name, Ascension);
		
		SceneInfo RitaHayworth = new SceneInfo();
		RitaHayworth.name = "Rita Hayworth";
		RitaHayworth.creator = "Austin P.";
		RitaHayworth.total_collectables = 3;
		RitaHayworth.gold_boundary = new GoldBoundary { boundary = 63f};
		RitaHayworth.silver_boundary = new SilverBoundary { boundary = 96f};
		RitaHayworth.bronze_boundary = new BronzeBoundary { boundary = 139f};
		RitaHayworth.category = Category.World4;
		RitaHayworth.category_index = 4;
		RitaHayworth.image_name = "Rita Hayworth";
		SceneInfos.Add(RitaHayworth.name, RitaHayworth);
		
		SceneInfo HeadsUp = new SceneInfo();
		HeadsUp.name = "Heads Up!";
		HeadsUp.creator = "Qummar G.";
		HeadsUp.total_collectables = 3;
		HeadsUp.gold_boundary = new GoldBoundary { boundary = 26f};
		HeadsUp.silver_boundary = new SilverBoundary { boundary = 43f};
		HeadsUp.bronze_boundary = new BronzeBoundary { boundary = 60f};
		HeadsUp.category = Category.World5;
		HeadsUp.category_index = 1;
		HeadsUp.image_name = "Heads Up!";
		SceneInfos.Add(HeadsUp.name, HeadsUp);
		
		SceneInfo DeathAlley = new SceneInfo();
		DeathAlley.name = "Death Alley";
		DeathAlley.creator = "Qummar G.";
		DeathAlley.total_collectables = 3;
		DeathAlley.gold_boundary = new GoldBoundary { boundary = 11f};
		DeathAlley.silver_boundary = new SilverBoundary { boundary = 13f};
		DeathAlley.bronze_boundary = new BronzeBoundary { boundary = 18f};
		DeathAlley.category = Category.World5;
		DeathAlley.category_index = 2;
		DeathAlley.image_name = "Death Alley";
		SceneInfos.Add(DeathAlley.name, DeathAlley);
		
		SceneInfo MyBiggestFan = new SceneInfo();
		MyBiggestFan.name = "My Biggest Fan";
		MyBiggestFan.creator = "Austin P.";
		MyBiggestFan.total_collectables = 3;
		MyBiggestFan.gold_boundary = new GoldBoundary { boundary = 25f};
		MyBiggestFan.silver_boundary = new SilverBoundary { boundary = 35f};
		MyBiggestFan.bronze_boundary = new BronzeBoundary { boundary = 45f};
		MyBiggestFan.category = Category.World5;
		MyBiggestFan.category_index = 3;
		MyBiggestFan.image_name = "My Biggest Fan";
		SceneInfos.Add(MyBiggestFan.name, MyBiggestFan);
		
		SceneInfo Endurance = new SceneInfo();
		Endurance.name = "Endurance";
		Endurance.creator = "Austin P.";
		Endurance.total_collectables = 3;
		Endurance.gold_boundary = new GoldBoundary { boundary = 11f};
		Endurance.silver_boundary = new SilverBoundary { boundary = 13f};
		Endurance.bronze_boundary = new BronzeBoundary { boundary = 18f};
		Endurance.category = Category.World5;
		Endurance.category_index = 4;
		Endurance.image_name = "Endurance";
		SceneInfos.Add(Endurance.name, Endurance);
		
		SceneInfo TheTrek = new SceneInfo();
		TheTrek.name = "The Trek";
		TheTrek.creator = "Parker N.";
		TheTrek.total_collectables = 3;
		TheTrek.gold_boundary = new GoldBoundary { boundary = 25f};
		TheTrek.silver_boundary = new SilverBoundary { boundary = 35f};
		TheTrek.bronze_boundary = new BronzeBoundary { boundary = 45f};
		TheTrek.category = Category.World6;
		TheTrek.category_index = 1;
		TheTrek.image_name = "The Trek";
		SceneInfos.Add(TheTrek.name, TheTrek);
		
		SceneInfo SlantMountain = new SceneInfo();
		SlantMountain.name = "Slant Mountain";
		SlantMountain.creator = "Parker N.";
		SlantMountain.total_collectables = 3;
		SlantMountain.gold_boundary = new GoldBoundary { boundary = 11f};
		SlantMountain.silver_boundary = new SilverBoundary { boundary = 21f};
		SlantMountain.bronze_boundary = new BronzeBoundary { boundary = 26f};
		SlantMountain.category = Category.World6;
		SlantMountain.category_index = 2;
		SlantMountain.image_name = "Slant Mountain";
		SceneInfos.Add(SlantMountain.name, SlantMountain);
		
		SceneInfo Dexterity = new SceneInfo();
		Dexterity.name = "Dexterity";
		Dexterity.creator = "Qummar G.";
		Dexterity.total_collectables = 3;
		Dexterity.gold_boundary = new GoldBoundary { boundary = 27f};
		Dexterity.silver_boundary = new SilverBoundary { boundary = 62f};
		Dexterity.bronze_boundary = new BronzeBoundary { boundary = 92f};
		Dexterity.category = Category.World6;
		Dexterity.category_index = 3;
		Dexterity.image_name = "Dexterity";
		SceneInfos.Add(Dexterity.name, Dexterity);
		
		SceneInfo Careful = new SceneInfo();
		Careful.name = "Careful";
		Careful.creator = "Qummar G.";
		Careful.total_collectables = 3;
		Careful.gold_boundary = new GoldBoundary { boundary = 18f};
		Careful.silver_boundary = new SilverBoundary { boundary = 29f};
		Careful.bronze_boundary = new BronzeBoundary { boundary = 42f};
		Careful.category = Category.World6;
		Careful.category_index = 4;
		Careful.image_name = "Careful";
		SceneInfos.Add(Careful.name, Careful);
		
		SceneInfo CakeWalk = new SceneInfo();
		CakeWalk.name = "Cake Walk";
		CakeWalk.creator = "Qummar G.";
		CakeWalk.total_collectables = 3;
		CakeWalk.gold_boundary = new GoldBoundary { boundary = 21f};
		CakeWalk.silver_boundary = new SilverBoundary { boundary = 25f};
		CakeWalk.bronze_boundary = new BronzeBoundary { boundary = 30f};
		CakeWalk.category = Category.World7;
		CakeWalk.category_index = 1;
		CakeWalk.image_name = "Cake Walk";
		SceneInfos.Add(CakeWalk.name, CakeWalk);
		
		SceneInfo FrustrationStation = new SceneInfo();
		FrustrationStation.name = "Frustration Station";
		FrustrationStation.creator = "Qummar G.";
		FrustrationStation.total_collectables = 3;
		FrustrationStation.gold_boundary = new GoldBoundary { boundary = 38f};
		FrustrationStation.silver_boundary = new SilverBoundary { boundary = 48f};
		FrustrationStation.bronze_boundary = new BronzeBoundary { boundary = 60f};
		FrustrationStation.category = Category.World7;
		FrustrationStation.category_index = 2;
		FrustrationStation.image_name = "Frustration Station";
		SceneInfos.Add(FrustrationStation.name, FrustrationStation);
		
		SceneInfo ChaosFactory = new SceneInfo();
		ChaosFactory.name = "Chaos Factory";
		ChaosFactory.creator = "Qummar G.";
		ChaosFactory.total_collectables = 3;
		ChaosFactory.gold_boundary = new GoldBoundary { boundary = 40f};
		ChaosFactory.silver_boundary = new SilverBoundary { boundary = 79f};
		ChaosFactory.bronze_boundary = new BronzeBoundary { boundary = 100f};
		ChaosFactory.category = Category.World7;
		ChaosFactory.category_index = 3;
		ChaosFactory.image_name = "Chaos Factory";
		SceneInfos.Add(ChaosFactory.name, ChaosFactory);
		
		SceneInfo QummarsCavern = new SceneInfo();
		QummarsCavern.name = "Qummar's Cavern";
		QummarsCavern.creator = "Qummar G.";
		QummarsCavern.total_collectables = 3;
		QummarsCavern.gold_boundary = new GoldBoundary { boundary = 75f};
		QummarsCavern.silver_boundary = new SilverBoundary { boundary = 118f};
		QummarsCavern.bronze_boundary = new BronzeBoundary { boundary = 180f};
		QummarsCavern.category = Category.World7;
		QummarsCavern.category_index = 4;
		QummarsCavern.image_name = "Qummar's Cavern";
		SceneInfos.Add(QummarsCavern.name, QummarsCavern);
		
		
		
		
		
		
		
		
		
		
		
		
		
	}

	public SceneInfo[] GetAllInfo() {
		return SceneInfos == null ? null : SceneInfos.Values.ToArray();
	}
}