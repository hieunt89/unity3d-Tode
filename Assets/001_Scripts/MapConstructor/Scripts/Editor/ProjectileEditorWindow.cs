using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using System;

public class ProjectileEditorWindow : EditorWindow {

	ProjectileData projectile;
	List<ProjectileData> existProjectiles;

	List<string> projectileIds;

	int projectileIndex;

	[MenuItem("Window/Projectile Editor &P")]
	public static void ShowWindow()
	{
		var projectileEditorWindow = EditorWindow.GetWindow <ProjectileEditorWindow> ("Projectile Editor", true);
		projectileEditorWindow.minSize = new Vector2 (400, 600); 
	}

	void OnFocus () {
		existProjectiles = DataManager.Instance.LoadAllData<ProjectileData>();
	}

	void OnEnable () {
		existProjectiles = DataManager.Instance.LoadAllData<ProjectileData>();

		projectile = new ProjectileData ("projectile" + existProjectiles.Count);
	}

	void OnGUI()
	{
		EditorGUI.BeginChangeCheck ();
		var id = EditorGUILayout.TextField ("id",  projectile.Id);
		var name = EditorGUILayout.TextField ("Name", projectile.Name);
		var type = (ProjectileType) EditorGUILayout.EnumPopup ("Type", projectile.Type);

		if (projectile.Type == ProjectileType.homing) {
			projectile.Duration = 0f;
		}

		GUI.enabled = projectile.Type == ProjectileType.homing;
			var travelSpeed = EditorGUILayout.FloatField ("Travel Speed", projectile.TravelSpeed);
		GUI.enabled = true;

		GUI.enabled = projectile.Type == ProjectileType.throwing || projectile.Type == ProjectileType.laser;
			var duration = EditorGUILayout.FloatField ("Duration", projectile.Duration);
		GUI.enabled = true;

		GUI.enabled = projectile.Type == ProjectileType.laser;
			var maxDmgBuildTime = EditorGUILayout.FloatField ("Time to reach maxDmg", projectile.MaxDmgBuildTime);
			var tickInterval = EditorGUILayout.FloatField ("Tick interval", projectile.TickInterval);
		GUI.enabled = true;

		if (EditorGUI.EndChangeCheck ()) {
			projectile.Id = id;
			projectile.Name = name;
			projectile.Type = type;
			projectile.TravelSpeed = travelSpeed;
			projectile.Duration = duration;
			projectile.MaxDmgBuildTime = maxDmgBuildTime;
			projectile.TickInterval = tickInterval;
		}

		GUILayout.Space(5);

//		GUILayout.BeginHorizontal ();

		GUI.enabled = CheckFields ();
		if (GUILayout.Button("Save")){
			DataManager.Instance.SaveData (projectile);
		}
		GUI.enabled = true;

		if (GUILayout.Button("Load")){
			var data = DataManager.Instance.LoadData <ProjectileData> ();
			if(data != null){
				projectile = data;
			}
		}
		if (GUILayout.Button("Reset")){
			projectile = new ProjectileData ("projectile" + existProjectiles.Count);
		}
//		GUILayout.EndHorizontal();

		Repaint ();
	}

	private bool CheckFields () {
		var nameInput = !String.IsNullOrEmpty (projectile.Name);

		return nameInput;
	}
}
