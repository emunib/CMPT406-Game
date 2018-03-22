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

	private LinkedList<Item> items;		// The list of items currently collected

	// Use this for initialization, happens before start
	void Awake () {
		
		// if items doesn't exist, make this 
		if (script == null) {
			
			DontDestroyOnLoad (gameObject);
			script = this;
			items = new LinkedList<Item> ();
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

	}
	
	// Update is called once per frame
	void OnGUI () {
		
		LinkedListNode<Item> current = items.First;
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

		GUI.Label (new Rect (10, title_y_cur, Screen.width-20, 48),  "Scientist Notes: " + (numFound) + "/" + (numInScene) + " found in level", titleStyle);

		int i = 1;
		while (current != null) {

			if (current.Value.isSelected ()) {
				GUI.Label (new Rect (name_x_cur, 32 + (i * 34), Screen.width * 0.3f - 10, 30), current.Value.getName (), selectedStyle);
				GUI.Label (new Rect (desc_x_cur, 66, Screen.width * 0.7f - 20, Screen.height - 78), current.Value.getDescription (), descriptionStyle);
			} else {
				GUI.Label (new Rect (name_x_cur, 32 + (i * 34), Screen.width*0.3f-10, 30), current.Value.getName(), style);
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
		if (input.GetHorizontalLeftStick() < 0 && display) {

			LinkedListNode<Item> current = items.First;
			bool found = false;		// Remains false if no item in the list has yet been selected							

			while (current != null) {

				if (current.Value.isSelected ()) {
					current.Value.select ();
					if (current.Next == null) {
						items.First.Value.select ();
						return;
					}
					
					current.Next.Value.select ();
					return;
				}
				current = current.Next;

			}

			if (!found) {
				items.First.Value.select ();
			}

		}

		// Going up the list of items
		if (input.GetHorizontalLeftStick() > 0 && display) {

			LinkedListNode<Item> current = items.First;
			bool found = false;

			while (current != null) {

				if (current.Value.isSelected ()) {
					current.Value.select ();
					if (current.Previous == null) {
						items.Last.Value.select ();
						return;
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
		if (input.GetButton1Down() || Input.GetKeyDown("tab"))
			display = !display;
	}
		
}
