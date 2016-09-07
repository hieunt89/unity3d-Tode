using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditorInternal;

[CustomEditor (typeof(MapConstructor))]
public class MapConstructorEditor : Editor {
	int tpCount;
	int wpCount;
	int mapId;

	bool toggleWP;
	bool toggleTP;
	bool toggleWG;
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

		//
		wayPoints.onRemoveCallback += OnRemoveCallBack;
		towerPoints.onRemoveCallback += OnRemoveCallBack;
		wayGroups.onRemoveCallback += OnRemoveCallBack;

		wayPoints.drawElementCallback += OnDrawCallBack;
		towerPoints.drawElementCallback += OnDrawCallBack;
		wayGroups.drawElementCallback += OnDrawCallBack;
	}

	void OnDisable () {
		if (wayPoints != null) wayPoints.onRemoveCallback -= OnRemoveCallBack;
		if (towerPoints != null) towerPoints.onRemoveCallback -= OnRemoveCallBack;
		if (wayGroups != null) wayGroups.onRemoveCallback -= OnRemoveCallBack;
	}

	private void OnRemoveCallBack (ReorderableList list) {
		if (EditorUtility.DisplayDialog("Warning!", "Are you sure?", "Yes", "Hell No")) {
			ReorderableList.defaultBehaviours.DoRemoveButton (list);
		}
	}
	
	private void OnDrawCallBack (Rect rect, int index, bool isActive, bool isFocused) {
		// var props = new [] {"type", "amount", "spawnInterval", "waveDelay"};

		// for (int i = 0; i < props.Length; i++)
		// {
		// 	var sProp = serializedObject.FindProperty(props[i]);
		// 	var guiContent = new GUIContent ();
		// 	guiContent.text = sProp.displayName;
		// 	EditorGUILayout.PropertyField (sProp, guiContent);
		// }

		var item = wayGroups.serializedProperty.GetArrayElementAtIndex(index);
		// GUIContent guiContent;
		EditorGUI.PropertyField (
			new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
			item.FindPropertyRelative ("type"),
			
			GUIContent.none
		);

		EditorGUI.PropertyField (
			new Rect(rect.x + 80, rect.y, 60, EditorGUIUtility.singleLineHeight),
			item.FindPropertyRelative ("amount"),
			GUIContent.none
		);

		EditorGUI.PropertyField (
			new Rect(rect.x + 160, rect.y, 60, EditorGUIUtility.singleLineHeight),
			item.FindPropertyRelative ("spawnInterval"),
			GUIContent.none
		);

		EditorGUI.PropertyField (
			new Rect(rect.x + 240, rect.y, 60, EditorGUIUtility.singleLineHeight),
			item.FindPropertyRelative ("waveDelay"),
			GUIContent.none
		);
	}
	public override void OnInspectorGUI (){
		// DrawDefaultInspector();
		mapConstructor.mapId = EditorGUILayout.IntField ("Map Id", mapConstructor.mapId);
		this.mapId = mapConstructor.mapId;

		#region waypoint 
		toggleWP = EditorGUILayout.Foldout(toggleWP, "Way Point");
		if (toggleWP) {
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
		}
		EditorGUILayout.Space();
		#endregion waypoint

		#region towerpoint
		toggleTP = EditorGUILayout.Foldout(toggleTP, "Tower Point");
		if (toggleTP) {
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
		}
		EditorGUILayout.Space();
		#endregion towerpoint

		toggleWG = EditorGUILayout.Foldout(toggleWG, "Way Group");
		if (toggleWG) {
			// custom reorderable list 
			wayGroups.DoLayoutList();
		}
		#region map data
		// TODO: save and load map data to xml 
		// EditorGUILayout.LabelField("");
		
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Save")) {
			// Debug.Log ("save current map data to xml");
			var text = mapConstructor.Save();
			// Debug.Log (text);
			WriteMapData(text);
			// TODO: 
			// - [x]confirm windows 
			// - kiem tra cac truong co empty khong
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
