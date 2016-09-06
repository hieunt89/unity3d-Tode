using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(MapConstructor))]
public class MapConstructorEditor : Editor {
	public override void OnInspectorGUI (){
		// DrawDefaultInspector();
		var script = target as MapConstructor;

		script.mapId = EditorGUILayout.IntField ("Map Id", script.mapId);
		script.mapName = EditorGUILayout.TextField ("Map Name", script.mapName);

		#region tower
		EditorGUILayout.LabelField("Tower Point");
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Create Tower Point")) {
			Debug.Log ("create a tower point");

		}
		if (GUILayout.Button ("Remove Tower Point")) {
			Debug.Log ("remove a tower point");
		}
		EditorGUILayout.EndHorizontal();
		#endregion tower

		#region waypoint 
		EditorGUILayout.LabelField("Way Point");
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Create Way Point")) {
			Debug.Log ("create a way point");
		}
		if (GUILayout.Button ("Remove Way Point")) {
			Debug.Log ("remove a way point");
		}
		EditorGUILayout.EndHorizontal();
		#endregion waypoint
	}
}
