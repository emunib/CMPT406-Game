using System;
using UnityEngine;

[Serializable]
public class SceneInfo {
  [CustomLabel("Name of Scene")]
  public string name;
  [CustomLabel("Creator of Scene")]
  public string creator;
  [CustomLabel("Total Number of Collectables")]
  public int total_collectables;
  [CustomLabel("Gold Score Boundary")]
  public GoldBoundary gold_boundary;
  [CustomLabel("Silver Score Boundary")]
  public SilverBoundary silver_boundary;
  [CustomLabel("Bronze Score Boundary")]
  public BronzeBoundary bronze_boundary;
  [CustomLabel("Scene Category")]
  public Category category;
  [CustomLabel("Index in the Category")]
  public int category_index;
  [CustomLabel("Name of the scene image")]
  public string image_name;
  [ReadOnly] public bool locked = true;
  [ReadOnly] public int collected_collectables;
  [ReadOnly] public float previous_attempt_score;
  [ReadOnly] public float highscore;
  [ReadOnly] public Medal achieved_medal = Medal.None;
}

[Serializable]
public enum Category {
  Easy, Medium, Hard
}

[Serializable]
public class GoldBoundary {
  [ReadOnly] public Medal _medal = Medal.Gold;
  public float boundary;
}

[Serializable]
public class SilverBoundary {
  [ReadOnly] public Medal _medal = Medal.Silver;
  public float boundary;
}

[Serializable]
public class BronzeBoundary {
  [ReadOnly] public Medal _medal = Medal.Bronze; 
  public float boundary;
}

[Serializable]
public enum Medal {
  Gold = 0, Silver = 1, Bronze = 2, None = 3
}
