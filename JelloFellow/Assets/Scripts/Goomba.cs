using UnityEngine;

public class Goomba : GenericPlayer {
  private GoombaInput input;

  private void Start() {
    input = GetComponent<GoombaInput>();
    SetInput(input);
    SetIgnoreFields(false);
  }

  protected override void Update() {
    base.Update();
    
    // if I want to move
    input.horizontal = 1f;
  }
}
