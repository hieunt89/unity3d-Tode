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

		existProjectiles = new List<ProjectileData> ();
		DataManager.Instance.LoadAllData(existProjectiles);
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
		var pId = "p" + existProjectiles.Count;
		EditorGUILayout.LabelField ("id", pId);
		projectileConstructor.Projectile.Id = pId;
		GUILayout.EndHorizontal();

		EditorGUI.BeginChangeCheck();
		var name = EditorGUILayout.TextField ("Name", projectileConstructor.Projectile.Name);
		var travelSpeed = EditorGUILayout.FloatField ("Travel Speed", projectileConstructor.Projectile.TravelSpeed);
		var turnSpeed = EditorGUILayout.FloatField ("Turn Speed", projectileConstructor.Projectile.TurnSpeed);
		var range = EditorGUILayout.FloatField ("Range", projectileConstructor.Projectile.Range);
		if (EditorGUI.EndChangeCheck()) {
			projectileConstructor.Projectile.Name = name;
			projectileConstructor.Projectile.TravelSpeed = travelSpeed;
			projectileConstructor.Projectile.TurnSpeed = turnSpeed;
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
			DataManager.Instance.LoadData (projectileConstructor.Projectile);
		}
		if (GUILayout.Button("Reset")){
			projectileConstructor.Projectile = new ProjectileData ();
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
