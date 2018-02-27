using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaBallSpawner : MonoBehaviour {
	private const string metaball_path = "Prefabs/MetaBall";
	private GameObject main_metaball;

	private int max = 2;
	private void Awake() {
		main_metaball = Resources.Load(metaball_path) as GameObject;
		
	}

	private void SpawnMetaball() {
		GameObject metaball = Instantiate(main_metaball, transform);
		//metaball.transform.localPosition = 
	}
}
