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
		LoadProjectileDatas ();
		LoadTowerDatas ();
		LoadEnemyDatas ();
	}

	void LoadProjectileDatas(){
		projectileIdToData = new Dictionary<string, ProjectileData> ();
		projectileIdToData.Add("arrow", new ProjectileData(3.0f));
	}

	void LoadTowerDatas(){
		towerIdToData = new Dictionary<string, TowerData> ();
		towerIdToData.Add (
			"arrow1", 
			new TowerData ("arrow", AttackType.type1, 2f, 1, 2, 2f)
		);
	}

	void LoadEnemyDatas(){
		enemyIdToData = new Dictionary<string, EnemyData> ();
		enemyIdToData.Add (
			"enemy1",
			new EnemyData(1f, 1, AttackType.type1, 2f, 1, 2, 0f, ArmorType.type1, 1, 5)
		);
		enemyIdToData.Add (
			"enemy2",
			new EnemyData(1.5f, 1, AttackType.type1, 2f, 1, 2, 1f, ArmorType.type1, 1, 3)
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
}
