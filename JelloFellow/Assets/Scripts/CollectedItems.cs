using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedItems : MonoBehaviour {

	public static CollectedItems script;	// Make a static class of self, needed for this singleton data structure
	public bool display;

	private LinkedList<string> stringItems;			
	private LinkedList<GUIElement> names;		
	private LinkedList<GUIElement> descriptions;

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

		public void setName(string n){
			this.name = n;
		}

		public void setDescription(string d) {
			this.description = d;
		}

		public string getName(){
			return this.name;
		}

		public string getDescription() {
			return this.description;
		}

	}

	private LinkedList<Item> items;

	// Use this for initialization, happens before start
	void Awake () {
		
		// if items doesn't exist, make this 
		if (script == null) {
			
			DontDestroyOnLoad (gameObject);
			script = this;
			stringItems = new LinkedList<string> ();
			names = new LinkedList<GUIElement> ();
			descriptions = new LinkedList<GUIElement> ();
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
			current = current.Next;
			i++;

		}
	}

	public void AddItem(string name, string description){
		Item thing = new Item ();
		thing.setName (name);
		thing.setDescription (description);
		items.AddLast (thing);
	}
		
}
