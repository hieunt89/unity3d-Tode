using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class CharacterEditorWindow : EditorWindow {
	CharacterData character;
	UnityEngine.Object characterGo;
	List<CharacterData> existCharacters;
	List<float> armorValues;
	IDataUtils dataUtils;
	IPrefabUtils prefabUtils;

	[MenuItem("Tode/Character Editor &E")]
	public static void ShowWindow()
	{
		var characterEditorWindow = EditorWindow.GetWindow <CharacterEditorWindow> ("Character Editor", true);
		characterEditorWindow.minSize = new Vector2 (400, 600);
	}


	void OnFocus () {
		existCharacters = dataUtils.LoadAllData<CharacterData> ();
	}
	void OnEnable () {
		prefabUtils = DIContainer.GetModule <IPrefabUtils> ();

		dataUtils = DIContainer.GetModule <IDataUtils> ();

		existCharacters = dataUtils.LoadAllData<CharacterData> ();
		// check exist enemies null

		character = new CharacterData ("character" + existCharacters.Count);

		armorValues = new List<float> ();
		for (int i = 0; i < character.Armors.Count; i++) {
			armorValues.Add (0f);
		}
	}

	void OnGUI () {
//		GUILayout.BeginVertical("box");
//		EditorGUI.indentLevel++;

//		EditorGUI.BeginChangeCheck ();
		character.Id = EditorGUILayout.TextField ("id", character.Id);
		character.Name  = EditorGUILayout.TextField ("Name", character.Name);

		characterGo = EditorGUILayout.ObjectField ("Character GO", characterGo, typeof(GameObject), true);
		GUI.enabled = characterGo == null && character.Id.Length > 0;
			if (GUILayout.Button ("Create Character GO")) {
				characterGo = new GameObject (character.Id);
			}
		GUI.enabled = true;

		character.Hp = EditorGUILayout.IntField ("Hit Point", character.Hp);
		character.MoveSpeed = EditorGUILayout.FloatField ("Move Speed", character.MoveSpeed);
		character.TurnSpeed = EditorGUILayout.FloatField ("Turn Speed", character.TurnSpeed);
		character.LifeCount = EditorGUILayout.IntField ("Life Count", character.LifeCount);
		character.GoldWorth = EditorGUILayout.IntField ("Gold Worth", character.GoldWorth);
		character.AtkType = (AttackType)EditorGUILayout.EnumPopup ("Attack Type", character.AtkType);
		character.AtkSpeed = EditorGUILayout.FloatField ("Attack Speed", character.AtkSpeed);
		character.MinAtkDmg = EditorGUILayout.IntField ("Min Attack Damage", character.MinAtkDmg);
		character.MaxAtkDmg = EditorGUILayout.IntField ("Max Attack Damage", character.MaxAtkDmg);
		character.AtkRange = EditorGUILayout.FloatField ("Attack Range", character.AtkRange);

		if (character.Armors != null && character.Armors.Count > 0) {
			for (int i = 0; i < character.Armors.Count; i++) {
				GUILayout.BeginHorizontal ();
				EditorGUI.BeginChangeCheck ();
				armorValues [i] = Mathf.Round (EditorGUILayout.Slider (character.Armors [i].Type.ToString ().ToUpper () + " Armor Reduction", character.Armors [i].Reduction, 0f, 100f));
				if (EditorGUI.EndChangeCheck ()) {
					character.Armors [i].Reduction = armorValues [i];
				}
				GUILayout.EndHorizontal ();
			}
		}

//		EditorGUI.indentLevel--;
//		GUILayout.EndVertical();

//		GUILayout.BeginHorizontal ();
		GUI.enabled = CheckInputFields ();
		if (GUILayout.Button ("Save")) {
			dataUtils.CreateData (character);
			prefabUtils.CreatePrefab (characterGo as GameObject);

		}
		GUI.enabled = true;
		if (GUILayout.Button ("Load")) {
			character = dataUtils.LoadData <CharacterData> ();
			if (character == null) {
				character = new CharacterData ("character" + existCharacters.Count);
			}

			if (characterGo) {
				DestroyImmediate (characterGo);
			}
			characterGo = prefabUtils.InstantiatePrefab (ConstantString.PrefabPath + character.Id + ".prefab");

			armorValues = new List<float> ();
			for (int i = 0; i < character.Armors.Count; i++) {
				armorValues.Add (0f);
			}
		}
		if (GUILayout.Button("Reset")){
			if (EditorUtility.DisplayDialog ("Are you sure?", 
				"Do you want to reset " + character.Id + " data?",
				"Yes", "No")) {
				character = new CharacterData ("character" + existCharacters.Count);
				if (characterGo) {
					DestroyImmediate (characterGo);
				}
			}
		}

		if (GUILayout.Button("Delete")){
			if (EditorUtility.DisplayDialog ("Are you sure?", 
				"Do you want to delete " + character.Id + " data?",
				"Yes", "No")) {
				if (characterGo) {
					DestroyImmediate (characterGo);
				}
				dataUtils.DeleteData (ConstantString.DataPath + character.GetType().Name + "/" + character.Id + ".json");
				prefabUtils.DeletePrefab (ConstantString.PrefabPath + character.Id + ".prefab");
				character = new CharacterData ("character" + existCharacters.Count);
			}
		}
//		GUILayout.EndHorizontal ();
		Repaint ();
	}



	private bool CheckInputFields () {
		return characterGo && !String.IsNullOrEmpty (character.Name);
	}
}
