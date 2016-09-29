using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class EnemyContructorWindow : EditorWindow {
	EnemyData enemy;
	List<EnemyData> existEnemies;
	List<float> armorValues;

	[MenuItem("Window/Enemy Constructor &E")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(EnemyContructorWindow));
	}

	void OnEnable () {

		existEnemies = DataManager.Instance.LoadAllData<EnemyData> ();
		// check exist enemies null

		enemy = new EnemyData ("enemy" + existEnemies.Count, new List<ArmorData> () {
			new ArmorData (AttackType.physical, 0f),
			new ArmorData (AttackType.magical, 0f),
		});

		armorValues = new List<float> ();
		for (int i = 0; i < enemy.Armors.Count; i++) {
			armorValues.Add (0f);
		}
	}

	void OnGUI () {
		GUILayout.BeginVertical("box");
		EditorGUI.indentLevel++;

		EditorGUI.BeginChangeCheck ();
		var id = EditorGUILayout.TextField ("id", enemy.Id);
		var name = EditorGUILayout.TextField ("Name", enemy.Name);
		var hp = EditorGUILayout.IntField ("Hit Point", enemy.Hp);
		var moveSpeed = EditorGUILayout.FloatField ("Move Speed", enemy.MoveSpeed);
		var turnSpeed = EditorGUILayout.FloatField ("Turn Speed", enemy.TurnSpeed);
		var lifeCount = EditorGUILayout.IntField ("Life Count", enemy.LifeCount);
		var goldWorth = EditorGUILayout.IntField ("Gold Worth", enemy.GoldWorth);
		var atkType = (AttackType) EditorGUILayout.EnumPopup ("Attack Type", enemy.AtkType);
		var atkSpeed = EditorGUILayout.FloatField ("Attack Speed", enemy.AtkSpeed);
		var minAtkDmg = EditorGUILayout.IntField ("Min Attack Damage", enemy.MinAtkDmg);
		var maxAtkDmg = EditorGUILayout.IntField ("Max Attack Damage", enemy.MaxAtkDmg);
		var atkRange = EditorGUILayout.FloatField ("Attack Range", enemy.AtkRange);

		if (EditorGUI.EndChangeCheck ()) {
			enemy.Id = id;
			enemy.Name = name;
			enemy.Hp = hp;
			enemy.MoveSpeed = moveSpeed;
			enemy.TurnSpeed = turnSpeed;
			enemy.LifeCount = lifeCount;
			enemy.GoldWorth = goldWorth;
			enemy.AtkType = atkType;
			enemy.AtkSpeed = atkSpeed;
			enemy.MinAtkDmg = minAtkDmg;
			enemy.MaxAtkDmg = maxAtkDmg;
			enemy.AtkRange = atkRange;
		}

		if (enemy.Armors != null && enemy.Armors.Count > 0){
			for (int i = 0; i < enemy.Armors.Count; i++)
			{
				GUILayout.BeginHorizontal();
				EditorGUI.BeginChangeCheck ();
				armorValues[i] =  EditorGUILayout.Slider(enemy.Armors [i].Type.ToString ().ToUpper () + " Armor Reduction", enemy.Armors [i].Reduction, 0f, 100f);
				if (EditorGUI.EndChangeCheck ()) {
					enemy.Armors [i].Reduction = armorValues [i];
				}
				GUILayout.EndHorizontal();
			}
		}

		EditorGUI.indentLevel--;
		GUILayout.EndVertical();

		GUILayout.BeginHorizontal ();
		GUI.enabled = CheckFields ();
		if (GUILayout.Button("Save")){
			DataManager.Instance.SaveData (enemy);
		}
		GUI.enabled = true;
		if (GUILayout.Button("Load")){
			enemy = DataManager.Instance.LoadData <EnemyData> ();
		}
		if (GUILayout.Button("Rest")){
			enemy =  new EnemyData ("enemy" + existEnemies.Count, new List<ArmorData>(){
				new ArmorData(AttackType.physical, 0f),
				new ArmorData(AttackType.magical, 0f),
			});
		}
		GUILayout.EndHorizontal ();
		Repaint ();
	}



	private bool CheckFields () {
		var nameInput = !String.IsNullOrEmpty (enemy.Name);

		return nameInput;
	}
}
