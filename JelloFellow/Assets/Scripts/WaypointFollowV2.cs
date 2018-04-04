#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class WaypointFollowV2 : MonoBehaviour {
	[Header("Sprite")]
	public Sprite track_sprite;

	[Header("Waypoints")]
	public Vector3[] LocalWayPoints;
	[Tooltip("If true, waypoints act in circular fashion. ie 0,1,2,3,0,1,2,3,0....")]
	public bool CircularWaypoints;
	public bool DebugWaypoints;
	public bool PositionInfo;
	[Header("Stats")]
	public float SpinRatee;
	[Range(0,100)]
	public float Speed;

	public float track_size_offset_x;
	
	
	public bool buildTrack;

	
	private Transform Saw;
	

	private Vector3[] GlobalWaypoints;

	private Vector2 _nextPos,_myPos;
	
	float getscale ;

	
	private bool _goingUp = true;
	private int posCounter;
	

	void Start () {
		
		Saw = transform.Find("Saw");
		posCounter = 0;
		getscale= transform.localScale.x;
		//Set the global waypoints
		GlobalWaypoints = new Vector3[LocalWayPoints.Length];
		for (int i = 0; i < LocalWayPoints.Length; i++) {
			GlobalWaypoints[i] = LocalWayPoints[i]*getscale + transform.position;
		}
		
		if (buildTrack){
			BuildTracks();
		}

	}

	
	
	
	/// <summary>
	/// We're going to fill in the tracks based on the positions of the waypoints
	/// </summary>
	private void BuildTracks() {
		GameObject tr = new GameObject("track");
		SpriteRenderer sp = tr.AddComponent<SpriteRenderer>();
		sp.sprite = track_sprite;
		sp.drawMode = SpriteDrawMode.Tiled;
		sp.tileMode = SpriteTileMode.Continuous;
		tr.transform.parent = transform;
		
		
		
		float distance = Vector2.Distance(GlobalWaypoints[0], GlobalWaypoints[1])/getscale;
		sp.size = new Vector2(distance+track_size_offset_x,sp.size.y);

	
		
		
		
		//Put in Middle
		Vector3 middlesPos = (GlobalWaypoints[1] - GlobalWaypoints[0])/getscale;
		
		
		//Calculate the angle
		float angle = Mathf.Atan2(GlobalWaypoints[1].y - GlobalWaypoints[0].y, GlobalWaypoints[1].x - GlobalWaypoints[0].x) *
		              180 / Mathf.PI;

		middlesPos /= 2;
		middlesPos += LocalWayPoints[0];
		
		tr.transform.localPosition = middlesPos;
		Vector3 rotation = sp.transform.rotation.eulerAngles;
		rotation.z = angle;
		sp.transform.rotation = Quaternion.Euler(rotation);

		
	
		Vector3 p0, p1, pointToPoint;

		for (int i = 1; i < GlobalWaypoints.Length; i++) {
			
			GameObject newTrack = Instantiate(tr, transform);

			
			p0 = GlobalWaypoints[i];
			if (i == GlobalWaypoints.Length - 1) {
				if (CircularWaypoints) {
					p1 = GlobalWaypoints[0];

				}
				else {
					break;
				}
				
			}
			else {
				p1 = GlobalWaypoints[i+1];
			}		
		
			sp = newTrack.GetComponent<SpriteRenderer>();

			angle = Mathf.Atan2(p1.y - p0.y, p1.x - p0.x) *
			        180 / Mathf.PI;
			pointToPoint = p1- p0;
			middlesPos = p0+pointToPoint / 2;
			newTrack.transform.position = middlesPos;
			
			rotation = newTrack.transform.rotation.eulerAngles;
			rotation.z = angle;
			newTrack.transform.rotation = Quaternion.Euler(rotation);
			distance = Vector2.Distance(p0, p1)/getscale;
			sp.size = new Vector2(distance+track_size_offset_x,sp.size.y);
		}

	}
	
	
	
	
	// Update is called once per frame
	void Update () {
		
		//Debug.DrawRay(GlobalWaypoints[1],GlobalWaypoints[2]-GlobalWaypoints[1],Color.blue);
		MoveSaw();
		Spin();
		
		
		/*Vector3 velocity = CalculateSawMoveSawment();
		Saw.Translate(velocity);
		Vector3 rotation = Saw.transform.localEulerAngles;
		Saw.transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z + 10);
		*/

		
	}
	
	/// <summary>
	/// MoveSaws the saw to the next position
	/// </summary>
	private void MoveSaw() {
		Saw.localPosition= Vector3.MoveTowards(Saw.localPosition, _nextPos,Speed*Time.deltaTime);
		
		ChangeWaypoint();
	}

	private void Spin() {
		Saw.transform.Rotate (0,0,-SpinRatee);

	}
	
	
	/// <summary>
	/// Changes the waypointn to the next waypoint. If curcularwaypoints set to true go through waypoints array in
	/// circular fashion
	/// </summary>
	private void ChangeWaypoint() {
		if (Vector2.Distance(_nextPos, Saw.localPosition) <= 0.1) {

			if (_goingUp) {
				if (posCounter < LocalWayPoints.Length - 1) {
					posCounter++;
					
					
				}
				else {

					if (CircularWaypoints) {
						posCounter = 0;
					}
					else {
						_goingUp = false;

					}

				}
			}
			else {
				if (posCounter > 0) {
					posCounter--;

				}
				else {

					_goingUp = true;
				}
			}

			_nextPos = LocalWayPoints[posCounter];

		}
	}
	/*
	private Vector3 CalculateSawMoveSawment() {
		int nextWaypointIndex = fromWaypointIndex + 1;
		float distance = Vector3.Distance(GlobalWaypoints[fromWaypointIndex], GlobalWaypoints[nextWaypointIndex]);
		percentBetweenWaypoints += Time.deltaTime * Speed/distance;

		Vector3 newPos = Vector3.Lerp(GlobalWaypoints[fromWaypointIndex], GlobalWaypoints[nextWaypointIndex],
			percentBetweenWaypoints);

		if (percentBetweenWaypoints >= 1) {
			percentBetweenWaypoints = 0;
			fromWaypointIndex++;
			if (fromWaypointIndex >=GlobalWaypoints.Length-1) {
				fromWaypointIndex = 0;
				System.Array.Reverse(GlobalWaypoints);
			}
		}

		return newPos - Saw.transform.position;


	}*/
	

	
#if UNITY_EDITOR

	/// <summary>
	/// To make visualization of waypoints easier. turndrawgizmos off in 
	/// </summary>
	private void OnDrawGizmos() {

		getscale = transform.localScale.x;
		if (DebugWaypoints && LocalWayPoints != null) {
			Gizmos.color = Color.green;
			float size = .3f;

			for (int i = 0; i < LocalWayPoints.Length; i++) {
				Vector3 globalWaypointPos = LocalWayPoints[i]*getscale+ transform.position;
				Gizmos.DrawLine(globalWaypointPos-Vector3.up*size, globalWaypointPos+Vector3.up*size);
				Gizmos.DrawLine(globalWaypointPos-Vector3.right*size, globalWaypointPos+Vector3.right*size);

				if (PositionInfo) {
					Handles.Label(LocalWayPoints[i]*getscale + transform.position,
						"Element " + i + "\nLocalCoordinates " + LocalWayPoints[i]);
				}

			}
			
		}
	}
#endif
}
