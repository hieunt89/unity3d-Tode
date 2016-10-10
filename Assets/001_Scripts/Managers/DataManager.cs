using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

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
	Dictionary<string, CharacterData> characterIdToData;
	Dictionary<string, MapData> mapIdToData;
	Dictionary<string, Skill> skillIdToData;
	List<Tree<string>> towerTrees;

	public DataManager(){
		LoadData <ProjectileData>(out projectileIdToData);
		LoadData <TowerData> (out towerIdToData);
		LoadData <CharacterData> (out characterIdToData);
//		LoadData <MapData> (out mapIdToData);
		LoadTreeData (TreeType.Towers);
		LoadSkillData ();
	}

	void LoadSkillData(){
		CombatSkill s1 = new CombatSkill ();
		s1.aoe = 1f;
		s1.cooldown = 2f;
		s1.range = 6f;
		s1.prjId = "projectile0";
		s1.id = "skill1";

		List<SkillEffect> efl = new List<SkillEffect> ();
		SkillEffect e1 = new SkillEffect ();
		e1.effect = Effect.HpReduce;
		e1.duration = 0f;
		e1.value = 100;
		efl.Add (e1);

		s1.effectList = efl;

		skillIdToData = new Dictionary<string, Skill> ();
		skillIdToData.Add (s1.id, s1);
	}

//	public T GetSkill<T> (string id) where T : Skill{
//		if (skillIdToData.ContainsKey (id)) {
//			var skill = skillIdToData [id];
//			if (skill is CombatSkill) {
//				return skill as CombatSkill;
//			} else if (skill is SummonSkill) {
//				return skill as SummonSkill;
//			}
//		} 
//		return null;
//	}

	void LoadTreeData(TreeType treeType){
		towerTrees = new List<Tree<string>> ();
		BinaryFormatter bf = new BinaryFormatter ();

		var texts = Resources.LoadAll<TextAsset> ("Tree/" + treeType.ToString ());
		for (int i = 0; i < texts.Length; i++) {
			Stream s = new MemoryStream(texts[i].bytes);
			Tree<string> tree = bf.Deserialize (s) as Tree<string>;
			towerTrees.Add (tree);
		}
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

	public MapData GetMapData(string id){
		if (mapIdToData.ContainsKey (id)) {
			return mapIdToData [id];	
		} else {
			return null;
		}
	}

	public MapData LoadMapData(string id){
		return LoadDataById<MapData> (id);
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

	public CharacterData GetCharacterData(string id){
		if (characterIdToData.ContainsKey (id)) {
			return characterIdToData [id];
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
		TextAsset file = Resources.Load <TextAsset> (path) as TextAsset;
		return (T)JsonUtility.FromJson (file.text, typeof(T));
	
	}

	#endregion json data

}
