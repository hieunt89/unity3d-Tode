using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(ProjectileConstructor))]
public class ProjectileConstructorEditor : Editor {
	ProjectileConstructor projectileConstructor;
	SerializedObject pc;
	void OnEnable(){
		projectileConstructor = (ProjectileConstructor) target as ProjectileConstructor;
		pc = new SerializedObject(projectileConstructor);
	}
	bool toggleNextUpgrade;
	public override void OnInspectorGUI (){
		// DrawDefaultInspector();	// test

		if(projectileConstructor == null)
			return;

		pc.Update();
		GUILayout.BeginVertical("box");
		EditorGUI.indentLevel++;
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Add Projectile")){
			projectileConstructor.Projectiles.Add(new ProjectileData());
		}
		if (GUILayout.Button("Clear Projectiles")){
			projectileConstructor.Projectiles.Clear ();
		}
		GUILayout.EndHorizontal();

		if (projectileConstructor.Projectiles != null && projectileConstructor.Projectiles.Count > 0){
			for (int i = 0; i < projectileConstructor.Projectiles.Count; i++)
			{
				GUILayout.BeginVertical("box");

				GUILayout.BeginHorizontal();
				var pId = "p" + i;
				EditorGUILayout.LabelField ("id", pId);
				projectileConstructor.Projectiles[i].Id = pId;
				if (GUILayout.Button("Remove")){
					projectileConstructor.Projectiles.RemoveAt(i);
					continue;
				}	
				GUILayout.EndHorizontal();
				EditorGUI.BeginChangeCheck();
				var id = EditorGUILayout.TextField ("Id", projectileConstructor.Projectiles[i].Id);
				var travelSpeed = EditorGUILayout.FloatField ("Travel Speed", projectileConstructor.Projectiles[i].TravelSpeed);
				var turnSpeed = EditorGUILayout.FloatField ("Turn Speed", projectileConstructor.Projectiles[i].TurnSpeed);
				var range = EditorGUILayout.FloatField ("Range", projectileConstructor.Projectiles[i].Range);
				if (EditorGUI.EndChangeCheck()) {
					projectileConstructor.Projectiles[i].Id = id;
					projectileConstructor.Projectiles[i].TravelSpeed = travelSpeed;
					projectileConstructor.Projectiles[i].TurnSpeed = turnSpeed;
					projectileConstructor.Projectiles[i].Range = range;
				}
				GUILayout.EndVertical();
				GUILayout.Space(5);
			}

		}
		EditorGUI.indentLevel--;
		GUILayout.EndVertical();

		pc.ApplyModifiedProperties();

		Repaint ();

	}
}
