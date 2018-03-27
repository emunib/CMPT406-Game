/*

    Based code on snipet from https://gamedev.stackexchange.com/questions/112662/unity-autoscroll-when-selecting-buttons-out-of-viewport

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class SceneSelectorScene : MonoBehaviour {
  private const string scenebutton_path = "Prefabs/UI/SceneButton";
  private const string scenepreview_path = "LevelPreviews/";
  private const string scenepreviewtest_path = "LevelPreviews/Test";
  private const int rows_fixed = 4;
  
  private float scrollSpeed;
  private ScrollRect scrollRect;
  private Button[] buttonsArray;
  private int index;
  private float yPos;
  private bool upInput;
  private bool downInput;
  private bool leftInput;
  private bool rightInput;
  private Input2D input;
  private int row;
  private int ammountOfRows;

  [SerializeField] public string[] scenes;
  private Object scenebutton;
  private Color old_color;
  private Color highlight_color;
  private int old_index;

  private void Start() {
    highlight_color = new Color32(0xFF, 0xCA, 0x3A, 0xFF);
    scenebutton = Resources.Load(scenebutton_path);

    for (int i = 0; i < scenes.Length; i++) {
      GameObject button = Instantiate(scenebutton) as GameObject;
      if (button != null) {
        button.transform.SetParent(transform.Find("Panel"), false);
        button.GetComponentInChildren<SceneButton>().linked_scene_name = scenes[i];

        string _name = scenes[i];
        if (name == "MainMenu") {
          _name = "Back";
        }

        button.GetComponentInChildren<Text>().text = _name;
        button.GetComponentInChildren<Text>().color = new Color32(0x00, 0xBC, 0x66, 0xFF);

        Sprite sprite = Resources.Load<Sprite>(scenepreview_path + scenes[i]);
        button.GetComponent<Image>().sprite = sprite != null ? sprite : Resources.Load<Sprite>(scenepreviewtest_path);
      } else {
        Debug.LogError("Button is null.");
      }
    }

    InvokeRepeating("CheckForControllerInput", 0.0f, 0.14f);
    input = InputController.instance.GetInput();
    scrollRect = GetComponent<ScrollRect>();
    buttonsArray = GetComponentsInChildren<Button>();

    old_index = index;
    old_color = buttonsArray[index].GetComponentInChildren<Text>().color;
    buttonsArray[index].GetComponentInChildren<Text>().color = highlight_color;
    buttonsArray[index].Select();

    row = 0;
    scrollSpeed = 6f;
    //Checks to see how many rows of scenes there are
    if (buttonsArray.Length % rows_fixed == 0) {
      ammountOfRows = buttonsArray.Length / rows_fixed;
    } else {
      ammountOfRows = (buttonsArray.Length / rows_fixed) + 1;
    }

    yPos = 1f - (float) index / (buttonsArray.Length - 1);
  }

  private void Update() {    
    //If selected click the button
    if (input.GetButton3Down()) {
      buttonsArray[index].onClick.Invoke();
    }
    
    //Scroll the screen to the proper hight
    scrollRect.verticalNormalizedPosition = Mathf.Lerp(scrollRect.verticalNormalizedPosition, yPos + (-Mathf.Sign(yPos) * 0.2f), Time.deltaTime * scrollSpeed);
  }

  private void CheckForControllerInput() {
    float hor_m = input.GetHorizontalLeftStick();
    float ver_m = input.GetVerticalLeftStick();

    if (Mathf.Abs(hor_m) > 0 || Mathf.Abs(ver_m) > 0) {
      //Move Up
      if (ver_m > 0) {
        if (index >= rows_fixed) {
          index = Mathf.Clamp(index - rows_fixed, 0, buttonsArray.Length - 1);
        }

        if (row > 0) {
          row--;
        }
      }
      //Move Down
      else if (ver_m < 0) {
        if (index <= buttonsArray.Length - (rows_fixed + 1)) {
          index = Mathf.Clamp(index + rows_fixed, 0, buttonsArray.Length - 1);
        }

        if (row < ammountOfRows - 1) {
          row++;
        }
      }

      //Move Left
      if (hor_m < 0) {
        if (index % rows_fixed != 0) {
          index = Mathf.Clamp(index - 1, 0, buttonsArray.Length - 1);
        }
      }
      // Move Right
      else if (hor_m > 0) {
        if (index % rows_fixed != rows_fixed-1) {
          index = Mathf.Clamp(index + 1, 0, buttonsArray.Length - 1);
        }
      }

      buttonsArray[old_index].GetComponentInChildren<Text>().color = old_color;

      old_index = index;
      old_color = buttonsArray[index].GetComponentInChildren<Text>().color;
      buttonsArray[index].GetComponentInChildren<Text>().color = highlight_color;

      buttonsArray[index].Select();
      //Don't scroll the screen if there are 2 or less rows of scenes
      if (ammountOfRows > 2) {
        yPos = 1 - (float) row / (ammountOfRows - 1);
      }
    }
  }
}