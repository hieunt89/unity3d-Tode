using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditorInternal;

[CustomEditor (typeof(MapConstructor))]
public class MapConstructorEditor : Editor {
	int tpCount;
	int wpCount;
	int wCount;

	bool toggleWP;
	bool toggleTP;
	bool toggleW;
	private ReorderableList wayPoints;
	private ReorderableList towerPoints;
	// private ReorderableList waves;
	private SerializedObject mc;
	private SerializedProperty waves;
	private ReorderableList waveGroups;
	private MapConstructor mapConstructor;

	public GUIStyle guiTitleStyle {
		get {
			var guiTitleStyle = new GUIStyle (GUI.skin.label);
			guiTitleStyle.normal.textColor = Color.black;
			guiTitleStyle.fontStyle = FontStyle.Bold;
			guiTitleStyle.fontSize = 16;
			guiTitleStyle.fixedHeight = 30;
			guiTitleStyle.alignment = TextAnchor.MiddleCenter;
			return guiTitleStyle;
		}
	}
	void OnEnable () {
		mapConstructor = target as MapConstructor;
		mc = new SerializedObject(mapConstructor);
        waves = mc.FindProperty("waves");

		if (EditorPrefs.HasKey("TWP"))
			toggleWP = EditorPrefs.GetBool("TWP");
		if (EditorPrefs.HasKey("TTP"))
			toggleTP = EditorPrefs.GetBool("TTP");
		if (EditorPrefs.HasKey("TW"))
			toggleW = EditorPrefs.GetBool("TW");
		// mapConstructor = Selection.activeGameObject.GetComponent <MapConstructor> ();

		// mapConstructor.waves = new List<Wave> ();

		wayPoints = new ReorderableList (serializedObject, serializedObject.FindProperty("wayPoints"), true, true, true, true);
		towerPoints = new ReorderableList (serializedObject, serializedObject.FindProperty("towerPoints"), true, true, true, true);
		// waves = new ReorderableList (serializedObject, serializedObject.FindProperty("waves"), true, true, true, true);
		waveGroups = new ReorderableList (serializedObject, serializedObject.FindProperty("waveGroups"), true, true, true, true);
		

		//
		wayPoints.onRemoveCallback += OnRemoveCallBack;
		towerPoints.onRemoveCallback += OnRemoveCallBack;
		// waves.onRemoveCallback += OnRemoveCallBack;
		waveGroups.onRemoveCallback += OnRemoveCallBack;

		//
		wayPoints.drawElementCallback += OnDrawWayPointCallBack;
		towerPoints.drawElementCallback += OnDrawTowerPointCallBack;
		// waves.drawElementCallback += OnDrawCallBack;
		waveGroups.drawElementCallback += OnDrawWaveGroupCallBack;
	}

	void OnDisable () {
		if (wayPoints != null) wayPoints.onRemoveCallback -= OnRemoveCallBack;
		if (towerPoints != null) towerPoints.onRemoveCallback -= OnRemoveCallBack;
		// if (waves != null) waveGroups.onRemoveCallback -= OnRemoveCallBack;
		if (waveGroups != null) waveGroups.onRemoveCallback -= OnRemoveCallBack;
	}


	// TODO separate remove call back
	private void OnRemoveCallBack (ReorderableList list) {
		if (EditorUtility.DisplayDialog("Warning!", "Are you sure?", "Yes", "Hell No")) {
			ReorderableList.defaultBehaviours.DoRemoveButton (list);
		}
	}
	
	private void OnDrawWayPointCallBack (Rect rect, int index, bool isActive, bool isFocused) {
		var wp = wayPoints.serializedProperty.GetArrayElementAtIndex(index);
		EditorGUI.PropertyField (
			new Rect(rect.x, rect.y, 120, EditorGUIUtility.singleLineHeight),
			wp.FindPropertyRelative("id"),
			GUIContent.none
		);

		EditorGUI.PropertyField (
			new Rect(rect.x + 130 , rect.y, 320, EditorGUIUtility.singleLineHeight),
			wp.FindPropertyRelative("wayPointPosition"),
			GUIContent.none
		);
	}

