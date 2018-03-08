using System;
using UnityEngine;
using Object = UnityEngine.Object;

[RequireComponent(typeof(PlayerConfigurator))]
public class PointsToNodeAdapter : MonoBehaviour
{
	private JellySprite _jelly;
	public Object _playerScript;

	private void Update () {
		if (!_jelly)
		{
			_jelly = GetComponent<JellySprite>();
			var children = new ChildComponent[_jelly.ReferencePoints.Count - 1];
			
			for (var i = 0; i < children.Length; i++)
			{
				var childComonent = new ChildComponent
				{
					Child = _jelly.ReferencePoints[i + 1].transform,
					RaycastOrigin = false
				};

				children[i] = childComonent;
			}

			var centre = _jelly.CentralPoint.GameObject;
			centre.AddComponent(Type.GetType(_playerScript.name));

			var config = _jelly.GetComponent<PlayerConfigurator>();
			config.ChildComponents = children;

			var player = centre.GetComponent<GenericPlayer>();
			if (player != null) player.config = config;
		}
		else
		{
			Destroy(this);
		}
	}
}
