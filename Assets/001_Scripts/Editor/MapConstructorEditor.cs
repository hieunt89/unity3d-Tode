using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditorInternal;

[CustomEditor (typeof(MapConstructor))]
public class MapConstructorEditor : Editor {
	int tpCount;
	int wpCount;
	int mapId;

	
	private ReorderableList wayPoints;
	private ReorderableList towerPoints;
	private ReorderableList wayGroups;
	private MapConstructor mapConstructor;

	void OnEnable () {
		// script = target as MapConstructor;
		mapConstructor = Selection.activeGameObject.GetComponent <MapConstructor> ();
		wayPoints = new ReorderableList (serializedObject, serializedObject.FindProperty("wayPoints"), true, true, true, true);
		towerPoints = new ReorderableList (serializedObject, serializedObject.FindProperty("towerPoints"), true, true, true, true);
		wayGroups = new ReorderableList (serializedObject, serializedObject.FindProperty("waveGroups"), true, true, true, true);
	}
	public override void OnInspectorGUI (){
		// DrawDefaultInspector();

		mapConstructor.mapId = EditorGUILayout.IntField ("Map Id", mapConstructor.mapId);
		this.mapId = mapConstructor.mapId;
	
		#region waypoint 
		EditorGUILayout.LabelField("Way Point");
		
		wayPoints.DoLayoutList();

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Create Way Point")) {
			Debug.Log ("create a way point");
			mapConstructor.CreateWayPoint (wpCount);
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

		towerPoints.DoLayoutList();

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Create Tower Point")) {
			Debug.Log ("create a tower point");
			mapConstructor.CreateTowerPoint(tpCount);
			tpCount++;
		}
		if (GUILayout.Button ("Remove Tower Point")) {
			Debug.Log ("remove a tower point");
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
		#endregion towerpoint

		// custom reorderable list 
		wayGroups.DoLayoutList();

		#region map data
		// TODO: save and load map data to xml 
		// EditorGUILayout.LabelField("");
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Save")) {
			// Debug.Log ("save current map data to xml");
			var text = mapConstructor.Save();
			// Debug.Log (text);
			WriteMapData(text);
			// TODO: confirm windows 
		}
		if (GUILayout.Button ("Load")) {
			Debug.Log ("load current map data from xml then setup scene");

			mapConstructor.Load (LoadMapData());
			// TODO: confirm windows
		}
		if (GUILayout.Button ("Clear")) {
			Debug.Log ("clear all current map data and scene");
			// TODO: confirm windows
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
		#endregion map data

		serializedObject.ApplyModifiedProperties();
	}

	const string mapGroupDataDirectory = "Assets/Resources/Maps";	
	public void WriteMapData (string data) {

		var path = EditorUtility.SaveFilePanel("Save Map Data", mapGroupDataDirectory, "map_"+mapId+".txt", "txt");
		
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

	private string LoadMapData () {
		var path = EditorUtility.OpenFilePanel("Load Map Data", mapGroupDataDirectory, "txt");

		var reader = new WWW("file:///" + path);
		while(!reader.isDone){
		}

		return reader.text;
	}
}
