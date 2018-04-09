using UnityEngine;
using UnityEngine.UI;

public class ControlInterpreter : MonoBehaviour {
	private Text control_text;

	private void Start () {
		control_text = GetComponent<Text>();
		if (InputController.instance.type() == ControllerType.PS4) {
			control_text.text = control_text.text.Replace("=", "▲");
		} else if (InputController.instance.type() == ControllerType.XBOXONE) {
			control_text.text = control_text.text.Replace("=", "Y");
		} else {
			control_text.text = control_text.text.Replace("=", "Unkown");
		}
	}
}
