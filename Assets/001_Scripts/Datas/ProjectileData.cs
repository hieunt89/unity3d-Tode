using UnityEngine;
using System.Collections;

public class ProjectileData {
	[SerializeField] public float travelSpeed;
	public ProjectileData (float travelSpeed)
	{
		this.travelSpeed = travelSpeed;
	}
}
