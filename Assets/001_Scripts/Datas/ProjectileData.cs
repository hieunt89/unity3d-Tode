using UnityEngine;
using System.Collections;

[System.Serializable]
public class ProjectileData {
	[SerializeField] private string id;

	public string Id {
		get {
			return id;
		}
		set {
			id = value;
		}
	}

	[SerializeField] private float travelSpeed;

	public float TravelSpeed {
		get {
			return travelSpeed;
		}
		set {
			travelSpeed = value;
		}
	}

	[SerializeField] private float turnSpeed;

	public float TurnSpeed {
		get {
			return turnSpeed;
		}
		set {
			turnSpeed = value;
		}
	}

	[SerializeField] private float range;

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

	public ProjectileData (string id, float travelSpeed, float turnSpeed, float range)
	{
		this.id = id;
		this.travelSpeed = travelSpeed;
		this.turnSpeed = turnSpeed;
		this.range = range;
	}
	
}
