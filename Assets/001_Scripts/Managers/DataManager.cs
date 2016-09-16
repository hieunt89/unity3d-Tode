using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Reflection;

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
		projectileIdToData.Add("prj1", new ProjectileData("prj1", 1.0f, 5.0f, 0f));
	}

	void LoadTowerData(){
		List<string> nextUpgrade = new List<string> ();
		nextUpgrade.Add ("tower2");

		towerIdToData = new Dictionary<string, TowerData> ();
		towerIdToData.Add (
			"tower1", 
			new TowerData ("t1", "arrow 1 name", "prj1", AttackType.physical, 2f, 1, 2, 1f, 150, nextUpgrade, 1f)
		);
		towerIdToData.Add (
			"tower2", 
			new TowerData ("t2", "arrow 2 name", "prj1", AttackType.physical, 2f, 1, 2, 1f, 150, null, 3f)
		);

	}

	void LoadEnemyData(){
		List<ArmorData> armorList = new List<ArmorData> ();
		armorList.Add (new ArmorData (AttackType.magical, ArmorRating.none));
		armorList.Add (new ArmorData (AttackType.physical, ArmorRating.high));

		enemyIdToData = new Dictionary<string, EnemyData> ();
		enemyIdToData.Add (
			"enemy1",
			new EnemyData("e1", "enemy 1 name", 0.5f, 5f, 1, 5, AttackType.physical, 2f, 1, 2, 0f, armorList, 5)
		);
		enemyIdToData.Add (
			"enemy2",
			new EnemyData("e1", "enemy 2 name", 0.5f, 5f, 1, 5, AttackType.physical, 2f, 1, 2, 1f, armorList, 3)
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

	#region json data
	const string dataDirectory = "Assets/Resources/Data/";

//	public void SaveMapData (MapData mapData) {
//
//		var jsonString = JsonUtility.ToJson(mapData);
//		Debug.Log(jsonString);
//		var path = EditorUtility.SaveFilePanel("Save Map Data", dataDirectory , mapData.Id +".json", "json");
//
//		if (!string.IsNullOrEmpty(path))
//		{
//			using (FileStream fs = new FileStream (path, FileMode.Create)) {
//				using (StreamWriter writer = new StreamWriter(fs)) {
//					writer.Write(jsonString);
//				}
//			}
//		}
//		AssetDatabase.Refresh();
//	}

//	public void LoadMapData (MapData mapData) {
//		var path = EditorUtility.OpenFilePanel("Load Map Data", dataDirectory, "json");
//
//		var reader = new WWW("file:///" + path);
//		while(!reader.isDone){
//		}
//		Debug.Log(reader.text);
//		JsonUtility.FromJsonOverwrite (reader.text, mapData);
//	}

	public void SaveMapData<T> (T data) {

		var jsonString = JsonUtility.ToJson(data);
		Debug.Log(jsonString);

		FieldInfo field = typeof(T).GetField("id");
		string id = (string) field.GetValue(data);
		Debug.Log (dataDirectory + data.ToString());
		var path = EditorUtility.SaveFilePanel("Save " + data.ToString(), dataDirectory + data.ToString(), id +".json", "json");

		if (!string.IsNullOrEmpty(path))
		{
			using (FileStream fs = new FileStream (path, FileMode.Create)) {
				using (StreamWriter writer = new StreamWriter(fs)) {
					writer.Write(jsonString);
				}
			}
		}
		AssetDatabase.Refresh();
	}

	public void LoadMapData<T> (T data) {
		var path = EditorUtility.OpenFilePanel("Load " + data.ToString(), dataDirectory + data.ToString(), "json");

		var reader = new WWW("file:///" + path);
		while(!reader.isDone){
		}

		JsonUtility.FromJsonOverwrite (reader.text, data);
	}
	#endregion json data

	#region test scriptable object
	string databasePath = "Assets/Resources/Maps";

	private MapData LoadMap () {
		MapData currentMapData = (MapData) AssetDatabase.LoadAssetAtPath (databasePath, typeof(MapData));
		if (currentMapData != null) {
			return currentMapData;
		} 
		return CreateMap();
	}

	private MapData CreateMap () {
		MapData currentMapData = (MapData) ScriptableObject.CreateInstance(typeof(MapData));
		if (currentMapData != null) {
			AssetDatabase.CreateAsset(currentMapData, databasePath);
			AssetDatabase.Refresh();
			AssetDatabase.SaveAssets();
			Debug.Log ("Create map ...");
			return currentMapData;
		} 
		return null;
	}
	#endregion

}
