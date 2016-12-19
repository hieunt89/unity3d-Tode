using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;

public class CombatSkillEditorWindow : EditorWindow {

	public CombatSkillList combatSkillList;
	CombatSkillData combatSkillData;

	List <CombatSkillData> existCombarskills;
	List<string> combatskillIds;

	List <ProjectileData> existProjectiles;

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

	IDataUtils dataAssetUtils;

	[MenuItem("Tode/Combat Skill Editor &P")]
	public static void ShowWindow()
	{
		var combatSkillEditorWindow = EditorWindow.GetWindow <CombatSkillEditorWindow> ("Combat Skill Editor", true);
		combatSkillEditorWindow.minSize = new Vector2 (400, 600); 
	}

	void OnEnable () {
		dataAssetUtils = DIContainer.GetModule <IDataUtils> ();

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
			CreateNewList ();
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

				combatSkillData = combatSkillList.combatSkills[combatSkillIndex];
				if (combatSkillData.effectList == null) {
					combatSkillData.effectList = new List<SkillEffect> ();
				}
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

				combatSkillData = combatSkillList.combatSkills[combatSkillIndex-1];

				combatSkillData.id = EditorGUILayout.TextField ("Id", combatSkillData.id);
				combatSkillData.name = EditorGUILayout.TextField ("Name", combatSkillData.name);
				combatSkillData.description = EditorGUILayout.TextField ("Description", combatSkillData.description);
				combatSkillData.spritePath = EditorGUILayout.TextField ("Sprite", combatSkillData.spritePath);
		
				combatSkillData.cooldown = EditorGUILayout.FloatField ("Cooldown", combatSkillData.cooldown);
				combatSkillData.castRange = EditorGUILayout.FloatField ("Cast Range", combatSkillData.castRange);
				combatSkillData.castTime = EditorGUILayout.FloatField ("Cast Time", combatSkillData.castTime);
				combatSkillData.goldCost = EditorGUILayout.IntField ("Cost", combatSkillData.goldCost);
		
				if (projectileIds.Count > 0) {
					projectileIndex = EditorGUILayout.Popup ("Projectile", projectileIndex, projectileIds.ToArray());
					combatSkillData.projectileId = projectileIds[projectileIndex];
		
					var selectedProjectile = existProjectiles [projectileIndex];
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
				combatSkillData.aoe = EditorGUILayout.FloatField ("AOE", combatSkillData.aoe);
				combatSkillData.attackType = (AttackType) EditorGUILayout.EnumPopup ("Attack Type", combatSkillData.attackType);
				combatSkillData.damage = EditorGUILayout.IntField ("Damage", combatSkillData.damage);
		
				GUILayout.Space(5);
				toggleSkillEffect = EditorGUILayout.Foldout (toggleSkillEffect, "Skill Effect");
				if (toggleSkillEffect) {
					GUILayout.BeginVertical("box");
					for (int i = 0; i < combatSkillData.effectList.Count; i++) {
						EditorGUI.BeginChangeCheck ();
						GUILayout.BeginHorizontal ();
						var _effectType = (EffectType) EditorGUILayout.EnumPopup ("Effect Type", combatSkillData.effectList[i].effectType);
						//				GUILayout.FlexibleSpace ();
						if (GUILayout.Button ("Remove", GUILayout.MaxWidth (80))) {
							combatSkillData.effectList.RemoveAt(i);
							continue;
						}
						GUILayout.EndHorizontal ();
						var _effectValue = EditorGUILayout.FloatField ("Value", combatSkillData.effectList[i].value);
						var _effectDuration = EditorGUILayout.FloatField ("Duration", combatSkillData.effectList[i].duration);
						if (EditorGUI.EndChangeCheck ()) {
							combatSkillData.effectList[i].skillId = combatSkillData.id;
							combatSkillData.effectList[i].effectType = _effectType;
							combatSkillData.effectList[i].value = _effectValue;
							combatSkillData.effectList[i].duration = _effectDuration;
						}
						GUILayout.Space(5);
		
					}
					if (GUILayout.Button("Add New Skill Effect")){
						combatSkillData.effectList.Add (new SkillEffect ()); 
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

	void LoadExistData () {
		combatSkillList = dataAssetUtils.LoadAllData <CombatSkillList> ();
		if (combatSkillList == null) {
			CreateNewList ();
		}
		selectedIndexes = new List<bool> ();
		for (int i = 0; i < combatSkillList.combatSkills.Count ; i++) {
			selectedIndexes.Add (false);
		}

		existProjectiles = dataAssetUtils.LoadAllData <ProjectileData> ();
	}

	void CreateNewList () {
		combatSkillIndex = 1;

		combatSkillList = ScriptableObject.CreateInstance<CombatSkillList>();

		if (combatSkillList) 
		{
			combatSkillList.combatSkills = new List<CombatSkillData>();
		}
		dataAssetUtils.CreateData <CombatSkillList> (combatSkillList);
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

	void SetupProjectileIDs () {
		projectileIds = new List<string> ();
		if (existProjectiles.Count > 0) {
			for (int i = 0; i < existProjectiles.Count; i++) {
				projectileIds.Add (existProjectiles[i].Id);
			}				
		} 
	}

	int SetupProjectileIndex () {
		for (int i = 0; i < existProjectiles.Count; i++) {
			if (combatSkillData.projectileId == existProjectiles[i].Id) {
				return i;
			}
		}
		return 0;
	}
}
