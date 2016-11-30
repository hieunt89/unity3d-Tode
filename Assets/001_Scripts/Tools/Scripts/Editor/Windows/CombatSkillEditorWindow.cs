using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;

public class CombatSkillEditorWindow : EditorWindow {

	public CombatSkillList combatSkillList;
	CombatSkillData combatSkill;

	int projectileIndex = 1;
	int viewIndex = 0;
	bool toggleEditMode = false;
	Vector2 scrollPosition;
	bool isSelectedAll = false;
	List<bool> selectedIndexes;

	[MenuItem("Tode/Projectile Editor &P")]
	public static void ShowWindow()
	{
		var projectileEditorWindow = EditorWindow.GetWindow <ProjectileEditorWindow> ("Projectile Editor", true);
		projectileEditorWindow.minSize = new Vector2 (400, 600); 
	}

	void OnEnable () {

		combatSkillList = AssetDatabase.LoadAssetAtPath (ConstantString.CombatSkillDataPath, typeof(CombatSkillList)) as CombatSkillList;
		if (combatSkillList == null) {
			CreateNewItemList ();
		}

		selectedIndexes = new List<bool> ();
		for (int i = 0; i < combatSkillList.combatSkills.Count ; i++) {
			selectedIndexes.Add (false);
		}
	}

	void OnGUI()
	{
		switch (viewIndex) {
		case 0:
			DrawProjectileList ();
			break;
		case 1:
			DrawSelectedProjectile ();
			break;
		default:
			break;
		}
	}