	private void OnDrawTowerPointCallBack (Rect rect, int index, bool isActive, bool isFocused) {
		var tp = towerPoints.serializedProperty.GetArrayElementAtIndex(index);
		EditorGUI.PropertyField (
			new Rect(rect.x, rect.y, 120, EditorGUIUtility.singleLineHeight),
			tp.FindPropertyRelative("id"),
			GUIContent.none
		);

		EditorGUI.PropertyField (
			new Rect(rect.x + 130, rect.y, 320, EditorGUIUtility.singleLineHeight),
			tp.FindPropertyRelative("towerPointPosition"),
			GUIContent.none
		);
	}

	private void OnDrawWaveGroupCallBack (Rect rect, int index, bool isActive, bool isFocused) {
		// var props = new [] {"type", "amount", "spawnInterval", "waveDelay"};

		// for (int i = 0; i < props.Length; i++)
		// {
		// 	var sProp = serializedObject.FindProperty(props[i]);
		// 	var guiContent = new GUIContent ();
		// 	guiContent.text = sProp.displayName;
		// 	EditorGUILayout.PropertyField (sProp, guiContent);
		// }

		var wg = waveGroups.serializedProperty.GetArrayElementAtIndex(index);
		// GUIContent guiContent;
		EditorGUI.PropertyField (
			new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight),
			wg.FindPropertyRelative ("type"),
			GUIContent.none
		);

		EditorGUI.PropertyField (
			new Rect(rect.x + 100, rect.y, 60, EditorGUIUtility.singleLineHeight),
			wg.FindPropertyRelative ("amount"),
			GUIContent.none
		);

		EditorGUI.PropertyField (
			new Rect(rect.x + 160, rect.y, 60, EditorGUIUtility.singleLineHeight),
			wg.FindPropertyRelative ("spawnInterval"),
			GUIContent.none
		);

