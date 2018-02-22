using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedItems : MonoBehaviour {

	public static CollectedItems script;	// Make a static class of self, needed for this singleton data structure
	public bool display;

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

		} else if (script != this) {
			
			// Destroy this object if items already exists
			Destroy (gameObject);

		}

	}
	
	// Update is called once per frame
	void OnGUI () {
		
		LinkedListNode<Item> current = items.First;

		int i = 0;
		while (current != null && display) {
			//GUIText x = GUI.Label (new Rect (10, 10 + (i * 40), 100, 30), current.Value);
			GUI.Label (new Rect (10, 10 + (i * 30), 100, 30), current.Value.getName());
			if (current.Value.isSelected ()) {
				GUI.Label (new Rect (200, 10, 100, 1080), current.Value.getDescription());
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
		Item thing = new Item ();
		thing.setName (name);
		thing.setDescription (description);
		items.AddLast (thing);
	}

	public void Update() {
		
		if (Input.GetKeyDown(KeyCode.DownArrow)) {

			LinkedListNode<Item> current = items.First;
			bool found = false;

			while (current != null && display) {

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

	}
		
}
