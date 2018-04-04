/*

    Based code on snipet from https://gamedev.stackexchange.com/questions/112662/unity-autoscroll-when-selecting-buttons-out-of-viewport

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSelector : MonoBehaviour {
  private const string menuchoosesound_path = "Sounds/menu_choose";
  private const string menuselectsound_path = "Sounds/menu_select2";
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
  
  private AudioSource _audio_source;
  private AudioClip _choose_sound;
  private AudioClip _scroll_sound;
  private float annoyance_sign;
  
  public void Start() {
    highlight_color = new Color32(0xFF, 0xCA, 0x3A, 0xFF);
    
    InvokeRepeating("CheckForControllerInput", 0.0f, 0.14f);
    buttonsArray = GetComponentsInChildren<Button>();

    _audio_source = gameObject.AddComponent<AudioSource>();
    _audio_source.playOnAwake = false;
    
    _choose_sound = Resources.Load<AudioClip>(menuchoosesound_path);
    _scroll_sound = Resources.Load<AudioClip>(menuselectsound_path);
    
    if (textColor) {
      old_index = index;
      old_color = buttonsArray[index].GetComponentInChildren<Text>().color;
      buttonsArray[index].GetComponentInChildren<Text>().color = highlight_color;
      
      Rigidbody2D _rigidbody = buttonsArray[index].GetComponent<Rigidbody2D>();
      if(_rigidbody) _rigidbody.AddForce((Mathf.Sign(Random.Range(-1f, 1f)) == -1 ? Vector2.right : Vector2.left) * force, ForceMode2D.Impulse);
    }

    _audio_source.PlayOneShot(_scroll_sound, 1f);
    buttonsArray[index].Select();
  }

  public void Update() {
    input = InputController.instance.input;
    
    //If selected click the button
    if (input.GetButton3Down()) {
      _audio_source.PlayOneShot(_choose_sound, 1f);
      buttonsArray[index].onClick.Invoke();
    }
  }

  private void CheckForControllerInput() {
    input = InputController.instance.input;
    float ver_m = input.GetVerticalLeftStick();
    
    if (ver_m != 0f && ver_m != annoyance_sign) {
      _audio_source.PlayOneShot(_scroll_sound, 1f);
      annoyance_sign = Mathf.Sign(ver_m);
    }

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