using UnityEngine;
using System.Collections;

[System.Serializable]
public class ProjectileData {
	[SerializeField] private string id;
	[SerializeField] private ProjectileType type;
	[SerializeField] private float travelSpeed;
	[SerializeField] private float turnSpeed;
	[SerializeField] private float travelTime;
	[SerializeField] private float range;

	public string Id {
		get {
			return id;
		}
		set {
			id = value;
		}
	}

	public ProjectileType Type {
		get {
			return type;
		}
		set {
			type = value;
		}
	}

	public float TravelSpeed {
		get {
			return travelSpeed;
		}
		set {
			travelSpeed = value;
		}
	}

	public float TravelTime {
		get {
			return this.travelTime;
		}
		set {
			travelTime = value;
		}
	}

	public float TurnSpeed {
		get {
			return turnSpeed;
		}
		set {
			turnSpeed = value;
		}
	}
		
	public float Range {
		get {
			return range;
		}
		set {
			range = value;
		}
	}

	public ProjectileData () {

	}

	public ProjectileData (string id, ProjectileType type, float travelSpeed, float turnSpeed, float travelTime, float range)
	{
		this.id = id;
		this.type = type;
		this.travelSpeed = travelSpeed;
		this.turnSpeed = turnSpeed;
		this.travelTime = travelTime;
		this.range = range;
	}
	
}
