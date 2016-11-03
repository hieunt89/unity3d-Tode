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

		if (projectileGo == null) {
			if (GUILayout.Button ("Create Projectile GO")) {
				projectileGo = new GameObject (projectile.Id);
			}
		} else {
			projectileGo = EditorGUILayout.ObjectField ("Projectile GO", projectileGo, typeof(GameObject), true);


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
				dataUtils.SaveData (projectile);
				prefabUtils.SavePrefab (projectileGo as GameObject);
			}
			GUI.enabled = true;

			if (GUILayout.Button("Load")){
				var data = dataUtils.LoadData <ProjectileData> ();
				if(data != null){
					projectile = data;
				}
			}
			if (GUILayout.Button("Reset")){
				projectile = new ProjectileData ("projectile" + existProjectiles.Count);
			}
//		GUILayout.EndHorizontal();
		}
		Repaint ();
	}

	private bool CheckInputFields () {
		return projectileGo && !String.IsNullOrEmpty (projectile.Name);
	}
}