		EditorGUI.PropertyField (
			new Rect(rect.x + 220, rect.y, 60, EditorGUIUtility.singleLineHeight),
			wg.FindPropertyRelative ("waveDelay"),
			GUIContent.none
		);
	}
	public override void OnInspectorGUI (){
		// DrawDefaultInspector();	// test

		if(mapConstructor == null)
			return;

		serializedObject.Update ();

		wpCount = EditorPrefs.GetInt("WPC");
		tpCount = EditorPrefs.GetInt("TPC");
		wCount = EditorPrefs.GetInt("WC");

		
		
		EditorGUILayout.BeginVertical ("box");

		EditorGUILayout.Space();
		EditorGUILayout.LabelField ("MAP CONSTRUCTOR", guiTitleStyle);
		EditorGUILayout.Space();

		mapConstructor.mapId = EditorGUILayout.IntField ("Map Id", mapConstructor.mapId);
		
		EditorGUILayout.Space();

		#region waypoint 
		EditorGUI.indentLevel++;
		toggleWP = EditorGUILayout.Foldout(toggleWP, "Way Point");
		EditorPrefs.SetBool("TWP", toggleWP);
		if (toggleWP) {
			EditorGUI.indentLevel++;
			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Create A New Way Point")) {
				Debug.Log ("create a way point");
				mapConstructor.CreateNewWayPoint (wpCount);
				wpCount++;
				EditorPrefs.SetInt("WPC", wpCount);
			}
			// if (GUILayout.Button ("Remove Way Point")) {
			// 	Debug.Log ("remove a way point");
			// }
			if (GUILayout.Button ("Clear All Way Points")) {
				// Debug.Log ("clear all way points");
				mapConstructor.ClearAllWayPoints();
				if (EditorPrefs.HasKey("WPC"))
					EditorPrefs.SetInt("WPC", 0);
			}
			EditorGUILayout.EndHorizontal();
			wayPoints.DoLayoutList();
			EditorGUI.indentLevel--;
		}
		EditorGUI.indentLevel--;
		EditorGUILayout.Space();
		#endregion waypoint

		#region towerpoint
		EditorGUI.indentLevel++;
		toggleTP = EditorGUILayout.Foldout(toggleTP, "Tower Point");
		EditorPrefs.SetBool("TTP", toggleTP);
		if (toggleTP) {
			EditorGUI.indentLevel++;
			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Create A New Tower Point")) {
				// Debug.Log ("create a tower point");
				mapConstructor.CreateNewTowerPoint(tpCount);
				tpCount++;
				EditorPrefs.SetInt("TPC", tpCount);
			}
			// if (GUILayout.Button ("Remove Tower Point")) {
			// 	Debug.Log ("remove a tower point");
			// }
			if (GUILayout.Button ("Clear All Tower Points")) {
				// Debug.Log ("clear all tower points");
				mapConstructor.ClearAllTowerPoints();
				if (EditorPrefs.HasKey("TPC"))
					EditorPrefs.SetInt("TPC", 0);
			}
			EditorGUILayout.EndHorizontal();
			// custom reorderable list 
			towerPoints.DoLayoutList();
			EditorGUI.indentLevel--;
		}
		EditorGUI.indentLevel--;
		EditorGUILayout.Space();
		#endregion towerpoint

		EditorGUI.indentLevel++;
		toggleW = EditorGUILayout.Foldout(toggleW, "Wave");
		EditorPrefs.SetBool("TW", toggleW);
		if (toggleW) {
			EditorGUI.indentLevel++;
			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Create A New Wave")) {
				mapConstructor.CreateNewWave(wCount);
				wCount++;
				EditorPrefs.SetInt("WC", wCount);
			}
			if (GUILayout.Button ("Clear All Waves")) {
				mapConstructor.ClearAllWaves();
				if (EditorPrefs.HasKey("WC"))
					EditorPrefs.SetInt("WC", 0);
			}
			EditorGUILayout.EndHorizontal ();
			
			// for (int i = 0; i < mapConstructor.waves.Count; i++)
			// {
			// 	EditorGUILayout.LabelField(mapConstructor.waves[i].id.ToString());
			// }
			for(int i = 0; i < waves.arraySize; i++){
				SerializedProperty wg = waves.GetArrayElementAtIndex(i);
				SerializedProperty waveGroups = wg.FindPropertyRelative("waveGroups");
			}
			for(int i = 0; i < waves.arraySize; i++){
				 if(GUILayout.Button("Add New Wave To Sub List", GUILayout.MaxWidth(240),GUILayout.MaxHeight(20))){
                    waveGroups.InsertArrayElementAtIndex(waveGroups.arraySize);
                    waveGroups.GetArrayElementAtIndex(waveGroups.arraySize -1).intValue = 0;
			}
 
			for(int a = 0; a < MySubList.arraySize; a++){
                    EditorGUILayout.PropertyField(MySubList.GetArrayElementAtIndex(a));
                    if(GUILayout.Button("Remove  (" + a.ToString() + ") From Sub List",GUILayout.MaxWidth(150),GUILayout.MaxHeight(15))){
                        MySubList.DeleteArrayElementAtIndex(a);
                    }
			}
			 }
			
			// waves.DoLayoutList();
			// waveGroups.DoLayoutList();

			
			EditorGUI.indentLevel--;
		}
		EditorGUI.indentLevel--;
		EditorGUILayout.EndVertical ();

		#region map constructor window
		EditorGUILayout.BeginVertical ();
		if (GUILayout.Button ("Open Map Constructor Window")) {
				Debug.Log ("open map constructor window");
		}
		EditorGUILayout.EndVertical ();
		#endregion map constructor window

		#region data
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
		#endregion data
		

		if (GUI.changed)
			EditorUtility.SetDirty(mapConstructor);

		serializedObject.ApplyModifiedProperties();
	}
	
	public void OnSceneGUI (){
		if (mapConstructor == null)
			return;

		if(mapConstructor.wayPoints != null && mapConstructor.wayPoints.Count > 0) {
			// wp =
			for (int i = 0; i < mapConstructor.wayPoints.Count; i++)
			{
				// mapConstructor.wayPoints[i].wayPointGo.transform.position = 
			}
		} 
	}

	#region database
	const string mapGroupDataDirectory = "Assets/Resources/Maps";	
	public void WriteMapData (string data) {

		var path = EditorUtility.SaveFilePanel("Save Map Data", mapGroupDataDirectory, "map_"+mapConstructor.mapId+".txt", "txt");
		
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
	#endregion database
}
