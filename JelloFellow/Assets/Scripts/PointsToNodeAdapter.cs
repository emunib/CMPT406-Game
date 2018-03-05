using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(PlayerConfigurator))]
public class PointsToNodeAdapter : MonoBehaviour
{
	private JellySprite _jelly;

	// Update is called once per frame
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
					RaycastOrigin = true
				};

				children[i] = childComonent;
			}

			var centre = _jelly.CentralPoint.GameObject;
			centre.AddComponent<FellowPlayer>();

			var config = _jelly.GetComponent<PlayerConfigurator>();
			config.ChildComponents = children;

			var player = centre.GetComponent<FellowPlayer>();
			player.config = config;
		}
		else
		{
			Destroy(this);
		}
	}
}
