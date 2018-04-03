using UnityEngine;
using UnityEngine.UI;

public class CategoryOrganizer : MonoBehaviour {
	private Text world_title;

	private void Init(string _world_title) {
		world_title = transform.Find("Title").gameObject.GetComponent<Text>();
		world_title.text = _world_title;
	}
}
