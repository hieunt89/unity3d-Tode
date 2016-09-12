using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
// using UnityEditorInternal;

[CustomEditor (typeof(MapConstructor))]
public class MapConstructorEditor : Editor {
	bool togglePath;
	bool toggleTP;
	bool toggleW;
	private SerializedObject mc;
	private SerializedProperty _paths;	
	// private SerializedProperty _wayPoints;	
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
	#region MONO
	void OnEnable () {
		mapConstructor = (MapConstructor) target as MapConstructor;
		mc = new SerializedObject(mapConstructor);
        
		_paths = mc.FindProperty ("paths");
		// _wayPoints = mc.FindProperty ("wayPoints");
		_towerPoints = mc.FindProperty ("towerPoints");
		_waves = mc.FindProperty("waves");
		
		if (EditorPrefs.HasKey("TP"))
			togglePath = EditorPrefs.GetBool("TP");
		if (EditorPrefs.HasKey("TTP"))
			toggleTP = EditorPrefs.GetBool("TTP");
		if (EditorPrefs.HasKey("TW"))
			toggleW = EditorPrefs.GetBool("TW");

		GenerateStyle ();
	}
	#endregion MONO



	public override void OnInspectorGUI (){
		DrawDefaultInspector();	// test

		if(mapConstructor == null)
			return;
		
		mc.Update();
		
		

		EditorGUILayout.LabelField ("MAP CONSTRUCTOR", headerStyle, GUILayout.MinHeight (40));
		// EditorGUILayout.Space();

		EditorGUILayout.BeginVertical (bodyStyle);
		EditorGUILayout.Space();

		mapConstructor.mapId = EditorGUILayout.IntField ("Map Id", mapConstructor.mapId);
		mapConstructor.baseColor = EditorGUILayout.ColorField("Base Color", mapConstructor.baseColor);
		mapConstructor.pathColor = EditorGUILayout.ColorField("Path Color", mapConstructor.pathColor);
		mapConstructor.wayPointColor = EditorGUILayout.ColorField("Way Points Color", mapConstructor.wayPointColor);
		mapConstructor.towerPointColor = EditorGUILayout.ColorField("Tower Points Color", mapConstructor.towerPointColor);

		quickCreationMode = GUILayout.Toggle(quickCreationMode, "Interactive Creation", GUI.skin.button);
		if (quickCreationMode) {
			if (mapConstructor.gameObject.GetComponent <BoxCollider> () == null)
				mapConstructor.gameObject.AddComponent <BoxCollider> ().size = new Vector3(50f, 0.01f, 50f);
			EditorGUILayout.HelpBox ("- Left click on the plane to create a new WAY POINT.\n- Shift + left click on the plane to create a new TOWER POINT.\n-You should turn off Interactive Creation Mode to drag the points.", MessageType.Info, true);
		} 		
		EditorGUILayout.Space();

		OnPathInspectorGUI ();
		// OnWayPointInspectorGUI();
		EditorGUILayout.Space();
		
		OnTowerPointInspectorGUI();
		EditorGUILayout.Space();

		if (mapConstructor.paths.Count > 0) {
			OnWaveInspectorGUI();
			EditorGUILayout.Space();
		}

		OnDataInspectorGUI ();
		EditorGUILayout.EndVertical ();
		

		EditorGUILayout.LabelField ("fdj", footerStyle, GUILayout.MinHeight (20));
		EditorGUILayout.Space();
		
		mc.ApplyModifiedProperties();

		Repaint ();
	}
	#region Custom Inspector

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
		Handles.color = mapConstructor.baseColor;;
		Handles.Disc (mapConstructor.transform.rotation, mapConstructor.transform.position, Vector3.up, 1f, false, 1f);
		Handles.Label (mapConstructor.transform.position + new Vector3(0f, mapConstructor.pointSize, 0f), mapConstructor.name);

		// 2d gui on scene view
		Handles.BeginGUI();
		GUILayout.BeginArea(new Rect(10f, 10f, 80f, 80), GUI.skin.box);
		if (GUILayout.Button("Create WP")){
			// CreateWayPoint (new Vector3(mapConstructor.wayPoints.Count, 0f, 0f));
		}
		if (GUILayout.Button("Create TP")){
			CreateTowerPoint (new Vector3(mapConstructor.towerPoints.Count, 0f, 1f));
		}

		GUILayout.Space(5f);
		mapConstructor.pointSize = GUILayout.HorizontalSlider (mapConstructor.pointSize, 0f, mapConstructor.maxPointSize);
		GUILayout.EndArea();
		Handles.EndGUI(); 

