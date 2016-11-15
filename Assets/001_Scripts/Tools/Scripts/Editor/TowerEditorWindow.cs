using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using System;

public class TowerEditorWindow : EditorWindow {
	static TowerEditorWindow towerEditorWindow;
	TowerData tower;
	GameObject towerGo;
	bool toggleProjectile;
	bool toggleSkillTrees;
	List<TowerData> existTowers;
	List<ProjectileData> existProjectiles;

	List<Tree<string>> trees;

	List<string> existSkillTreeIDs;
	List<int> selectedIndexes;
//	List<string> existTowerIds;
	List<string> projectileIds;

	int projectileIndex;

	IDataUtils dataUtils;
	IPrefabUtils prefabUtils;
	IDataUtils binartyUtils;

	[MenuItem("Tode/Tower Editor &T")]
	public static void ShowWindow()
	{
		towerEditorWindow = EditorWindow.GetWindow <TowerEditorWindow> ("Tower Editor", true);
		towerEditorWindow.minSize = new Vector2 (400, 600);
	}

	void OnEnable () {
		prefabUtils = DIContainer.GetModule <IPrefabUtils> ();
		dataUtils = DIContainer.GetModule <IDataUtils> ();
		binartyUtils = new BinaryUtils () as IDataUtils;

		existTowers = dataUtils.LoadAllData <TowerData>();
		existProjectiles = dataUtils.LoadAllData <ProjectileData>();

		trees = binartyUtils.LoadAllData <Tree<string>> ();
		existSkillTreeIDs = new List<string> ();
		selectedIndexes = new List<int> ();

		for (int i = 0; i < trees.Count; i++) {
			if (trees[i].treeType == TreeType.CombatSkills || trees[i].treeType == TreeType.SummonSkills ) 
				existSkillTreeIDs.Add(trees[i].id);
		}

		tower = new TowerData("tower" + existTowers.Count);

	}

	void OnFocus () {
		existTowers = dataUtils.LoadAllData <TowerData>();
		existProjectiles =  dataUtils.LoadAllData <ProjectileData>();

		if (existProjectiles.Count > 0) {
			projectileIds = new List<string> ();
			for (int i = 0; i < existProjectiles.Count; i++) {
				projectileIds.Add(existProjectiles[i].Id);
			}
		}
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
		SceneView.onSceneGUIDelegate += this.OnSceneGUI;
	}

	void OnDestroy () {
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
	}


