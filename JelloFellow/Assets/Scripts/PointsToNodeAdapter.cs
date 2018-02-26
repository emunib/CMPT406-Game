using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

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

			var player = centre.GetComponent<FellowPlayer>();
			player.ChildComponents = children;
			player.apply_gravity_tochild = true;
			player.apply_movement_tochild = true;
			player.move_speed = 20f;
			player.jump_angle_force = 30f;
		}
		else
		{
			Destroy(this);
		}
	}
}
