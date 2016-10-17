using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

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
	Dictionary<string, SkillData> skillIdToData;
	List<Tree<string>> towerTrees;
	Dictionary<string, Tree<string>> skillTrees;

	public DataManager(){
		LoadData <ProjectileData>(out projectileIdToData);
		LoadData <TowerData> (out towerIdToData);
		LoadData <CharacterData> (out characterIdToData);
		LoadData <MapData> (out mapIdToData);
		LoadTreeData (TreeType.Towers);
		LoadSkillData ();
		LoadSkillTree ();
	}

	void LoadSkillData(){
		CombatSkillData s = new CombatSkillData ();
		s.id = "fb1";
		s.name = "fireball level 1";
		s.castRange = 6f;
		s.castTime = 2f;
		s.cooldown = 3f;
		s.expToNextLvl = 50;
		s.attackType = AttackType.magical;
		s.damage = 200;
		s.cost = 100;

		s.aoe = 2f;
		s.projectileId = "projectile1";

		List<SkillEffect> efl = new List<SkillEffect> ();
		SkillEffect ef = new SkillEffect ();
		ef.skillId = s.id;
		ef.duration = 5f;
		ef.effect = Effect.MoveSpeedSlow;
		ef.value = 50;

		efl.Add (ef);

		s.effectList = efl;

		skillIdToData = new Dictionary<string, SkillData> ();
		skillIdToData.Add (s.id, s);
	}

	void LoadSkillTree(){
		skillTrees = new Dictionary<string, Tree<string>> ();

		Tree<string> tree = new Tree<string> ();
		tree.treeName = "fireball_tree";
		tree.Root = new Node<string> ("fb1");

		skillTrees.Add (tree.treeName, tree);
	}

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
		
	#region GetData
	public List<Tree<string>> GetSkillTrees(params string[] names){
		List<Tree<string>> listTree = new List<Tree<string>> ();
		for (int i = 0; i < names.Length; i++) {
			var tree = GetSkillTree (names[i]);
			if (tree != null) {
				listTree.Add (tree);
			}
		}
		return listTree;
	}

	public Tree<string> GetSkillTree(string name){
		if (skillTrees.ContainsKey (name)) {
			return skillTrees [name];
		} else {
			return null;
		}
	}

	public SkillData GetSkillData(string id){
		if (skillIdToData.ContainsKey (id)) {
			return skillIdToData [id];
		} else {
			return null;
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

	public MapData GetMapData(int index){
		if (mapIdToData.Count > index) {
			return mapIdToData.Values.ElementAt(index);
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

	public CharacterData GetCharacterData(string id){
		if (characterIdToData.ContainsKey (id)) {
			return characterIdToData [id];
		} else {
			return null;
		}
	}
	#endregion

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
