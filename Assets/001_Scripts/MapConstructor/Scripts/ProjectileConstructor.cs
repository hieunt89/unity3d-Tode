using UnityEngine;

[System.Serializable]
public class ProjectileConstructor : MonoBehaviour {
	
	[SerializeField] private ProjectileData projectile;

	public ProjectileData Projectile {
		get {
			return projectile;
		}
		set {
			projectile = value;
		}
	}
}
