using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor (typeof(ProjectileConstructor))]
public class ProjectileConstructorEditor : Editor {
	ProjectileConstructor projectileConstructor;
	SerializedObject pc;

	List<ProjectileData> existProjectiles;

	void OnEnable(){
		projectileConstructor = (ProjectileConstructor) target as ProjectileConstructor;
		pc = new SerializedObject(projectileConstructor);

		if (projectileConstructor.Projectile == null) {
			projectileConstructor.Projectile = new ProjectileData ("projectile" + existProjectiles.Count);
		}

		existProjectiles = DataManager.Instance.LoadAllData<ProjectileData>();
	}
	bool toggleNextUpgrade;
	public override void OnInspectorGUI (){
		// DrawDefaultInspector();	// test

		if(projectileConstructor == null)
			return;

		pc.Update();

		GUILayout.BeginVertical("box");
		EditorGUI.indentLevel++;
		EditorGUILayout.LabelField ("PROJECTILE CONSTRUCTOR");

		GUILayout.BeginHorizontal();

		GUILayout.EndHorizontal();

		EditorGUI.BeginChangeCheck();
		var id = EditorGUILayout.TextField ("id",  projectileConstructor.Projectile.Id);
		var name = EditorGUILayout.TextField ("Name", projectileConstructor.Projectile.Name);
		var type = (ProjectileType) EditorGUILayout.EnumPopup ("Type", projectileConstructor.Projectile.Type);
		var turnSpeed = EditorGUILayout.FloatField ("Turn Speed", projectileConstructor.Projectile.TurnSpeed);

		GUI.enabled = projectileConstructor.Projectile.Type == ProjectileType.homing;
		var travelSpeed = EditorGUILayout.FloatField ("Travel Speed", projectileConstructor.Projectile.TravelSpeed);
		GUI.enabled = true;

		GUI.enabled = projectileConstructor.Projectile.Type == ProjectileType.throwing;
		var travelTime = EditorGUILayout.FloatField ("Travel Time", projectileConstructor.Projectile.TravelTime);
		GUI.enabled = true;

		var range = EditorGUILayout.FloatField ("Range", projectileConstructor.Projectile.Range);
		if (EditorGUI.EndChangeCheck()) {
			projectileConstructor.Projectile.Id = id;
			projectileConstructor.Projectile.Name = name;
			projectileConstructor.Projectile.Type = type;
			projectileConstructor.Projectile.TravelSpeed = travelSpeed;
			projectileConstructor.Projectile.TurnSpeed = turnSpeed;
			projectileConstructor.Projectile.TravelTime = travelTime;
			projectileConstructor.Projectile.Range = range;
		}

		EditorGUI.indentLevel--;
		GUILayout.EndVertical();

		GUILayout.BeginHorizontal ();
		GUI.enabled = CheckFields ();
		if (GUILayout.Button("Save")){
			DataManager.Instance.SaveData (projectileConstructor.Projectile);
		}
		GUI.enabled = true;
		if (GUILayout.Button("Load")){
			projectileConstructor.Projectile = DataManager.Instance.LoadData <ProjectileData> ();
		}
		if (GUILayout.Button("Reset")){
			projectileConstructor.Projectile = new ProjectileData ("projectile" + existProjectiles.Count);
		}
		GUILayout.EndHorizontal();

		pc.ApplyModifiedProperties();

		Repaint ();
	}

	private bool CheckFields () {
		var nameInput = !String.IsNullOrEmpty (projectileConstructor.Projectile.Name);

		return nameInput;
	}
}