	void OnGUI()
	{
		//		EditorGUI.BeginChangeCheck ();

		tower.Id = EditorGUILayout.TextField ("Id", tower.Id);
		tower.Name = EditorGUILayout.TextField ("Name", tower.Name);

		towerGo = (GameObject) EditorGUILayout.ObjectField ("Tower GO", towerGo, typeof(GameObject), true);
		GUI.enabled = towerGo == null && tower.Id.Length > 0;
			if (GUILayout.Button ("Create Tower GO")) {
				towerGo = new GameObject (tower.Id);
			}
		GUI.enabled = true;

		projectileIndex = EditorGUILayout.Popup ("Projectile", projectileIndex, projectileIds.ToArray());
		tower.ProjectileIndex = projectileIndex;
		tower.ProjectileId = projectileIds[projectileIndex];

		GUILayout.BeginVertical ("box");
		toggleProjectile = EditorGUILayout.Foldout (toggleProjectile, "Projectile Detail");
		if (toggleProjectile) {
			EditorGUILayout.LabelField ("Name", existProjectiles[projectileIndex].Name);
			EditorGUILayout.LabelField ("Type", existProjectiles[projectileIndex].Type.ToString ());
			EditorGUILayout.LabelField ("TravelSpeed", existProjectiles[projectileIndex].TravelSpeed.ToString ());
			EditorGUILayout.LabelField ("Duration", existProjectiles[projectileIndex].Duration.ToString ());
			EditorGUILayout.LabelField ("MaxDmgBuildTime", existProjectiles[projectileIndex].MaxDmgBuildTime.ToString ());
			EditorGUILayout.LabelField ("TickInterval", existProjectiles[projectileIndex].TickInterval.ToString ());
		}
		GUILayout.EndVertical ();

		tower.AtkType =  (AttackType) EditorGUILayout.EnumPopup ("Attack Type", tower.AtkType);
		tower.AtkRange = EditorGUILayout.FloatField ("Tower Range",tower.AtkRange);
		tower.MinDmg = EditorGUILayout.IntField ("Min Damage", tower.MinDmg);
		tower.MaxDmg = EditorGUILayout.IntField ("Max Damage", tower.MaxDmg);
		tower.AtkSpeed = EditorGUILayout.FloatField ("Attack Speed", tower.AtkSpeed);
		tower.AtkTime = EditorGUILayout.FloatField ("Attack Time", tower.AtkTime);
		tower.TurnSpeed = EditorGUILayout.FloatField ("Turn Speed", tower.TurnSpeed);
		tower.AtkPoint = EditorGUILayout.Vector3Field ("Attack Point", tower.AtkPoint);
		tower.GoldRequired = EditorGUILayout.IntField ("Gold Cost", tower.GoldRequired);
		tower.BuildTime = EditorGUILayout.FloatField ("Build Time", tower.BuildTime);
		tower.Aoe = EditorGUILayout.FloatField ("AOE", tower.Aoe);

		toggleSkillTrees = EditorGUILayout.Foldout (toggleSkillTrees, "Skill Trees");
		if (toggleSkillTrees) {
			for (int skillTreeIndex = 0; skillTreeIndex < tower.TreeSkillNames.Count; skillTreeIndex++) {
				GUILayout.BeginHorizontal ();
				selectedIndexes[skillTreeIndex] = EditorGUILayout.Popup (selectedIndexes[skillTreeIndex], existSkillTreeIDs.ToArray ());
				tower.TreeSkillNames[skillTreeIndex] = existSkillTreeIDs [selectedIndexes[skillTreeIndex]];
				if (GUILayout.Button ("Remove")) {
					tower.TreeSkillNames.RemoveAt (skillTreeIndex);
					selectedIndexes.RemoveAt (skillTreeIndex);
					continue;
				}
				GUILayout.EndHorizontal ();
			}

			if (GUILayout.Button ("Add Skill Tree")) {
				tower.TreeSkillNames.Add ("new");
				selectedIndexes.Add (0);
			}
		}

		GUILayout.Space(5);

//		GUILayout.BeginHorizontal ();
		GUI.enabled = CheckInputFields ();
		if (GUILayout.Button("Save")){
			tower.AtkPoint = towerGo.transform.InverseTransformPoint (tower.AtkPoint);

			dataUtils.CreateData (tower);
			prefabUtils.CreatePrefab (towerGo as GameObject);
		}
		GUI.enabled = true;

		if (GUILayout.Button("Load")){
			tower = dataUtils.LoadData <TowerData> ();
			if (tower == null) {
				tower = new TowerData ("tower" + existTowers.Count);
			} else {
				for (int i = 0; i < tower.TreeSkillNames.Count; i++) {
					if (tower.TreeSkillNames [i].Equals (existSkillTreeIDs [i]))
						selectedIndexes.Add (i);

					// TODO: loi se xay ra khi 2 list khac nhau ve so luong phan tu ...
				}
			}

			if (towerGo) {
				DestroyImmediate (towerGo);
			}

			towerGo = prefabUtils.InstantiatePrefab (ConstantString.PrefabPath + tower.Id + ".prefab");
			if (towerGo != null) {
				tower.AtkPoint = towerGo.transform.TransformPoint (tower.AtkPoint);
			}
			projectileIndex = tower.ProjectileIndex;
		}

		if (GUILayout.Button("Reset")){
			if (EditorUtility.DisplayDialog ("Are you sure?", 
				"Do you want to reset " + tower.Id + " data?",
				"Yes", "No")) {
				tower = new TowerData ("tower" + existTowers.Count);
				if (towerGo) {
					DestroyImmediate (towerGo);
				}
			}
		}

		if (GUILayout.Button("Delete")){
			if (EditorUtility.DisplayDialog ("Are you sure?", 
				"Do you want to delete " + tower.Id + " data?",
				"Yes", "No")) {
				if (towerGo) {
					DestroyImmediate (towerGo);
				}
				dataUtils.DeleteData (ConstantString.DataPath + tower.GetType().Name + "/" + tower.Id + ".json");
				prefabUtils.DeletePrefab (ConstantString.PrefabPath + tower.Id + ".prefab");
				tower = new TowerData ("tower" + existTowers.Count);
			}
		}
//		GUILayout.EndHorizontal();

		Repaint ();
	}

	public void OnSceneGUI (SceneView _sceneView){
		Handles.color = Color.red;
		tower.AtkPoint = Handles.FreeMoveHandle(tower.AtkPoint, Quaternion.identity, .1f, Vector3.one, Handles.SphereCap);
	}

	private bool CheckInputFields () {
		var nameInput = !String.IsNullOrEmpty (tower.Name);

		return towerGo && nameInput;
	}
}
