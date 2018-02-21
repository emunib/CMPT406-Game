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
			items.AddLast ("Haha nice");
			items.AddLast ("Rock on");

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

	void AddItem(){
		string thing = "Collectable " + items.Count;
		items.AddLast (thing);
	}
}
