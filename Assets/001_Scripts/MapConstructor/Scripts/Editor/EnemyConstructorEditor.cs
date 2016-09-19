using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

[CustomEditor (typeof(EnemyConstructor))]
public class EnemyConstructorEditor : Editor {
	EnemyConstructor enemyConstructor;
	SerializedObject ec;

	List<EnemyData> existEnemies;
	void OnEnable(){
		enemyConstructor = (EnemyConstructor) target as EnemyConstructor;
		ec = new SerializedObject(enemyConstructor);
		enemyConstructor.Enemy = new EnemyData (new List<ArmorData>(){
			new ArmorData(AttackType.physical, ArmorRating.none),
			new ArmorData(AttackType.magical, ArmorRating.none),
		});

		existEnemies = new List<EnemyData> ();
		DataManager.Instance.LoadAllData (existEnemies);

	}
	bool toggleNextUpgrade;
	public override void OnInspectorGUI (){
		// DrawDefaultInspector();	// test
		
		if(enemyConstructor == null)
			return;
		
		ec.Update();
		GUILayout.BeginVertical("box");
		EditorGUI.indentLevel++;
		EditorGUILayout.LabelField ("ENEMY CONSTRUCTOR");

		var eId = "e" + existEnemies.Count;
		EditorGUILayout.LabelField ("id", eId);
		enemyConstructor.Enemy.Id = eId;
			
		EditorGUI.BeginChangeCheck ();
		var name = EditorGUILayout.TextField ("Name", enemyConstructor.Enemy.Name);
		var hp = EditorGUILayout.IntField ("Hit Point", enemyConstructor.Enemy.Hp);
		var moveSpeed = EditorGUILayout.FloatField ("Move Speed", enemyConstructor.Enemy.MoveSpeed);
		var turnSpeed = EditorGUILayout.FloatField ("Turn Speed", enemyConstructor.Enemy.TurnSpeed);
		var lifeCount = EditorGUILayout.IntField ("Life Count", enemyConstructor.Enemy.LifeCount);
		var goldWorth = EditorGUILayout.IntField ("Gold Worth", enemyConstructor.Enemy.GoldWorth);
		var atkType = (AttackType) EditorGUILayout.EnumPopup ("Attack Type", enemyConstructor.Enemy.AtkType);
		var atkSpeed = EditorGUILayout.FloatField ("Attack Speed", enemyConstructor.Enemy.AtkSpeed);
		var minAtkDmg = EditorGUILayout.IntField ("Min Attack Damage", enemyConstructor.Enemy.MinAtkDmg);
		var maxAtkDmg = EditorGUILayout.IntField ("Max Attack Damage", enemyConstructor.Enemy.MaxAtkDmg);
		var atkRange = EditorGUILayout.FloatField ("Attack Range", enemyConstructor.Enemy.AtkRange);

		if (EditorGUI.EndChangeCheck ()) {
			enemyConstructor.Enemy.Name = name;
			enemyConstructor.Enemy.Hp = hp;
			enemyConstructor.Enemy.MoveSpeed = moveSpeed;
			enemyConstructor.Enemy.TurnSpeed = turnSpeed;
			enemyConstructor.Enemy.LifeCount = lifeCount;
			enemyConstructor.Enemy.GoldWorth = goldWorth;
			enemyConstructor.Enemy.AtkType = atkType;
			enemyConstructor.Enemy.AtkSpeed = atkSpeed;
			enemyConstructor.Enemy.MinAtkDmg = minAtkDmg;
			enemyConstructor.Enemy.MaxAtkDmg = maxAtkDmg;
			enemyConstructor.Enemy.AtkRange = atkRange;
		}

		if (enemyConstructor.Enemy.Armors != null && enemyConstructor.Enemy.Armors.Count > 0){
			for (int j = 0; j < enemyConstructor.Enemy.Armors.Count; j++)
			{
				GUILayout.BeginHorizontal();
				enemyConstructor.Enemy.Armors[j].rating = (ArmorRating) EditorGUILayout.EnumPopup(enemyConstructor.Enemy.Armors[j].type.ToString ().ToUpper() + " Armor Rating", enemyConstructor.Enemy.Armors[j].rating);
				GUILayout.EndHorizontal();
			}
		}
			
		EditorGUI.indentLevel--;
		GUILayout.EndVertical();

		GUILayout.BeginHorizontal ();
		GUI.enabled = CheckFields ();
		if (GUILayout.Button("Save")){
			DataManager.Instance.SaveData (enemyConstructor.Enemy);
		}
		GUI.enabled = true;
		if (GUILayout.Button("Load")){
			DataManager.Instance.LoadData (enemyConstructor.Enemy);
		}
		if (GUILayout.Button("Rest")){
			enemyConstructor.Enemy =  new EnemyData (new List<ArmorData>(){
				new ArmorData(AttackType.physical, ArmorRating.none),
				new ArmorData(AttackType.magical, ArmorRating.none),
			});
		}
		GUILayout.EndHorizontal ();
		ec.ApplyModifiedProperties();

		Repaint ();
	}

	private bool CheckFields () {
		var nameInput = !String.IsNullOrEmpty (enemyConstructor.Enemy.Name);

		return nameInput;
	}
}
