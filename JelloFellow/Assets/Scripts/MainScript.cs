using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
	[CustomLabel("Gravity Force")] [Tooltip("Force at which gravity is applied to objects.")] [SerializeField]
	private float gravity_force = -9.81f;
	
	private void Awake()
	{
		if (!GameObject.FindGameObjectWithTag("Main"))
		{
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
	
	public float GravityForce()
	{
		return gravity_force;
	}
}
