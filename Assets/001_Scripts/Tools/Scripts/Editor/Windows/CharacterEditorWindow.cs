using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class CharacterEditorWindow : EditorWindow {
//	CharacterData character;
//	GameObject characterGo;
//	List<CharacterData> existCharacters;
//	IDataUtils dataUtils;
//	IPrefabUtils prefabUtils;
	CharacterList characterList;
	CharacterData character;
	int characterIndex = 1;

	List<bool> selectedIndexes;
	int viewIndex = 0;
	bool toggleEditMode = false;
	bool isSelectedAll = false;
	Vector2 scrollPosition;

	List<float> armorValues;

	[MenuItem("Tode/Character Editor &E")]
	public static void ShowWindow()
	{
		var characterEditorWindow = EditorWindow.GetWindow <CharacterEditorWindow> ("Character Editor", true);
		characterEditorWindow.minSize = new Vector2 (400, 600);
	}

	void OnEnable () {
		characterList = AssetDatabase.LoadAssetAtPath (ConstantString.CharacterDataPath, typeof(CharacterList)) as CharacterList;
		if (characterList == null) {
			CreateNewItemList ();
		}
		selectedIndexes = new List<bool> ();
		for (int i = 0; i < characterList.characters.Count ; i++) {
			selectedIndexes.Add (false);
		}

	}

	void OnFocus () {
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
		SceneView.onSceneGUIDelegate += this.OnSceneGUI;
	}

	void OnDestroy () {
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
	}

	void OnGUI()
	{
		switch (viewIndex) {
		case 0:
			DrawCharacterList ();
			break;
		case 1:
			DrawCharacterDetail ();
			break;
		default:
			break;
		}
	}

	void DrawCharacterList (){
		EditorGUILayout.BeginHorizontal ("box", GUILayout.Height (25));
		if (GUILayout.Button ("Add")) {
			AddCharacterData ();
		}

		GUILayout.FlexibleSpace ();

		if (GUILayout.Button (toggleEditMode ? "Done" : "Edit Mode")) {
			toggleEditMode = !toggleEditMode;
		}
		if (toggleEditMode) {
			if (GUILayout.Button (isSelectedAll ? "Deselect All" : "Select All")) {
				isSelectedAll = !isSelectedAll;
				for (int i = 0; i < selectedIndexes.Count; i++) {
					selectedIndexes[i] = isSelectedAll;
				}
			}
			if (GUILayout.Button ("Delete Selected")) {
				if (EditorUtility.DisplayDialog ("Are you sure?", 
					"Do you want to delete all selected data?",
					"Yes", "No")) {
					for (int i = selectedIndexes.Count - 1; i >= 0; i--) {
						if (selectedIndexes[i]) {
							characterList.characters.RemoveAt (i);
							selectedIndexes.RemoveAt (i);
						}
					}
					isSelectedAll = false;
					toggleEditMode = false;
				}
			}
		}

		EditorGUILayout.EndHorizontal ();


		EditorGUILayout.BeginVertical ();
		scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition, GUILayout.Height (position.height - 40));
		for (int i = 0; i < characterList.characters.Count; i++) {
			EditorGUILayout.BeginHorizontal ();

			var btnLabel = characterList.characters[i].intId + " - " + characterList.characters[i].Name;
			if (GUILayout.Button (btnLabel)) {
				characterIndex = i;
				viewIndex = 1;
			}
			GUI.enabled = toggleEditMode;
			selectedIndexes[i] = EditorGUILayout.Toggle (selectedIndexes[i], GUILayout.Width (30));
			GUI.enabled = true;
			EditorGUILayout.EndHorizontal ();

		}
		EditorGUILayout.EndScrollView ();
		EditorGUILayout.EndVertical ();
	}

	void DrawCharacterDetail (){
		GUI.SetNextControlName ("DummyFocus");
		GUI.Button (new Rect (0,0,0,0), "", GUIStyle.none);

		GUILayout.BeginHorizontal ("box");

		if (GUILayout.Button("<", GUILayout.ExpandWidth(false))) 
		{
			if (characterIndex > 1)
			{	
				characterIndex --;
				GUI.FocusControl ("DummyFocus");
			}

		}
		if (GUILayout.Button(">", GUILayout.ExpandWidth(false))) 
		{
			if (characterIndex < characterList.characters.Count) 
			{
				characterIndex ++;
				GUI.FocusControl ("Dummy");
			}
		}

		GUILayout.Space(100);

		if (GUILayout.Button("Add", GUILayout.ExpandWidth(false))) 
		{
			AddCharacterData();
		}

		if (GUILayout.Button("Delete", GUILayout.ExpandWidth(false))) 
		{
			DeleteTowerData (characterIndex - 1);
		}

		GUILayout.FlexibleSpace ();

		if (GUILayout.Button("Back", GUILayout.ExpandWidth(false))) 
		{
			viewIndex = 0;
		}
		GUILayout.EndHorizontal ();

		if (characterList.characters == null)
			Debug.Log("wtf");
		if (characterList.characters.Count > 0) {
			GUILayout.BeginHorizontal ();
			characterIndex = Mathf.Clamp (EditorGUILayout.IntField ("Current Tower", characterIndex, GUILayout.ExpandWidth (false)), 1, characterList.characters.Count);	// important
			EditorGUILayout.LabelField ("of   " + characterList.characters.Count.ToString () + "  items", "", GUILayout.ExpandWidth (false));
			GUILayout.EndHorizontal ();
			GUILayout.Space (10);

			character = characterList.characters [characterIndex - 1];
			SetupArmorValues ();

			character.Id = EditorGUILayout.TextField ("id", character.Id);
			character.Name = EditorGUILayout.TextField ("Name", character.Name);
	
			character.View = (GameObject)EditorGUILayout.ObjectField ("View", character.View, typeof(GameObject), true);
			GUI.enabled = character.View;
			if (character.View) {
				character.AtkPoint = character.View.transform.InverseTransformPoint (character.AtkPoint);
			}
			character.AtkPoint = EditorGUILayout.Vector3Field ("Attack Point", character.AtkPoint);

			GUI.enabled = true;
	
			character.Hp = EditorGUILayout.IntField ("Hit Point", character.Hp);
			character.HpRegenRate = EditorGUILayout.Slider ("HP Regen Rate", character.HpRegenRate, 0f, 1f);
			character.HpRegenInterval = EditorGUILayout.FloatField ("HP Regen Interval", character.HpRegenInterval);
			
			
			character.MoveSpeed = EditorGUILayout.FloatField ("Move Speed", character.MoveSpeed);
			character.TurnSpeed = EditorGUILayout.FloatField ("Turn Speed", character.TurnSpeed);
			character.LifeCount = EditorGUILayout.IntField ("Life Count", character.LifeCount);
			character.GoldWorth = EditorGUILayout.IntField ("Gold Worth", character.GoldWorth);
			character.AtkType = (AttackType)EditorGUILayout.EnumPopup ("Attack Type", character.AtkType);
			character.AtkSpeed = EditorGUILayout.FloatField ("Attack Speed", character.AtkSpeed);
			character.AtkTime = EditorGUILayout.FloatField ("Attack Time", character.AtkTime);
	
			character.MinAtkDmg = EditorGUILayout.IntField ("Min Attack Damage", character.MinAtkDmg);
			character.MaxAtkDmg = EditorGUILayout.IntField ("Max Attack Damage", character.MaxAtkDmg);
			character.AtkRange = EditorGUILayout.FloatField ("Attack Range", character.AtkRange);
			character.DyingTime = EditorGUILayout.FloatField ("Dying Time", character.DyingTime);
			if (character.Armors != null && character.Armors.Count > 0) {
				for (int i = 0; i < character.Armors.Count; i++) {
					GUILayout.BeginHorizontal ();
					character.Armors [i].Reduction = Mathf.Round (EditorGUILayout.Slider (character.Armors [i].Type.ToString ().ToUpper () + " Armor Reduction", character.Armors [i].Reduction, 0f, 100f));
					GUILayout.EndHorizontal ();
				}
			}

		} else {
			GUILayout.Label ("This Tower List is Empty.");
		}
	}

	public void OnSceneGUI (SceneView _sceneView){
		Handles.color = Color.green;
		character.AtkPoint = Handles.FreeMoveHandle(character.AtkPoint, Quaternion.identity, .1f, Vector3.one, Handles.SphereCap);
	}

	void CreateNewItemList (){
	}

	void AddCharacterData () {
		CharacterData newCharacterData = new CharacterData();
		int towerId = 0;
		if (characterList.characters.Count > 0){
			towerId =characterList.characters [characterList.characters.Count - 1].intId + 1;
		}else {
			towerId = 0;
		}
		newCharacterData.intId = towerId;
		characterList.characters.Add (newCharacterData);
		selectedIndexes.Add (false);
		characterIndex = characterList.characters.Count;
	}

	void DeleteTowerData (int index) {
		if (EditorUtility.DisplayDialog ("Are you sure?", 
			"Do you want to delete " + characterList.characters[index].intId + " data?",
			"Yes", "No")) {
			characterList.characters.RemoveAt (index);
			selectedIndexes.RemoveAt (index);
		}
	}

	void SetupArmorValues () {
		armorValues = new List<float> ();
		for (int i = 0; i < character.Armors.Count; i++) {
			armorValues.Add (character.Armors[i].Reduction);
		}
	}
