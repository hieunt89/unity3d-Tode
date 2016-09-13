using UnityEngine;
using System.Collections;

public class ProjectileData {
	[SerializeField] public float travelSpeed;
	[SerializeField] public float turnSpeed;
	[SerializeField] public float range;

	public ProjectileData (float travelSpeed, float turnSpeed, float range)
	{
		this.travelSpeed = travelSpeed;
		this.turnSpeed = turnSpeed;
		this.range = range;
	}
	
}
