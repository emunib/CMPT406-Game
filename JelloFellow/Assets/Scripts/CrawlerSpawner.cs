using System.Linq;
using UnityEngine;

public class CrawlerSpawner : MonoBehaviour {
	private const string gravityfield_sprite_path = "Prefabs/Components/AI/Crawler";
	private const int ai_count_limit = 1;
	
	[Header("General")]
	[Tooltip("Spawn points for the AI.")] [SerializeField]
	private Transform[] SpawnPoints;
	//[Tooltip("Prefab of the spawning AI.")] [SerializeField]
	//private GameObject AIPrefab;

	private string unique_key;
	private int ai_count;
	
	private void Awake() {
		unique_key = RandomString(6);
		gameObject.name += unique_key;
		/* make sure we have something in spawnpoints */
		if (SpawnPoints.Length > 0) {
			for (int i = 0; i < SpawnPoints.Length; i++) {
				/* delete all the visual for the spawn points */
				foreach (SpriteRenderer _renderer in SpawnPoints[i].GetComponentsInChildren<SpriteRenderer>()) {
					Destroy(_renderer);
				}
			}
		}
	}

	private void Start() {
		InvokeRepeating("Spawn", 0f, 1f);
	}

	private void Death() {
		if (ai_count > 0) ai_count--;
	}
	
	private void Spawn() {
		if (ai_count < ai_count_limit) {
			/* make sure we have something in spawnpoints */
			if (SpawnPoints.Length > 0) {
				int random_spawnpoint = Random.Range(0, SpawnPoints.Length);

				GameObject tmp_ai = Resources.Load(gravityfield_sprite_path) as GameObject;				
				tmp_ai = Instantiate(tmp_ai, SpawnPoints[random_spawnpoint].position, SpawnPoints[random_spawnpoint].rotation);
				tmp_ai.name += unique_key;
				ai_count++;
			}
		}
	}
	
	private static readonly System.Random random = new System.Random();
	private static string RandomString(int length) {
		const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		return new string(Enumerable.Range(1, length).Select(_ => chars[random.Next(chars.Length)]).ToArray());
	}
}