		// render waypoints on scene view
		if (mapConstructor.paths.Count > 0){
			for (int i = 0; i < mapConstructor.paths.Count; i++)
			{
				if (mapConstructor.paths[i].points.Count > 0){
					for (int j = 0; j < mapConstructor.paths[i].points.Count; j++)
					{
						GameObject point = mapConstructor.paths[i].points[j].wayPointGo;
						
						if (point != null) {
							Handles.Label (point.transform.position + new Vector3(0f, mapConstructor.pointSize * .5f, 0f), point.name, new GUIStyle (GUI.skin.label));
							if (j == 0){
								Handles.color = Color.green;
							} else if (j == mapConstructor.paths[i].points.Count - 1) {
								Handles.color = Color.red;
							} else {
								Handles.color = mapConstructor.baseColor;
							}
							Handles.SphereCap (0, point.transform.position, point.transform.rotation, mapConstructor.pointSize * .5f);
							
							// draw line between way points
							if(j < mapConstructor.paths[i].points.Count - 1) {
								Handles.color = mapConstructor.pathColor;
								GameObject newPoint = mapConstructor.paths[i].points[j + 1].wayPointGo;
								Handles.DrawLine (point.transform.position, newPoint.transform.position);
								Handles.color = mapConstructor.baseColor;
							}
														
							point.transform.position = Handles.FreeMoveHandle (point.transform.position, Quaternion.identity, mapConstructor.pointSize * .5f, Vector3.one, Handles.RectangleCap);
						}
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
					Handles.color = mapConstructor.towerPointColor;
					Handles.CubeCap (0, towerPoint.transform.position, towerPoint.transform.rotation, mapConstructor.pointSize * .5f);
					Handles.color = mapConstructor.baseColor;;
					
					towerPoint.transform.position = Handles.FreeMoveHandle (towerPoint.transform.position, Quaternion.identity, mapConstructor.pointSize * .5f, Vector3.one, Handles.RectangleCap);
				}
			}
		}
	}

	GUIStyle headerStyle = new GUIStyle();
	GUIStyle bodyStyle = new GUIStyle();
	GUIStyle footerStyle = new GUIStyle();
	GUIStyle titleAStyle = new GUIStyle();
	GUIStyle titleBStyle = new GUIStyle();
	GUIStyle titleCStyle = new GUIStyle();

	void GenerateStyle () {
		Texture2D mBg = (Texture2D) Resources.Load ("map_constructor_bg");
		Texture2D mBg2 = (Texture2D) Resources.Load ("map_constructor_bg_2");
		Font mFont = (Font)Resources.Load("HELVETICANEUEBOLD");

		headerStyle.normal.background = mBg;
		headerStyle.font = mFont;
		headerStyle.fontSize = 32;
		headerStyle.normal.textColor = Color.white;
		headerStyle.alignment = TextAnchor.MiddleCenter;

		// bodyStyle.normal.background = mBg2;
		bodyStyle.fontSize = 12;
		bodyStyle.normal.textColor = Color.white;
		bodyStyle.overflow = new RectOffset(1,1,1,1);
		
		footerStyle.normal.background = mBg;
		footerStyle.font = mFont;
		footerStyle.fontSize = 12;
		footerStyle.normal.textColor = Color.white;
		footerStyle.alignment = TextAnchor.LowerRight;
		footerStyle.padding = new RectOffset(0, 2, 0, 2);

		titleAStyle.font = mFont;
		titleAStyle.fontSize = 25;
		titleAStyle.normal.textColor = Color.white;

		titleBStyle.font = mFont;
		titleBStyle.fontSize = 18;
		titleBStyle.normal.textColor = Color.white;

		titleCStyle.font = mFont;
		titleCStyle.fontSize = 12;
		titleCStyle.normal.textColor = Color.white;

	}