	void DrawProjectileList () {
		EditorGUILayout.BeginHorizontal ("box", GUILayout.Height (25));
		if (GUILayout.Button ("Add")) {
			AddProjectileData ();
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
							combatSkillList.combatSkills.RemoveAt (i);
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
		for (int i = 0; i < combatSkillList.combatSkills.Count; i++) {
			EditorGUILayout.BeginHorizontal ();

			var btnLabel = combatSkillList.combatSkills[i].name;
			if (GUILayout.Button (btnLabel)) {
				projectileIndex = i;
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

	void DrawSelectedProjectile () {
		GUI.SetNextControlName ("DummyFocus");
		GUI.Button (new Rect (0,0,0,0), "", GUIStyle.none);

		if (combatSkillList != null)
		{
			GUILayout.BeginHorizontal ("box");
			if (GUILayout.Button("<", GUILayout.ExpandWidth(false))) 
			{
				if (projectileIndex > 1)
				{	
					projectileIndex --;
					GUI.FocusControl ("DummyFocus");
				}

			}
			if (GUILayout.Button(">", GUILayout.ExpandWidth(false))) 
			{
				if (projectileIndex < combatSkillList.combatSkills.Count) 
				{
					projectileIndex ++;
					GUI.FocusControl ("Dummy");
				}
			}

			GUILayout.Space(100);

			if (GUILayout.Button("Add", GUILayout.ExpandWidth(false))) 
			{
				AddProjectileData();
			}
			if (GUILayout.Button("Delete", GUILayout.ExpandWidth(false))) 
			{
				DeleteProjectileData (projectileIndex - 1);
			}

			GUILayout.FlexibleSpace ();

			if (GUILayout.Button("Back", GUILayout.ExpandWidth(false))) 
			{
				viewIndex = 0;
			}
			GUILayout.EndHorizontal ();

			if (combatSkillList.combatSkills.Count > 0) 
			{
				GUILayout.BeginHorizontal ();
				projectileIndex = Mathf.Clamp (EditorGUILayout.IntField ("Current Item", projectileIndex, GUILayout.ExpandWidth(false)), 1, combatSkillList.combatSkills.Count);
				EditorGUILayout.LabelField ("of   " +  combatSkillList.combatSkills.Count.ToString() + "  items", "", GUILayout.ExpandWidth(false));
				GUILayout.EndHorizontal ();
				GUILayout.Space(10);

				combatSkill = combatSkillList.combatSkills[projectileIndex-1];

//				combatSkill.id = EditorGUILayout.TextField ("Id", combatSkill.id);
//				combatSkill.name = EditorGUILayout.TextField ("Name", combatSkill.name);
//				combatSkill.description = EditorGUILayout.TextField ("Description", combatSkill.description);
//				combatSkill.spritePath = EditorGUILayout.TextField ("Sprite", combatSkill.spritePath);
//		
//				combatSkill.cooldown = EditorGUILayout.FloatField ("Cooldown", combatSkill.cooldown);
//				combatSkill.castRange = EditorGUILayout.FloatField ("Cast Range", combatSkill.castRange);
//				combatSkill.castTime = EditorGUILayout.FloatField ("Cast Time", combatSkill.castTime);
//				combatSkill.goldCost = EditorGUILayout.IntField ("Cost", combatSkill.goldCost);
//		
//				if (projectileIds.Count > 0) {
//					projectileIndex = EditorGUILayout.Popup ("Projectile", projectileIndex, projectileIds.ToArray());
//					combatSkill.projectileId = projectileIds[projectileIndex];
//		
//					GUILayout.BeginVertical ("box");
//					toggleProjectile = EditorGUILayout.Foldout (toggleProjectile, "Projectile Detail");
//					if (toggleProjectile) {
//						EditorGUILayout.LabelField ("Name", existProjectiles[projectileIndex].Name);
//						EditorGUILayout.LabelField ("Type", existProjectiles[projectileIndex].Type.ToString ());
//						EditorGUILayout.LabelField ("TravelSpeed", existProjectiles[projectileIndex].TravelSpeed.ToString ());
//						EditorGUILayout.LabelField ("Duration", existProjectiles[projectileIndex].Duration.ToString ());
//						EditorGUILayout.LabelField ("MaxDmgBuildTime", existProjectiles[projectileIndex].MaxDmgBuildTime.ToString ());
//						EditorGUILayout.LabelField ("TickInterval", existProjectiles[projectileIndex].TickInterval.ToString ());
//					}
//					GUILayout.EndVertical ();
//				}
//				combatSkill.aoe = EditorGUILayout.FloatField ("AOE", combatSkill.aoe);
//				combatSkill.attackType = (AttackType) EditorGUILayout.EnumPopup ("Attack Type", combatSkill.attackType);
//				combatSkill.damage = EditorGUILayout.IntField ("Damage", combatSkill.damage);
//		
//				GUILayout.Space(5);
//				toggleSkillEffect = EditorGUILayout.Foldout (toggleSkillEffect, "Skill Effect");
//				if (toggleSkillEffect) {
//					GUILayout.BeginVertical("box");
//					for (int i = 0; i < combatSkill.effectList.Count; i++) {
//						EditorGUI.BeginChangeCheck ();
//						GUILayout.BeginHorizontal ();
//						var _effectType = (EffectType) EditorGUILayout.EnumPopup ("Effect Type", combatSkill.effectList[i].effectType);
//						//				GUILayout.FlexibleSpace ();
//						if (GUILayout.Button ("Remove", GUILayout.MaxWidth (80))) {
//							combatSkill.effectList.RemoveAt(i);
//							continue;
//						}
//						GUILayout.EndHorizontal ();
//						var _effectValue = EditorGUILayout.FloatField ("Value", combatSkill.effectList[i].value);
//						var _effectDuration = EditorGUILayout.FloatField ("Duration", combatSkill.effectList[i].duration);
//						if (EditorGUI.EndChangeCheck ()) {
//							combatSkill.effectList[i].skillId = combatSkill.id;
//							combatSkill.effectList[i].effectType = _effectType;
//							combatSkill.effectList[i].value = _effectValue;
//							combatSkill.effectList[i].duration = _effectDuration;
//						}
//						GUILayout.Space(5);
//		
//					}
//					if (GUILayout.Button("Add New Skill Effect")){
//						combatSkill.effectList.Add (new SkillEffect ()); 
//					}
//					GUILayout.EndVertical();
//				}
				GUILayout.Space(5);
		
				GUILayout.Space(10);
			} 
			else 
			{
				GUILayout.Label ("This Projectile List is Empty.");
			}
		}
		if (GUI.changed) 
		{
			EditorUtility.SetDirty(combatSkillList);
		}
	}

	void OnDestroy () {

	}

	void OpenItemList () {

	}

	void CreateNewItemList () {
		projectileIndex = 1;
		combatSkillList = CreateProjectileList();
		if (combatSkillList) 
		{
			combatSkillList.combatSkills = new List<CombatSkillData>();
		}
	}

	//	[MenuItem("Assets/Create/Inventory Item List")]
	public static CombatSkillList CreateProjectileList()
	{
		CombatSkillList asset = ScriptableObject.CreateInstance<CombatSkillList>();

		AssetDatabase.CreateAsset(asset, ConstantString.CombatSkillDataPath);
		AssetDatabase.SaveAssets();
		return asset;
	}

	void DeleteProjectileData (int index) 
	{
		if (EditorUtility.DisplayDialog ("Are you sure?", 
			"Do you want to delete " + combatSkillList.combatSkills[index].id + " data?",
			"Yes", "No")) {
			combatSkillList.combatSkills.RemoveAt (index);
			selectedIndexes.RemoveAt (index);
		}
	}

	void AddProjectileData () {
		CombatSkillData newCombatSkillData = new CombatSkillData();
		newCombatSkillData.id = Guid.NewGuid().ToString ();
		combatSkillList.combatSkills.Add (newCombatSkillData);
		selectedIndexes.Add (false);
		projectileIndex = combatSkillList.combatSkills.Count;
	}
//	CombatSkillData combatSkill;
//	SummonSkillData summonSkill;
//	bool toggleProjectile;
//	SkillType skillType;
//
//	List<SummonSkillData> existSummonSkills;
//
//	List<ProjectileData> existProjectiles;
//	List<string> projectileIds;
//	int projectileIndex;
//
//	bool toggleSkillEffect;
//
//	IDataUtils dataUtils;
//
//	[MenuItem ("Tode/Combat Skill Editor")]
//	public static void ShowWindow () {
//		var combatSkillEditorWindow = EditorWindow.GetWindow <CombatSkillEditorWindow> ("Combat Skill Editor", true);
//		combatSkillEditorWindow.minSize = new Vector2 (400, 600);
//	}
//
//	void LoadExistData () {
//		existProjectiles = dataUtils.LoadAllData <ProjectileData> ();
//	}
//
//	void SetupProjectileIDs () {
//
//		projectileIds = new List<string> ();
//		for (int i = 0; i < existProjectiles.Count; i++) {
//			projectileIds.Add(existProjectiles[i].Id);
//		}
//	}
//	void OnEnable () {
//		dataUtils = DIContainer.GetModule <IDataUtils> ();
//
//		combatSkill = new CombatSkillData (Guid.NewGuid().ToString());
//
//		LoadExistData ();
//		SetupProjectileIDs ();
//	}
//
//	void OnFocus () {
//		LoadExistData ();
//		SetupProjectileIDs ();
//
//	}
//
//	void OnGUI () {
//		EditorGUI.BeginChangeCheck ();
//		combatSkill.id = EditorGUILayout.TextField ("Id", combatSkill.id);
//		combatSkill.name = EditorGUILayout.TextField ("Name", combatSkill.name);
//		combatSkill.description = EditorGUILayout.TextField ("Description", combatSkill.description);
//		combatSkill.spritePath = EditorGUILayout.TextField ("Sprite", combatSkill.spritePath);
//
//		combatSkill.cooldown = EditorGUILayout.FloatField ("Cooldown", combatSkill.cooldown);
//		combatSkill.castRange = EditorGUILayout.FloatField ("Cast Range", combatSkill.castRange);
//		combatSkill.castTime = EditorGUILayout.FloatField ("Cast Time", combatSkill.castTime);
//		combatSkill.goldCost = EditorGUILayout.IntField ("Cost", combatSkill.goldCost);
//
//		if (projectileIds.Count > 0) {
//			projectileIndex = EditorGUILayout.Popup ("Projectile", projectileIndex, projectileIds.ToArray());
//			combatSkill.projectileId = projectileIds[projectileIndex];
//
//			GUILayout.BeginVertical ("box");
//			toggleProjectile = EditorGUILayout.Foldout (toggleProjectile, "Projectile Detail");
//			if (toggleProjectile) {
//				EditorGUILayout.LabelField ("Name", existProjectiles[projectileIndex].Name);
//				EditorGUILayout.LabelField ("Type", existProjectiles[projectileIndex].Type.ToString ());
//				EditorGUILayout.LabelField ("TravelSpeed", existProjectiles[projectileIndex].TravelSpeed.ToString ());
//				EditorGUILayout.LabelField ("Duration", existProjectiles[projectileIndex].Duration.ToString ());
//				EditorGUILayout.LabelField ("MaxDmgBuildTime", existProjectiles[projectileIndex].MaxDmgBuildTime.ToString ());
//				EditorGUILayout.LabelField ("TickInterval", existProjectiles[projectileIndex].TickInterval.ToString ());
//			}
//			GUILayout.EndVertical ();
//		}
//		combatSkill.aoe = EditorGUILayout.FloatField ("AOE", combatSkill.aoe);
//		combatSkill.attackType = (AttackType) EditorGUILayout.EnumPopup ("Attack Type", combatSkill.attackType);
//		combatSkill.damage = EditorGUILayout.IntField ("Damage", combatSkill.damage);
//
//		GUILayout.Space(5);
//		toggleSkillEffect = EditorGUILayout.Foldout (toggleSkillEffect, "Skill Effect");
//		if (toggleSkillEffect) {
//			GUILayout.BeginVertical("box");
//			for (int i = 0; i < combatSkill.effectList.Count; i++) {
//				EditorGUI.BeginChangeCheck ();
//				GUILayout.BeginHorizontal ();
//				var _effectType = (EffectType) EditorGUILayout.EnumPopup ("Effect Type", combatSkill.effectList[i].effectType);
//				//				GUILayout.FlexibleSpace ();
//				if (GUILayout.Button ("Remove", GUILayout.MaxWidth (80))) {
//					combatSkill.effectList.RemoveAt(i);
//					continue;
//				}
//				GUILayout.EndHorizontal ();
//				var _effectValue = EditorGUILayout.FloatField ("Value", combatSkill.effectList[i].value);
//				var _effectDuration = EditorGUILayout.FloatField ("Duration", combatSkill.effectList[i].duration);
//				if (EditorGUI.EndChangeCheck ()) {
//					combatSkill.effectList[i].skillId = combatSkill.id;
//					combatSkill.effectList[i].effectType = _effectType;
//					combatSkill.effectList[i].value = _effectValue;
//					combatSkill.effectList[i].duration = _effectDuration;
//				}
//				GUILayout.Space(5);
//
//			}
//			if (GUILayout.Button("Add New Skill Effect")){
//				combatSkill.effectList.Add (new SkillEffect ()); 
//			}
//			GUILayout.EndVertical();
//		}
//		GUILayout.Space(5);
//
//		GUI.enabled = !String.IsNullOrEmpty (combatSkill.name) && combatSkill.effectList != null; 
//		if (GUILayout.Button("Save")){
//			//			Debug.Log (combatSkill.effectList.Count);
//			dataUtils.CreateData (combatSkill);
//		}
//		GUI.enabled = true;
//		if (GUILayout.Button("Load")){
//			var data = dataUtils.LoadData <CombatSkillData> ();
//			if (data == null) {
//				combatSkill = new CombatSkillData (Guid.NewGuid().ToString ());
//			} else {
//				combatSkill = data;
//			}
//
//			SetupProjectileIDs ();
//
//			if (existProjectiles.Count > 0) {
//				for (int i = 0; i < existProjectiles.Count; i++) {
//					if (combatSkill.projectileId.Equals (projectileIds [i])) {
//						projectileIndex = i;
//					}
//				}
//			}
//			else {
//				projectileIndex = 0;
//
//			}
//		}
//		if (GUILayout.Button("Reset")){
//			combatSkill = new CombatSkillData (Guid.NewGuid().ToString ());
//			projectileIndex = 0;
//		}
//	}
//
}
