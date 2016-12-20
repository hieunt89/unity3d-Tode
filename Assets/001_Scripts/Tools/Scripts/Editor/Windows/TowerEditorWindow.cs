﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using System;

public class TowerEditorWindow : EditorWindow {
	static TowerEditorWindow towerEditorWindow;
	public TowerList towerList;
	TowerData tower;
	int towerIndex = 1;
	int viewIndex = 0;
	bool toggleEditMode = false;
	bool isSelectedAll = false;
	bool toggleAtkPoint = false;

	Vector2 scrollPosition;
	List<bool> selectedTowerIndexes;

	ProjectileList _existProjectiles;
	List<ProjectileData> existProjectiles;
	bool toggleProjectile;
	List<string> projectileIds;
	int projectileIndex;

	bool toggleSkillTrees;
	List<Tree<string>> existTrees;
	List<string> existSkillTreeIDs;
	List<int> skillTreeIndexes;

	IDataUtils binartyUtils;
	IDataUtils dataUtils;

	[MenuItem("Tode/Tower Editor &T")]
	public static void ShowWindow()
	{
		towerEditorWindow = EditorWindow.GetWindow <TowerEditorWindow> ("Tower Editor", true);
		towerEditorWindow.minSize = new Vector2 (400, 600);
	}

	void OnEnable () {
		dataUtils = DIContainer.GetModule <IDataUtils> ();

		LoadExistData ();
		SetupProjectileIDs ();
		SetupSkillTreeIndexes ();

		selectedTowerIndexes = new List<bool> ();
		for (int i = 0; i < towerList.towers.Count ; i++) {
			selectedTowerIndexes.Add (false);
		}
	}

