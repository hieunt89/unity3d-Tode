using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using System;

public class TowerEditorWindow : EditorWindow {
	
	TowerData tower;
	List<TowerData> existTowers;
	List<ProjectileData> existProjectiles;

//	List<string> existTowerIds;
	List<string> projectileIds;

	int projectileIndex;

	IDataUtils dataUtils;

	[MenuItem("Tode/Tower Editor &T")]
	public static void ShowWindow()
	{
		var towerEditorWindow = EditorWindow.GetWindow <TowerEditorWindow> ("Tower Editor", true);
		towerEditorWindow.minSize = new Vector2 (400, 600);
	}

	void OnFocus () {
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

		EditorGUI.BeginChangeCheck ();

		var id = EditorGUILayout.TextField ("Id", tower.Id);
		var name = EditorGUILayout.TextField ("Name", tower.Name);
		//		var prjType = EditorGUILayout.TextField ("projectile", towerConstructor.Tower.PrjType);
		projectileIndex = EditorGUILayout.Popup ("Projectile", projectileIndex, projectileIds.ToArray());
		var atkType = EditorGUILayout.EnumPopup ("Attack Type", tower.AtkType);
		var atkRange = EditorGUILayout.FloatField ("Tower Range",tower.AtkRange);
		var minDmg = EditorGUILayout.IntField ("Min Damage", tower.MinDmg);
		var maxDmg = EditorGUILayout.IntField ("Max Damage", tower.MaxDmg);
		var atkSpeed = EditorGUILayout.FloatField ("Attack Speed", tower.AtkSpeed);
		var atkTime = EditorGUILayout.FloatField ("Attack Time", tower.AtkTime);
		var goldRequired = EditorGUILayout.IntField ("Gold Cost", tower.GoldRequired);
		var buildTime = EditorGUILayout.FloatField ("Build Time", tower.BuildTime);
		var aoe = EditorGUILayout.FloatField ("AOE", tower.Aoe);

		if (EditorGUI.EndChangeCheck ()) {
			tower.Id = id;
			tower.Name = name;
			tower.ProjectileIndex = projectileIndex;
			tower.ProjectileId = projectileIds[projectileIndex];
			tower.AtkType = (AttackType) atkType;
			tower.AtkRange = atkRange;
			tower.MinDmg = minDmg;
			tower.MaxDmg = maxDmg;
			tower.AtkSpeed = atkSpeed;
			tower.AtkTime = atkTime;
			tower.GoldRequired = goldRequired;
			tower.BuildTime = buildTime;
			tower.Aoe = aoe;
		}

		GUILayout.Space(5);

//		GUILayout.BeginHorizontal ();
		GUI.enabled = CheckFields ();
		if (GUILayout.Button("Save")){
			dataUtils.SaveData (tower);
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

//		GUILayout.EndHorizontal();

		Repaint ();
	}

	private bool CheckFields () {
		var nameInput = !String.IsNullOrEmpty (tower.Name);

		return nameInput;
	}
}
