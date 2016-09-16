using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ProjectileConstructor : MonoBehaviour {
	
	[SerializeField] private List<ProjectileData> projectiles;

	public List<ProjectileData> Projectiles {
		get {
			return projectiles;
		}
		set {
			projectiles = value;
		}
	}
}
