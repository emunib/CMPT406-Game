using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedItems : MonoBehaviour {

	public static CollectedItems script;

	private LinkedList<string> items;

	// Use this for initialization, happens before start
	void Awake () {
		
		// if items doesn't exist, make this 
		if (script == null) {
			
			DontDestroyOnLoad (gameObject);
			script = this;
			items = new LinkedList<string> ();

		} else if (script != this) {
			
			// Destroy this object if items already exists
			Destroy (gameObject);

		}

	}
	
	// Update is called once per frame
	void OnGUI () {
		
		LinkedListNode<string> current = items.First;

		int i = 0;
		while (current != null) {
			GUI.Label (new Rect (10, 10 + (i * 40), 100, 30), current.Value);
			current = current.Next;
			i++;
		}
	}

	public void AddItem(){
		string thing = "Collectable " + (items.Count + 1);
		items.AddLast (thing);
		GUI.Label (new Rect (10, 10 + (items.Count * 30), 100, 30), items.Last.Value);
	}
}
