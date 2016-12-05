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

	IDataUtils gameAssetUtils;
	IDataUtils binartyUtils;
	IDataUtils jsonUtils;

	Dictionary<string, ProjectileData> projectileIdToData;
	Dictionary<string, TowerData> towerIdToData;
	Dictionary<string, CharacterData> characterIdToData;
	Dictionary<string, MapData> mapIdToData;
	Dictionary<string, SkillData> skillIdToData;

	List<Tree<string>> trees;
	List<Tree<string>> towerTrees;
	Dictionary<string, Tree<string>> skillTrees;

	public DataManager(){
		gameAssetUtils = DIContainer.GetModule <IDataUtils> ();
		binartyUtils = new GameData () as IDataUtils;
		jsonUtils = new GameData () as IDataUtils; 

		LoadData <ProjectileData>(out projectileIdToData);
		LoadData <TowerData> (out towerIdToData);
		LoadData <CharacterData> (out characterIdToData);
		LoadData <MapData> (out mapIdToData);
		LoadSkillData ();

		trees = binartyUtils.LoadAllData <Tree<string>> ();
		LoadTowerTreeData ();
		LoadSkillTreeData ();
	}

	void LoadSkillData(){
		skillIdToData = new Dictionary<string, SkillData> ();

		var combatSkills = jsonUtils.LoadAllData<CombatSkillData> ();
		for (int i = 0; i < combatSkills.Count; i++) {
			skillIdToData.Add (combatSkills[i].id, combatSkills[i]);
		}

		var summonSkills = jsonUtils.LoadAllData<SummonSkillData> ();
		for (int i = 0; i < summonSkills.Count; i++) {
			skillIdToData.Add (summonSkills[i].id, summonSkills[i]);
		}
	}

	void LoadTowerTreeData(){

		towerTrees = new List<Tree<string>> ();
		for (int i = 0; i < trees.Count; i++) {
			if (trees[i].treeType == TreeType.Towers) 
				towerTrees.Add(trees[i]);
		}
	}

	void LoadSkillTreeData(){
		skillTrees = new Dictionary<string, Tree<string>> ();

		for (int i = 0; i < trees.Count; i++) {
			if (trees[i].treeType == TreeType.CombatSkills || trees[i].treeType == TreeType.SummonSkills ) 
				skillTrees.Add(trees[i].id, trees[i]);
		}
	}

	void LoadData <T> (out Dictionary<string, T> d){
		d = new Dictionary<string, T> ();

		List<T> datas = gameAssetUtils.LoadAllData<T> ();

//		foreach (T data in datas) {
//			FieldInfo field = typeof(T).GetField("intId");
//			string id = field.GetValue(data).ToString ();
//			d.Add (id, data);
//		}
	}

	#region GetData
	public List<Tree<string>> GetSkillTrees(List<string> names){
		List<Tree<string>> listTree = new List<Tree<string>> ();
		for (int i = 0; i < names.Count; i++) {
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
