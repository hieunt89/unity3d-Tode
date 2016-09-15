using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataManager {
	#region Singleton
	private static DataManager instance = null;
	public static DataManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new DataManager();
			}
			return instance;
		}
	}
	#endregion

	Dictionary<string, ProjectileData> projectileIdToData;
	Dictionary<string, TowerData> towerIdToData;
	Dictionary<string, EnemyData> enemyIdToData;

	public DataManager(){
		LoadProjectileData ();
		LoadTowerData ();
		LoadEnemyData ();
	}

	void LoadProjectileData(){
		projectileIdToData = new Dictionary<string, ProjectileData> ();
		projectileIdToData.Add("prj1", new ProjectileData("prj1", ProjectileType.homing, 1.0f, 5.0f, 0f));
	}

	void LoadTowerData(){
		List<string> nextUpgrade = new List<string> ();
		nextUpgrade.Add ("tower2");

		towerIdToData = new Dictionary<string, TowerData> ();
		towerIdToData.Add (
			"tower1", 
			new TowerData ("tower1", "arrow 1 name", "prj1", AttackType.physical, 2f, 1, 2, 1f, 150, nextUpgrade, 1f)
		);
		towerIdToData.Add (
			"tower2", 
			new TowerData ("tower2", "arrow 2 name", "prj1", AttackType.physical, 2f, 1, 2, 1f, 150, null, 3f)
		);

	}

	void LoadEnemyData(){
		List<ArmorData> armorList = new List<ArmorData> ();
		armorList.Add (new ArmorData (AttackType.magical, ArmorRating.none));
		armorList.Add (new ArmorData (AttackType.physical, ArmorRating.high));

		enemyIdToData = new Dictionary<string, EnemyData> ();
		enemyIdToData.Add (
			"enemy1",
			new EnemyData("enemy1", "enemy 1 name", 0.5f, 5f, 1, 5, AttackType.physical, 2f, 1, 2, 0f, armorList, 5)
		);
		enemyIdToData.Add (
			"enemy2",
			new EnemyData("enemy2", "enemy 2 name", 0.5f, 5f, 1, 5, AttackType.physical, 2f, 1, 2, 1f, armorList, 3)
		);
	}

	public TowerData GetTowerData(string id){
		if (towerIdToData.ContainsKey (id)) {
			return towerIdToData [id];	
		} else {
			return null;
		}
	}

	public ProjectileData GetProjectileData(string id){
		if (projectileIdToData.ContainsKey (id)) {
			return projectileIdToData [id];
		} else {
			return null;
		}
	}

	public EnemyData GetEnemyData(string id){
		if (enemyIdToData.ContainsKey (id)) {
			return enemyIdToData [id];
		} else {
			return null;
		}
	}

	public float GetArmorReduction(ArmorRating rating){
		return (int)rating * 0.01f;
	}
}
