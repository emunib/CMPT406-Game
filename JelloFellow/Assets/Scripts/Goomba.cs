using UnityEngine;

public class Goomba : GenericPlayer {
  private GoombaInput input;

  private void Start() {
    input = GetComponent<GoombaInput>();
    SetInput(input);
    SetIgnoreFields(false);
  }

  private void Update() {
    if (IsGround()) {
      
    }
    // if I want to move
    input.horizontal = 1f;
  }
}
