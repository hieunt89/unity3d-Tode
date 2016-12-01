using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class SummonSkillEditorWindow : EditorWindow {

	public SummonSkillList summonSkillList;
	SummonSkillData summonSkill;

	ProjectileList existProjectiles;
	List<string> projectileIds;
	int projectileIndex;
	bool toggleProjectile;
	bool toggleSkillEffect;


	int summonSkillIndex = 1;
	int viewIndex = 0;
	bool toggleEditMode = false;
	Vector2 scrollPosition;
	bool isSelectedAll = false;
	List<bool> selectedIndexes;

	[MenuItem ("Tode/Summon Skill Editor")]
	public static void ShowWindow () {
		var summonSkillEditorWindow = EditorWindow.GetWindow <SummonSkillEditorWindow> ("Summon Skill Editor", true);
		summonSkillEditorWindow.minSize = new Vector2 (400, 600);
	}
	void OnEnable () {
		summonSkillList = AssetDatabase.LoadAssetAtPath (ConstantString.SummonSkillDataPath, typeof(SummonSkillList)) as SummonSkillList;

		selectedIndexes = new List<bool> ();
		for (int i = 0; i < summonSkillList.summonSkills.Count ; i++) {
			selectedIndexes.Add (false);
		}
	}

	void OnFocus () {
	}

	void OnGUI()
	{
		if (summonSkillList == null) {
			CreateNewItemList ();
			return;
		}
		switch (viewIndex) {
		case 0:
			DrawSummonSkillList ();
			break;
		case 1:
			DrawSelectedSummonSkill ();
			break;
		default:
			break;
		}
	}

	void DrawSummonSkillList () {
		EditorGUILayout.BeginHorizontal ("box", GUILayout.Height (25));
		if (GUILayout.Button ("Add")) {
			AddSummonSkillData ();
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
							summonSkillList.summonSkills.RemoveAt (i);
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
		for (int i = 0; i < summonSkillList.summonSkills.Count; i++) {
			EditorGUILayout.BeginHorizontal ();

			var btnLabel = summonSkillList.summonSkills[i].id;
			if (GUILayout.Button (btnLabel)) {
				summonSkillIndex = i;
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

	void DrawSelectedSummonSkill () {
		GUI.SetNextControlName ("DummyFocus");
		GUI.Button (new Rect (0,0,0,0), "", GUIStyle.none);

		if (summonSkillList != null)
		{
			GUILayout.BeginHorizontal ("box");
			if (GUILayout.Button("<", GUILayout.ExpandWidth(false))) 
			{
				if (summonSkillIndex > 1)
				{	
					summonSkillIndex --;
					GUI.FocusControl ("DummyFocus");
				}

			}
			if (GUILayout.Button(">", GUILayout.ExpandWidth(false))) 
			{
				if (summonSkillIndex < summonSkillList.summonSkills.Count) 
				{
					summonSkillIndex ++;
					GUI.FocusControl ("Dummy");
				}
			}

			GUILayout.Space(100);

			if (GUILayout.Button("Add", GUILayout.ExpandWidth(false))) 
			{
				AddSummonSkillData();
			}
			if (GUILayout.Button("Delete", GUILayout.ExpandWidth(false))) 
			{
				DeleteSummonSkillData (summonSkillIndex - 1);
			}

			GUILayout.FlexibleSpace ();

			if (GUILayout.Button("Back", GUILayout.ExpandWidth(false))) 
			{
				viewIndex = 0;
			}
			GUILayout.EndHorizontal ();

			if (summonSkillList.summonSkills.Count > 0) 
			{
				GUILayout.BeginHorizontal ();
				summonSkillIndex = Mathf.Clamp (EditorGUILayout.IntField ("Current Item", summonSkillIndex, GUILayout.ExpandWidth(false)), 1, summonSkillList.summonSkills.Count);
				EditorGUILayout.LabelField ("of   " +  summonSkillList.summonSkills.Count.ToString() + "  items", "", GUILayout.ExpandWidth(false));
				GUILayout.EndHorizontal ();
				GUILayout.Space(10);

				summonSkill = summonSkillList.summonSkills[summonSkillIndex-1];

				summonSkill.id = EditorGUILayout.TextField ("Id", summonSkill.id);
				summonSkill.name = EditorGUILayout.TextField ("Name", summonSkill.name);
				summonSkill.description = EditorGUILayout.TextField ("Description", summonSkill.description);
				summonSkill.spritePath = EditorGUILayout.TextField ("Sprite", summonSkill.spritePath);
				summonSkill.cooldown = EditorGUILayout.FloatField ("Cooldown", summonSkill.cooldown);
				summonSkill.castRange = EditorGUILayout.FloatField ("Cast Range", summonSkill.castRange);
				summonSkill.castTime = EditorGUILayout.FloatField ("Cast Time", summonSkill.castTime);
				summonSkill.goldCost = EditorGUILayout.IntField ("Cost", summonSkill.goldCost);
				summonSkill.summonId = EditorGUILayout.TextField ("AOE", summonSkill.summonId);
				summonSkill.summonCount = EditorGUILayout.IntField ("Attack Type", summonSkill.summonCount);
				summonSkill.duration = EditorGUILayout.FloatField ("Damage", summonSkill.duration);

				GUILayout.Space(10);
			} 
			else 
			{
				GUILayout.Label ("This Projectile List is Empty.");
			}
		}
		if (GUI.changed) 
		{
			EditorUtility.SetDirty(summonSkillList);
		}
	}

	void OnDestroy () {

	}

	void OpenItemList () {

	}

	void CreateNewItemList () {
		summonSkillIndex = 1;
		summonSkillList = CreateProjectileList();
		if (summonSkillList) 
		{
			summonSkillList.summonSkills = new List<SummonSkillData>();
		}
	}

	//	[MenuItem("Assets/Create/Inventory Item List")]
	public static SummonSkillList CreateProjectileList()
	{
		SummonSkillList asset = ScriptableObject.CreateInstance<SummonSkillList>();

		AssetDatabase.CreateAsset(asset, ConstantString.SummonSkillDataPath);
		AssetDatabase.SaveAssets();
		return asset;
	}

	void AddSummonSkillData () {
		SummonSkillData newSummonSkillData = new SummonSkillData();
		newSummonSkillData.id = Guid.NewGuid().ToString ();
		summonSkillList.summonSkills.Add (newSummonSkillData);
		selectedIndexes.Add (false);
		summonSkillIndex = summonSkillList.summonSkills.Count;
	}

	void DeleteSummonSkillData (int index) 
	{
		if (EditorUtility.DisplayDialog ("Are you sure?", 
			"Do you want to delete " + summonSkillList.summonSkills[index].id + " data?",
			"Yes", "No")) {
			summonSkillList.summonSkills.RemoveAt (index);
			selectedIndexes.RemoveAt (index);
		}
	}
}
