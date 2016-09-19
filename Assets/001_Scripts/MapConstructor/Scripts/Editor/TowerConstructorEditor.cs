using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(TowerConstructor))]
public class TowerConstructorEditor : Editor {
	TowerConstructor towerConstructor;
	SerializedObject tc;

	List<TowerData> existTowers;
	void OnEnable(){
		towerConstructor = (TowerConstructor) target as TowerConstructor;
		tc = new SerializedObject(towerConstructor);
		existTowers = new List<TowerData> ();
		DataManager.Instance.LoadAllData (existTowers);
	}
	bool toggleNextUpgrade;
	public override void OnInspectorGUI (){
		// DrawDefaultInspector();	// test
		
		if(towerConstructor == null)
			return;
		
		tc.Update();
		GUILayout.BeginVertical("box");
		EditorGUI.indentLevel++;
		EditorGUILayout.LabelField ("TOWER CONSTRUCTOR");

		var tId = "t" + existTowers.Count;
		EditorGUILayout.LabelField ("id", tId);
		towerConstructor.Tower.Id = tId;

		EditorGUI.BeginChangeCheck ();
		var name = EditorGUILayout.TextField ("name", towerConstructor.Tower.Name);
		var prjType = EditorGUILayout.TextField ("projectile", towerConstructor.Tower.PrjType);
		var atkType = EditorGUILayout.EnumPopup ("Attack Type", towerConstructor.Tower.AtkType);
		var atkRange = EditorGUILayout.FloatField ("Tower Range", towerConstructor.Tower.AtkRange);
		var minDmg = EditorGUILayout.IntField ("Min Damage", towerConstructor.Tower.MinDmg);
		var maxDmg = EditorGUILayout.IntField ("Max Damage", towerConstructor.Tower.MaxDmg);
		var atkSpeed = EditorGUILayout.FloatField ("Attack Speed", towerConstructor.Tower.AtkSpeed);
		var goldRequired = EditorGUILayout.IntField ("Gold Cost", towerConstructor.Tower.GoldRequired);
		var buildTime = EditorGUILayout.FloatField ("Build Time", towerConstructor.Tower.BuildTime);
		if (EditorGUI.EndChangeCheck ()) {
			towerConstructor.Tower.Name = name;
			towerConstructor.Tower.PrjType = prjType;
			towerConstructor.Tower.AtkType = (AttackType) atkType;
			towerConstructor.Tower.AtkRange = atkRange;
			towerConstructor.Tower.MinDmg = minDmg;
			towerConstructor.Tower.MaxDmg = maxDmg;
			towerConstructor.Tower.AtkSpeed = atkSpeed;
			towerConstructor.Tower.GoldRequired = goldRequired;
			towerConstructor.Tower.BuildTime = buildTime;
		}

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField ("Next Upgrades");
		if (GUILayout.Button("Add Upgrade")){
			if (towerConstructor.Tower.NextUpgrade == null)
				towerConstructor.Tower.NextUpgrade = new List <string> ();
			towerConstructor.Tower.NextUpgrade.Add("");
		}
		if (GUILayout.Button("Clear Upgrades")){
			towerConstructor.Tower.NextUpgrade.Clear();
		}
		GUILayout.EndHorizontal();

		if (towerConstructor.Tower.NextUpgrade != null && towerConstructor.Tower.NextUpgrade.Count > 0){
			EditorGUI.indentLevel++;
			for (int j = 0; j < towerConstructor.Tower.NextUpgrade.Count; j++)
			{

				GUILayout.BeginHorizontal();
				EditorGUILayout.TextField ("Branch " + (j + 1), towerConstructor.Tower.NextUpgrade[j]);
				if (GUILayout.Button("Remove Upgrade")){
					towerConstructor.Tower.NextUpgrade.RemoveAt(j);
				}
				GUILayout.EndHorizontal();
			}
			EditorGUI.indentLevel--;
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
			DataManager.Instance.LoadData (towerConstructor.Tower);
		}
		if (GUILayout.Button("Reset")){
			towerConstructor.tower = new TowerData ();
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
