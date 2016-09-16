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
				enemyConstructor.enemies[i].Id = eId;
				if (GUILayout.Button("Remove")){
					enemyConstructor.enemies.RemoveAt(i);
					continue;
				}	
				GUILayout.EndHorizontal();
				enemyConstructor.enemies[i].Name = EditorGUILayout.TextField ("Name", enemyConstructor.enemies[i].Name);
				enemyConstructor.enemies[i].Hp = EditorGUILayout.IntField ("Hit Point", enemyConstructor.enemies[i].Hp);
				enemyConstructor.enemies[i].MoveSpeed = EditorGUILayout.FloatField ("Move Speed", enemyConstructor.enemies[i].MoveSpeed);
				enemyConstructor.enemies[i].TurnSpeed = EditorGUILayout.FloatField ("Turn Speed", enemyConstructor.enemies[i].TurnSpeed);
				enemyConstructor.enemies[i].LifeCount = EditorGUILayout.IntField ("Life Count", enemyConstructor.enemies[i].LifeCount);
				enemyConstructor.enemies[i].GoldWorth = EditorGUILayout.IntField ("Gold Worth", enemyConstructor.enemies[i].GoldWorth);
				enemyConstructor.enemies[i].AtkType = (AttackType) EditorGUILayout.EnumPopup ("Attack Type", enemyConstructor.enemies[i].AtkType);
				enemyConstructor.enemies[i].AtkSpeed = EditorGUILayout.FloatField ("Attack Speed", enemyConstructor.enemies[i].AtkSpeed);
				enemyConstructor.enemies[i].MinAtkDmg = EditorGUILayout.IntField ("Min Attack Damage", enemyConstructor.enemies[i].MinAtkDmg);
				enemyConstructor.enemies[i].MaxAtkDmg = EditorGUILayout.IntField ("Max Attack Damage", enemyConstructor.enemies[i].MaxAtkDmg);
				enemyConstructor.enemies[i].AtkRange = EditorGUILayout.FloatField ("Attack Range", enemyConstructor.enemies[i].AtkRange);

				if (enemyConstructor.enemies[i].Armors != null && enemyConstructor.enemies[i].Armors.Count > 0){
					for (int j = 0; j < enemyConstructor.enemies[i].Armors.Count; j++)
					{
						GUILayout.BeginHorizontal();
						enemyConstructor.enemies[i].Armors[j].rating = (ArmorRating) EditorGUILayout.EnumPopup(enemyConstructor.enemies[i].Armors[j].type.ToString ().ToUpper() + " Armor Rating", enemyConstructor.enemies[i].Armors[j].rating);
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
