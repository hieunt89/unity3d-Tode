using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "Inventory/List", order = 1)]
public class ProjectileList : ScriptableObject {
	public List<ProjectileData> projectiles;

}
