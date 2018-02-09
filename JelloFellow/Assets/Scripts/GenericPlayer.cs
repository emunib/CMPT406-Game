﻿using UnityEngine;

public class GenericPlayer : GravityField {
  private const float GravityForce = 9.8f;

  /// <summary>
  /// The input for this player.
  /// </summary>
  private Input2D input;

  private Vector2 new_gravity;
  private bool apply_constant_gravity;
  
  protected override void Awake() {
    base.Awake();

    /* get the default values */
    new_gravity = Vector2.zero;
    apply_constant_gravity = true;
  }

  private void Update() {
    /* make sure input is not null */
    if (input != null) {
      /* get the gravity vectors */
      float horizontal_gravity = input.GetHorizontalGravity();
      float vertical_gravity = input.GetVerticalGravity();

      /* make sure gravity is not 0 or dont change */
      if (horizontal_gravity != 0.0f || vertical_gravity != 0.0f) {
        if (apply_constant_gravity) {
          /* apply constant force by normalizing gravity vectors */
          /* this normalizes by using the Sign function as it just gives you {-1, 0, 1} */
          horizontal_gravity = Mathf.Sign(horizontal_gravity) * GravityForce;
          vertical_gravity = Mathf.Sign(vertical_gravity) * GravityForce;
        } else {
          /* apply gravity with variable force */
          horizontal_gravity *= GravityForce;
          vertical_gravity *= GravityForce;
        }

        new_gravity = new Vector2(horizontal_gravity, vertical_gravity);
      }
    } else {
      Debug.LogWarning("Input has not been assigned for this player (" + gameObject.name + ")");
    }
  }

  protected override void FixedUpdate() {
    base.FixedUpdate();
    
    /* don't apply gravity until it is changed (uses default until done so) */
    if(!new_gravity.Equals(Vector2.zero)) ApplyGravity(new_gravity);
  }

  /// <summary>
  /// Set the input for this player.
  /// Important if this were to be used by an AI where AI would be the
  /// "controller" which can replicate player moves if need to be.
  /// </summary>
  /// <param name="_input">Input of the player.</param>
  public void SetInput(Input2D _input) {
    input = _input;
  }

  private void ApplyGravity(Vector2 _gravity) {
    SetGravity(_gravity);
  }
}