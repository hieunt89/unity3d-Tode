﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ProjectileEditorWindow : EditorWindow {

	public ProjectileList projectileList;
	GameObject projectileGo;
	private int projectileindex = 1;
	public int viewIndex = 0;
	bool toggleEditMode = false;
	List<bool> selectedIndexes;

	[MenuItem("Tode/Projectile Editor &P")]
	public static void ShowWindow()
	{
		var projectileEditorWindow = EditorWindow.GetWindow <ProjectileEditorWindow> ("Projectile Editor", true);
		projectileEditorWindow.minSize = new Vector2 (400, 600); 
	}

	void OnEnable () {
		
		projectileList = AssetDatabase.LoadAssetAtPath (ConstantString.ProjectileDataPath, typeof(ProjectileList)) as ProjectileList;
		if (projectileList == null) {
			CreateNewItemList ();
		}
		selectedIndexes = new List<bool> ();
		for (int i = 0; i < projectileList.projectiles.Count ; i++) {
			
		}
	}

	void OnGUI()
	{
		GUI.SetNextControlName ("DummyFocus");
		GUI.Button (new Rect (0,0,0,0), "", GUIStyle.none);

		if (projectileList.projectiles.Count > 0) {

			if (viewIndex == 0) {
				DrawProjectileList ();
			}
			if (viewIndex == 1) {
				DrawProjectileDetail ();
			}
		} 
	}
	Vector2 scrollPosition;

	void DrawProjectileList () {
		EditorGUILayout.BeginHorizontal ("box");
		if (GUILayout.Button ("Add")) {

		}

		GUILayout.FlexibleSpace ();

		if (GUILayout.Button (toggleEditMode ? "Done" : "Edit Mode")) {
			toggleEditMode = !toggleEditMode;
		}

		EditorGUILayout.EndHorizontal ();


		EditorGUILayout.BeginVertical ();
		scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition, GUILayout.Height (position.height));
		for (int i = 0; i < projectileList.projectiles.Count; i++) {
			EditorGUILayout.BeginHorizontal ();
//			if (toggleEditMode) {
//				if (EditorGUILayout.Toggle (false)) {
//					selectedIndexes.Add (i);
//				} 
////				else if (selectedIndexes.Contains (i)) {
////					selectedIndexes.Remove(i);	
////					continue;
////				}
////				if (GUILayout.Button ("X", GUILayout.Width (30))) {
////					projectileList.projectiles.RemoveAt (i);
////					continue;
////				}
//			}
			var btnLabel = projectileList.projectiles[i].intId + " - " + projectileList.projectiles[i].Name;
			if (GUILayout.Button (btnLabel)) {
				projectileindex = i;
				viewIndex = 1;
			}
		EditorGUILayout.EndHorizontal ();

		}
		EditorGUILayout.EndScrollView ();
		EditorGUILayout.EndVertical ();
	}

	void DrawProjectileDetail () {
		
		GUILayout.BeginHorizontal ();

		GUILayout.Space(10);

		if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false))) 
		{
			if (projectileindex > 1)
			{	
				projectileindex --;
				GUI.FocusControl ("DummyFocus");
			}

		}
		GUILayout.Space(5);
		if (GUILayout.Button("Next", GUILayout.ExpandWidth(false))) 
		{
			if (projectileindex < projectileList.projectiles.Count) 
			{
				projectileindex ++;
				GUI.FocusControl ("Dummy");
			}
		}

		GUILayout.Space(60);

		if (GUILayout.Button("Add Item", GUILayout.ExpandWidth(false))) 
		{
			AddProjectileData();
		}
		if (GUILayout.Button("Delete Item", GUILayout.ExpandWidth(false))) 
		{
			DeleteProjectileData (projectileindex - 1);
		}

		GUILayout.FlexibleSpace ();

		if (GUILayout.Button("Back", GUILayout.ExpandWidth(false))) 
		{
			viewIndex = 0;
		}

		GUILayout.EndHorizontal ();
		if (projectileList.projectiles == null)
			Debug.Log("wtf");
		if (projectileList.projectiles.Count > 0) 
		{
			GUILayout.BeginHorizontal ();
			projectileindex = Mathf.Clamp (EditorGUILayout.IntField ("Current Item", projectileindex, GUILayout.ExpandWidth(false)), 1, projectileList.projectiles.Count);
			//Mathf.Clamp (viewIndex, 1, inventoryItemList.itemList.Count);
			EditorGUILayout.LabelField ("of   " +  projectileList.projectiles.Count.ToString() + "  items", "", GUILayout.ExpandWidth(false));
			GUILayout.EndHorizontal ();
			GUILayout.Space(10);

			EditorGUILayout.LabelField ("id",  projectileList.projectiles[projectileindex-1].intId.ToString());
			projectileList.projectiles[projectileindex-1].Name = EditorGUILayout.TextField ("Name", projectileList.projectiles[projectileindex-1].Name);
			projectileList.projectiles[projectileindex-1].View = (GameObject) EditorGUILayout.ObjectField ("Projectile GO", projectileList.projectiles[projectileindex-1].View, typeof(GameObject), true);
			projectileList.projectiles[projectileindex-1].Type = (ProjectileType) EditorGUILayout.EnumPopup ("Type", projectileList.projectiles[projectileindex-1].Type);

			if (projectileList.projectiles[projectileindex-1].Type == ProjectileType.homing) {
				projectileList.projectiles[projectileindex-1].Duration = 0f;
			}

			GUI.enabled = projectileList.projectiles[projectileindex-1].Type == ProjectileType.homing;
			projectileList.projectiles[projectileindex-1].TravelSpeed = EditorGUILayout.FloatField ("Travel Speed", projectileList.projectiles[projectileindex-1].TravelSpeed);
			GUI.enabled = true;

			GUI.enabled = projectileList.projectiles[projectileindex-1].Type == ProjectileType.throwing || projectileList.projectiles[projectileindex-1].Type == ProjectileType.laser;
			projectileList.projectiles[projectileindex-1].Duration = EditorGUILayout.FloatField ("Duration", projectileList.projectiles[projectileindex-1].Duration);
			GUI.enabled = true;

			GUI.enabled = projectileList.projectiles[projectileindex-1].Type == ProjectileType.laser;
			projectileList.projectiles[projectileindex-1].MaxDmgBuildTime = EditorGUILayout.FloatField ("Time to reach maxDmg", projectileList.projectiles[projectileindex-1].MaxDmgBuildTime);
			projectileList.projectiles[projectileindex-1].TickInterval = EditorGUILayout.FloatField ("Tick interval", projectileList.projectiles[projectileindex-1].TickInterval);
			GUI.enabled = true;

			GUILayout.Space(10);

		} 
		else 
		{
			GUILayout.Label ("This Inventory List is Empty.");
		}
	}

	void OnDestroy () {

	}

	void OpenItemList () {

	}

	void CreateNewItemList () {
		projectileindex = 1;
		projectileList = CreateProjectileList();
		if (projectileList) 
		{
			projectileList.projectiles = new List<ProjectileData>();
		}

	}

	[MenuItem("Assets/Create/Inventory Item List")]
	public static ProjectileList  CreateProjectileList()
	{
		ProjectileList asset = ScriptableObject.CreateInstance<ProjectileList>();

		AssetDatabase.CreateAsset(asset, ConstantString.ProjectileDataPath);
		AssetDatabase.SaveAssets();
		return asset;
	}

	void DeleteProjectileData (int index) 
	{
		if (EditorUtility.DisplayDialog ("Are you sure?", 
			"Do you want to delete " + projectileList.projectiles[index].intId + " data?",
			"Yes", "No")) {
			projectileList.projectiles.RemoveAt (index);
		}
	}

	void AddProjectileData () {
		ProjectileData newProjectileData = new ProjectileData();
		int projectileId = 0;
		if (projectileList.projectiles.Count > 0){
			projectileId = projectileList.projectiles [projectileList.projectiles.Count - 1].intId + 1;
		}else {
			projectileId = 0;
		}
		newProjectileData.intId = projectileId;
		projectileList.projectiles.Add (newProjectileData);
		projectileindex = projectileList.projectiles.Count;
	}

}
