using UnityEngine;

public class GenericComponent : GravityComponent {


    private void Start() {
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();

        rb2d.velocity = Vector2.zero;
    }
}
