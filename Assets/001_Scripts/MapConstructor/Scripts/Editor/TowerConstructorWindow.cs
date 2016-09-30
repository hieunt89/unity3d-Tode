using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using System;

public class TowerConstructorWindow : EditorWindow {
	
	TowerData tower;
	List<TowerData> existTowers;
	List<ProjectileData> existProjectiles;

	List<string> existTowerIds;
	List<string> projectileIds;

	int projectileIndex;

	[MenuItem("Window/Tower Constructor &T")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(TowerConstructorWindow));
	}

	void OnEnable () {
		
		existTowers = DataManager.Instance.LoadAllData <TowerData>();

		tower = new TowerData("tower" + existTowers.Count, new List<int> (), new List<string> ());

		if (existTowers.Count > 0) {
			existTowerIds = new List<string> ();
			for (int i = 0; i < existTowers.Count; i++) {
				existTowerIds.Add(existTowers[i].Id);
			}
		}

		existProjectiles =  DataManager.Instance.LoadAllData <ProjectileData>();

		if (existProjectiles.Count > 0) {
			projectileIds = new List<string> ();
			for (int i = 0; i < existProjectiles.Count; i++) {
				projectileIds.Add(existProjectiles[i].Id);
			}
		}

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
		var goldRequired = EditorGUILayout.IntField ("Gold Cost", tower.GoldRequired);
		var buildTime = EditorGUILayout.FloatField ("Build Time", tower.BuildTime);

		if (EditorGUI.EndChangeCheck ()) {
			tower.Id = id;
			tower.Name = name;
			tower.PrjTypeIndex = projectileIndex;
			tower.PrjType = projectileIds[projectileIndex];
			tower.AtkType = (AttackType) atkType;
			tower.AtkRange = atkRange;
			tower.MinDmg = minDmg;
			tower.MaxDmg = maxDmg;
			tower.AtkSpeed = atkSpeed;
			tower.GoldRequired = goldRequired;
			tower.BuildTime = buildTime;
		}

		GUILayout.Space(5);

//		GUILayout.BeginHorizontal ();
		GUI.enabled = CheckFields ();
		if (GUILayout.Button("Save")){
			DataManager.Instance.SaveData (tower);
		}
		GUI.enabled = true;
		if (GUILayout.Button("Load")){
			tower = DataManager.Instance.LoadData <TowerData> ();
			projectileIndex = tower.PrjTypeIndex;
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
