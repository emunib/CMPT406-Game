using UnityEngine;

public class MetaBallSpawner : MonoBehaviour {
	private const string metaball_path = "Prefabs/MetaBall";
	private const float spawnrate = 0.5f;
	private const int max_metaballs = 6;
	private const float speed = 2f;
	
	private GameObject main_metaball;
	private int metaball_count;
	
	private void Awake() {
		metaball_count = 0;
		main_metaball = Resources.Load(metaball_path) as GameObject;
		InvokeRepeating("SpawnMetaball", 0f, spawnrate);
	}

	private void SpawnMetaball() {
		if (metaball_count < max_metaballs) {
			Vector2 randomPosition = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(0f, Screen.width), Random.Range(0, Screen.height)));

			GameObject metaball = Instantiate(main_metaball, transform);
			float scale = Random.Range(0.4f, 2f) * 8f;
			metaball.transform.localScale = new Vector2(scale, scale);
			metaball.transform.position = randomPosition;
			metaball.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * speed;
			metaball_count++;
		} else {
			CancelInvoke("SpawnMetaball");
		}
	}
}
