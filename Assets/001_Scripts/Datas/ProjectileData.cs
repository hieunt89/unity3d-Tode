using UnityEngine;
using System.Collections;

public class ProjectileData {
	[SerializeField] public float travelSpeed;
	[SerializeField] public float range;

	public ProjectileData (float travelSpeed, float range)
	{
		this.travelSpeed = travelSpeed;
		this.range = range;
	}
	
}
