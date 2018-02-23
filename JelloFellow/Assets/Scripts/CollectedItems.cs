﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedItems : MonoBehaviour {

	private int numInScene;
	private int numFound;

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

		numInScene = GameObject.FindGameObjectsWithTag ("Collectable").Length;
		numFound = 0;

	}
	
	// Update is called once per frame
	void OnGUI () {
		
		LinkedListNode<Item> current = items.First;
		int itemsInScene = GameObject.FindGameObjectsWithTag ("Collectable").Length;
		GUI.Label (new Rect (10, 10, 100, 30), "Items found: " + (numFound) + "/" + (numInScene));

		int i = 1;
		while (current != null && display) {
			
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

		if (!isCollected(name)) {
			Item thing = new Item ();
			thing.setName (name);
			thing.setDescription (description);
			items.AddLast (thing);
			numFound++;
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
