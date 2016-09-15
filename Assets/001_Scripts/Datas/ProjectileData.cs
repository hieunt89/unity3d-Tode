using UnityEngine;
using System.Collections;

public class ProjectileData {
	[SerializeField] public string id;
	[SerializeField] public ProjectileType type;
	[SerializeField] public float travelSpeed;
	[SerializeField] public float turnSpeed;
	[SerializeField] public float range;

	public ProjectileData (string id, ProjectileType type, float travelSpeed, float turnSpeed, float range)
	{
		this.id = id;
		this.type = type;
		this.travelSpeed = travelSpeed;
		this.turnSpeed = turnSpeed;
		this.range = range;
	}
	
}
