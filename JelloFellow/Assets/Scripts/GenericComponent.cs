using UnityEngine;

public class GenericComponent : GravityComponent {


    private void Start() {
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();

        //Gravity player_gravity = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Gravity>();

        rb2d.velocity = Vector2.zero;
    }
}
