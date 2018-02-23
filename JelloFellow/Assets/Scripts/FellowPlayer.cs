using UnityEngine;

public class FellowPlayer : GenericPlayer {
	private Input2D _input;
	
	protected override void Start() {
		base.Start();
		
		_input = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>().GetInput();
		SetInput(_input);
		SetFieldRadius(2f);
	}

}
