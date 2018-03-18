using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmooshTrigger : MonoBehaviour {

  private int AmountJellyColliders;
  private int numColliding;

  private void Start() {
    numColliding = 0;
    AmountJellyColliders = GetComponentInParent<Smoosher>().AmountJellyColliders;

  }

  private void OnTriggerEnter2D(Collider2D other) {
    //if (other.gameObject.layer == LayerMask.NameToLayer("Player")&&other.gameObject.name!="Field") {
    if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
      numColliding++;
      if (numColliding >AmountJellyColliders) {
        SendMessageUpwards("PlayerEnteredTrigger",other.gameObject.transform.parent.GetComponentInChildren<GenericPlayer>());
      }
    }

    
    
  }


  private void OnTriggerExit2D(Collider2D other) {
    
    
    if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
      numColliding--;
    }

    if (numColliding <= AmountJellyColliders) {
      SendMessageUpwards("PlayerLeftTrigger");  

    }
  }
}
