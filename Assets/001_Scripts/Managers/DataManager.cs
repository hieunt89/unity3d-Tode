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
	Dictionary<ArmorRating, float> armorRatingToReduction;

	public DataManager(){
		LoadProjectileData ();
		LoadTowerData ();
		LoadEnemyData ();
		LoadArmorRatingRef ();
	}

	void LoadArmorRatingRef(){
		armorRatingToReduction = new Dictionary<ArmorRating, float> ();
		armorRatingToReduction.Add (ArmorRating.none, 0f);
		armorRatingToReduction.Add (ArmorRating.low, 0.1f);
		armorRatingToReduction.Add (ArmorRating.medium, 0.2f);
		armorRatingToReduction.Add (ArmorRating.high, 0.5f);
		armorRatingToReduction.Add (ArmorRating.immume, 1f);
	}

	void LoadProjectileData(){
		projectileIdToData = new Dictionary<string, ProjectileData> ();
		projectileIdToData.Add("arrow", new ProjectileData(3.0f, 0f));
	}

	void LoadTowerData(){
		towerIdToData = new Dictionary<string, TowerData> ();
		towerIdToData.Add (
			"arrow1", 
			new TowerData ("arrow", AttackType.physical, 2f, 1, 2, 1f)
		);
	}

	void LoadEnemyData(){
		List<ArmorData> armorList = new List<ArmorData> ();
		armorList.Add (new ArmorData (AttackType.magical, ArmorRating.none));
		armorList.Add (new ArmorData (AttackType.physical, ArmorRating.high));

		enemyIdToData = new Dictionary<string, EnemyData> ();
		enemyIdToData.Add (
			"enemy1",
			new EnemyData(1f, 1, AttackType.physical, 2f, 1, 2, 0f, armorList, 5)
		);
		enemyIdToData.Add (
			"enemy2",
			new EnemyData(1.5f, 1, AttackType.physical, 2f, 1, 2, 1f, armorList, 3)
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
		if (armorRatingToReduction.ContainsKey (rating)) {
			return armorRatingToReduction [rating];
		} else {
			return 0f;
		}
	}
}
