using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
	private float prompt_y_cur;
	private float name_x_cur;
	private float desc_x_cur;
	private float title_y_rem;
	private float curTime = 0;				// How long the remaining GUI has been on the screen for

	public static CollectedItems script;	// Make a static class of self, needed for this singleton data structure
	public bool display;					// Trigger on/off to display the items menu
	public bool remaining;					// Trigger on/off remaining notes display
	public bool showImage;					// Show the preview image for the collectable
	public Font textFont;					// Font used for the text 
	public float stayTime = 5;				// How long the remaining collectables GUI should remain on screen


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
		private bool collected = false;					// Whether it's been collected
		private bool selected = false;					// Whether or not the current item is selected
		private Texture2D image = null;

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
		/// Sets the image.
		/// </summary>
		/// <param name="t">Texture2D t.</param>
		public void setImage (Texture2D t) {
			this.image = t;
		}

		/// <summary>
		/// Sets the collected.
		/// </summary>
		/// <param name="c">If set to <c>true</c> c.</param>
		public void setCollected (bool c) {
			this.collected = c;
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
		/// Check if the item is collected.
		/// </summary>
		/// <returns><c>true</c>, if item was collected, <c>false</c> otherwise.</returns>
		public bool isCollected() {
			return collected;
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

		/// <summary>
		/// Gets the Image.
		/// </summary>
		/// <returns>The description.</returns>
		public Texture2D getImage () {
			return this.image;
		}

		/// <summary>
		/// Saves the item
		/// </summary>
		public void save() {

			if (!Directory.Exists(Application.persistentDataPath + "/Collectables")) {
				Directory.CreateDirectory(Application.persistentDataPath + "/Collectables");
			}

			BinaryFormatter bf = new BinaryFormatter ();
			FileStream stream = new FileStream (Application.persistentDataPath + "/Collectables/" + this.getName() + ".bin", FileMode.Create);

			if (!Directory.Exists(Application.persistentDataPath + "/Collectables")) {
				Directory.CreateDirectory(Application.persistentDataPath + "/Collectables");
			}

			string[] stats = new string[4];
			stats [0] = this.getName ();
			stats [1] = this.getDescription ();
			stats [2] = this.isCollected() ? "true" : "false";

			if (this.getImage() == null) {
				stats [3] = "No Image";
			} else {
				#if UNITY_EDITOR
				stats [3] = AssetDatabase.GetAssetPath (this.getImage ());
				#endif
			}

			bf.Serialize (stream, stats);
			stream.Close ();
		}

		/// <summary>
		/// Loads the item
		/// </summary>
		public static void load(string filepath, LinkedList<Item> l) {


			BinaryFormatter bf = new BinaryFormatter ();
			FileStream stream = new FileStream (filepath, FileMode.Open);

			string[] stats = bf.Deserialize (stream) as string[];

			script = GameObject.Find ("CollectedItems").GetComponent<CollectedItems> ();
			if (stats [3] == null) {
				script.AddItem (stats [0], stats [1], bool.Parse(stats[2]), null);
			} else {
				#if UNITY_EDITOR
				Texture2D t = (Texture2D)AssetDatabase.LoadAssetAtPath (stats [2], typeof(Texture2D));
				script.AddItem (stats [0], stats [1], bool.Parse(stats[2]), t);
				#endif
			}

			stream.Close ();


		}


	}

	private LinkedList<Item> items;				// The list of items currently collected
	private LinkedList<Item> itemsToDisplay;	// Create a Linked list of items to be displayed 
	private Item[] itemsInScene;				// Create a Linked list of items that are in the scene

	void OnApplicationQuit(){
		LinkedListNode<Item> current = items.First;
		while (current != null) {
			current.Value.save ();
			current = current.Next;
		}
	}

	// Use this for initialization, happens before start
	void Start () {
		
		// if items doesn't exist, make this 
		if (script == null) {

			numInScene = GameObject.FindGameObjectsWithTag ("Collectable").Length;
			if (numInScene == null) {
				numInScene = 0;
			}
			numFound = 0;
			
			DontDestroyOnLoad (gameObject);
			script = this;
			items = new LinkedList<Item> ();
			itemsToDisplay = new LinkedList<Item> ();
			display = false;
			remaining = false;
			showImage = false;

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

			// Since start() is called after on scene loaded, the number of collected items
			// must be counted here as well, as it would be overwritten by the singleton
			// when the game is started
			countCollected();


		} else if (script != this) {
			
			// Destroy this object if items already exists
			Destroy (gameObject);

		}

	}

	/// <summary>
	/// Counts the collected items in the scene and adjusts 
	/// the scripts data accordingly.
	/// </summary>
	void countCollected() {

		if (GameController.instance.currSceneName != "SceneSelector" && GameController.instance.currSceneName !="MainMenu") {
			numFound = 0;
			if (!Directory.Exists (Application.persistentDataPath + "/Collectables")) {
				Directory.CreateDirectory (Application.persistentDataPath + "/Collectables");
			}

			foreach (string file in Directory.GetFiles(Application.persistentDataPath + "/Collectables")) {

				Item.load (file, items);

			}

			// Get an array of all collectables in the level
			GameObject[] obs = GameObject.FindGameObjectsWithTag ("Collectable");

			if (obs != null) {
				numInScene = obs.Length;
			} else {
				numInScene = 0;
			}

			numInScene = GameObject.FindGameObjectsWithTag ("Collectable").Length;

			for (int i = 0; i < numInScene; i++) {

				// Get the object's name
				string name = obs [i].gameObject.name;
				LinkedListNode<Item> cur = items.First;
				Collectable c = obs [i].GetComponent<Collectable> ();

				if (c.isCollected ()) {
					numFound++;
				}

			}

		} else {

			if (!Directory.Exists (Application.persistentDataPath + "/Collectables")) {
				Directory.CreateDirectory (Application.persistentDataPath + "/Collectables");
			}

			foreach (string file in Directory.GetFiles(Application.persistentDataPath + "/Collectables")) {

				Item.load (file, items);

			}

			numInScene = items.Count;
			LinkedListNode<Item> cur = items.First;

			while (cur != null) {
				
				if (cur.Value.isCollected ()) {
					numFound++;
				}

				cur = cur.Next;

			}

		}
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		//WaitForSeconds (10);
		countCollected ();
		//needToCount = true;
	}

	/// <summary>
	/// Gets the number of items in scene.
	/// </summary>
	/// <returns>The number of items.</returns>
	public int getNumInScene () {
		return numInScene;
	}

	/// <summary>
	/// Gets the number of items found.
	/// </summary>
	/// <returns>The number found.</returns>
	public int getNumFound () {
		return numFound;
	}

	/// <summary>
	/// Gets the number of items found.
	/// </summary>
	public void increaseNumFound () {
		numFound++;
	}

	/// <summary>
	/// Gets the number of items found.
	/// </summary>
	public void increaseNumInScene () {
		numInScene++;
	}

	/// <summary>
	/// Gets the number of items found.
	/// </summary>
	public void zeroNumFound () {
		numFound = 0;
	}

	/// <summary>
	/// Gets the number of items found.
	/// </summary>
	public void zeroNumInScene () {
		numInScene = 0;
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

		if (prompt_y_cur > -120 && display) {
			prompt_y_cur = prompt_y_cur - 120 * Time.deltaTime * 5;
		} else if (prompt_y_cur < title_y && !display) {
			prompt_y_cur = prompt_y_cur + (title_y - prompt_y_cur) * Time.deltaTime * 5;
		}

		if (title_y_rem < title_y && remaining) {
			title_y_rem = title_y_rem + (title_y - title_y_rem) * Time.deltaTime * 5;
		} else if (title_y_rem > -120 && !remaining) {
			title_y_rem = title_y_rem - 120 * Time.deltaTime * 5;
		}

		titleStyle.fontSize = 30 * Screen.height/400;
		style.fontSize = 16 * Screen.height/400;
		selectedStyle.fontSize = 16 * Screen.height/400;

		string name = GameController.instance.currSceneName;
		if (name == "SceneSelector" || name =="MainMenu") {
			  
			GUI.Label (new Rect (10, title_y_cur, Screen.width - 20, Screen.height / 10), "Scientist Notes: " + (numFound) + "/" + (items.Count)
			+ " found", titleStyle);

			GUI.Label (new Rect (10, prompt_y_cur, Screen.width/4 - 20, Screen.height / 15), "Y - View Collection", style);
			
		} else {
			if (numInScene > 0) {
				GUI.Label (new Rect (10, title_y_rem, Screen.width / 4, Screen.height / 10), (numFound) + "/" + (numInScene)
					+ " Collectables", titleStyle);
			} else {
				GUI.Label (new Rect (10, title_y_rem, Screen.width / 4, Screen.height / 10), "No Collectables", titleStyle);
			}
		}

		Texture2D img = null;

		int i = 1;
		while (current != null && !remaining) {
			float y = Screen.height/16;
			float height = Screen.height / 15;
			if (current.Value.isSelected ()) {
				
				GUI.Label (new Rect (name_x_cur, y + (i * height*1.15f), Screen.width * 0.3f - 10, height), current.Value.getName (), selectedStyle);
				GUI.Label (new Rect (desc_x_cur, y + (height*1.15f), Screen.width * 0.7f - 20, Screen.height - (height*2.5f)),
					current.Value.getDescription (), descriptionStyle);

				// Save the image to be later drawn on top
				img = current.Value.getImage ();
				
			} else {
				GUI.Label (new Rect (name_x_cur, y + (i * height*1.15f), Screen.width*0.3f-10, height), current.Value.getName(), style);
			}

			current = current.Next;
			i++;

		}

		if (showImage && img != null) 
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), img, ScaleMode.ScaleToFit);
		
	}

	/// <summary>
	/// Adds the item to the list of collectables.
	/// </summary>
	/// <param name="name">Name of the item.</param>
	/// <param name="description">Description of the item.</param>
	public void AddItem(string name, string description, bool collected, Texture2D image){

		if (!isCollected (name)) {
			
			Item thing = new Item ();
			thing.setName (name);
			thing.setDescription (description);
			thing.setImage (image);
			thing.setCollected (collected);

			if (items == null)
				items = new LinkedList<Item> ();
			
			items.AddLast (thing);

			if (itemsToDisplay == null)
				itemsToDisplay = new LinkedList<Item> ();
			if (itemsToDisplay.Count <= 10) {
				itemsToDisplay.AddLast (thing);
			}

			if (collected)
				//numFound++;

			// If it's the first item found, it should be selected when the menu opens
			if (numFound == 1) {
				items.First.Value.select ();
			}
		} else {
			setCollected (name);
		}

	}

	/// <summary>
	/// Checks if the item of the given name is already collected
	/// </summary>
	/// <returns><c>true</c>, if it was collected, <c>false</c> otherwise.</returns>
	/// <param name="name">Name.</param>
	public bool isCollected(string name) {

		if (items == null) 
			return false;

		LinkedListNode<Item> current = items.First;

		while (current != null) {
			if (current.Value.getName () == name)
				return true;
			current = current.Next;
		}

		return false;

	}

	/// <summary>
	/// Checks if the item of the given name is already collected
	/// </summary>
	/// <returns><c>true</c>, if it was collected, <c>false</c> otherwise.</returns>
	/// <param name="name">Name.</param>
	public bool getCollected(string name) {

		if (items == null) 
			return false;

		LinkedListNode<Item> current = items.First;

		while (current != null) {
			if (current.Value.getName () == name && current.Value.isCollected())
				return true;
			current = current.Next;
		}

		return false;

	}

	/// <summary>
	/// Checks if the item of the given name is already collected
	/// </summary>
	/// <returns><c>true</c>, if it was collected, <c>false</c> otherwise.</returns>
	/// <param name="name">Name.</param>
	public void setCollected(string name) {

		LinkedListNode<Item> current = items.First;

		while (current != null) {
			if (current.Value.getName () == name) {
				current.Value.setCollected (true);
				break;
			}
			current = current.Next;
		}


	}

	public void Update() {
	 	
		//Input2D input = InputController.instance.GetInput();

		Input2D input = InputController.instance.input;

		
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
			string name = GameController.instance.currSceneName;
			if (name == "SceneSelector" || name =="MainMenu") {
				display = !display;
			} else {
				remaining = !remaining;
			}
		}

		// Going up the list of items
		if ((input.GetButton3Down () || Input.GetKeyDown (KeyCode.Space)) && display) {
			showImage = !showImage;
		}
			
	

		if (remaining) {
			curTime = curTime + Time.deltaTime;
			if (curTime > stayTime) {
				remaining = false;
				curTime = 0;
			}
		}

	}

	public int GetNumInScene()
	{
		return numInScene;
	}

	public int GetNumFound()
	{
		return numFound;
	}
		
}
