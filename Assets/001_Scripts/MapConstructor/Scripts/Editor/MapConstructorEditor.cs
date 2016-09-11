using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
// using UnityEditorInternal;

[CustomEditor (typeof(MapConstructor))]
public class MapConstructorEditor : Editor {
	// TODO: make map constructor window
	bool toggleWP;
	bool toggleTP;
	bool toggleW;
	// private ReorderableList wpROL;
	// private ReorderableList tpROL;
	// private ReorderableList waves;
	// private ReorderableList wgROL;
	private SerializedObject mc;
	private SerializedProperty _wayPoints;	
	private SerializedProperty _towerPoints;	
	private SerializedProperty _waves;	
		
	private MapConstructor mapConstructor;
	private Event currentEvent;
	private bool quickCreationMode = false;
	private bool resetTool = false;

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
		mapConstructor = (MapConstructor) target as MapConstructor;
		mc = new SerializedObject(mapConstructor);
        
		_wayPoints = mc.FindProperty ("wayPoints");
		_towerPoints = mc.FindProperty ("towerPoints");
		_waves = mc.FindProperty("waves");
		
		if (EditorPrefs.HasKey("TWP"))
			toggleWP = EditorPrefs.GetBool("TWP");
		if (EditorPrefs.HasKey("TTP"))
			toggleTP = EditorPrefs.GetBool("TTP");
		if (EditorPrefs.HasKey("TW"))
			toggleW = EditorPrefs.GetBool("TW");

		// wpROL = new ReorderableList (mc, _wayPoints, true, true, true, true);
		// tpROL = new ReorderableList (mc, _towerPoints, true, true, true, true);
		// waves = new ReorderableList (mc, mc.FindProperty("waves"), true, true, true, true);
		// wgROL = new ReorderableList (mc, mc.FindProperty("waveGroups"), true, true, true, true);	// test
		

		//
		// wpROL.onRemoveCallback += OnRemoveCallBack;
		// tpROL.onRemoveCallback += OnRemoveCallBack;
		// waves.onRemoveCallback += OnRemoveCallBack;
		// wgROL.onRemoveCallback += OnRemoveCallBack;

		//
		// wpROL.drawElementCallback += OnDrawWayPointElementCallBack;
		// tpROL.drawElementCallback += OnDrawTowerPointElementCallBack;
		// waves.drawElementCallback += OnDrawWaveCallBack;
		// wgROL.drawElementCallback += OnDrawWaveGroupElementCallBack;

