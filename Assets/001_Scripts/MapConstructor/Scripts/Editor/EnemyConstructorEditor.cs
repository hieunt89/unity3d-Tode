using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

[CustomEditor (typeof(EnemyConstructor))]
public class EnemyConstructorEditor : Editor {
	EnemyConstructor enemyConstructor;
	SerializedObject ec;

	List<EnemyData> existEnemies;

	List<float> armorValues;

	void OnEnable(){
		enemyConstructor = (EnemyConstructor) target as EnemyConstructor;
		ec = new SerializedObject(enemyConstructor);

		existEnemies = DataManager.Instance.LoadAllData<EnemyData> ();
		// check exist enemies null

		if (enemyConstructor.Enemy == null) {
			enemyConstructor.Enemy = new EnemyData ("enemy" + existEnemies.Count, new List<ArmorData> () {
				new ArmorData (AttackType.physical, 0f),
				new ArmorData (AttackType.magical, 0f),
			});
		}

		armorValues = new List<float> ();
		for (int i = 0; i < enemyConstructor.Enemy.Armors.Count; i++) {
			armorValues.Add (0f);
		}
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
			
		EditorGUI.BeginChangeCheck ();
		var id = EditorGUILayout.TextField ("id", enemyConstructor.Enemy.Id);
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
			enemyConstructor.Enemy.Id = id;
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
			for (int i = 0; i < enemyConstructor.Enemy.Armors.Count; i++)
			{
				GUILayout.BeginHorizontal();
				EditorGUI.BeginChangeCheck ();
				armorValues[i] =  EditorGUILayout.Slider(enemyConstructor.Enemy.Armors [i].Type.ToString ().ToUpper () + " Armor Reduction", enemyConstructor.Enemy.Armors [i].Reduction, 0f, 100f);
				if (EditorGUI.EndChangeCheck ()) {
					enemyConstructor.Enemy.Armors [i].Reduction = armorValues [i];
				}
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
			enemyConstructor.Enemy = DataManager.Instance.LoadData <EnemyData> ();
		}
		if (GUILayout.Button("Rest")){
			enemyConstructor.Enemy =  new EnemyData ("enemy" + existEnemies.Count, new List<ArmorData>(){
				new ArmorData(AttackType.physical, 0f),
				new ArmorData(AttackType.magical, 0f),
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
