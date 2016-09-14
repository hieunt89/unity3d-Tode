using UnityEngine;
using System.Collections;

public class ProjectileData {
	[SerializeField] public string id;
	[SerializeField] public float travelSpeed;
	[SerializeField] public float turnSpeed;
	[SerializeField] public float range;

	public ProjectileData (string id, float travelSpeed, float turnSpeed, float range)
	{
		this.id = id;
		this.travelSpeed = travelSpeed;
		this.turnSpeed = turnSpeed;
		this.range = range;
	}
	
}
