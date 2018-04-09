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
		
		SceneInfo Welcome = new SceneInfo();
		Welcome.name = "Welcome";
		Welcome.creator = "Sarah V.";
		Welcome.total_collectables = 5;
		Welcome.gold_boundary = new GoldBoundary { boundary = 5f};
		Welcome.silver_boundary = new SilverBoundary { boundary = 7f};
		Welcome.bronze_boundary = new BronzeBoundary { boundary = 15f};
		Welcome.category = Category.World1;
		Welcome.category_index = 1;
		Welcome.image_name = "Welcome";
		SceneInfos.Add(Welcome.name, Welcome);
		
		SceneInfo EasyPeezy = new SceneInfo();
		EasyPeezy.name = "Easy Peezy";
		EasyPeezy.creator = "Sarah V.";
		EasyPeezy.total_collectables = 2;
		EasyPeezy.gold_boundary = new GoldBoundary { boundary = 5f};
		EasyPeezy.silver_boundary = new SilverBoundary { boundary = 7f};
		EasyPeezy.bronze_boundary = new BronzeBoundary { boundary = 10f};
		EasyPeezy.category = Category.World1;
		EasyPeezy.category_index = 2;
		EasyPeezy.image_name = "Easy Peezy";
		SceneInfos.Add(EasyPeezy.name, EasyPeezy);
		
		SceneInfo TheSpikeyWay = new SceneInfo();
		TheSpikeyWay.name = "The Spikey Way";
		TheSpikeyWay.creator = "Qammer G.";
		TheSpikeyWay.total_collectables = 1;
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
		CloseQuarters.total_collectables = 1;
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
		Push.total_collectables = 1;
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
		Lift.total_collectables = 2;
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
		LivingBreezy.total_collectables = 1;
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
		YeeHaw.total_collectables = 1;
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
		Squeeeeze.total_collectables = 1;
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
		Jump.total_collectables = 2;
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
		Breakout.total_collectables = 2;
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
		DeathHallway.total_collectables = 1;
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
		Ram.total_collectables = 1;
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
		TinyTest.total_collectables = 1;
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
		Ascension.total_collectables = 2;
		Ascension.gold_boundary = new GoldBoundary { boundary = 20f};
		Ascension.silver_boundary = new SilverBoundary { boundary = 30f};
		Ascension.bronze_boundary = new BronzeBoundary { boundary = 40f};
		Ascension.category = Category.World4;
		Ascension.category_index = 3;
		Ascension.image_name = "Ascension";
		SceneInfos.Add(Ascension.name, Ascension);
		
		SceneInfo RitaHayworth = new SceneInfo();
		RitaHayworth.name = "Rita Hayworth";
		RitaHayworth.creator = "Austin P.";
		RitaHayworth.total_collectables = 1;
		RitaHayworth.gold_boundary = new GoldBoundary { boundary = 63f};
		RitaHayworth.silver_boundary = new SilverBoundary { boundary = 96f};
		RitaHayworth.bronze_boundary = new BronzeBoundary { boundary = 139f};
		RitaHayworth.category = Category.World4;
		RitaHayworth.category_index = 4;
		RitaHayworth.image_name = "Rita Hayworth";
		SceneInfos.Add(RitaHayworth.name, RitaHayworth);
		
		SceneInfo HeadsUp = new SceneInfo();
		HeadsUp.name = "Heads Up!";
		HeadsUp.creator = "Qammer G.";
		HeadsUp.total_collectables = 2;
		HeadsUp.gold_boundary = new GoldBoundary { boundary = 26f};
		HeadsUp.silver_boundary = new SilverBoundary { boundary = 43f};
		HeadsUp.bronze_boundary = new BronzeBoundary { boundary = 60f};
		HeadsUp.category = Category.World5;
		HeadsUp.category_index = 1;
		HeadsUp.image_name = "Heads Up!";
		SceneInfos.Add(HeadsUp.name, HeadsUp);
		
		SceneInfo DeathAlley = new SceneInfo();
		DeathAlley.name = "Death Alley";
		DeathAlley.creator = "Qammer G.";
		DeathAlley.total_collectables = 1;
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
		MyBiggestFan.total_collectables = 1;
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
		Endurance.total_collectables = 2;
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
		TheTrek.total_collectables = 2;
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
		Dexterity.creator = "Qammer G.";
		Dexterity.total_collectables = 2;
		Dexterity.gold_boundary = new GoldBoundary { boundary = 27f};
		Dexterity.silver_boundary = new SilverBoundary { boundary = 62f};
		Dexterity.bronze_boundary = new BronzeBoundary { boundary = 92f};
		Dexterity.category = Category.World6;
		Dexterity.category_index = 3;
		Dexterity.image_name = "Dexterity";
		SceneInfos.Add(Dexterity.name, Dexterity);
		
		SceneInfo Careful = new SceneInfo();
		Careful.name = "Careful";
		Careful.creator = "Qammer G.";
		Careful.total_collectables = 1;
		Careful.gold_boundary = new GoldBoundary { boundary = 18f};
		Careful.silver_boundary = new SilverBoundary { boundary = 29f};
		Careful.bronze_boundary = new BronzeBoundary { boundary = 42f};
		Careful.category = Category.World6;
		Careful.category_index = 4;
		Careful.image_name = "Careful";
		SceneInfos.Add(Careful.name, Careful);
		
		SceneInfo CakeWalk = new SceneInfo();
		CakeWalk.name = "Cake Walk";
		CakeWalk.creator = "Qammer G.";
		CakeWalk.total_collectables = 2;
		CakeWalk.gold_boundary = new GoldBoundary { boundary = 21f};
		CakeWalk.silver_boundary = new SilverBoundary { boundary = 25f};
		CakeWalk.bronze_boundary = new BronzeBoundary { boundary = 30f};
		CakeWalk.category = Category.World7;
		CakeWalk.category_index = 1;
		CakeWalk.image_name = "Cake Walk";
		SceneInfos.Add(CakeWalk.name, CakeWalk);
		
		SceneInfo FrustrationStation = new SceneInfo();
		FrustrationStation.name = "Frustration Station";
		FrustrationStation.creator = "Qammer G.";
		FrustrationStation.total_collectables = 2;
		FrustrationStation.gold_boundary = new GoldBoundary { boundary = 38f};
		FrustrationStation.silver_boundary = new SilverBoundary { boundary = 48f};
		FrustrationStation.bronze_boundary = new BronzeBoundary { boundary = 60f};
		FrustrationStation.category = Category.World7;
		FrustrationStation.category_index = 2;
		FrustrationStation.image_name = "Frustration Station";
		SceneInfos.Add(FrustrationStation.name, FrustrationStation);
		
		SceneInfo ChaosFactory = new SceneInfo();
		ChaosFactory.name = "Chaos Factory";
		ChaosFactory.creator = "Qammer G.";
		ChaosFactory.total_collectables = 2;
		ChaosFactory.gold_boundary = new GoldBoundary { boundary = 40f};
		ChaosFactory.silver_boundary = new SilverBoundary { boundary = 79f};
		ChaosFactory.bronze_boundary = new BronzeBoundary { boundary = 100f};
		ChaosFactory.category = Category.World7;
		ChaosFactory.category_index = 3;
		ChaosFactory.image_name = "Chaos Factory";
		SceneInfos.Add(ChaosFactory.name, ChaosFactory);
		
		SceneInfo QammersCavern = new SceneInfo();
		QammersCavern.name = "Qammer's Cavern";
		QammersCavern.creator = "Qammer G.";
		QammersCavern.total_collectables = 1;
		QammersCavern.gold_boundary = new GoldBoundary { boundary = 75f};
		QammersCavern.silver_boundary = new SilverBoundary { boundary = 118f};
		QammersCavern.bronze_boundary = new BronzeBoundary { boundary = 180f};
		QammersCavern.category = Category.World7;
		QammersCavern.category_index = 4;
		QammersCavern.image_name = "Qammer's Cavern";
		SceneInfos.Add(QammersCavern.name, QammersCavern);
		
		SceneInfo Chute = new SceneInfo();
		Chute.name = "Chute";
		Chute.creator = "Qammer G.";
		Chute.total_collectables = 3;
		Chute.gold_boundary = new GoldBoundary { boundary = 1000f};
		Chute.silver_boundary = new SilverBoundary { boundary = 1001f};
		Chute.bronze_boundary = new BronzeBoundary { boundary = 1002f};
		Chute.category = Category.Bonus;
		Chute.category_index = 1;
		Chute.image_name = "Chute";
		SceneInfos.Add(Chute.name, Chute);
		
		/*SceneInfo GoodLuck = new SceneInfo();
		GoodLuck.name = "GoodLuck";
		GoodLuck.creator = "Qammer G.";
		GoodLuck.total_collectables = 5;
		GoodLuck.gold_boundary = new GoldBoundary { boundary = 1000f};
		GoodLuck.silver_boundary = new SilverBoundary { boundary = 1001f};
		GoodLuck.bronze_boundary = new BronzeBoundary { boundary = 1002f};
		GoodLuck.category = Category.Bonus;
		GoodLuck.category_index = 2;
		GoodLuck.image_name = "GoodLuck";
		SceneInfos.Add(GoodLuck.name, GoodLuck);*/
		
		SceneInfo Hellscape = new SceneInfo();
		Hellscape.name = "Hellscape";
		Hellscape.creator = "Qammer G.";
		Hellscape.total_collectables = 2;
		Hellscape.gold_boundary = new GoldBoundary { boundary = 1000f};
		Hellscape.silver_boundary = new SilverBoundary { boundary = 1001f};
		Hellscape.bronze_boundary = new BronzeBoundary { boundary = 1002f};
		Hellscape.category = Category.Bonus;
		Hellscape.category_index = 2;
		Hellscape.image_name = "Hellscape";
		SceneInfos.Add(Hellscape.name, Hellscape);
		
		SceneInfo Medium = new SceneInfo();
		Medium.name = "Medium";
		Medium.creator = "Qammer G.";
		Medium.total_collectables = 3;
		Medium.gold_boundary = new GoldBoundary { boundary = 1000f};
		Medium.silver_boundary = new SilverBoundary { boundary = 1001f};
		Medium.bronze_boundary = new BronzeBoundary { boundary = 1002f};
		Medium.category = Category.Bonus;
		Medium.category_index = 3;
		Medium.image_name = "Medium";
		SceneInfos.Add(Medium.name, Medium);
		
		SceneInfo MoshPit = new SceneInfo();
		MoshPit.name = "Mosh Pit";
		MoshPit.creator = "Qammer G.";
		MoshPit.total_collectables = 2;
		MoshPit.gold_boundary = new GoldBoundary { boundary = 1000f};
		MoshPit.silver_boundary = new SilverBoundary { boundary = 1001f};
		MoshPit.bronze_boundary = new BronzeBoundary { boundary = 1002f};
		MoshPit.category = Category.Bonus;
		MoshPit.category_index = 4;
		MoshPit.image_name = "Mosh Pit";
		SceneInfos.Add(MoshPit.name, MoshPit);
		
		SceneInfo Newton = new SceneInfo();
		Newton.name = "Newton";
		Newton.creator = "Qammer G.";
		Newton.total_collectables = 1;
		Newton.gold_boundary = new GoldBoundary { boundary = 1000f};
		Newton.silver_boundary = new SilverBoundary { boundary = 1001f};
		Newton.bronze_boundary = new BronzeBoundary { boundary = 1002f};
		Newton.category = Category.Bonus;
		Newton.category_index = 5;
		Newton.image_name = "Newton";
		SceneInfos.Add(Newton.name, Newton);
		
		SceneInfo SawLand = new SceneInfo();
		SawLand.name = "SawLand";
		SawLand.creator = "Qammer G.";
		SawLand.total_collectables = 2;
		SawLand.gold_boundary = new GoldBoundary { boundary = 1000f};
		SawLand.silver_boundary = new SilverBoundary { boundary = 1001f};
		SawLand.bronze_boundary = new BronzeBoundary { boundary = 1002f};
		SawLand.category = Category.Bonus;
		SawLand.category_index = 6;
		SawLand.image_name = "SawLand";
		SceneInfos.Add(SawLand.name, SawLand);
		
		SceneInfo TheVacuum = new SceneInfo();
		TheVacuum.name = "The Vacuum";
		TheVacuum.creator = "Qammer G.";
		TheVacuum.total_collectables = 1;
		TheVacuum.gold_boundary = new GoldBoundary { boundary = 1000f};
		TheVacuum.silver_boundary = new SilverBoundary { boundary = 1001f};
		TheVacuum.bronze_boundary = new BronzeBoundary { boundary = 1002f};
		TheVacuum.category = Category.Bonus;
		TheVacuum.category_index = 7;
		TheVacuum.image_name = "The Vacuum";
		SceneInfos.Add(TheVacuum.name, TheVacuum);
	}

	public SceneInfo[] GetAllInfo() {
		return SceneInfos == null ? null : SceneInfos.Values.ToArray();
	}
}