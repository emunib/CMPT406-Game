using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedItems : MonoBehaviour {

	private int numInScene;
	private int numFound;
	private GUIStyle titleStyle;
	private GUIStyle style;
	private GUIStyle selectedStyle;

	public static CollectedItems script;	// Make a static class of self, needed for this singleton data structure
	public bool display;					// Trigger on/off to display the items menu

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
			display = true;

			titleStyle = new GUIStyle ("Box");
			titleStyle.fontSize = 30;
			titleStyle.alignment = TextAnchor.MiddleCenter;
			titleStyle.normal.textColor = Color.white;

			style = new GUIStyle ("Box");
			style.alignment = TextAnchor.MiddleCenter;
			style.fontSize = 18;
			titleStyle.normal.textColor = Color.white;

			selectedStyle = new GUIStyle ("Box");
			selectedStyle.alignment = TextAnchor.MiddleCenter;
			selectedStyle.fontSize = 18;
			selectedStyle.normal.textColor = Color.yellow;

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

		//GUI.Box(new Rect (10, 10, Screen.width-20, 48), "");
		GUI.Label (new Rect (10, 10, Screen.width-20, 48),  "Scientist Notes: " + (numFound) + "/" + (numInScene) + " found in level", titleStyle);

		int i = 1;
		while (current != null && display) {

			if (current.Value.isSelected ()) {
				GUI.Label (new Rect (10, 32 + (i * 34), Screen.width / 2 - 10, 30), current.Value.getName (), selectedStyle);
				GUI.Label (new Rect (Screen.width / 2 + 10, 66, Screen.width / 2 - 20, Screen.height - 78), current.Value.getDescription (), style);
			} else {
				GUI.Label (new Rect (10, 32 + (i * 34), Screen.width/2-10, 30), current.Value.getName(), style);
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

		// Going to the list of items
		if (Input.GetKeyDown(KeyCode.DownArrow) && display) {

			LinkedListNode<Item> current = items.First;
			bool found = false;		// Remains false if no item in the list has yet been selected							

			while (current != null) {

				if (current.Value.isSelected ()) {
					current.Value.select ();
					if (current.Next == null) {
						items.First.Value.select ();
						return;
					} else {
						current.Next.Value.select ();
						return;
					}
					found = true;
				}
				current = current.Next;

			}

			if (!found) {
				items.First.Value.select ();
			}

		}

		// Going up the list of items
		if (Input.GetKeyDown(KeyCode.UpArrow) && display) {

			LinkedListNode<Item> current = items.First;
			bool found = false;

			while (current != null) {

				if (current.Value.isSelected ()) {
					current.Value.select ();
					if (current.Previous == null) {
						items.Last.Value.select ();
						return;
					} else {
						current.Previous.Value.select ();
						return;
					}
					found = true;
				}
				current = current.Next;

			}

			if (!found) {
				items.Last.Value.select ();
			}

		}

	}
		
}
