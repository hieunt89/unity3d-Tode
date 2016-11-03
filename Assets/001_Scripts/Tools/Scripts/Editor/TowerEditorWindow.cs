﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using System;

public class TowerEditorWindow : EditorWindow {
	
	TowerData tower;
	UnityEngine.Object towerGo;
	List<TowerData> existTowers;
	List<ProjectileData> existProjectiles;

//	List<string> existTowerIds;
	List<string> projectileIds;

	int projectileIndex;

	IDataUtils dataUtils;
	IPrefabUtils prefabUtils;

	[MenuItem("Tode/Tower Editor &T")]
	public static void ShowWindow()
	{
		var towerEditorWindow = EditorWindow.GetWindow <TowerEditorWindow> ("Tower Editor", true);
		towerEditorWindow.minSize = new Vector2 (400, 600);
	}

	void OnFocus () {
		prefabUtils = DIContainer.GetModule <IPrefabUtils> ();
		dataUtils = DIContainer.GetModule <IDataUtils> ();
		existTowers = dataUtils.LoadAllData <TowerData>();

		existProjectiles =  dataUtils.LoadAllData <ProjectileData>();

		if (existProjectiles.Count > 0) {
			projectileIds = new List<string> ();
			for (int i = 0; i < existProjectiles.Count; i++) {
				projectileIds.Add(existProjectiles[i].Id);
			}
		}
	}

	void OnEnable () {
		dataUtils = DIContainer.GetModule <IDataUtils> ();
		existTowers = dataUtils.LoadAllData <TowerData>();

		existProjectiles =  dataUtils.LoadAllData <ProjectileData>();

		tower = new TowerData("tower" + existTowers.Count);

	}
	void OnGUI()
	{

//		EditorGUI.BeginChangeCheck ();

		tower.Id = EditorGUILayout.TextField ("Id", tower.Id);
		tower.Name = EditorGUILayout.TextField ("Name", tower.Name);

		if (towerGo == null) {
			if (GUILayout.Button ("Create Tower GO")) {
				towerGo = new GameObject (tower.Id);
			}
		} else {
			towerGo = EditorGUILayout.ObjectField ("Tower GO", towerGo, typeof(GameObject), true);

			projectileIndex = EditorGUILayout.Popup ("Projectile", projectileIndex, projectileIds.ToArray());
			tower.ProjectileIndex = projectileIndex;
			tower.ProjectileId = projectileIds[projectileIndex];

			tower.AtkType =  (AttackType) EditorGUILayout.EnumPopup ("Attack Type", tower.AtkType);
			tower.AtkRange = EditorGUILayout.FloatField ("Tower Range",tower.AtkRange);
			tower.MinDmg = EditorGUILayout.IntField ("Min Damage", tower.MinDmg);
			tower.MaxDmg = EditorGUILayout.IntField ("Max Damage", tower.MaxDmg);
			tower.AtkSpeed = EditorGUILayout.FloatField ("Attack Speed", tower.AtkSpeed);
			tower.AtkTime = EditorGUILayout.FloatField ("Attack Time", tower.AtkTime);
			tower.GoldRequired = EditorGUILayout.IntField ("Gold Cost", tower.GoldRequired);
			tower.BuildTime = EditorGUILayout.FloatField ("Build Time", tower.BuildTime);
			tower.Aoe = EditorGUILayout.FloatField ("AOE", tower.Aoe);

	//		if (EditorGUI.EndChangeCheck ()) {
	//			tower.Id = id;
	//			tower.Name = name;
	//			tower.ProjectileIndex = projectileIndex;
	//			tower.ProjectileId = projectileIds[projectileIndex];
	//			tower.AtkType = (AttackType) atkType;
	//			tower.AtkRange = atkRange;
	//			tower.MinDmg = minDmg;
	//			tower.MaxDmg = maxDmg;
	//			tower.AtkSpeed = atkSpeed;
	//			tower.AtkTime = atkTime;
	//			tower.GoldRequired = goldRequired;
	//			tower.BuildTime = buildTime;
	//			tower.Aoe = aoe;
	//		}

			GUILayout.Space(5);

	//		GUILayout.BeginHorizontal ();
			GUI.enabled = CheckInputFields ();
			if (GUILayout.Button("Save")){
				dataUtils.SaveData (tower);
				prefabUtils.SavePrefab (towerGo as GameObject);
			}
			GUI.enabled = true;
			if (GUILayout.Button("Load")){
				tower = dataUtils.LoadData <TowerData> ();
				if(tower == null){
					tower = new TowerData("tower" + existTowers.Count);
				}
				projectileIndex = tower.ProjectileIndex;
			}
			if (GUILayout.Button("Reset")){
				tower = new TowerData ("tower" + existTowers.Count);
			}
		}
//		GUILayout.EndHorizontal();

		Repaint ();
	}

	private bool CheckInputFields () {
		var nameInput = !String.IsNullOrEmpty (tower.Name);

		return towerGo && nameInput;
	}
}
