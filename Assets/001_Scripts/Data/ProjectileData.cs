using UnityEngine;
using System.Collections;

[System.Serializable]
public class ProjectileData {
	[SerializeField] public string id;
	[SerializeField] public int intId;

	[SerializeField] private string name;
	[SerializeField] private GameObject view;

	[SerializeField] private ProjectileType type;

	[SerializeField] private float travelSpeed;
	[SerializeField] private float duration;

	[SerializeField] private float maxDmgBuildTime;
	[SerializeField] private float tickInterval;

	public float MaxDmgBuildTime {
		get {
			return maxDmgBuildTime;
		}set {
			maxDmgBuildTime = value;
		}
	}

	public float TickInterval {
		get {
			return tickInterval;
		}set {
			tickInterval = value;
		}
	}

	public string Id {
		get {
			return id;
		}
		set {
			id = value;
		}
	}

	public string Name {
		get {
			return this.name;
		}
		set {
			name = value;
		}
	}

	public GameObject View {
		get {
			return this.view;
		}
		set {
			view = value;
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

	public float Duration {
		get {
			return this.duration;
		}
		set {
			duration = value;
		}
	}
		
	public ProjectileData ()
	{
	}
	
	public ProjectileData (string id) {
		this.id = id;
	}

//	public ProjectileData (string id, ProjectileType type, float travelSpeed, float duration)
//	{
//		this.id = id;
//		this.type = type;
//		this.travelSpeed = travelSpeed;
//		this.duration = duration;
//	}
}
