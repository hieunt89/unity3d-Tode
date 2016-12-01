using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;

public class CombatSkillEditorWindow : EditorWindow {

	public CombatSkillList combatSkillList;
	CombatSkillData combatSkill;

	ProjectileList existProjectiles;
	List<string> projectileIds;
	int projectileIndex;
	bool toggleProjectile;
	bool toggleSkillEffect;


	int combatSkillIndex = 1;
	int viewIndex = 0;
	bool toggleEditMode = false;
	Vector2 scrollPosition;
	bool isSelectedAll = false;
	List<bool> selectedIndexes;


	[MenuItem("Tode/Combat Skill Editor &P")]
	public static void ShowWindow()
	{
		var combatSkillEditorWindow = EditorWindow.GetWindow <CombatSkillEditorWindow> ("Combat Skill Editor", true);
		combatSkillEditorWindow.minSize = new Vector2 (400, 600); 
	}

	void OnEnable () {

		combatSkillList = AssetDatabase.LoadAssetAtPath (ConstantString.CombatSkillDataPath, typeof(CombatSkillList)) as CombatSkillList;

		selectedIndexes = new List<bool> ();
		for (int i = 0; i < combatSkillList.combatSkills.Count ; i++) {
			selectedIndexes.Add (false);
		}

		LoadExistData ();
		SetupProjectileIDs ();
	}

	void OnFocus () {
		LoadExistData ();
		SetupProjectileIDs ();
	}

	void OnGUI()
	{
		if (combatSkillList == null) {
			CreateNewItemList ();
			return;
		}
		switch (viewIndex) {
		case 0:
			DrawCombatSkillList ();
			break;
		case 1:
			DrawSelectedCombatSkill ();
			break;
		default:
			break;
		}
	}