//
//	void OnFocus () {
//		existCharacters = dataUtils.LoadAllData<CharacterData> ();
//
//		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
//		SceneView.onSceneGUIDelegate += this.OnSceneGUI;
//	}
//
//	void OnDestroy () {
//		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
//	}
//
//	void OnEnable () {
//		prefabUtils = DIContainer.GetModule <IPrefabUtils> ();
//
//		dataUtils = DIContainer.GetModule <IDataUtils> ();
//
//		existCharacters = dataUtils.LoadAllData<CharacterData> ();
//		// check exist enemies null
//
//		character = new CharacterData ("character" + existCharacters.Count);
//
//		armorValues = new List<float> ();
//		for (int i = 0; i < character.Armors.Count; i++) {
//			armorValues.Add (0f);
//		}
//	}
//
//	void OnGUI () {
////		GUILayout.BeginVertical("box");
////		EditorGUI.indentLevel++;
//
////		EditorGUI.BeginChangeCheck ();
//		character.Id = EditorGUILayout.TextField ("id", character.Id);
//		character.Name  = EditorGUILayout.TextField ("Name", character.Name);
//
//		characterGo = (GameObject) EditorGUILayout.ObjectField ("Character GO", characterGo, typeof(GameObject), true);
//		GUI.enabled = characterGo == null && character.Id.Length > 0;
//			if (GUILayout.Button ("Create Character GO")) {
//				characterGo = new GameObject (character.Id);
//			}
//		GUI.enabled = true;
//
//		character.Hp = EditorGUILayout.IntField ("Hit Point", character.Hp);
//		character.HpRegenRate = EditorGUILayout.Slider ("HP Regen Rate", character.HpRegenRate, 0f, 1f);
//		character.HpRegenInterval = EditorGUILayout.FloatField ("HP Regen Interval", character.HpRegenInterval);
//		
//		
//		character.MoveSpeed = EditorGUILayout.FloatField ("Move Speed", character.MoveSpeed);
//		character.TurnSpeed = EditorGUILayout.FloatField ("Turn Speed", character.TurnSpeed);
//		character.LifeCount = EditorGUILayout.IntField ("Life Count", character.LifeCount);
//		character.GoldWorth = EditorGUILayout.IntField ("Gold Worth", character.GoldWorth);
//		character.AtkType = (AttackType)EditorGUILayout.EnumPopup ("Attack Type", character.AtkType);
//		character.AtkSpeed = EditorGUILayout.FloatField ("Attack Speed", character.AtkSpeed);
//		character.AtkTime = EditorGUILayout.FloatField ("Attack Time", character.AtkTime);
//		character.AtkPoint = EditorGUILayout.Vector3Field ("Attack Point", character.AtkPoint);
//
//		character.MinAtkDmg = EditorGUILayout.IntField ("Min Attack Damage", character.MinAtkDmg);
//		character.MaxAtkDmg = EditorGUILayout.IntField ("Max Attack Damage", character.MaxAtkDmg);
//		character.AtkRange = EditorGUILayout.FloatField ("Attack Range", character.AtkRange);
//		character.DyingTime = EditorGUILayout.FloatField ("Dying Time", character.DyingTime);
//		if (character.Armors != null && character.Armors.Count > 0) {
//			for (int i = 0; i < character.Armors.Count; i++) {
//				GUILayout.BeginHorizontal ();
//				EditorGUI.BeginChangeCheck ();
//				armorValues [i] = Mathf.Round (EditorGUILayout.Slider (character.Armors [i].Type.ToString ().ToUpper () + " Armor Reduction", character.Armors [i].Reduction, 0f, 100f));
//				if (EditorGUI.EndChangeCheck ()) {
//					character.Armors [i].Reduction = armorValues [i];
//				}
//				GUILayout.EndHorizontal ();
//			}
//		}
//
////		EditorGUI.indentLevel--;
////		GUILayout.EndVertical();
//
////		GUILayout.BeginHorizontal ();
//		GUI.enabled = CheckInputFields ();
//		if (GUILayout.Button ("Save")) {
//			character.AtkPoint = characterGo.transform.InverseTransformPoint (character.AtkPoint);
//			dataUtils.CreateData (character);
//			prefabUtils.CreatePrefab (characterGo as GameObject);
//
//		}
//		GUI.enabled = true;
//		if (GUILayout.Button ("Load")) {
//			character = dataUtils.LoadData <CharacterData> ();
//			if (character == null) {
//				character = new CharacterData ("character" + existCharacters.Count);
//			}
//
//			if (characterGo) {
//				DestroyImmediate (characterGo);
//			}
//			characterGo = prefabUtils.InstantiatePrefab (ConstantString.PrefabPath + character.Id + ".prefab");
//			if (characterGo != null) {
//				character.AtkPoint = characterGo.transform.TransformPoint (character.AtkPoint);
//			}
//			armorValues = new List<float> ();
//			for (int i = 0; i < character.Armors.Count; i++) {
//				armorValues.Add (0f);
//			}
//		}
//		if (GUILayout.Button("Reset")){
//			if (EditorUtility.DisplayDialog ("Are you sure?", 
//				"Do you want to reset " + character.Id + " data?",
//				"Yes", "No")) {
//				character = new CharacterData ("character" + existCharacters.Count);
//				if (characterGo) {
//					DestroyImmediate (characterGo);
//				}
//			}
//		}
//
//		if (GUILayout.Button("Delete")){
//			if (EditorUtility.DisplayDialog ("Are you sure?", 
//				"Do you want to delete " + character.Id + " data?",
//				"Yes", "No")) {
//				if (characterGo) {
//					DestroyImmediate (characterGo);
//				}
//				dataUtils.DeleteData (ConstantString.DataPath + character.GetType().Name + "/" + character.Id + ".json");
//				prefabUtils.DeletePrefab (ConstantString.PrefabPath + character.Id + ".prefab");
//				character = new CharacterData ("character" + existCharacters.Count);
//			}
//		}
////		GUILayout.EndHorizontal ();
//		Repaint ();
//	}
//
//	public void OnSceneGUI (SceneView _sceneView){
//		Handles.color = Color.green;
//		character.AtkPoint = Handles.FreeMoveHandle(character.AtkPoint, Quaternion.identity, .1f, Vector3.one, Handles.SphereCap);
//	}
//
//
//	private bool CheckInputFields () {
//		return characterGo && !String.IsNullOrEmpty (character.Name);
//	}
}
