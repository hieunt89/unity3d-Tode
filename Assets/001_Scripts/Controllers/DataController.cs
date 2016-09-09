using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataController {
	private static DataController instance = null;
	public static DataController Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new DataController();
			}
			return instance;
		}
	}

	Dictionary<string, ProjectileData> projectileIdToData;
	Dictionary<string, TowerData> towerIdToData;

	public DataController(){
		LoadProjectileDatas ();
		LoadTowerDatas ();
	}

	void LoadProjectileDatas(){
		projectileIdToData = new Dictionary<string, ProjectileData> ();
		projectileIdToData.Add("arrow", new ProjectileData(3.0f));
	}

	void LoadTowerDatas(){
		towerIdToData = new Dictionary<string, TowerData> ();
		towerIdToData.Add ("arrow1", 
			new TowerData (
				"arrow",
				AttackType.type1,
				2f,
				1,
				2,
				2f
			)
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
}