	private void OnPathInspectorGUI () {
		EditorGUILayout.BeginVertical("box");
		EditorGUI.indentLevel++;
		// togglePath = EditorGUILayout.Foldout(togglePath, "Paths");
		// EditorPrefs.SetBool("TP", togglePath);
		// if (togglePath) {
			// EditorGUI.indentLevel++;

			EditorGUILayout.LabelField("Path", titleBStyle);
			GUILayout.Space (5);
			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Create Path", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
				CreatePath ();
			}
			if (GUILayout.Button ("Clear Paths", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
				ClearPaths();
			}
			EditorGUILayout.EndHorizontal();
			
			if (_paths != null && _paths.arraySize > 0) {
				for(int i = 0; i < _paths.arraySize; i++){
					SerializedProperty path = _paths.GetArrayElementAtIndex(i);
					SerializedProperty wayPoints = path.FindPropertyRelative("points");

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField (path.FindPropertyRelative("pathId").intValue.ToString(), GUILayout.MinWidth (60), GUILayout.MaxWidth (100));
					// EditorGUILayout.LabelField ("Enemy Id", GUILayout.MinWidth (100), GUILayout.MaxWidth (100));
					// EditorGUILayout.LabelField ("Amount", GUILayout.MinWidth (80), GUILayout.MaxWidth (80));
					// EditorGUILayout.LabelField ("Interval", GUILayout.MinWidth (80), GUILayout.MaxWidth (80));
					// EditorGUILayout.LabelField ("Delay", GUILayout.MinWidth (80), GUILayout.MaxWidth (80));
					// EditorGUILayout.LabelField ("Path Id", GUILayout.MinWidth (100), GUILayout.MaxWidth (100));

					if(GUILayout.Button("Add WP", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
						GameObject wp = new GameObject("p" + i + "p" +mapConstructor.paths[i].points.Count);
						wp.transform.SetParent (mapConstructor.transform);
						wp.transform.position = new Vector3(mapConstructor.paths[i].points.Count, 0f, mapConstructor.paths[i].pathId);
						mapConstructor.paths[i].points.Add(new WayPointData(wp));
					}
					if(GUILayout.Button("Clear WPs", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {	
						for (int j = 0; j < mapConstructor.paths[i].points.Count; j++)
						{
							DestroyImmediate(mapConstructor.paths[i].points[j].wayPointGo);
						}
						mapConstructor.paths[i].points.Clear();			
					}

					EditorGUILayout.EndHorizontal();
					
					if (wayPoints.arraySize > 0) {
						for (int j = 0; j < wayPoints.arraySize; j++)
						{
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField ("p" + j, GUILayout.MinWidth (60), GUILayout.MaxWidth (100));
							// Debug.Log(wayPoints.arraySize);
							// Debug.Log(mapConstructor.paths[i].points.Count);
							// if (mapConstructor.paths[i].points[j].wayPointGo != null) {
							// 	EditorGUI.BeginChangeCheck();				
							// 	var pos = EditorGUILayout.Vector3Field ("Position", mapConstructor.paths[i].points[j].wayPointGo.transform.position);
							// 	if (EditorGUI.EndChangeCheck ()) {
							// 		Undo.RecordObject(mapConstructor,"Change Way Point Position");
							// 		mapConstructor.paths[i].points[j].wayPointGo.name = "wp" + i;
							// 		mapConstructor.paths[i].points[j].wayPointGo.transform.position = pos;
							// 		EditorUtility.SetDirty(mapConstructor);
							// 	}	
							// }
							
							if (GUILayout.Button("Remove", GUILayout.MinWidth (175), GUILayout.MaxWidth (175))) {
								DestroyImmediate(mapConstructor.paths[i].points[j].wayPointGo);
								mapConstructor.paths[i].points.RemoveAt(j);
							}
							EditorGUILayout.EndHorizontal();
						}
					}
				}
			// }
			// EditorGUI.indentLevel--;
		}
		EditorGUI.indentLevel--;
		EditorGUILayout.EndVertical();
	}

	private void OnWayPointInspectorGUI () {
		// EditorGUILayout.BeginVertical("box");
		// EditorGUI.indentLevel++;
		// toggleWP = EditorGUILayout.Foldout(toggleWP, "Way Point");
		// EditorPrefs.SetBool("TWP", toggleWP);
		// if (toggleWP) {
		// 	EditorGUI.indentLevel++;
		// 	EditorGUILayout.BeginHorizontal ();
		// 	if (GUILayout.Button ("Create WP", GUILayout.MaxWidth(100))) {
		// 		CreateWayPoint(new Vector3(mapConstructor.wayPoints.Count, 0f, 0f));
		// 	}
		// 	if (GUILayout.Button ("Clear WPs", GUILayout.MaxWidth(100))) {
		// 		ClearWayPoints ();
		// 	}
		// 	EditorGUILayout.EndHorizontal();
		// 	for (int i = 0; i < mapConstructor.wayPoints.Count; i++)
		// 	{
		// 		EditorGUI.BeginChangeCheck();				
		// 		var pos = EditorGUILayout.Vector3Field ("Position", mapConstructor.wayPoints[i].wayPointGo.transform.position);
		// 		if (EditorGUI.EndChangeCheck ()) {
		// 			Undo.RecordObject(mapConstructor,"Change Way Point Position");
		// 			mapConstructor.wayPoints[i].wayPointGo.name = "wp" + i;
		// 			mapConstructor.wayPoints[i].wayPointGo.transform.position = pos;
		// 			EditorUtility.SetDirty(mapConstructor);
		// 		}

		// 		if (GUILayout.Button("Remove")) {
		// 			DestroyImmediate(mapConstructor.wayPoints[i].wayPointGo);
		// 			mapConstructor.wayPoints.RemoveAt(i);
		// 		}
		// 	}
		// 	EditorGUI.indentLevel--;
		// }
		// EditorGUI.indentLevel--;
		// EditorGUILayout.EndVertical();
	}

	private void OnTowerPointInspectorGUI () {
		EditorGUILayout.BeginVertical("box");
		EditorGUI.indentLevel++;
		// toggleTP = EditorGUILayout.Foldout(toggleTP, "Tower Point");
		// EditorPrefs.SetBool("TTP", toggleTP);
		// if (toggleTP) {
		// 	EditorGUI.indentLevel++;
		EditorGUILayout.LabelField("Tower Point", titleBStyle);
		GUILayout.Space (5);

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Create TP", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
			CreateTowerPoint(new Vector3(mapConstructor.towerPoints.Count, 0f, -1f));
		}
		if (GUILayout.Button ("Clear TPs", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
			ClearTowerPoints();
		}
		EditorGUILayout.EndHorizontal();

		for (int i = 0; i < mapConstructor.towerPoints.Count; i++)
		{	
			// EditorGUILayout.BeginHorizontal();
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.LabelField ("t" + i, GUILayout.MinWidth (85), GUILayout.MaxWidth (85));
			
			var pos = EditorGUILayout.Vector3Field ("Pos", mapConstructor.towerPoints[i].towerPointGo.transform.position);
			if (EditorGUI.EndChangeCheck ()) {
				mapConstructor.towerPoints[i].id = "t"+i;
				mapConstructor.towerPoints[i].towerPointGo.name = "t"+i;
				mapConstructor.towerPoints[i].towerPointGo.transform.position = pos;
				EditorUtility.SetDirty(mapConstructor);
			}

			if (GUILayout.Button("Remove",GUILayout.MinWidth (175), GUILayout.MaxWidth (175))) {
				DestroyImmediate(mapConstructor.towerPoints[i].towerPointGo);
				mapConstructor.towerPoints.RemoveAt(i);
			}
			// EditorGUILayout.EndHorizontal();
		}
			// EditorGUI.indentLevel--;
		// }
		EditorGUI.indentLevel--;
		EditorGUILayout.EndVertical();
	}
	
	private void OnWaveInspectorGUI () {
		EditorGUILayout.BeginVertical("box");
		EditorGUI.indentLevel++;
		// toggleW = EditorGUILayout.Foldout(toggleW, "Wave");
		// EditorPrefs.SetBool("TW", toggleW);
		// if (toggleW) {
		// 	EditorGUI.indentLevel++;
		EditorGUILayout.LabelField("Wave", titleBStyle);
		GUILayout.Space (5);
			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Create Wave", GUILayout.MinWidth (100), GUILayout.MaxWidth (100))) {
				CreateWave();
			}
			if (GUILayout.Button ("Clear Waves", GUILayout.MinWidth (100), GUILayout.MaxWidth (100))) {
				ClearWaves();
			}
			EditorGUILayout.EndHorizontal ();
			
			// render wave data
			if (_waves != null && _waves.arraySize > 0) {
				for(int i = 0; i < _waves.arraySize; i++){
					SerializedProperty wave = _waves.GetArrayElementAtIndex(i);
					SerializedProperty groups = wave.FindPropertyRelative("groups");

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField (wave.FindPropertyRelative("waveId").intValue.ToString(), GUILayout.MinWidth (60), GUILayout.MaxWidth (100));
					EditorGUILayout.LabelField ("Enemy Id", GUILayout.MinWidth (100), GUILayout.MaxWidth (100));
					EditorGUILayout.LabelField ("Amount", GUILayout.MinWidth (100), GUILayout.MaxWidth (100));
					EditorGUILayout.LabelField ("Interval", GUILayout.MinWidth (100), GUILayout.MaxWidth (100));
					EditorGUILayout.LabelField ("Delay", GUILayout.MinWidth (100), GUILayout.MaxWidth (100));
					EditorGUILayout.LabelField ("Path Id", GUILayout.MinWidth (100), GUILayout.MaxWidth (100));

					if(GUILayout.Button("Add Group", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
						groups.InsertArrayElementAtIndex(groups.arraySize);	
					}
					if(GUILayout.Button("Clear Groups", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {	
						groups.ClearArray();			
					}
					EditorGUILayout.EndHorizontal();

					// render wave groups
					// EditorGUI.indentLevel++;
					if (groups.arraySize > 0) {
						for (int j = 0; j < groups.arraySize; j++)
						{
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField ("G" + j, GUILayout.MinWidth (60), GUILayout.MaxWidth (100));
							
							// TODO: add enemy id 														
							SerializedProperty amountProp = groups.GetArrayElementAtIndex(j).FindPropertyRelative("amount");
							SerializedProperty spawnIntervalProp = groups.GetArrayElementAtIndex(j).FindPropertyRelative("spawnInterval");
							SerializedProperty waveDelayProp = groups.GetArrayElementAtIndex(j).FindPropertyRelative("waveDelay");
							SerializedProperty pathIdProp = groups.GetArrayElementAtIndex(j).FindPropertyRelative("pathId");

							// TODO: handle change data on inspector
							EditorGUI.BeginChangeCheck();
							var enemyId = EditorGUILayout.Popup (0, new string[3]{"e01", "e02", "e03"}, GUILayout.MinWidth (100), GUILayout.MaxWidth (100)); // test
							var amount = EditorGUILayout.IntField (amountProp.intValue, GUILayout.MinWidth (100), GUILayout.MaxWidth (100));
							var spawnInterval = EditorGUILayout.FloatField (spawnIntervalProp.floatValue, GUILayout.MinWidth (100), GUILayout.MaxWidth (100));
							var waveDelay = EditorGUILayout.FloatField (waveDelayProp.floatValue, GUILayout.MinWidth (100), GUILayout.MaxWidth (100));
							var pathId = EditorGUILayout.Popup (0, new string[3]{"p01", "p02", "p03"}, GUILayout.MinWidth (100), GUILayout.MaxWidth (100));  // test
							if (EditorGUI.EndChangeCheck()){
								
							}

							if(GUILayout.Button("Remove", GUILayout.MinWidth (175), GUILayout.MaxWidth (175))) {
								groups.DeleteArrayElementAtIndex(j);
							}
							EditorGUILayout.EndHorizontal();
						}
					}
					// EditorGUI.indentLevel--;
				}
			}
		// 	EditorGUI.indentLevel--;
		// }
		EditorGUI.indentLevel--;
		EditorGUILayout.EndVertical();
	}

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
			ResetData();
			EditorPrefs.SetInt("WPC", 0);
			EditorPrefs.SetInt("TPC", 0);
			EditorPrefs.SetInt("WC", 0);

			mc.ApplyModifiedProperties();
			// TODO: confirm windows
		}
		EditorGUILayout.EndHorizontal();
	}
	#endregion Custom Inspector

	#region private methods
	void CheckRay (Ray ray) {
		RaycastHit hit = (RaycastHit) HandleUtility.RaySnap (ray);
		if (hit.transform != null){
			if (currentEvent.shift) {
				CreateTowerPoint(hit.point);
			} else {
				// CreateWayPoint(hit.point);
			}
		}
	}

	private void CreatePath () {
		mapConstructor.paths.Add(new PathData(mapConstructor.paths.Count, new List<WayPointData> ()));
	}

	private void ClearPaths () {
		mapConstructor.paths.Clear ();
	}

	// private void CreateWayPoint (Vector3 position) {
	// 	GameObject wp = new GameObject ("wp" + (mapConstructor.wayPoints.Count+1).ToString());
	// 	if (wp != null) {
	// 		wp.transform.SetParent (mapConstructor.transform);
	// 		wp.transform.position = position;
	// 		mapConstructor.wayPoints.Add (new WayPointData(wp.name, wp));
	// 	}
	// }

	// private void ClearWayPoints() {
	// 	if (mapConstructor.wayPoints != null && mapConstructor.wayPoints.Count > 0){
	// 		for (int i = 0; i < mapConstructor.wayPoints.Count; i++)
	// 		{
	// 			DestroyImmediate (mapConstructor.wayPoints[i].wayPointGo);
	// 		}
	// 	}
	// 	mapConstructor.wayPoints.Clear ();
	// }

	private void CreateTowerPoint (Vector3 position) {
		GameObject tp = new GameObject ("tp" + (mapConstructor.towerPoints.Count+1));
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
	
	private void CreateWave(){
		mapConstructor.waves.Add (new WaveData(mapConstructor.waves.Count, new List<WaveGroup> ()));
	}
	private void ClearWaves() {
		mapConstructor.waves.Clear();
	}

	private void ResetData() {
		// ClearWayPoints();
		ClearTowerPoints();
		ClearWaves();
	}
	#endregion private methods

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
