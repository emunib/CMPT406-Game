/*

    Based code on snipet from https://gamedev.stackexchange.com/questions/112662/unity-autoscroll-when-selecting-buttons-out-of-viewport

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSelector : MonoBehaviour {
  private const float force = 2f;
  
  [SerializeField] private Button[] buttonsArray;
  [SerializeField] private bool textColor;
  private int index;
  private float yPos;
  private bool upInput;
  private bool downInput;
  private bool leftInput;
  private bool rightInput;
  private Input2D input;
  private Color old_color;
  private Color highlight_color;
  private int old_index;

  public void Start() {
    highlight_color = new Color32(0x00, 0x6D, 0x66, 0xFF);
    
    InvokeRepeating("CheckForControllerInput", 0.0f, 0.1f);
    input = InputController.instance.GetInput();
    buttonsArray = GetComponentsInChildren<Button>();

    if (textColor) {
      old_index = index;
      old_color = buttonsArray[index].GetComponentInChildren<Text>().color;
      buttonsArray[index].GetComponentInChildren<Text>().color = highlight_color;
      
      Rigidbody2D _rigidbody = buttonsArray[index].GetComponent<Rigidbody2D>();
      if(_rigidbody) _rigidbody.AddForce((Mathf.Sign(Random.Range(-1f, 1f)) == -1 ? Vector2.right : Vector2.left) * force, ForceMode2D.Impulse);
    }

    buttonsArray[index].Select();
  }

  public void Update() {    
    //If selected click the button
    if (input.GetButton3Down()) {
      buttonsArray[index].onClick.Invoke();
    }
  }

  private void CheckForControllerInput() {
    float ver_m = input.GetVerticalLeftStick();

    if (Mathf.Abs(ver_m) > 0) {
      //Move Up
      if (ver_m > 0) {
        if (index % 4 != 0) {
          index = Mathf.Clamp(index - 1, 0, buttonsArray.Length - 1);
        }
      }
      //Move Down
      else if (ver_m < 0) {
        if (index % 4 != 3) {
          index = Mathf.Clamp(index + 1, 0, buttonsArray.Length - 1);
        }
      }
      
      if (textColor) {
        buttonsArray[old_index].GetComponentInChildren<Text>().color = old_color;
        
        old_index = index;
        old_color = buttonsArray[index].GetComponentInChildren<Text>().color;
        buttonsArray[index].GetComponentInChildren<Text>().color = highlight_color;

        Rigidbody2D _rigidbody = buttonsArray[index].GetComponent<Rigidbody2D>();
        if(_rigidbody) _rigidbody.AddForce((Mathf.Sign(Random.Range(-1f, 1f)) == -1 ? Vector2.right : Vector2.left) * force, ForceMode2D.Impulse);
      }
      
      buttonsArray[index].Select();
    }

//    if (Mathf.Abs(hor_m) > 0 || Mathf.Abs(ver_m) > 0) {
//      //Move Up
//      if (ver_m > 0) {
//        if (index % 4 != 0) {
//          index = Mathf.Clamp(index - 1, 0, buttonsArray.Length - 1);
//        }
//      }
//      //Move Down
//      else if (ver_m < 0) {
//        if (index % 4 != 3) {
//          index = Mathf.Clamp(index + 1, 0, buttonsArray.Length - 1);
//        }
//      }
//      
//      //Move Up
//      if (ver_m > 0) {
//        if (index >= 4) {
//          index = Mathf.Clamp(index - 4, 0, buttonsArray.Length - 1);
//        }
//      }
//      //Move Down
//      else if (ver_m < 0) {
//        if (index <= buttonsArray.Length - 5) {
//          index = Mathf.Clamp(index + 4, 0, buttonsArray.Length - 1);
//        }
//      }
//
//      //Move Left
//      if (hor_m < 0) {
//        if (index % 4 != 0) {
//          index = Mathf.Clamp(index - 1, 0, buttonsArray.Length - 1);
//        }
//      }
//      // Move Right
//      else if (hor_m > 0) {
//        if (index % 4 != 3) {
//          index = Mathf.Clamp(index + 1, 0, buttonsArray.Length - 1);
//        }
//      }
//
//      buttonsArray[index].Select();
//    }
  }
}