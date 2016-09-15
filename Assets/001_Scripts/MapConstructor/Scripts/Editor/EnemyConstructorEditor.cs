using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(EnemyConstructor))]
public class EnemyConstructorEditor : Editor {
	EnemyConstructor enemyConstructor;
	SerializedObject ec;
	void OnEnable(){
		enemyConstructor = (EnemyConstructor) target as EnemyConstructor;
		ec = new SerializedObject(enemyConstructor);
	}
	bool toggleNextUpgrade;
	public override void OnInspectorGUI (){
		// DrawDefaultInspector();	// test
		
		if(enemyConstructor == null)
			return;
		
		ec.Update();
		GUILayout.BeginVertical("box");
		EditorGUI.indentLevel++;
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Add Enemy")){
			enemyConstructor.enemies.Add(new EnemyData(new List<ArmorData> () 
			{
				new ArmorData(AttackType.physical, ArmorRating.none), 
				new ArmorData(AttackType.magical, ArmorRating.none)}));
		}
		if (GUILayout.Button("Clear Enemies")){
			enemyConstructor.enemies.Clear ();
		}
		GUILayout.EndHorizontal();

		if (enemyConstructor.enemies != null && enemyConstructor.enemies.Count > 0){
			for (int i = 0; i < enemyConstructor.enemies.Count; i++)
			{
				GUILayout.BeginVertical("box");

				GUILayout.BeginHorizontal();
				var eId = "e" + i;
				EditorGUILayout.LabelField ("id", eId);
				enemyConstructor.enemies[i].id = eId;
				if (GUILayout.Button("Remove")){
					enemyConstructor.enemies.RemoveAt(i);
					continue;
				}	
				GUILayout.EndHorizontal();
				enemyConstructor.enemies[i].name = EditorGUILayout.TextField ("Name", enemyConstructor.enemies[i].name);
				enemyConstructor.enemies[i].hp = EditorGUILayout.IntField ("Hit Point", enemyConstructor.enemies[i].hp);
				enemyConstructor.enemies[i].moveSpeed = EditorGUILayout.FloatField ("Move Speed", enemyConstructor.enemies[i].moveSpeed);
				enemyConstructor.enemies[i].turnSpeed = EditorGUILayout.FloatField ("Turn Speed", enemyConstructor.enemies[i].turnSpeed);
				enemyConstructor.enemies[i].lifeCount = EditorGUILayout.IntField ("Life Count", enemyConstructor.enemies[i].lifeCount);
				enemyConstructor.enemies[i].goldWorth = EditorGUILayout.IntField ("Gold Worth", enemyConstructor.enemies[i].goldWorth);
				enemyConstructor.enemies[i].atkType = (AttackType) EditorGUILayout.EnumPopup ("Attack Type", enemyConstructor.enemies[i].atkType);
				enemyConstructor.enemies[i].atkSpeed = EditorGUILayout.FloatField ("Attack Speed", enemyConstructor.enemies[i].atkSpeed);
				enemyConstructor.enemies[i].minAtkDmg = EditorGUILayout.IntField ("Min Attack Damage", enemyConstructor.enemies[i].minAtkDmg);
				enemyConstructor.enemies[i].maxAtkDmg = EditorGUILayout.IntField ("Max Attack Damage", enemyConstructor.enemies[i].maxAtkDmg);
				enemyConstructor.enemies[i].atkRange = EditorGUILayout.FloatField ("Attack Range", enemyConstructor.enemies[i].atkRange);

				if (enemyConstructor.enemies[i].armors != null && enemyConstructor.enemies[i].armors.Count > 0){
					for (int j = 0; j < enemyConstructor.enemies[i].armors.Count; j++)
					{
						GUILayout.BeginHorizontal();
						enemyConstructor.enemies[i].armors[j].rating = (ArmorRating) EditorGUILayout.EnumPopup(enemyConstructor.enemies[i].armors[j].type.ToString ().ToUpper() + " Armor Rating", enemyConstructor.enemies[i].armors[j].rating);
						GUILayout.EndHorizontal();
					}
				}
				GUILayout.EndVertical();
				GUILayout.Space(5);
			}

		}
		EditorGUI.indentLevel--;
		GUILayout.EndVertical();

		ec.ApplyModifiedProperties();

		Repaint ();
		
	}
}