		// wpROL.drawHeaderCallback += OnDrawWayPointHeaderCallBack;
		// tpROL.drawHeaderCallback += OnDrawTowerPointHeaderCallBack;
		// wgROL.drawHeaderCallback += OnDrawWaveGroupHeaderCallBack;
	}

	void OnDisable () {
		// if (wpROL != null) wpROL.onRemoveCallback -= OnRemoveCallBack;
		// if (tpROL != null) tpROL.onRemoveCallback -= OnRemoveCallBack;
		// if (waves != null) waveGroups.onRemoveCallback -= OnRemoveCallBack;
		// if (wgROL != null) wgROL.onRemoveCallback -= OnRemoveCallBack;
	}


	// TODO separate remove call back
	// private void OnRemoveCallBack (ReorderableList _list) {
	// 	if (EditorUtility.DisplayDialog("Warning!", "Are you sure?", "Yes", "No")) {
	// 		ReorderableList.defaultBehaviours.DoRemoveButton (_list);
	// 	}
	// }
	
	// private void OnDrawWayPointElementCallBack (Rect _rect, int _index, bool _isActive, bool _isFocused) {
	// 	var wp = wpROL.serializedProperty.GetArrayElementAtIndex(_index);
	// 	EditorGUI.PropertyField (
	// 		new Rect(_rect.x, _rect.y, 120, EditorGUIUtility.singleLineHeight),
	// 		wp.FindPropertyRelative("id"),
	// 		GUIContent.none
	// 	);

	// 	EditorGUI.PropertyField (
	// 		new Rect(_rect.x + 130 , _rect.y, 320, EditorGUIUtility.singleLineHeight),
	// 		wp.FindPropertyRelative("wayPointPosition"),
	// 		GUIContent.none
	// 	);
	// }

	// private void OnDrawTowerPointElementCallBack (Rect _rect, int _index, bool _isActive, bool _isFocused) {
	// 	var tp = tpROL.serializedProperty.GetArrayElementAtIndex(_index);
	// 	EditorGUI.PropertyField (
	// 		new Rect(_rect.x, _rect.y, 120, EditorGUIUtility.singleLineHeight),
	// 		tp.FindPropertyRelative("id"),
	// 		GUIContent.none
	// 	);

	// 	EditorGUI.PropertyField (
	// 		new Rect(_rect.x + 130, _rect.y, 320, EditorGUIUtility.singleLineHeight),
	// 		tp.FindPropertyRelative("towerPointPosition"),
	// 		GUIContent.none
	// 	);
	// }

	// private void OnDrawWaveCallBack (Rect rect, int index, bool isActive, bool isFocused) {
	// 	var w = waves.serializedProperty.GetArrayElementAtIndex(index);
	// 	EditorGUI.PropertyField (
	// 		new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight),
	// 		w.FindPropertyRelative ("id"),
	// 		GUIContent.none
	// 	);
	// 	waveGroups.DoLayoutList ();
	// }
	// private void OnDrawWaveGroupElementCallBack (Rect _rect, int _index, bool _isActive, bool _isFocused) {
	// 	// var props = new [] {"type", "amount", "spawnInterval", "waveDelay"};
	// 	// for (int i = 0; i < props.Length; i++)
	// 	// {
	// 	// 	var sProp = mc.FindProperty(props[i]);
	// 	// 	var guiContent = new GUIContent ();
	// 	// 	guiContent.text = sProp.displayName;
	// 	// 	EditorGUILayout.PropertyField (sProp, guiContent);
	// 	// }

	// 	var wg = wgROL.serializedProperty.GetArrayElementAtIndex(_index);
	// 	// GUIContent guiContent;
	// 	EditorGUI.PropertyField (
	// 		new Rect(_rect.x, _rect.y, 100, EditorGUIUtility.singleLineHeight),
	// 		wg.FindPropertyRelative ("type"),
	// 		GUIContent.none
	// 	);

	// 	EditorGUI.PropertyField (
	// 		new Rect(_rect.x + 100, _rect.y, 60, EditorGUIUtility.singleLineHeight),
	// 		wg.FindPropertyRelative ("pathId"),
	// 		GUIContent.none
	// 	);

	// 	EditorGUI.PropertyField (
	// 		new Rect(_rect.x + 160, _rect.y, 60, EditorGUIUtility.singleLineHeight),
	// 		wg.FindPropertyRelative ("amount"),
	// 		GUIContent.none
	// 	);

	// 	EditorGUI.PropertyField (
	// 		new Rect(_rect.x + 220, _rect.y, 60, EditorGUIUtility.singleLineHeight),
	// 		wg.FindPropertyRelative ("spawnInterval"),
	// 		GUIContent.none
	// 	);

	// 	EditorGUI.PropertyField (
	// 		new Rect(_rect.x + 280, _rect.y, 60, EditorGUIUtility.singleLineHeight),
	// 		wg.FindPropertyRelative ("waveDelay"),
	// 		GUIContent.none
	// 	);
	// }

	// private void OnDrawWayPointHeaderCallBack(Rect rect){
	// 	EditorGUI.LabelField (rect, "Way Point List");
	// }
	// private void OnDrawTowerPointHeaderCallBack(Rect rect){
	// 	EditorGUI.LabelField (rect, "Tower Point List");
	// }
	// private void OnDrawWaveGroupHeaderCallBack(Rect rect){
	// 	EditorGUI.LabelField (rect, "Wave Group List");
	// }

	public override void OnInspectorGUI (){
		DrawDefaultInspector();	// test

		if(mapConstructor == null)
			return;
		
		mc.Update();
		
		EditorGUILayout.BeginVertical ("box");
		EditorGUILayout.Space();

		EditorGUILayout.LabelField ("MAP CONSTRUCTOR", guiTitleStyle);
		EditorGUILayout.Space();

		mapConstructor.mapId = EditorGUILayout.IntField ("Map Id", mapConstructor.mapId);

		quickCreationMode = GUILayout.Toggle(quickCreationMode, "Interactive Creation", GUI.skin.button);
		if (quickCreationMode) {
			if (mapConstructor.gameObject.GetComponent <BoxCollider> () == null)
				mapConstructor.gameObject.AddComponent <BoxCollider> ().size = new Vector3(50f, 0.01f, 50f);
			EditorGUILayout.HelpBox ("- Left click on the plane to create a new WAY POINT.\n- Shift + left click on the plane to create a new TOWER POINT.\n-You should turn off Interactive Creation Mode to drag the points.", MessageType.Info, true);
		} 		

		EditorGUILayout.Space();

		OnWayPointInspectorGUI();
		OnTowerPointInspectorGUI();
		if (mapConstructor.wayPoints.Count > 0) {
			OnWaveInspectorGUI();
		}

		EditorGUILayout.EndVertical ();
		OnDataInspectorGUI ();
		mc.ApplyModifiedProperties();

		Repaint ();
	}
	
	public void OnSceneGUI (){
		if (mapConstructor == null)
			return;
		
		currentEvent = Event.current;
	
		if (currentEvent.isKey && currentEvent.type == EventType.KeyDown && currentEvent.character == 'l'){
			quickCreationMode = !quickCreationMode;
		}

		if (quickCreationMode) {
			Tools.current = Tool.None;
			HandleUtility.AddDefaultControl (GUIUtility.GetControlID(FocusType.Passive));
			resetTool = false;

			// get position of mouse and find world hit point;
			Vector2 mousePos = currentEvent.mousePosition;
			Ray ray = HandleUtility.GUIPointToWorldRay(mousePos); 
			if (currentEvent.isMouse && currentEvent.type == EventType.MouseDown && currentEvent.button == 0 && !currentEvent.alt) {
				CheckRay(ray);
			}	
		} else {
			if (!resetTool)
				Tools.current = Tool.Move;
				resetTool = true;
		}
		Handles.color = mapConstructor.pointColor;;
		Handles.Disc (mapConstructor.transform.rotation, mapConstructor.transform.position, Vector3.up, 1f, false, 1f);
		Handles.Label (mapConstructor.transform.position + new Vector3(0f, mapConstructor.pointSize, 0f), mapConstructor.name);

		// 2d gui on scene view
		Handles.BeginGUI();
		GUILayout.BeginArea(new Rect(10f, 10f, 80f, 80), GUI.skin.box);
		if (GUILayout.Button("Create WP")){
			CreateWayPoint (new Vector3(mapConstructor.wayPoints.Count, 0f, 0f));
		}
		if (GUILayout.Button("Create TP")){
			CreateTowerPoint (new Vector3(mapConstructor.towerPoints.Count, 0f, 1f));
		}

		GUILayout.Space(5f);
		mapConstructor.pointSize = GUILayout.HorizontalSlider (mapConstructor.pointSize, 0f, mapConstructor.maxPointSize);
		GUILayout.EndArea();
		Handles.EndGUI(); 

		// render waypoints on scene view
		if (mapConstructor.wayPoints.Count > 0){
			for (int i = 0; i < mapConstructor.wayPoints.Count; i++)
			{
				GameObject wayPoint = mapConstructor.wayPoints[i].wayPointGo;
				 
				if (wayPoint != null) {
					Handles.Label (wayPoint.transform.position + new Vector3(0f, mapConstructor.pointSize * .5f, 0f), wayPoint.name, new GUIStyle (GUI.skin.label));
					if (i == 0){
						Handles.color = Color.green;
					} else if (i == mapConstructor.wayPoints.Count - 1) {
						Handles.color = Color.red;
					}
					Handles.SphereCap (0, wayPoint.transform.position, wayPoint.transform.rotation, mapConstructor.pointSize * .5f);
					Handles.color = mapConstructor.pointColor;;
					
					wayPoint.transform.position = Handles.FreeMoveHandle (wayPoint.transform.position, Quaternion.identity, mapConstructor.pointSize * .5f, Vector3.one, Handles.RectangleCap);
					if(i < mapConstructor.wayPoints.Count - 1) {
						Handles.color = Color.yellow;
						GameObject newWayPoint = mapConstructor.wayPoints[i + 1].wayPointGo;
						Handles.DrawLine (wayPoint.transform.position, newWayPoint.transform.position);
						Handles.color = mapConstructor.pointColor;;
					}
				}

			}
		}

		// render towerpoints on scene view
		if (mapConstructor.towerPoints.Count > 0){
			for (int i = 0; i < mapConstructor.towerPoints.Count; i++)
			{
				GameObject towerPoint = mapConstructor.towerPoints[i].towerPointGo;
				 
				if (towerPoint != null) {
					Handles.Label (towerPoint.transform.position + new Vector3(0f, mapConstructor.pointSize * .5f, 0f), towerPoint.name, new GUIStyle (GUI.skin.label));
					Handles.color = Color.blue;
					Handles.CubeCap (0, towerPoint.transform.position, towerPoint.transform.rotation, mapConstructor.pointSize * .5f);
					Handles.color = mapConstructor.pointColor;;
					
					towerPoint.transform.position = Handles.FreeMoveHandle (towerPoint.transform.position, Quaternion.identity, mapConstructor.pointSize * .5f, Vector3.one, Handles.RectangleCap);
				}
			}
		}
	}

	#region private methods
	void CheckRay (Ray ray) {
		RaycastHit hit = (RaycastHit) HandleUtility.RaySnap (ray);
		if (hit.transform != null){
			if (currentEvent.shift) {
				CreateTowerPoint(hit.point);
			} else {
				CreateWayPoint(hit.point);
			}
		}
	}
	private void CreateWayPoint (Vector3 position) {
		GameObject wp = new GameObject ("wp_" + (mapConstructor.wayPoints.Count+1).ToString());
		if (wp != null) {
			wp.transform.SetParent (mapConstructor.transform);
			wp.transform.position = position;
			mapConstructor.wayPoints.Add (new WayPointData(wp.name, wp));
		}
	}

	private void ClearWayPoints() {
		for (int i = 0; i < mapConstructor.wayPoints.Count; i++)
		{
			DestroyImmediate (mapConstructor.wayPoints[i].wayPointGo);
		}
		mapConstructor.wayPoints.Clear ();
	}

	private void CreateTowerPoint (Vector3 position) {
		GameObject tp = new GameObject ("tp_" + (mapConstructor.towerPoints.Count+1).ToString());
		if (tp != null) {
			tp.transform.SetParent (mapConstructor.transform);
			tp.transform.position = position;
			mapConstructor.towerPoints.Add (new TowerPointData(tp.name, tp));
		}
		
	}

	private void ClearTowerPoints() {
		for (int i = 0; i < mapConstructor.towerPoints.Count; i++)
		{
			DestroyImmediate (mapConstructor.towerPoints[i].towerPointGo);
		}
		mapConstructor.towerPoints.Clear ();
	}
	#endregion private methods

	#region waypoint 
	private void OnWayPointInspectorGUI () {
		
		EditorGUI.indentLevel++;
		toggleWP = EditorGUILayout.Foldout(toggleWP, "Way Point");
		EditorPrefs.SetBool("TWP", toggleWP);
		if (toggleWP) {
			EditorGUI.indentLevel++;
			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Create WP", GUILayout.MaxWidth(100))) {
				CreateWayPoint(new Vector3(mapConstructor.wayPoints.Count, 0f, 0f));
			}
			if (GUILayout.Button ("Clear WPs", GUILayout.MaxWidth(100))) {
				ClearWayPoints ();
			}
			EditorGUILayout.EndHorizontal();
			for (int i = 0; i < mapConstructor.wayPoints.Count; i++)
			{
				EditorGUI.BeginChangeCheck();
				var id = EditorGUILayout.TextField ("Id", mapConstructor.wayPoints[i].wayPointGo.name);;
				
				var pos = EditorGUILayout.Vector3Field ("Position", mapConstructor.wayPoints[i].wayPointGo.transform.position);
				if (EditorGUI.EndChangeCheck ()) {
					mapConstructor.wayPoints[i].id = id;
					mapConstructor.wayPoints[i].wayPointGo.name = id;
					mapConstructor.wayPoints[i].wayPointGo.transform.position = pos;
					EditorUtility.SetDirty(mapConstructor);
				}

				if (GUILayout.Button("Remove")) {
					DestroyImmediate(mapConstructor.wayPoints[i].wayPointGo);
					mapConstructor.wayPoints.RemoveAt(i);
				}
			}
			EditorGUI.indentLevel--;
		}
		EditorGUI.indentLevel--;
		EditorGUILayout.Space();
	}
	#endregion waypoint

	#region towerpoint
	private void OnTowerPointInspectorGUI () {
		EditorGUI.indentLevel++;
		toggleTP = EditorGUILayout.Foldout(toggleTP, "Tower Point");
		EditorPrefs.SetBool("TTP", toggleTP);
		if (toggleTP) {
			EditorGUI.indentLevel++;
			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Create TP", GUILayout.MaxWidth(100))) {
				CreateTowerPoint(new Vector3(mapConstructor.towerPoints.Count, 0f, 1f));
			}
			if (GUILayout.Button ("Clear TPs", GUILayout.MaxWidth(100))) {
				ClearTowerPoints();
			}
			EditorGUILayout.EndHorizontal();

			for (int i = 0; i < mapConstructor.towerPoints.Count; i++)
			{
				EditorGUI.BeginChangeCheck();
				var id = EditorGUILayout.TextField ("Id", mapConstructor.towerPoints[i].towerPointGo.name);;
				
				var pos = EditorGUILayout.Vector3Field ("Position", mapConstructor.towerPoints[i].towerPointGo.transform.position);
				if (EditorGUI.EndChangeCheck ()) {
					mapConstructor.towerPoints[i].id = id;
					mapConstructor.towerPoints[i].towerPointGo.name = id;
					mapConstructor.towerPoints[i].towerPointGo.transform.position = pos;
					EditorUtility.SetDirty(mapConstructor);
				}

				if (GUILayout.Button("Remove", GUILayout.MaxWidth (100))) {
					DestroyImmediate(mapConstructor.towerPoints[i].towerPointGo);
					mapConstructor.towerPoints.RemoveAt(i);
				}
			}
			EditorGUI.indentLevel--;
		}
		EditorGUI.indentLevel--;
		EditorGUILayout.Space();
		
	}
	#endregion towerpoint

	#region wave
	private void OnWaveInspectorGUI () {
		EditorGUI.indentLevel++;
		toggleW = EditorGUILayout.Foldout(toggleW, "Wave");
		EditorPrefs.SetBool("TW", toggleW);
		if (toggleW) {
			EditorGUI.indentLevel++;
			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Create Wave", GUILayout.MaxWidth (100))) {
				mapConstructor.waves.Add (new WaveData(mapConstructor.waves.Count, new List<WaveGroup> ()));
			}
			if (GUILayout.Button ("Clear Waves", GUILayout.MaxWidth (100))) {
				mapConstructor.waves.Clear();
			}
			EditorGUILayout.EndHorizontal ();
			
			// display wave data
			if (_waves != null && _waves.arraySize > 0) {
				for(int i = 0; i < _waves.arraySize; i++){
					SerializedProperty wave = _waves.GetArrayElementAtIndex(i);
					SerializedProperty groups = wave.FindPropertyRelative("groups");

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField (wave.FindPropertyRelative("id").intValue.ToString());

					if(GUILayout.Button("Add Group", GUILayout.MaxWidth (100))) {
						groups.InsertArrayElementAtIndex(groups.arraySize);	
					}
					if(GUILayout.Button("Clear Groups", GUILayout.MaxWidth (100))) {	
						groups.ClearArray();			
					}
					EditorGUILayout.EndHorizontal();

					// render wave groups
					EditorGUI.indentLevel++;
					if (groups.arraySize > 0) {
						for (int j = 0; j < groups.arraySize; j++)
						{
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField (j.ToString());
							if(GUILayout.Button("Remove Group", GUILayout.MaxWidth (100))) {
								groups.DeleteArrayElementAtIndex(j);
							}
							EditorGUILayout.EndHorizontal();
						}
					}
					EditorGUI.indentLevel--;
				}
			}
			EditorGUI.indentLevel--;
		}
		EditorGUI.indentLevel--;
	}
	#endregion wave
	
	#region data
	private void OnDataInspectorGUI () {
		// TODO: save and load map data to xml 
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Save")) {
			var text = mapConstructor.Save();
			WriteMapData(text);
			// TODO: 
			// - [x]confirm windows 
			// - kiem tra cac truong co empty khong
		}
		if (GUILayout.Button ("Load")) {
			mapConstructor.Load (LoadMapData());
			// TODO: confirm windows
		}
		if (GUILayout.Button ("Reset")) {
			mapConstructor.Reset();
			EditorPrefs.SetInt("WPC", 0);
			EditorPrefs.SetInt("TPC", 0);
			EditorPrefs.SetInt("WC", 0);

			mc.ApplyModifiedProperties();
			// TODO: confirm windows
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
	}
	#endregion data

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