	void OnFocus () {
		LoadExistData ();
		SetupProjectileIDs ();
		SetupSkillTreeIndexes ();

		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
		SceneView.onSceneGUIDelegate += this.OnSceneGUI;
	}
	void OnDestroy () {
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
	}
	void OnGUI()
	{
		if (towerList == null) {
			CreateTowerList ();
			return;
		}
		switch (viewIndex) {
		case 0:
			DrawTowerList ();
			break;
		case 1:
			DrawTowerDetail ();
			break;
		default:
			break;
		}
	}
	void DrawTowerList() {
		EditorGUILayout.BeginHorizontal ("box", GUILayout.Height (25));
		if (GUILayout.Button ("Add")) {
			AddTowerData ();
		}

		GUILayout.FlexibleSpace ();

		if (GUILayout.Button (toggleEditMode ? "Done" : "Edit Mode")) {
			toggleEditMode = !toggleEditMode;
		}
		if (toggleEditMode) {
			if (GUILayout.Button (isSelectedAll ? "Deselect All" : "Select All")) {
				isSelectedAll = !isSelectedAll;
				for (int i = 0; i < selectedTowerIndexes.Count; i++) {
					selectedTowerIndexes[i] = isSelectedAll;
				}
			}
			if (GUILayout.Button ("Delete Selected")) {
				if (EditorUtility.DisplayDialog ("Are you sure?", 
					"Do you want to delete all selected data?",
					"Yes", "No")) {
					for (int i = selectedTowerIndexes.Count - 1; i >= 0; i--) {
						if (selectedTowerIndexes[i]) {
							towerList.towers.RemoveAt (i);
							selectedTowerIndexes.RemoveAt (i);
						}
					}
					isSelectedAll = false;
					toggleEditMode = false;
				}
			}
		}

		EditorGUILayout.EndHorizontal ();


		EditorGUILayout.BeginVertical ();
		scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition, GUILayout.Height (position.height - 40));
		if (towerList.towers != null ) {
			if (towerList.towers.Count > 0) {
				for (int i = 0; i < towerList.towers.Count; i++) {
					EditorGUILayout.BeginHorizontal ();

					var btnLabel = towerList.towers[i].Id;
					if (GUILayout.Button (btnLabel)) {
						towerIndex = i;
						viewIndex = 1;
						tower = towerList.towers [towerIndex];
						projectileIndex = SetupProjectileIndex ();
					}
					GUI.enabled = toggleEditMode;
					selectedTowerIndexes[i] = EditorGUILayout.Toggle (selectedTowerIndexes[i], GUILayout.Width (30));
					GUI.enabled = true;
					EditorGUILayout.EndHorizontal ();

				}
			}else {
				GUILayout.Label ("Tower List is Empty.");
			}
		}
		EditorGUILayout.EndScrollView ();
		EditorGUILayout.EndVertical ();
	}

	void DrawTowerDetail () {
		GUI.SetNextControlName ("DummyFocus");
		GUI.Button (new Rect (0,0,0,0), "", GUIStyle.none);

		if (towerList != null) {
			GUILayout.BeginHorizontal ("box");

			if (GUILayout.Button ("<", GUILayout.ExpandWidth (false))) {
				if (towerIndex > 1) {	
					towerIndex--;
					tower = towerList.towers [towerIndex - 1];
					SetupSkillTreeIndexes ();
					GUI.FocusControl ("DummyFocus");
				}

			}
			if (GUILayout.Button (">", GUILayout.ExpandWidth (false))) {
				if (towerIndex < towerList.towers.Count) {
					towerIndex++;
					tower = towerList.towers [towerIndex - 1];
					SetupSkillTreeIndexes ();
					GUI.FocusControl ("Dummy");
				}
			}

			GUILayout.Space (100);

			if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
				AddTowerData ();
			}
			if (GUILayout.Button ("Delete", GUILayout.ExpandWidth (false))) {
				DeleteTowerData (towerIndex - 1);
			}

			GUILayout.FlexibleSpace ();

			if (GUILayout.Button ("Back", GUILayout.ExpandWidth (false))) {
				viewIndex = 0;
			}
			GUILayout.EndHorizontal ();

			if (towerList.towers.Count > 0) {
				GUILayout.BeginHorizontal ();
				towerIndex = Mathf.Clamp (EditorGUILayout.IntField ("Current Tower", towerIndex, GUILayout.ExpandWidth (false)), 1, towerList.towers.Count);	// important
				EditorGUILayout.LabelField ("of   " + towerList.towers.Count.ToString () + "  items", "", GUILayout.ExpandWidth (false));
				GUILayout.EndHorizontal ();
				GUILayout.Space (10);

				tower = towerList.towers[towerIndex - 1];
				tower.Id = EditorGUILayout.TextField ("Id", tower.Id);
				tower.Name = EditorGUILayout.TextField ("Name", tower.Name);
	
				tower.View = (GameObject)EditorGUILayout.ObjectField ("Tower GO", tower.View, typeof(GameObject), true);
	
				GUI.enabled = tower.View;
				if (tower.View) {
					tower.AtkPoint = tower.View.transform.InverseTransformPoint (tower.AtkPoint);
				}
				tower.AtkPoint = EditorGUILayout.Vector3Field ("Attack Point", tower.AtkPoint);
				GUI.enabled = true;

				if (projectileIds.Count > 0) {
					projectileIndex = EditorGUILayout.Popup ("Projectile", projectileIndex, projectileIds.ToArray ());
					tower.ProjectileId = projectileIds [projectileIndex];

					var selectedProjectile = existProjectiles [projectileIndex];
					GUILayout.BeginVertical ("box");

					toggleProjectile = EditorGUILayout.Foldout (toggleProjectile, "Projectile Detail");
					if (toggleProjectile) {
						EditorGUILayout.LabelField ("Name", selectedProjectile.Name);
						EditorGUILayout.LabelField ("Type", selectedProjectile.Type.ToString ());
						EditorGUILayout.LabelField ("TravelSpeed", selectedProjectile.TravelSpeed.ToString ());
						EditorGUILayout.LabelField ("Duration", selectedProjectile.Duration.ToString ());
						EditorGUILayout.LabelField ("MaxDmgBuildTime", selectedProjectile.MaxDmgBuildTime.ToString ());
						EditorGUILayout.LabelField ("TickInterval", selectedProjectile.TickInterval.ToString ());
					}
					GUILayout.EndVertical ();
				}
	
				tower.AtkType = (AttackType)EditorGUILayout.EnumPopup ("Attack Type", tower.AtkType);
				tower.AtkRange = EditorGUILayout.FloatField ("Tower Range", tower.AtkRange);
				tower.MinDmg = EditorGUILayout.IntField ("Min Damage", tower.MinDmg);
				tower.MaxDmg = EditorGUILayout.IntField ("Max Damage", tower.MaxDmg);
				tower.AtkSpeed = EditorGUILayout.FloatField ("Attack Speed", tower.AtkSpeed);
				tower.AtkTime = EditorGUILayout.FloatField ("Attack Time", tower.AtkTime);
				tower.TurnSpeed = EditorGUILayout.FloatField ("Turn Speed", tower.TurnSpeed);

				tower.GoldRequired = EditorGUILayout.IntField ("Gold Cost", tower.GoldRequired);
				tower.BuildTime = EditorGUILayout.FloatField ("Build Time", tower.BuildTime);
				tower.Aoe = EditorGUILayout.FloatField ("AOE", tower.Aoe);
	
				if (existTrees != null) {
					toggleSkillTrees = EditorGUILayout.Foldout (toggleSkillTrees, "Skill Trees " + tower.TreeSkillNames.Count);
					if (toggleSkillTrees) {
						for (int skillTreeIndex = 0; skillTreeIndex < tower.TreeSkillNames.Count; skillTreeIndex++) {
							GUILayout.BeginHorizontal ();
							skillTreeIndexes [skillTreeIndex] = EditorGUILayout.Popup (skillTreeIndexes [skillTreeIndex], existSkillTreeIDs.ToArray ());
							tower.TreeSkillNames [skillTreeIndex] = existSkillTreeIDs [skillTreeIndexes [skillTreeIndex]];
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
				}
			}
//			else {
//				GUILayout.Label ("This Tower List is Empty.");
//			}
		}
		if (GUI.changed) {
			EditorUtility.SetDirty (towerList);
		}
	}

	public void OnSceneGUI (SceneView _sceneView){
		if (tower != null){
			Handles.color = Color.red;
			tower.AtkPoint = Handles.FreeMoveHandle(tower.AtkPoint, Quaternion.identity, .1f, Vector3.one, Handles.SphereCap);
		}
	}

	void CreateTowerList () {
		towerIndex = 1;
//		towerList = CreateTowerList();

		towerList = ScriptableObject.CreateInstance<TowerList>();

		dataUtils.CreateData <TowerList> (towerList);

		if (towerList) 
		{
			towerList.towers = new List<TowerData>();
		}
	}

	void AddTowerData () {
		TowerData newTowerData = new TowerData();
		newTowerData.Id = Guid.NewGuid().ToString();
		towerList.towers.Add (newTowerData);
		selectedTowerIndexes.Add (false);
		towerIndex = towerList.towers.Count;
	}

	void DeleteTowerData (int index) {
		if (EditorUtility.DisplayDialog ("Are you sure?", 
			"Do you want to delete " + towerList.towers[index].Id + " data?",
			"Yes", "No")) {
			towerList.towers.RemoveAt (index);
			selectedTowerIndexes.RemoveAt (index);
		}
	}

	void LoadExistData () {
		towerList = dataUtils.LoadData <TowerList> ();
		if (towerList == null) {
			CreateTowerList ();
		}
		existProjectiles = dataUtils.LoadAllData <ProjectileData> ();
	}

	void SetupProjectileIDs () {
		projectileIds = new List<string> ();
		if (existProjectiles.Count > 0) {
			for (int i = 0; i < existProjectiles.Count; i++) {
				projectileIds.Add (existProjectiles[i].Id);
			}				
		} 
	}

	int SetupProjectileIndex () {
		for (int i = 0; i < existProjectiles.Count; i++) {
			if (tower.ProjectileId == existProjectiles[i].Id) {
				return i;
			}
		}
		return 0;
	}

	void SetupSkillTreeIndexes () {
//		existSkillTreeIDs = new List<string> ();
//		for (int i = 0; i < existTrees.Count; i++) {
//			if (existTrees[i].treeType == TreeType.CombatSkills || existTrees[i].treeType == TreeType.SummonSkills ) 
//				existSkillTreeIDs.Add(existTrees[i].id);
//		}
//		skillTreeIndexes = new List<int> ();
//		if (existSkillTreeIDs.Count > 0) {
//			for (int i = 0; i < tower.TreeSkillNames.Count; i++) {
//				skillTreeIndexes.Add (GetSkillTreeIndex (tower.TreeSkillNames[i]));
//			}
//		} else {
//			tower.TreeSkillNames.Clear ();
//		}
	}
	int GetSkillTreeIndex (string towerValue) {
//		for (int i = 0; i < existSkillTreeIDs.Count; i++) {
//			if (existSkillTreeIDs [i] != null) {
//				if (towerValue.Equals (existSkillTreeIDs [i])) {
//					return i;
//				}
//			}
//		}
		return 0;
	}
}