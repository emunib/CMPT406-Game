using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedItems : MonoBehaviour {

	private int numInScene;
	private int numFound;
	private GUIStyle titleStyle;
	private GUIStyle style;
	private GUIStyle selectedStyle;
	private GUIStyle descriptionStyle;

	private float title_y;
	private float name_x;
	private float desc_x;

	private float title_y_cur;
	private float name_x_cur;
	private float desc_x_cur;

	public static CollectedItems script;	// Make a static class of self, needed for this singleton data structure
	public bool display;					// Trigger on/off to display the items menu
	public bool remaining;					// Trigger on/off remaining notes display
	public Font textFont;					// Font used for the text 

	/**
	 * 	Item
	 * 
	 * 	Inner class that will represent each item collected.
	 * 
	 **/
	[System.Serializable]
	public class Item {

		private string name = "No Name.";				// Name of the collectable item
		private string description = "No description.";	// Its description. 
		private bool selected = false;					// Whether or not the current item is selected

		/// <summary>
		/// Sets the name.
		/// </summary>
		/// <param name="n">String n.</param>
		public void setName (string n) {
			this.name = n;
		}

		/// <summary>
		/// Sets the description.
		/// </summary>
		/// <param name="d">String D.</param>
		public void setDescription (string d) {
			this.description = d;
		}

		/// <summary>
		/// Toggle whether the current item is selected.
		/// </summary>
		public void select () {
			if (this.selected) {
				this.selected = false;
			} else {
				this.selected = true;
			}
		}

		/// <summary>
		/// Check if the item is selected.
		/// </summary>
		/// <returns><c>true</c>, if item was selected, <c>false</c> otherwise.</returns>
		public bool isSelected() {
			return selected;
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <returns>The name.</returns>
		public string getName () {
			return this.name;
		}

		/// <summary>
		/// Gets the description.
		/// </summary>
		/// <returns>The description.</returns>
		public string getDescription () {
			return this.description;
		}

	}

	private LinkedList<Item> items;				// The list of items currently collected
	private LinkedList<Item> itemsToDisplay;	// Create a Linked list of items to be displayed 

	// Use this for initialization, happens before start
	void Start () {
		
		// if items doesn't exist, make this 
		if (script == null) {
			
			DontDestroyOnLoad (gameObject);
			script = this;
			items = new LinkedList<Item> ();
			itemsToDisplay = new LinkedList<Item> ();
			display = false;

			title_y = 10.0f;
			name_x = 10.0f;
			desc_x = Screen.width * 0.3f + 10;

			title_y_cur = 10.0f;
			name_x_cur = 10.0f;
			desc_x_cur = Screen.width * 0.3f + 10;

			titleStyle = new GUIStyle ("Box");
			titleStyle.fontSize = 30;
			titleStyle.alignment = TextAnchor.MiddleCenter;
			titleStyle.normal.textColor = Color.white;
			titleStyle.font = textFont;

			style = new GUIStyle ("Box");
			style.alignment = TextAnchor.MiddleCenter;
			style.fontSize = 18;
			style.normal.textColor = Color.white;
			style.font = textFont;

			selectedStyle = new GUIStyle ("Box");
			selectedStyle.alignment = TextAnchor.MiddleCenter;
			selectedStyle.fontSize = 18;
			selectedStyle.normal.textColor = Color.yellow;
			selectedStyle.font = textFont;

			descriptionStyle = new GUIStyle ("Box");
			descriptionStyle.alignment = TextAnchor.UpperLeft;
			descriptionStyle.fontSize = 18;
			descriptionStyle.normal.textColor = Color.white;
			descriptionStyle.wordWrap = true;
			descriptionStyle.padding = new RectOffset(40, 40, 40, 40);


		} else if (script != this) {
			
			// Destroy this object if items already exists
			Destroy (gameObject);

		}

		numInScene = GameObject.FindGameObjectsWithTag ("Collectable").Length;
		numFound = 0;

		//int i = 1;
		//while (current != null) {


			//if (current.Value.isSelected ()) {

			//} else {

			//}

			//current = current.Next;
			//i++;

		//}

	}
	
	// Update is called once per frame
	void OnGUI () {
		
		LinkedListNode<Item> current = itemsToDisplay.First;
		int itemsInScene = GameObject.FindGameObjectsWithTag ("Collectable").Length;

		// Make sure position is correct
		// If current position is higher than it should be, and it's to be displayed, then it should go down
		if (title_y_cur < title_y && display) {
			title_y_cur = title_y_cur + (title_y - title_y_cur) * Time.deltaTime * 5;
			name_x_cur = name_x_cur + (name_x - name_x_cur) * Time.deltaTime * 5;
			desc_x_cur = desc_x_cur + (desc_x - desc_x_cur) * Time.deltaTime * 5;
		} else if (title_y_cur > -120 && !display) {
			title_y_cur = title_y_cur - 120 * Time.deltaTime * 5;
			name_x_cur = name_x_cur - Screen.width * 0.3f * Time.deltaTime * 5;
			desc_x_cur = desc_x_cur + Screen.width * 0.7f * Time.deltaTime * 5;
		}

		titleStyle.fontSize = 30 * Screen.height/400;
		style.fontSize = 16 * Screen.height/400;
		selectedStyle.fontSize = 16 * Screen.height/400;
		GUI.Label (new Rect (10, title_y_cur, Screen.width-20, Screen.height/10),  "Scientist Notes: " + (numFound) + "/" + (numInScene)
			+ " found in level", titleStyle);






		int i = 1;
		while (current != null) {

			float y = Screen.height/16;
			float height = Screen.height / 15;
			if (current.Value.isSelected ()) {
				GUI.Label (new Rect (name_x_cur, y + (i * height*1.15f), Screen.width * 0.3f - 10, height), current.Value.getName (), selectedStyle);
				GUI.Label (new Rect (desc_x_cur, y + (height*1.15f), Screen.width * 0.7f - 20, Screen.height - (height*2.5f)),
					current.Value.getDescription (), descriptionStyle);
			} else {
				GUI.Label (new Rect (name_x_cur, y + (i * height*1.15f), Screen.width*0.3f-10, height), current.Value.getName(), style);
			}

			current = current.Next;
			i++;

		}
	}

	/// <summary>
	/// Adds the item to the list of collectables.
	/// </summary>
	/// <param name="name">Name of the item.</param>
	/// <param name="description">Description of the item.</param>
	public void AddItem(string name, string description){

		if (!isCollected(name)) {
			Item thing = new Item ();
			thing.setName (name);
			thing.setDescription (description);
			items.AddLast (thing);
			if (itemsToDisplay.Count <= 10) {
				itemsToDisplay.AddLast (thing);
			}
			numFound++;

			// If it's the first item found, it should be selected when the menu opens
			if (numFound == 1) {
				items.First.Value.select ();
			}
		}

	}

	/// <summary>
	/// Checks if the item of the given name is already collected
	/// </summary>
	/// <returns><c>true</c>, if it was collected, <c>false</c> otherwise.</returns>
	/// <param name="name">Name.</param>
	public bool isCollected(string name) {
		
		LinkedListNode<Item> current = items.First;

		while (current != null) {
			if (current.Value.getName () == name)
				return true;
			current = current.Next;
		}

		return false;

	}

	public void Update() {
		Input2D input = InputController.instance.GetInput();
		
		// Going to the list of items
		if ((input.GetHorizontalLeftStick() < 0 || Input.GetKeyDown (KeyCode.DownArrow)) && display) {

			LinkedListNode<Item> current = itemsToDisplay.First;
			LinkedListNode<Item> currentAll = items.First;
			bool found = false;		// Remains false if no item in the list has yet been selected		

			// make sure current and currentALl point to the same item
			while (current.Value.getName () != currentAll.Value.getName ()) {
				currentAll = currentAll.Next;
			}

			while (current != null) {

				if (current.Value.isSelected ()) {
					current.Value.select ();
					if (current.Next == null) {
						if (currentAll.Next == null) {
							itemsToDisplay.First.Value.select ();
							return;
						} else {
							itemsToDisplay.RemoveFirst ();
							LinkedListNode<Item> next = currentAll.Next;
							itemsToDisplay.AddLast (next.Value);
						}
					}
					
					current.Next.Value.select ();
					return;
				}
				current = current.Next;
				currentAll = currentAll.Next;
			}

			if (!found) {
				itemsToDisplay.First.Value.select ();
			}

		}

		// Going up the list of items
		if ((input.GetHorizontalLeftStick() > 0  || Input.GetKeyDown (KeyCode.UpArrow)) && display) {

			LinkedListNode<Item> current = itemsToDisplay.First;
			LinkedListNode<Item> currentAll = items.First;
			bool found = false;

			// make sure current and currentALl point to the same item
			while (current.Value.getName () != currentAll.Value.getName ()) {
				currentAll = currentAll.Next;
			}

			while (current != null) {

				if (current.Value.isSelected ()) {
					current.Value.select ();
					if (current.Previous == null) {
						if (currentAll.Previous == null) {
							itemsToDisplay.Last.Value.select ();
							return;
						} else {
							itemsToDisplay.RemoveLast ();
							LinkedListNode<Item> prev = currentAll.Previous;
							itemsToDisplay.AddFirst (prev.Value);
						}
					}
					
					current.Previous.Value.select ();
					return;
				}
				current = current.Next;

			}

			if (!found) {
				items.Last.Value.select ();
			}

		}

		// Going up the list of items
		if (input.GetButton1Down () || Input.GetKeyDown (KeyCode.Tab)) {
			//if (GameController.control.currSceneName == "SceneSelector") {
				Debug.Log ("scene selector");
				display = !display;
			//} else {
				Debug.Log ("not scene selector");
				remaining = !remaining;
			//}
		}

	}
		
}
