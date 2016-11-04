using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using System;

public class ProjectileEditorWindow : EditorWindow {

	ProjectileData projectile;
	UnityEngine.Object projectileGo; 
	List<ProjectileData> existProjectiles;

	List<string> projectileIds;

	int projectileIndex;
	IDataUtils dataUtils;
	IPrefabUtils prefabUtils;

	[MenuItem("Tode/Projectile Editor &P")]
	public static void ShowWindow()
	{
		var projectileEditorWindow = EditorWindow.GetWindow <ProjectileEditorWindow> ("Projectile Editor", true);
		projectileEditorWindow.minSize = new Vector2 (400, 600); 
	}


	void OnEnable () {
		prefabUtils = DIContainer.GetModule <IPrefabUtils> ();

		dataUtils = DIContainer.GetModule <IDataUtils> ();

		existProjectiles = dataUtils.LoadAllData<ProjectileData>();

		projectile = new ProjectileData ("projectile" + existProjectiles.Count);
	}

	void OnFocus () {
		existProjectiles = dataUtils.LoadAllData<ProjectileData>();
	}

	void OnGUI()
	{
//		EditorGUI.BeginChangeCheck ();
		projectile.Id = EditorGUILayout.TextField ("id",  projectile.Id);
		projectile.Name = EditorGUILayout.TextField ("Name", projectile.Name);

		projectileGo = EditorGUILayout.ObjectField ("Projectile GO", projectileGo, typeof(GameObject), true);
		GUI.enabled = projectileGo == null && projectile.Id.Length > 0;
		if (GUILayout.Button ("Create Projectile GO")) {
			projectileGo = new GameObject (projectile.Id);
		}
		GUI.enabled = true;

		projectile.Type = (ProjectileType) EditorGUILayout.EnumPopup ("Type", projectile.Type);

		if (projectile.Type == ProjectileType.homing) {
			projectile.Duration = 0f;
		}

		GUI.enabled = projectile.Type == ProjectileType.homing;
			projectile.TravelSpeed = EditorGUILayout.FloatField ("Travel Speed", projectile.TravelSpeed);
		GUI.enabled = true;

		GUI.enabled = projectile.Type == ProjectileType.throwing || projectile.Type == ProjectileType.laser;
			projectile.Duration = EditorGUILayout.FloatField ("Duration", projectile.Duration);
		GUI.enabled = true;

		GUI.enabled = projectile.Type == ProjectileType.laser;
			projectile.MaxDmgBuildTime = EditorGUILayout.FloatField ("Time to reach maxDmg", projectile.MaxDmgBuildTime);
			projectile.TickInterval = EditorGUILayout.FloatField ("Tick interval", projectile.TickInterval);
		GUI.enabled = true;

//		if (EditorGUI.EndChangeCheck ()) {
//			projectile.Id = id;
//			projectile.Name = name;
//			projectile.Type = type;
//			projectile.TravelSpeed = travelSpeed;
//			projectile.Duration = duration;
//			projectile.MaxDmgBuildTime = maxDmgBuildTime;
//			projectile.TickInterval = tickInterval;
//		}

		GUILayout.Space(5);

//		GUILayout.BeginHorizontal ();

		GUI.enabled = CheckInputFields ();
		if (GUILayout.Button("Save")){
			dataUtils.CreateData (projectile);
			prefabUtils.CreatePrefab (projectileGo as GameObject);
		}
		GUI.enabled = true;

		if (GUILayout.Button("Load")){
			projectile = dataUtils.LoadData <ProjectileData> ();
			if(projectile == null){
				projectile = new ProjectileData ("projectile" + existProjectiles.Count);
			}
			if (projectileGo) {
				DestroyImmediate (projectileGo);
			}
			projectileGo = prefabUtils.InstantiatePrefab (ConstantString.PrefabPath + projectile.Id + ".prefab");
		}

		if (GUILayout.Button("Reset")){
			if (EditorUtility.DisplayDialog ("Are you sure?", 
				"Do you want to reset " + projectile.Id + " data?",
				"Yes", "No")) {
				projectile = new ProjectileData ("projectile" + existProjectiles.Count);
				if (projectileGo) {
					DestroyImmediate (projectileGo);
				}
			}
		}

		if (GUILayout.Button("Delete")){
			if (EditorUtility.DisplayDialog ("Are you sure?", 
				"Do you want to delete " + projectile.Id + " data?",
				"Yes", "No")) {
				if (projectileGo) {
					DestroyImmediate (projectileGo);
				}
				dataUtils.DeleteData (ConstantString.DataPath + projectile.GetType().Name + "/" + projectile.Id + ".json");
				prefabUtils.DeletePrefab (ConstantString.PrefabPath + projectile.Id + ".prefab");
				projectile = new ProjectileData ("tower" + existProjectiles.Count);
			}
		}
//		GUILayout.EndHorizontal();
		Repaint ();
	}

	private bool CheckInputFields () {
		return projectileGo && !String.IsNullOrEmpty (projectile.Name);
	}
}
