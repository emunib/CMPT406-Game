using System;
using UnityEngine;

[Serializable]
public class SceneInfo : MonoBehaviour {
  [CustomLabel("Name of Scene")]
  public string _name;
  [CustomLabel("Creator of Scene")]
  public string _creator;
  [CustomLabel("Total Number of Collectables")]
  public int _total_collectables;
  [CustomLabel("Gold Score Boundary")]
  public GoldBoundary _gold_boundary;
  [CustomLabel("Silver Score Boundary")]
  public SilverBoundary _silver_boundary;
  [CustomLabel("Bronze Score Boundary")]
  public BronzeBoundary _bronze_boundary;
  [CustomLabel("Scene Category")]
  public Category _category;
  [CustomLabel("Index in the Category")]
  public int _category_index;
  [ReadOnly] public bool _locked = true;
  [ReadOnly] public int _collected_collectables;
  [ReadOnly] public float _previous_attempt_score;
  [ReadOnly] public float _highscore;
  [ReadOnly] public Medal _achieved_medal = Medal.None;
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
