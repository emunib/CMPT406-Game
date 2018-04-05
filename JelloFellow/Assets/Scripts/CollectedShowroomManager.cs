using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectedShowroomManager : MonoBehaviour {
  private const string day_path = "Prefabs/UI/Day";
  private readonly Color deselected_color = new Color32(0x00, 0xBC, 0x66, 0xFF);
  private readonly Color selected_color = new Color32(0xFF, 0xCA, 0x3A, 0xFF);
  private const float scroll_smooth_time = 0.08f;
  
  private Image background;
  private GameObject days_parent;
  private Text note;
  private Image daysview_background;
  private Image notesview_background;
  private RectTransform scrolling_pane;
  private Vector3 vertical_position;
  
  private GameObject day_resource;
  private SortedList<int, GameObject> days;
  private LinkedList<GameObject> days_crusormanager;
  private LinkedListNode<GameObject> cursor;
  
  private void Awake() {
    background = transform.Find("Background").gameObject.GetComponent<Image>();
    GameObject daysview = transform.Find("DaysView").gameObject;
    days_parent = daysview.transform.Find("Days").gameObject;
    scrolling_pane = days_parent.GetComponent<RectTransform>();
    daysview_background = daysview.GetComponent<Image>();
    GameObject notetext = transform.Find("NotesView").Find("NoteText").gameObject;
    note = notetext.transform.Find("Text").gameObject.GetComponent<Text>();
    notesview_background = notetext.GetComponent<Image>();
    note.text = "None Found";

    day_resource = Resources.Load<GameObject>(day_path);
    days = new SortedList<int, GameObject>();
    StartCoroutine(UpdateCursor());
  }

  public void RefreshDays() {
    /* update the days sortedlist if a key does not exist */
    SortedDictionary<int, string>.KeyCollection keys = new SortedDictionary<int, string>(MainScript.instance.GetScenesInformation().Collectables).Keys;
    foreach (int day in keys) {
      if (!days.ContainsKey(day)) {        
        days.Add(day, null);
      }
    }

    RefreshTableOrder();
  }

  private void RefreshTableOrder() {
    /* if we just added a day actually refresh it otherwise do nothing */
    if (days.ContainsValue(null)) {
      /* delete all days gameobject */
      foreach (int day in days.Keys) {
        GameObject day_gameobject;
        if ((day_gameobject = days[day]) != null) {
          Destroy(day_gameobject);
        }
      }
      
      /* read all the gameobjects and instantiate in order */
      List<int> keys = new List<int>(days.Keys);
      foreach (int day in keys) {
        GameObject day_gameobject = Instantiate(day_resource, days_parent.transform);
        Text day_text = day_gameobject.transform.Find("Text").gameObject.GetComponent<Text>();
        day_text.text = "Day " + day;
        days[day] = day_gameobject;
      }
      
      if (days.Count > 0f) {
        days_crusormanager = new LinkedList<GameObject>(days.Values);
        cursor = days_crusormanager.First;
        SelectCursor();
      }
    }
  }

  private IEnumerator UpdateCursor() {
    /* wait a frame for instantiat to complete so we can set the cursor */
    yield return null;

    if (days.Count > 0f) {
      days_crusormanager = new LinkedList<GameObject>(days.Values);
      cursor = days_crusormanager.First;
      SelectCursor();
    }

    while (true) {
      if (cursor != null) {
        Input2D _input = InputController.instance.input;
        float vertical = _input.GetVerticalLeftStick();

        float pos_y = days.IndexOfKey(GetDayAtCursor()) * 100 - 100 / 2f;
        vertical_position = new Vector3(0f, scrolling_pane.sizeDelta.y / -2 + pos_y, 0f);

        /* go up (handle categories themselves) */
        if (vertical == 1f) {
          if (cursor.Previous != null) {
            DeselectCursor();
            cursor = cursor.Previous;
            SelectCursor();
          }
        } else if (vertical == -1f) { /* go down */
          if (cursor.Next != null) {
            DeselectCursor();
            cursor = cursor.Next;
            SelectCursor();
          }
        }
      }

      yield return new WaitForSecondsRealtime(0.15f);
    }
  }
  
  private Vector3 scroll_velocity;
  private void Update() {    
    /* smoothly scroll to the vertical position of the category */
    scrolling_pane.localPosition = Vector3.SmoothDamp(scrolling_pane.localPosition, vertical_position, ref scroll_velocity, scroll_smooth_time);
  }

  private void SelectCursor() {
    if (cursor != null) {
      GameObject value = cursor.Value;
      value.GetComponentInChildren<Text>().color = selected_color;
      note.text = MainScript.instance.GetScenesInformation().Collectables[GetDayAtCursor()];
    }
  }

  private int GetDayAtCursor() {
    List<int> keys = new List<int>(days.Keys);
    foreach (int day in keys) {
      if (days[day] == cursor.Value) {
        return day;
      }
    }

    return -1;
  }
  
  private void DeselectCursor() {
    GameObject value = cursor.Value;
    value.GetComponentInChildren<Text>().color = deselected_color;
  }
}
