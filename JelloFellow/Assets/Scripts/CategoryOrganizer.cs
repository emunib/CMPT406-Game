using UnityEngine;
using UnityEngine.UI;

public class CategoryOrganizer : MonoBehaviour {
	private Text world_title;
	private Image background;
	private string title;
	private Color title_color;
	private Color background_color;
	
	private string title_updated;
	private Color title_color_updated;
	private Color background_color_updated;
	
	private void Start() {
		world_title = transform.Find("Title").gameObject.GetComponent<Text>();
		title = world_title.text;
		title_color = world_title.color;
		background = GetComponent<Image>();
		background_color = background.color;
	}

	/// <summary>
	/// Set the title text and its color. The title color will automatically always be
	/// alpha 1f.
	/// </summary>
	/// <param name="_title">The string representing the title</param>
	/// <param name="_title_color">Color for the title</param>
	public void SetTitle(string _title, Color _title_color) {
		_title_color.a = 1.0f;
		title_updated = _title;
		title_color_updated = _title_color;
	}

	/// <summary>
	/// Sets the background color.
	/// </summary>
	/// <param name="_background_color">Color of the background</param>
	public void SetBackgroundColor(Color _background_color) {
		background_color_updated = _background_color;
	}

	private void Update() {
		if (title != title_updated) {
			world_title.text = title = title_updated;
		}

		if (title_color != title_color_updated) {
			world_title.color = title_color = title_color_updated;
		}

		if (background_color != background_color_updated) {
			background.color = background_color = background_color_updated;
		}
	}
}
