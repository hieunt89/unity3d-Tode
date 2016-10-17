﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class CharacterEditorWindow : EditorWindow {
	CharacterData character;
	List<CharacterData> existCharacters;
	List<float> armorValues;

	[MenuItem("Window/Character Editor &E")]
	public static void ShowWindow()
	{
		var characterEditorWindow = EditorWindow.GetWindow <CharacterEditorWindow> ("Character Editor", true);
		characterEditorWindow.minSize = new Vector2 (400, 600);
	}

	void OnFocus () {
		existCharacters = DataManager.Instance.LoadAllData<CharacterData> ();
	}
	void OnEnable () {
		existCharacters = DataManager.Instance.LoadAllData<CharacterData> ();
		// check exist enemies null

		character = new CharacterData ("character" + existCharacters.Count, new List<ArmorData> () {
			new ArmorData (AttackType.physical, 0f),
			new ArmorData (AttackType.magical, 0f),
		});

		armorValues = new List<float> ();
		for (int i = 0; i < character.Armors.Count; i++) {
			armorValues.Add (0f);
		}
	}

	void OnGUI () {
//		GUILayout.BeginVertical("box");
//		EditorGUI.indentLevel++;

		EditorGUI.BeginChangeCheck ();
		var id = EditorGUILayout.TextField ("id", character.Id);
		var name = EditorGUILayout.TextField ("Name", character.Name);
		var hp = EditorGUILayout.IntField ("Hit Point", character.Hp);
		var moveSpeed = EditorGUILayout.FloatField ("Move Speed", character.MoveSpeed);
		var turnSpeed = EditorGUILayout.FloatField ("Turn Speed", character.TurnSpeed);
		var lifeCount = EditorGUILayout.IntField ("Life Count", character.LifeCount);
		var goldWorth = EditorGUILayout.IntField ("Gold Worth", character.GoldWorth);
		var atkType = (AttackType) EditorGUILayout.EnumPopup ("Attack Type", character.AtkType);
		var atkSpeed = EditorGUILayout.FloatField ("Attack Speed", character.AtkSpeed);
		var minAtkDmg = EditorGUILayout.IntField ("Min Attack Damage", character.MinAtkDmg);
		var maxAtkDmg = EditorGUILayout.IntField ("Max Attack Damage", character.MaxAtkDmg);
		var atkRange = EditorGUILayout.FloatField ("Attack Range", character.AtkRange);

		if (EditorGUI.EndChangeCheck ()) {
			character.Id = id;
			character.Name = name;
			character.Hp = hp;
			character.MoveSpeed = moveSpeed;
			character.TurnSpeed = turnSpeed;
			character.LifeCount = lifeCount;
			character.GoldWorth = goldWorth;
			character.AtkType = atkType;
			character.AtkSpeed = atkSpeed;
			character.MinAtkDmg = minAtkDmg;
			character.MaxAtkDmg = maxAtkDmg;
			character.AtkRange = atkRange;
		}

		if (character.Armors != null && character.Armors.Count > 0){
			for (int i = 0; i < character.Armors.Count; i++)
			{
				GUILayout.BeginHorizontal();
				EditorGUI.BeginChangeCheck ();
				armorValues[i] = Mathf.Round(EditorGUILayout.Slider(character.Armors [i].Type.ToString ().ToUpper () + " Armor Reduction", character.Armors [i].Reduction, 0f, 100f));
				if (EditorGUI.EndChangeCheck ()) {
					character.Armors [i].Reduction = armorValues [i];
				}
				GUILayout.EndHorizontal();
			}
		}

//		EditorGUI.indentLevel--;
//		GUILayout.EndVertical();

//		GUILayout.BeginHorizontal ();
		GUI.enabled = CheckFields ();
		if (GUILayout.Button("Save")){
			DataManager.Instance.SaveData (character);
		}
		GUI.enabled = true;
		if (GUILayout.Button("Load")){
			character = DataManager.Instance.LoadData <CharacterData> ();
			if(character == null){
				character = new CharacterData ("character" + existCharacters.Count, new List<ArmorData> () {
					new ArmorData (AttackType.physical, 0f),
					new ArmorData (AttackType.magical, 0f),
				});
			}
			armorValues = new List<float> ();
			for (int i = 0; i < character.Armors.Count; i++) {
				armorValues.Add (0f);
			}
		}
		if (GUILayout.Button("Rest")){
			character =  new CharacterData ("character" + existCharacters.Count, new List<ArmorData>(){
				new ArmorData(AttackType.physical, 0f),
				new ArmorData(AttackType.magical, 0f),
			});
		}
//		GUILayout.EndHorizontal ();
		Repaint ();
	}



	private bool CheckFields () {
		var nameInput = !String.IsNullOrEmpty (character.Name);

		return nameInput;
	}
}
