using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Door : MonoBehaviour {

    public float speed = 3;
    public Transform posB;
    private Vector2 nextPos;
    private bool move = false;
    private void Start() {
        nextPos = posB.position;
    }

    private void Update() {
        if (move) {
            Move();
        }
    }


    private void Move() {
        transform.position = Vector2.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);

    }
    private void Open() {
        move = true;
    }
    
    
    
}
