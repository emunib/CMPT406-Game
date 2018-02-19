using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoor : MonoBehaviour {


	public ButtonComponent[] Buttons;
	public GameObject[] Doors;

	
	public void OpenDoors() {

		//Only open doors if all buttons are set.
		foreach (ButtonComponent btn in Buttons) {
			if (!btn.set) {
				return;
			}
		}
		
		//Open door. For now just destroys. Move to somewhere may be better. Could do by sending msg. 
		foreach (GameObject door in Doors) {
			Destroy(door);
		}
		
	}
	
	
	
}