	void DrawCombatSkillList () {
		EditorGUILayout.BeginHorizontal ("box", GUILayout.Height (25));
		if (GUILayout.Button ("Add")) {
			AddCombatSkillData ();
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

			var btnLabel = combatSkillList.combatSkills[i].id;
			if (GUILayout.Button (btnLabel)) {
				combatSkillIndex = i;
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

	void DrawSelectedCombatSkill () {
		GUI.SetNextControlName ("DummyFocus");
		GUI.Button (new Rect (0,0,0,0), "", GUIStyle.none);

		if (combatSkillList != null)
		{
			GUILayout.BeginHorizontal ("box");
			if (GUILayout.Button("<", GUILayout.ExpandWidth(false))) 
			{
				if (combatSkillIndex > 1)
				{	
					combatSkillIndex --;
					GUI.FocusControl ("DummyFocus");
				}

			}
			if (GUILayout.Button(">", GUILayout.ExpandWidth(false))) 
			{
				if (combatSkillIndex < combatSkillList.combatSkills.Count) 
				{
					combatSkillIndex ++;
					GUI.FocusControl ("Dummy");
				}
			}

			GUILayout.Space(100);

			if (GUILayout.Button("Add", GUILayout.ExpandWidth(false))) 
			{
				AddCombatSkillData();
			}
			if (GUILayout.Button("Delete", GUILayout.ExpandWidth(false))) 
			{
				DeleteCombatSkillData (combatSkillIndex - 1);
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
				combatSkillIndex = Mathf.Clamp (EditorGUILayout.IntField ("Current Item", combatSkillIndex, GUILayout.ExpandWidth(false)), 1, combatSkillList.combatSkills.Count);
				EditorGUILayout.LabelField ("of   " +  combatSkillList.combatSkills.Count.ToString() + "  items", "", GUILayout.ExpandWidth(false));
				GUILayout.EndHorizontal ();
				GUILayout.Space(10);

				combatSkill = combatSkillList.combatSkills[combatSkillIndex-1];

				combatSkill.id = EditorGUILayout.TextField ("Id", combatSkill.id);
				combatSkill.name = EditorGUILayout.TextField ("Name", combatSkill.name);
				combatSkill.description = EditorGUILayout.TextField ("Description", combatSkill.description);
				combatSkill.spritePath = EditorGUILayout.TextField ("Sprite", combatSkill.spritePath);
		
				combatSkill.cooldown = EditorGUILayout.FloatField ("Cooldown", combatSkill.cooldown);
				combatSkill.castRange = EditorGUILayout.FloatField ("Cast Range", combatSkill.castRange);
				combatSkill.castTime = EditorGUILayout.FloatField ("Cast Time", combatSkill.castTime);
				combatSkill.goldCost = EditorGUILayout.IntField ("Cost", combatSkill.goldCost);
		
				if (projectileIds.Count > 0) {
					projectileIndex = EditorGUILayout.Popup ("Projectile", projectileIndex, projectileIds.ToArray());
					combatSkill.projectileId = projectileIds[projectileIndex];
		
					var selectedProjectile = existProjectiles.projectiles [projectileIndex];
					GUILayout.BeginVertical ("box");
					toggleProjectile = EditorGUILayout.Foldout (toggleProjectile, "Projectile Detail");
					if (toggleProjectile) {
						EditorGUILayout.LabelField ("Name", selectedProjectile.Name);
						EditorGUILayout.LabelField ("Type", selectedProjectile.Type.ToString ());
						EditorGUILayout.LabelField ("TravelSpeed", selectedProjectile.TravelSpeed.ToString ());
						EditorGUILayout.LabelField ("Duration", selectedProjectile.Duration.ToString ());
						EditorGUILayout.LabelField ("MaxDmgBuildTime", selectedProjectile.MaxDmgBuildTime.ToString ());
						EditorGUILayout.LabelField ("TickInterval", selectedProjectile.TickInterval.ToString ());
					}
					GUILayout.EndVertical ();
				}
				combatSkill.aoe = EditorGUILayout.FloatField ("AOE", combatSkill.aoe);
				combatSkill.attackType = (AttackType) EditorGUILayout.EnumPopup ("Attack Type", combatSkill.attackType);
				combatSkill.damage = EditorGUILayout.IntField ("Damage", combatSkill.damage);
		
				GUILayout.Space(5);
				toggleSkillEffect = EditorGUILayout.Foldout (toggleSkillEffect, "Skill Effect");
				if (toggleSkillEffect) {
					GUILayout.BeginVertical("box");
					for (int i = 0; i < combatSkill.effectList.Count; i++) {
						EditorGUI.BeginChangeCheck ();
						GUILayout.BeginHorizontal ();
						var _effectType = (EffectType) EditorGUILayout.EnumPopup ("Effect Type", combatSkill.effectList[i].effectType);
						//				GUILayout.FlexibleSpace ();
						if (GUILayout.Button ("Remove", GUILayout.MaxWidth (80))) {
							combatSkill.effectList.RemoveAt(i);
							continue;
						}
						GUILayout.EndHorizontal ();
						var _effectValue = EditorGUILayout.FloatField ("Value", combatSkill.effectList[i].value);
						var _effectDuration = EditorGUILayout.FloatField ("Duration", combatSkill.effectList[i].duration);
						if (EditorGUI.EndChangeCheck ()) {
							combatSkill.effectList[i].skillId = combatSkill.id;
							combatSkill.effectList[i].effectType = _effectType;
							combatSkill.effectList[i].value = _effectValue;
							combatSkill.effectList[i].duration = _effectDuration;
						}
						GUILayout.Space(5);
		
					}
					if (GUILayout.Button("Add New Skill Effect")){
						combatSkill.effectList.Add (new SkillEffect ()); 
					}
					GUILayout.EndVertical();
				}
		
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
		combatSkillIndex = 1;
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

	void DeleteCombatSkillData (int index) 
	{
		if (EditorUtility.DisplayDialog ("Are you sure?", 
			"Do you want to delete " + combatSkillList.combatSkills[index].id + " data?",
			"Yes", "No")) {
			combatSkillList.combatSkills.RemoveAt (index);
			selectedIndexes.RemoveAt (index);
		}
	}

	void AddCombatSkillData () {
		CombatSkillData newCombatSkillData = new CombatSkillData();
		newCombatSkillData.id = Guid.NewGuid().ToString ();
		combatSkillList.combatSkills.Add (newCombatSkillData);
		selectedIndexes.Add (false);
		combatSkillIndex = combatSkillList.combatSkills.Count;
	}

	void LoadExistData () {
		existProjectiles = AssetDatabase.LoadAssetAtPath (ConstantString.ProjectileDataPath, typeof (ProjectileList)) as ProjectileList;
	}
	void SetupProjectileIDs () {
		projectileIds = new List<string> ();
		for (int i = 0; i < existProjectiles.projectiles.Count; i++) {
			projectileIds.Add(existProjectiles.projectiles[i].Id);
		}
	}
}
