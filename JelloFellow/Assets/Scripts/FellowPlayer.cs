using UnityEngine;
using UnityEngine.SceneManagement;

public class FellowPlayer : GenericPlayer {
	private Input2D _input;
	private UnityJellySprite _jelly;
	
	protected override void Start() {
		base.Start();
		
		_input = InputController.instance.GetInput();
		SetIgnoreFields(true);
		SetInput(_input);
		SetFieldRadius(2f);
		
		_jelly = GetComponent<JellySpriteReferencePoint>().ParentJellySprite.GetComponent<UnityJellySprite>();
	}

	protected override void Update() {
		/* spawn spin */
		if (_jelly.gameObject.transform.localScale != (Vector3) Vector2.one) {
			float angle = _jelly.gameObject.transform.rotation.eulerAngles.z == 0f ? 360f : _jelly.gameObject.transform.rotation.eulerAngles.z;
			_jelly.gameObject.transform.rotation = Quaternion.Slerp(_jelly.gameObject.transform.rotation, Quaternion.Euler(0,0,_jelly.gameObject.transform.localScale.x * angle), 1f);
		} else {
			base.Update();
		}
	}

	protected override void Death() {
		Debug.Log("Bleh I died.");
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
