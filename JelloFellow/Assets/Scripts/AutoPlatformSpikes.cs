using System.Linq;
using System.Collections.Generic;
using UnityEngine;



[SelectionBase]
[ExecuteInEditMode]
public class AutoPlatformSpikes : AutoPlatform {


	[SerializeField] private Sprite spike_sprite;
	[CustomLabel("Spike Offset")] [Tooltip("Offset of the spikes.")] [SerializeField]
	private float spike_offset;

	private float spike_offset_old;
	
	
	/// <summary>
	/// This is copypasta from AutoPlatform EXCEPT FOR FINAL IF
	/// </summary>
	protected override void Update(){
		/* if all are false change center to true */
		if (!extend_center && !extend_left && !extend_right) extend_center = true;
    
		/* only have one boolean be true */
		if (extend_left_old != extend_left) {
			extend_center = extend_center_old = false;
			extend_right = extend_right_old = false;
      
			extend_left_old = extend_left;
		}
    
		if (extend_right_old != extend_right) {
			extend_center = extend_center_old = false;
			extend_left = extend_left_old = false;
      
			extend_right_old = extend_right;
		}
    
		if (extend_center_old != extend_center) {
			extend_left = extend_left_old = false;
			extend_right = extend_right_old = false;
      
			extend_center_old = extend_center;
		}
    
		/* platform width changed so update it */			//NICKS CHANGE
		if (platform_width_old != platform_width || spike_offset_old!=spike_offset) {
			List<Transform> tempList = transform.Cast<Transform>().ToList();
			/* remove all gameobjects in parent */
			foreach (Transform child in tempList) {
				DestroyImmediate(child.gameObject);
			}
      
			/* only create the platform when all the childs are destroyed */
			CreatePlatform();
			platform_width_old = platform_width;
			
			// NICKS CHANGE
			spike_offset_old = spike_offset;
		}
		
		
		
	}


	protected override void CreatePlatform() {
		base.CreatePlatform();
		Vector2 spikesize = new Vector2(platform_width-1f , spike_sprite.bounds.size.y);
		GameObject spike_tile = CreateGameObjectFromSprite(spike_sprite, "Spikes", spikesize, true);
		spike_tile.transform.parent = transform;
		
		spike_tile.transform.localPosition = new Vector2(0f, spike_offset);

		spike_tile.transform.localRotation = Quaternion.identity;
		PolygonCollider2D spike_collider = spike_tile.AddComponent<PolygonCollider2D>();
		spike_collider.autoTiling = true;
		spike_collider.offset = Vector2.zero;
		spike_tile.AddComponent<ApplyDmg>();

	}

}
