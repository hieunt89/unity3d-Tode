using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(TowerConstructor))]
public class TowerConstructorEditor : Editor {
	TowerConstructor towerConstructor;
	SerializedObject tc;
	SerializedProperty towers;
	void OnEnable(){
		towerConstructor = (TowerConstructor) target as TowerConstructor;
		tc = new SerializedObject(towerConstructor);
		towers = tc.FindProperty("towers");
	}
	bool toggleNextUpgrade;
	public override void OnInspectorGUI (){
		// DrawDefaultInspector();	// test
		
		if(towerConstructor == null)
			return;
		
		tc.Update();
		GUILayout.BeginVertical("box");
		EditorGUI.indentLevel++;
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Add Tower")){
			towerConstructor.towers.Add(new TowerData());
		}
		if (GUILayout.Button("Clear Towers")){
			towerConstructor.towers.Clear ();
		}
		GUILayout.EndHorizontal();

		if (towerConstructor.towers.Count > 0){
			for (int i = 0; i < towerConstructor.towers.Count; i++)
			{
				GUILayout.BeginVertical("box");

				GUILayout.BeginHorizontal();
				var tId = "t" + i;
				EditorGUILayout.LabelField ("id", tId);
				towerConstructor.towers[i].id = tId;
				if (GUILayout.Button("Remove")){
					towerConstructor.towers.RemoveAt(i);
					continue;
				}	
				GUILayout.EndHorizontal();
				EditorGUILayout.TextField ("name", towerConstructor.towers[i].Name);
				EditorGUILayout.TextField ("projectile", towerConstructor.towers[i].PrjType);
				EditorGUILayout.EnumPopup ("Attack Type", towerConstructor.towers[i].AtkType);
				EditorGUILayout.FloatField ("Tower Range", towerConstructor.towers[i].AtkRange);
				EditorGUILayout.IntField ("Min Damage", towerConstructor.towers[i].MinDmg);
				EditorGUILayout.IntField ("Max Damage", towerConstructor.towers[i].MaxDmg);
				EditorGUILayout.FloatField ("Attack Speed", towerConstructor.towers[i].AtkSpeed);
				EditorGUILayout.FloatField ("Build Time", towerConstructor.towers[i].BuildTime);

				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField ("Next Upgrades");
				if (GUILayout.Button("Add Upgrade")){
					if (towerConstructor.towers[i].NextUpgrade == null)
						towerConstructor.towers[i].NextUpgrade = new List <string> ();
					towerConstructor.towers[i].NextUpgrade.Add("");
				}
				if (GUILayout.Button("Clear Upgrades")){
					towerConstructor.towers[i].NextUpgrade.Clear();
				}
				GUILayout.EndHorizontal();

				if (towerConstructor.towers[i].NextUpgrade != null && towerConstructor.towers[i].NextUpgrade.Count > 0){
					EditorGUI.indentLevel++;
					for (int j = 0; j < towerConstructor.towers[i].NextUpgrade.Count; j++)
					{

						GUILayout.BeginHorizontal();
						EditorGUILayout.TextField ("Branch " + (j + 1), towerConstructor.towers[i].NextUpgrade[j]);
						if (GUILayout.Button("Remove Upgrade")){
							towerConstructor.towers[i].NextUpgrade.RemoveAt(j);
						}
						GUILayout.EndHorizontal();
					}
					EditorGUI.indentLevel--;
				}

				GUILayout.EndVertical();
				GUILayout.Space(5);
			}

		}
		EditorGUI.indentLevel--;
		GUILayout.EndVertical();

		tc.ApplyModifiedProperties();

		Repaint ();
		
	}
}
