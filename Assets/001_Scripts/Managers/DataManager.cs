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

	IDataUtils dataUtils;

	Dictionary<string, ProjectileData> projectileIdToData;
	Dictionary<string, TowerData> towerIdToData;
	Dictionary<string, CharacterData> characterIdToData;
	Dictionary<string, MapData> mapIdToData;
	Dictionary<string, SkillData> skillIdToData;
	List<Tree<string>> towerTrees;
	Dictionary<string, Tree<string>> skillTrees;

	public DataManager(){
		dataUtils = DIContainer.GetModule <IDataUtils> ();

		LoadData <ProjectileData>(out projectileIdToData);
		LoadData <TowerData> (out towerIdToData);
		LoadData <CharacterData> (out characterIdToData);
		LoadData <MapData> (out mapIdToData);
		LoadTreeData (TreeType.Towers);

		LoadSkillData ();
		LoadSkillTree ();
	}

	void LoadSkillData(){
		skillIdToData = new Dictionary<string, SkillData> ();

		var combatSkills = dataUtils.LoadAllData<CombatSkillData> ();
		for (int i = 0; i < combatSkills.Count; i++) {
			skillIdToData.Add (combatSkills[i].id, combatSkills[i]);
		}

		var summonSkills = dataUtils.LoadAllData<SummonSkillData> ();
		for (int i = 0; i < summonSkills.Count; i++) {
			skillIdToData.Add (summonSkills[i].id, summonSkills[i]);
		}
	}

	void LoadSkillTree(){
		skillTrees = new Dictionary<string, Tree<string>> ();

		Tree<string> tree = new Tree<string> ();
		tree.treeName = "fireball_tree";
		tree.Root = new Node<string> ("skill0");
		tree.Root.AddChild (new Node<string> ("skill1"));

		skillTrees.Add (tree.treeName, tree);
	}

	void LoadTreeData(TreeType treeType){
		towerTrees = new List<Tree<string>> ();
		BinaryFormatter bf = new BinaryFormatter ();

		var texts = Resources.LoadAll<TextAsset> ("Data/Trees/" + treeType.ToString ());
		for (int i = 0; i < texts.Length; i++) {
			Stream s = new MemoryStream(texts[i].bytes);
			Tree<string> tree = bf.Deserialize (s) as Tree<string>;
			towerTrees.Add (tree);
		}
	}

	void LoadData <T> (out Dictionary<string, T> d){
		d = new Dictionary<string, T> ();

		List<T> datas = dataUtils.LoadAllData<T> ();

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

}
