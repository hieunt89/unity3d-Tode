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
	int projectileIndex;

	List<Tree<string>> existTrees;

	List<string> existSkillTreeIDs;
	List<int> skillTreeIndexes;
	List<string> projectileIds;

	IDataUtils dataUtils;
	IPrefabUtils prefabUtils;
	IDataUtils binartyUtils;

	[MenuItem("Tode/Tower Editor &T")]
	public static void ShowWindow()
	{
		towerEditorWindow = EditorWindow.GetWindow <TowerEditorWindow> ("Tower Editor", true);
		towerEditorWindow.minSize = new Vector2 (400, 600);
	}

	void LoadExistData () {
		existTowers = dataUtils.LoadAllData <TowerData>();
		existProjectiles = dataUtils.LoadAllData <ProjectileData>();
		existTrees = binartyUtils.LoadAllData <Tree<string>> ();
	}

	void SetupProjectileIndex () {
		if (existProjectiles.Count > 0) {
			projectileIds = new List<string> ();
			for (int i = 0; i < existProjectiles.Count; i++) {
				projectileIds.Add(existProjectiles[i].Id);
			}

				for (int j = 0; j < projectileIds.Count; j++) {
					if (tower.ProjectileId.Equals (projectileIds[j])) {
						projectileIndex = j;
						continue;
					}
					projectileIndex = 0;
				}
				
		}
	}

	void SetupSkillTreeIndexes () {
		
		existSkillTreeIDs = new List<string> ();
		for (int i = 0; i < existTrees.Count; i++) {
			if (existTrees[i].treeType == TreeType.CombatSkills || existTrees[i].treeType == TreeType.SummonSkills ) 
				existSkillTreeIDs.Add(existTrees[i].id);
		}
		skillTreeIndexes.Clear();
		if (existSkillTreeIDs.Count > 0) {
			for (int i = 0; i < tower.TreeSkillNames.Count; i++) {
				skillTreeIndexes.Add (GetSkillTreeIndex (tower.TreeSkillNames[i]));
			}
		} else {
			tower.TreeSkillNames.Clear ();
		}
	}

	void OnEnable () {
		prefabUtils = DIContainer.GetModule <IPrefabUtils> ();
		dataUtils = DIContainer.GetModule <IDataUtils> ();
		binartyUtils = new BinaryUtils () as IDataUtils;

		LoadExistData ();
		SetupProjectileIndex ();
		SetupSkillTreeIndexes ();

		tower = new TowerData("tower" + existTowers.Count);

	}

	void OnFocus () {
		LoadExistData ();
		SetupProjectileIndex ();
		SetupSkillTreeIndexes ();

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

		toggleSkillTrees = EditorGUILayout.Foldout (toggleSkillTrees, "Skill Trees " + tower.TreeSkillNames.Count);
		if (toggleSkillTrees) {
			for (int skillTreeIndex = 0; skillTreeIndex < tower.TreeSkillNames.Count; skillTreeIndex++) {
				GUILayout.BeginHorizontal ();
				skillTreeIndexes[skillTreeIndex] = EditorGUILayout.Popup (skillTreeIndexes[skillTreeIndex], existSkillTreeIDs.ToArray ());
				tower.TreeSkillNames[skillTreeIndex] = existSkillTreeIDs [skillTreeIndexes[skillTreeIndex]];
				if (GUILayout.Button ("Remove")) {
					tower.TreeSkillNames.RemoveAt (skillTreeIndex);
					skillTreeIndexes.RemoveAt (skillTreeIndex);
					continue;
				}
				GUILayout.EndHorizontal ();
			}
			GUI.enabled = existSkillTreeIDs.Count > 0;
			if (GUILayout.Button ("Add Skill Tree")) {
				tower.TreeSkillNames.Add ("new");
				skillTreeIndexes.Add (0);
			}
			GUI.enabled = true;
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
			if(tower == null){
				tower = new TowerData("tower" + existTowers.Count);
			} else {
				SetupProjectileIndex ();
				SetupSkillTreeIndexes ();
			}

			if (towerGo) {
				DestroyImmediate (towerGo);
			}

			towerGo = prefabUtils.InstantiatePrefab (ConstantString.PrefabPath + tower.Id + ".prefab");
			if (towerGo != null) {
				tower.AtkPoint = towerGo.transform.TransformPoint (tower.AtkPoint);
			}
		}

		if (GUILayout.Button("Reset")){
			if (EditorUtility.DisplayDialog ("Are you sure?", 
				"Do you want to reset " + tower.Id + " data?",
				"Yes", "No")) {
				tower = new TowerData ("tower" + existTowers.Count);
				if (towerGo) {
					DestroyImmediate (towerGo);
				}
				skillTreeIndexes.Clear ();
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

	int GetSkillTreeIndex (string towerValue) {
		for (int i = 0; i < existSkillTreeIDs.Count; i++) {
			if (towerValue.Equals (existSkillTreeIDs[i])){
				return i;
			}
		}
		return 0;
	}
}
