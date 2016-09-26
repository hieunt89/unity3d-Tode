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
	Dictionary<string, MapData> mapIdToData;
	List<Tree<string>> towerTrees;

	public DataManager(){
		LoadData <ProjectileData>(out projectileIdToData);
		LoadData <TowerData> (out towerIdToData);
		LoadData <EnemyData> (out enemyIdToData);
		LoadData <MapData> (out mapIdToData);
		LoadTowerTreeData ();
	}

	void LoadTowerTreeData(){
		towerTrees = new List<Tree<string>> ();

		Tree<string> t1 =  new Tree<string> ("tower0");
		t1.Root.AddChild ("tower1");
		t1.Root.Children [0].AddChild ("tower0");
		towerTrees.Add (t1);
	}

	void LoadData <T> (out Dictionary<string, T> d){
		d = new Dictionary<string, T> ();

		List<T> datas = LoadAllData<T> ();

		foreach (T data in datas) {
			FieldInfo field = typeof(T).GetField("id");
			string id = (string) field.GetValue(data);
			d.Add (id, data);
		}
	}

	public List<Node<string>> GetTowerRoots(){
		List <Node<string>> list = new List<Node<string>> ();
		for (int i = 0; i < towerTrees.Count; i++) {
			list.Add (towerTrees[i].Root);
		}
		return list;
	}

//	public Tree

	public MapData GetMapData(string id){
		if (mapIdToData.ContainsKey (id)) {
			return mapIdToData [id];	
		} else {
			return null;
		}
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

	#region json data
	const string dataDirectory = "Assets/Resources/Data/";

	public void SaveData<T> (T data) {

		var jsonString = JsonUtility.ToJson(data);
		Debug.Log(jsonString);

		FieldInfo field = typeof(T).GetField("id");
		string id = (string) field.GetValue(data);
		Debug.Log (dataDirectory + data.ToString());

		File.WriteAllText (dataDirectory + "/" + data.ToString () + "/"  + id + ".json", jsonString);

//		var path = EditorUtility.SaveFilePanel("Save " + data.ToString(), dataDirectory + data.ToString(), id +".json", "json");

//		if (!string.IsNullOrEmpty(path))
//		{
//			using (FileStream fs = new FileStream (path, FileMode.Create)) {
//				using (StreamWriter writer = new StreamWriter(fs)) {
//					writer.Write(jsonString);
//				}
//			}
//		}
		AssetDatabase.Refresh();
	}

	public T LoadData<T> () {
		var path = EditorUtility.OpenFilePanel("Load " +  typeof(T).ToString(), dataDirectory +  typeof(T).ToString(), "json");

		var reader = new WWW("file:///" + path);
		while(!reader.isDone){
		}

		return (T) JsonUtility.FromJson (reader.text, typeof(T));
	}

	public List<T> LoadAllData <T> () {
		var list = new List<T> ();
		var path = "Data/" + typeof(T).ToString();
		TextAsset[] files = Resources.LoadAll <TextAsset> (path) as TextAsset[];
		if (files != null && files.Length > 0) {
			for (int i = 0; i < files.Length; i++) {
				T datum = (T)JsonUtility.FromJson (files [i].text, typeof(T));
				list.Add (datum);
			}
		}
		return list;
	}

	public T LoadDataById <T> (string id) {
		var path = "Data/" + typeof(T).ToString() + "/" + id;
		Debug.Log (path);
		TextAsset file = Resources.Load <TextAsset> (path) as TextAsset;
		Debug.Log (file.text);
		return (T)JsonUtility.FromJson (file.text, typeof(T));
	
	}
	#endregion json data

}
