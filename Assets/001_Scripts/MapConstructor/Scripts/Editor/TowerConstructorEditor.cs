using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(TowerConstructor))]
public class TowerConstructorEditor : Editor {
	TowerConstructor towerConstructor;
	SerializedObject tc;

	List<TowerData> existTowers;
	List<ProjectileData> existProjectiles;

	List<string> existTowerIds;
	List<string> projectileIds;

	int projectileIndex;
	List<int> nextTowerIndexes;

	void OnEnable(){
		towerConstructor = (TowerConstructor) target as TowerConstructor;
		tc = new SerializedObject(towerConstructor);
				
		existTowers = DataManager.Instance.LoadAllData <TowerData>();

		if (towerConstructor.Tower == null)
			towerConstructor.Tower = new TowerData("tower" + existTowers.Count, new List<int> (), new List<string> ());
		
		existProjectiles =  DataManager.Instance.LoadAllData <ProjectileData>();

		if (existTowers.Count > 0) {
			existTowerIds = new List<string> ();
			for (int i = 0; i < existTowers.Count; i++) {
				existTowerIds.Add(existTowers[i].Id);
			}
		}

		GetProjectileIds ();
		projectileIndex = towerConstructor.Tower.PrjTypeIndex;

		if (towerConstructor.Tower.NextUpgradeIndexes.Count > 0) {
			nextTowerIndexes = towerConstructor.Tower.NextUpgradeIndexes;
		} else {
			nextTowerIndexes = new List<int> ();
		}
	}

	private void GetProjectileIds () {
		if (existProjectiles.Count > 0) {
			projectileIds = new List<string> ();
			for (int i = 0; i < existProjectiles.Count; i++) {
				projectileIds.Add(existProjectiles[i].Id);
			}
		}
	}

	public override void OnInspectorGUI (){
		// DrawDefaultInspector();	// test
		
		if(towerConstructor == null)
			return;
		
		tc.Update();
		GUILayout.BeginVertical("box");
		EditorGUI.indentLevel++;
		EditorGUILayout.LabelField ("TOWER CONSTRUCTOR");

		EditorGUI.BeginChangeCheck ();
		var id = EditorGUILayout.TextField ("Id", towerConstructor.Tower.Id);
		var name = EditorGUILayout.TextField ("Name", towerConstructor.Tower.Name);
//		var prjType = EditorGUILayout.TextField ("projectile", towerConstructor.Tower.PrjType);
		projectileIndex = EditorGUILayout.Popup ("Projectile", projectileIndex, projectileIds.ToArray());
		var atkType = EditorGUILayout.EnumPopup ("Attack Type", towerConstructor.Tower.AtkType);
		var atkRange = EditorGUILayout.FloatField ("Tower Range", towerConstructor.Tower.AtkRange);
		var minDmg = EditorGUILayout.IntField ("Min Damage", towerConstructor.Tower.MinDmg);
		var maxDmg = EditorGUILayout.IntField ("Max Damage", towerConstructor.Tower.MaxDmg);
		var atkSpeed = EditorGUILayout.FloatField ("Attack Speed", towerConstructor.Tower.AtkSpeed);
		var goldRequired = EditorGUILayout.IntField ("Gold Cost", towerConstructor.Tower.GoldRequired);
		var buildTime = EditorGUILayout.FloatField ("Build Time", towerConstructor.Tower.BuildTime);
		if (EditorGUI.EndChangeCheck ()) {
			towerConstructor.Tower.Id = id;
			towerConstructor.Tower.Name = name;
			towerConstructor.Tower.PrjTypeIndex = projectileIndex;
			towerConstructor.Tower.PrjType = projectileIds[projectileIndex];
			towerConstructor.Tower.AtkType = (AttackType) atkType;
			towerConstructor.Tower.AtkRange = atkRange;
			towerConstructor.Tower.MinDmg = minDmg;
			towerConstructor.Tower.MaxDmg = maxDmg;
			towerConstructor.Tower.AtkSpeed = atkSpeed;
			towerConstructor.Tower.GoldRequired = goldRequired;
			towerConstructor.Tower.BuildTime = buildTime;
		}

		GUILayout.EndVertical();
		GUILayout.Space(5);

		GUILayout.BeginHorizontal ();
		GUI.enabled = CheckFields ();
		if (GUILayout.Button("Save")){
			DataManager.Instance.SaveData (towerConstructor.Tower);
		}
		GUI.enabled = true;
		if (GUILayout.Button("Load")){
			towerConstructor.Tower = DataManager.Instance.LoadData <TowerData> ();
		}
		if (GUILayout.Button("Reset")){
			towerConstructor.Tower = new TowerData ("tower" + existTowers.Count);
			nextTowerIndexes.Clear ();
		}
		GUILayout.EndHorizontal();

		tc.ApplyModifiedProperties();

		Repaint ();
	}

	private bool CheckFields () {
		var nameInput = !String.IsNullOrEmpty (towerConstructor.Tower.Name);

		return nameInput;
	}
}
