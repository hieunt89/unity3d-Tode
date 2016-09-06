using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(MapConstructor))]
public class MapConstructorEditor : Editor {

	int tpCount = 0;
	public override void OnInspectorGUI (){
		// DrawDefaultInspector();
		var script = target as MapConstructor;

		script.mapId = EditorGUILayout.IntField ("Map Id", script.mapId);
		script.mapName = EditorGUILayout.TextField ("Map Name", script.mapName);
		EditorGUILayout.Space ();

		#region waypoint 
		EditorGUILayout.LabelField("Way Point");
		// TODO: 
		
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Create Way Point")) {
			Debug.Log ("create a way point");
		}
		if (GUILayout.Button ("Remove Way Point")) {
			Debug.Log ("remove a way point");
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space ();
		#endregion waypoint

		#region towerpoint
		EditorGUILayout.LabelField("Tower Point");
		// TODO: 
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Create Tower Point")) {
			Debug.Log ("create a tower point");
			script.CreateTowerPoint (tpCount);
			tpCount++;
		}
		if (GUILayout.Button ("Remove Tower Point")) {
			Debug.Log ("remove a tower point");
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space ();
		#endregion towerpoint
	}
}
