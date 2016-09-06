using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor (typeof(MapConstructor))]
public class MapConstructorEditor : Editor {
	int tpCount;
	int wpCount;
	int mapId;
	public override void OnInspectorGUI (){
		DrawDefaultInspector();
		var script = target as MapConstructor;

		script.mapId = EditorGUILayout.IntField ("Map Id", script.mapId);
		this.mapId = script.mapId;
		script.mapName = EditorGUILayout.TextField ("Map Name", script.mapName);

		#region waypoint 
		EditorGUILayout.LabelField("Way Point");
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Create Way Point")) {
			Debug.Log ("create a way point");
			script.CreateWayPoint (wpCount);
			wpCount++;
		}
		if (GUILayout.Button ("Remove Way Point")) {
			Debug.Log ("remove a way point");
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
		#endregion waypoint

		#region towerpoint
		EditorGUILayout.LabelField("Tower Point");
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Create Tower Point")) {
			Debug.Log ("create a tower point");
			script.CreateTowerPoint(tpCount);
			tpCount++;

		}
		if (GUILayout.Button ("Remove Tower Point")) {
			Debug.Log ("remove a tower point");
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
		#endregion towerpoint

		#region map data
		// TODO: save and load map data to xml 
		// EditorGUILayout.LabelField("");
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Save")) {
			// Debug.Log ("save current map data to xml");
			var text = script.Save();
			// Debug.Log (text);
			WriteMapData(text);
			// TODO: confirm windows 
		}
		if (GUILayout.Button ("Load")) {
			Debug.Log ("load current map data from xml then setup scene");
			// TODO: confirm windows
		}
		if (GUILayout.Button ("Clear")) {
			Debug.Log ("clear all current map data and scene");
			// TODO: confirm windows
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
		#endregion map data
	}

	const string mapGroupDataDirectory = "Assets/Resources/Maps";	
	public void WriteMapData (string data) {

		var path = EditorUtility.SaveFilePanel("Save Map Group Data", mapGroupDataDirectory, "map_"+mapId+".txt", "txt");
		
		if (!string.IsNullOrEmpty(path))
		{
			using (FileStream fs = new FileStream (path, FileMode.Create)) {
				using (StreamWriter writer = new StreamWriter(fs)) {
					writer.Write(data);
				}
			}
		}

		// refresh project database
		AssetDatabase.Refresh();
	}
}
