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
				EditorGUILayout.TextField ("name", towerConstructor.towers[i].name);
				EditorGUILayout.TextField ("projectile", towerConstructor.towers[i].prjType);
				EditorGUILayout.EnumPopup ("Attack Type", towerConstructor.towers[i].atkType);
				EditorGUILayout.FloatField ("Tower Range", towerConstructor.towers[i].atkRange);
				EditorGUILayout.IntField ("Min Damage", towerConstructor.towers[i].minDmg);
				EditorGUILayout.IntField ("Max Damage", towerConstructor.towers[i].maxDmg);
				EditorGUILayout.FloatField ("Attack Speed", towerConstructor.towers[i].atkSpeed);
				EditorGUILayout.FloatField ("Build Time", towerConstructor.towers[i].buildTime);

				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField ("Next Upgrades");
				if (GUILayout.Button("Add Upgrade")){
					if (towerConstructor.towers[i].nextUpgrade == null)
						towerConstructor.towers[i].nextUpgrade = new List <string> ();
					towerConstructor.towers[i].nextUpgrade.Add("");
				}
				if (GUILayout.Button("Clear Upgrades")){
					towerConstructor.towers[i].nextUpgrade.Clear();
				}
				GUILayout.EndHorizontal();

				if (towerConstructor.towers[i].nextUpgrade != null && towerConstructor.towers[i].nextUpgrade.Count > 0){
					EditorGUI.indentLevel++;
					for (int j = 0; j < towerConstructor.towers[i].nextUpgrade.Count; j++)
					{

						GUILayout.BeginHorizontal();
						EditorGUILayout.TextField ("Branch " + (j + 1), towerConstructor.towers[i].nextUpgrade[j]);
						if (GUILayout.Button("Remove Upgrade")){
							towerConstructor.towers[i].nextUpgrade.RemoveAt(j);
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
