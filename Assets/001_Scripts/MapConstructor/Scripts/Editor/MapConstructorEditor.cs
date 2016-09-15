using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

[CustomEditor (typeof(MapConstructor))]
[CanEditMultipleObjects]
public class MapConstructorEditor : Editor {
	private List<bool> toggleWayPoints;
	private List<bool> toggleWaveGroups;
	private SerializedObject mc;
		
	private MapConstructor mapConstructor;
	private Event currentEvent;
	private bool quickCreationMode = false;
	private bool resetTool = false;

	string[] enemyIdOptions = new string[] {"e01", "e02", "e03"};	// test	load data from json then create string[]
	List<List<int>> enemyIndexes = new List<List<int>> ();
	string[] pathIdOptions = new string[] {"p01", "p02", "p03"};	// test
	List<List<int>> pathIndexes = new List<List<int>> ();
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
        
		toggleWayPoints = new List<bool> (mapConstructor.paths.Count);
		for (int i = 0; i < mapConstructor.paths.Count; i++)
		{
			toggleWayPoints.Add(false);
		}

		toggleWaveGroups = new List<bool> (mapConstructor.waves.Count);
		for (int i = 0; i < mapConstructor.waves.Count; i++)
		{
			toggleWaveGroups.Add(false);
		}
		if (mapConstructor.waves.Count > 0) {
			for (int i = 0; i < mapConstructor.waves.Count; i++)
			{
				enemyIndexes.Add(new List<int> ());
				pathIndexes.Add (new List<int> ());
				for (int j = 0; j < mapConstructor.waves[i].groups.Count; j++)
				{
					enemyIndexes[i].Add (mapConstructor.waves[i].groups[j].EId);
					pathIndexes[i].Add (mapConstructor.waves[i].groups[j].PId);
				}
			}
		}
		GenerateStyle ();
		ConvertDataToJson ();
	}
	#endregion MONO

	public override void OnInspectorGUI (){
		// DrawDefaultInspector();	// test

		if(mapConstructor == null)
			return;
		
		mc.Update();
		
		EditorGUILayout.LabelField ("MAP CONSTRUCTOR", headerStyle, GUILayout.MinHeight (40));
		// EditorGUILayout.Space();

		EditorGUILayout.BeginVertical ();
		EditorGUILayout.Space();

		EditorGUI.BeginChangeCheck();
		var mapId = EditorGUILayout.IntField ("Map Id", mapConstructor.mapId);
		var pointSize = EditorGUILayout.Slider("Point Size", mapConstructor.pointSize, 0f, mapConstructor.maxPointSize);
		var baseColor = EditorGUILayout.ColorField("Base Color", mapConstructor.baseColor);
		var pathColor = EditorGUILayout.ColorField("Path Color", mapConstructor.pathColor);
		var wayPointColor = EditorGUILayout.ColorField("Way Points Color", mapConstructor.wayPointColor);
		var towerPointColor = EditorGUILayout.ColorField("Tower Points Color", mapConstructor.towerPointColor);
		if(EditorGUI.EndChangeCheck()){
			mapConstructor.mapId = mapId;
			mapConstructor.pointSize = pointSize;
			mapConstructor.baseColor = baseColor;
			mapConstructor.pathColor = pathColor;
			mapConstructor.wayPointColor = wayPointColor;
			mapConstructor.towerPointColor = towerPointColor;
			EditorUtility.SetDirty(mapConstructor);
		}

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
		Handles.Label (mapConstructor.transform.position + new Vector3(mapConstructor.pointSize, mapConstructor.pointSize, mapConstructor.pointSize), mapConstructor.name);

		// 2d gui on scene view
		Handles.BeginGUI();
		GUILayout.BeginArea(new Rect(10f, 10f, 180f, 28f), GUI.skin.box);
		// if (GUILayout.Button("Create WP")){
		// 	// CreateWayPoint (new Vector3(mapConstructor.wayPoints.Count, 0f, 0f));	// TODO: get current path id
		// }
		// if (GUILayout.Button("Create TP")){
		// 	CreateTowerPoint (new Vector3(mapConstructor.towerPoints.Count, 0f, 1f));
		// }

		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		GUILayout.Label ("Point Size");
		mapConstructor.pointSize = GUILayout.HorizontalSlider (mapConstructor.pointSize, 0f, mapConstructor.maxPointSize, GUILayout.MinWidth(100));
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		Handles.EndGUI(); 

		// render waypoints on scene view
		if (mapConstructor.paths.Count > 0){
			for (int i = 0; i < mapConstructor.paths.Count; i++)
			{
				if (mapConstructor.paths[i].Points.Count > 0){
					for (int j = 0; j < mapConstructor.paths[i].Points.Count; j++)
					{
						Vector3 point = mapConstructor.paths[i].Points[j].WayPointPos;
						
						if (point != null) {
							// TODO: waypoint data need and id to set a label for it view
							Handles.Label (point + new Vector3(mapConstructor.pointSize * .5f, mapConstructor.pointSize * .5f, mapConstructor.pointSize * .5f), i+"-"+j, titleAStyle);
							if (j == 0){
								Handles.color = Color.green;
							} else if (j == mapConstructor.paths[i].Points.Count - 1) {
								Handles.color = Color.red;
							} else {
								Handles.color = mapConstructor.baseColor;
							}
							Handles.SphereCap (0, point, Quaternion.identity, mapConstructor.pointSize * .5f);
							
							// draw line between way points
							if(j < mapConstructor.paths[i].Points.Count - 1) {
								Handles.color = mapConstructor.pathColor;
								Vector3 newPoint = mapConstructor.paths[i].Points[j + 1].WayPointPos;
								Handles.DrawLine (point, newPoint);
								Handles.color = mapConstructor.baseColor;
							}
														
							mapConstructor.paths[i].Points[j].WayPointPos = Handles.FreeMoveHandle (point, Quaternion.identity, mapConstructor.pointSize * .5f, Vector3.one, Handles.CircleCap);
//							point = Handles.PositionHandle (point, Quaternion.identity);
						}
					}
				}
			}
		}
		// render towerpoints on scene view
		if (mapConstructor.towerPoints.Count > 0){
			for (int i = 0; i < mapConstructor.towerPoints.Count; i++)
			{
				// GameObject towerPoint = mapConstructor.towerPoints[i].towerPointGo;
				Vector3 towerPoint = mapConstructor.towerPoints[i].TowerPointPos;
				 
				if (towerPoint != null) {
					Handles.Label (towerPoint + new Vector3(mapConstructor.pointSize * .5f, mapConstructor.pointSize * .5f, mapConstructor.pointSize * .5f), "t"+i, titleAStyle);
					Handles.color = mapConstructor.towerPointColor;
					Handles.CubeCap (0, towerPoint, Quaternion.identity, mapConstructor.pointSize * .5f);
					Handles.color = mapConstructor.baseColor;
					
					mapConstructor.towerPoints[i].TowerPointPos = Handles.FreeMoveHandle (towerPoint, Quaternion.identity, mapConstructor.pointSize * .5f, Vector3.one, Handles.RectangleCap);
				}
			}
		}

		SceneView.RepaintAll();
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

		bodyStyle.normal.background = mBg2;
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
		titleBStyle.normal.textColor = Color.black;

		titleCStyle.font = mFont;
		titleCStyle.fontSize = 12;
		titleCStyle.normal.textColor = Color.white;

	}

	private void OnPathInspectorGUI () {
		EditorGUILayout.BeginVertical("box");
		EditorGUI.indentLevel++;
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Path", titleBStyle);
		
		if (GUILayout.Button ("Create Path", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
			CreatePath ();
			toggleWayPoints.Add (false);
		}
		if (GUILayout.Button ("Clear Paths", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
			ClearPaths();
		}
		EditorGUILayout.EndHorizontal();
		GUILayout.Space (5);

		if (mapConstructor.paths != null && mapConstructor.paths.Count > 0) {
			
			for (int i = 0; i < mapConstructor.paths.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField ("Path " + mapConstructor.paths[i].PathId);
				if(GUILayout.Button("Add WP", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
					CreateWayPoint (i, new Vector3(mapConstructor.paths[i].Points.Count, 0f, mapConstructor.paths[i].PathId + 1));
				}
				if(GUILayout.Button("Clear WPs", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
					ClearWayPoints(i);	
				}
				if(GUILayout.Button("Align WPs", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
					AlignWayPoints(i);	
				}
				EditorGUILayout.EndHorizontal();
				if (mapConstructor.paths[i].Points.Count > 0) {
					toggleWayPoints[i] = EditorGUILayout.Foldout(toggleWayPoints[i], "Waypoints");
					if (toggleWayPoints[i]) {
						EditorGUI.indentLevel++;
						for (int j = 0; j < mapConstructor.paths[i].Points.Count; j++)
						{
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField ("p" + j, GUILayout.MinWidth (60), GUILayout.MaxWidth (100));
							
							EditorGUI.BeginChangeCheck();
							var position = EditorGUILayout.Vector3Field("Pos", mapConstructor.paths[i].Points[j].WayPointPos);
							if(EditorGUI.EndChangeCheck()){
								mapConstructor.paths[i].Points[j].WayPointPos = position;
								EditorUtility.SetDirty(mapConstructor);
							}
							
							if (GUILayout.Button("Remove", GUILayout.MinWidth (175), GUILayout.MaxWidth (175))) {
								mapConstructor.paths[i].Points.RemoveAt(j);
							}
							EditorGUILayout.EndHorizontal();
						}
						EditorGUI.indentLevel--;
					}
				}
			}
		}
		EditorGUI.indentLevel--;
		EditorGUILayout.EndVertical();
	}

	private void OnTowerPointInspectorGUI () {
		EditorGUILayout.BeginVertical("box");
		EditorGUI.indentLevel++;
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Tower Point", titleBStyle);
			
		if (GUILayout.Button ("Create TP", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
			CreateTowerPoint(mapConstructor.towerPoints.Count, new Vector3(mapConstructor.towerPoints.Count, 0f, 0f));
		}
		if (GUILayout.Button ("Clear TPs", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
			ClearTowerPoints();
		}
		if (GUILayout.Button ("Align TPs", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
			AlignTowerPoints();
		}
		EditorGUILayout.EndHorizontal();
		GUILayout.Space (5);
				
		if (mapConstructor.towerPoints != null && mapConstructor.towerPoints.Count > 0){
			for (int i = 0; i < mapConstructor.towerPoints.Count; i++)
			{	
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.LabelField ("t" + i);
				
				var pos = EditorGUILayout.Vector3Field ("Pos", mapConstructor.towerPoints[i].TowerPointPos);
				if (EditorGUI.EndChangeCheck ()) {
					mapConstructor.towerPoints[i].TowerPointPos = pos;
					EditorUtility.SetDirty(mapConstructor);
				}

				if (GUILayout.Button("Remove",GUILayout.MinWidth (175), GUILayout.MaxWidth (175))) {
					// DestroyImmediate(mapConstructor.towerPoints[i].towerPointGo);
					mapConstructor.towerPoints.RemoveAt(i);
				}
				// EditorGUILayout.EndHorizontal();
			}
		}		
		EditorGUI.indentLevel--;
		EditorGUILayout.EndVertical();
	}

	
	private void OnWaveInspectorGUI () {
		EditorGUILayout.BeginVertical("box");
		EditorGUI.indentLevel++;

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Wave", titleBStyle);
		if (GUILayout.Button ("Create Wave", GUILayout.MinWidth (100), GUILayout.MaxWidth (100))) {
			CreateWave();
			toggleWaveGroups.Add(false);
		}
		if (GUILayout.Button ("Clear Waves", GUILayout.MinWidth (100), GUILayout.MaxWidth (100))) {
			ClearWaves();
		}
		EditorGUILayout.EndHorizontal ();
		GUILayout.Space (5);

		// render wave data
		if (mapConstructor.waves!= null && mapConstructor.waves.Count > 0) {
			for (int i = 0; i < mapConstructor.waves.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField ("Wave " + mapConstructor.waves[i].waveId);
				if(GUILayout.Button("Remove", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
					mapConstructor.waves.RemoveAt (i);
					continue;
				}
				if(GUILayout.Button("Add Group", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
//					var g = new WaveGroup();
					var g = new WaveGroup(enemyIdOptions[0], pathIdOptions[0]);
					mapConstructor.waves[i].groups.Add(g);
				}
				if(GUILayout.Button("Clear Groups", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {	
					mapConstructor.waves[i].groups.Clear();
				}
				EditorGUILayout.EndHorizontal();

				if (mapConstructor.waves[i].groups.Count > 0) {
					toggleWaveGroups[i] = EditorGUILayout.Foldout(toggleWaveGroups[i], "Groups");
					if (toggleWaveGroups[i]) {
						EditorGUI.indentLevel++;
						for (int j = 0; j < mapConstructor.waves[i].groups.Count; j++)
						{
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField ("Group " + j, GUILayout.MinWidth (80), GUILayout.MaxWidth (80));
							
							EditorGUI.BeginChangeCheck();
							enemyIndexes[i][j] = EditorGUILayout.Popup (enemyIndexes[i][j], enemyIdOptions, GUILayout.MinWidth (100), GUILayout.MaxWidth (100));
							var amount = EditorGUILayout.IntField (mapConstructor.waves[i].groups[j].Amount, GUILayout.MinWidth (100), GUILayout.MaxWidth (100));
							var spawnInterval = EditorGUILayout.FloatField (mapConstructor.waves[i].groups[j].SpawnInterval, GUILayout.MinWidth (100), GUILayout.MaxWidth (100));
							var waveDelay = EditorGUILayout.FloatField (mapConstructor.waves[i].groups[j].WaveDelay, GUILayout.MinWidth (100), GUILayout.MaxWidth (100));
							pathIndexes[i][j] = EditorGUILayout.Popup (pathIndexes[i][j], pathIdOptions, GUILayout.MinWidth (100), GUILayout.MaxWidth (100));
							if (EditorGUI.EndChangeCheck()){
								mapConstructor.waves[i].groups[j].EId = enemyIndexes[i][j];
								mapConstructor.waves[i].groups[j].Amount = amount;
								mapConstructor.waves[i].groups[j].SpawnInterval = spawnInterval;
								mapConstructor.waves[i].groups[j].WaveDelay = waveDelay;
								mapConstructor.waves[i].groups[j].PId = pathIndexes[i][j];
								EditorUtility.SetDirty(mapConstructor);
							}

							if(GUILayout.Button("Remove", GUILayout.MinWidth (175), GUILayout.MaxWidth (175))) {
								mapConstructor.waves[i].groups.RemoveAt(j);
								enemyIndexes.RemoveAt(j);
								pathIndexes.RemoveAt(j);
							}
							EditorGUILayout.EndHorizontal();
						}
						EditorGUI.indentLevel--;
					}
				}
			}
		}
		EditorGUI.indentLevel--;
		EditorGUILayout.EndVertical();
	}
	

	// TODO: save and load map data to xml 
	private void OnDataInspectorGUI () {
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Save")) {
			// var text = mapConstructor.Save();
			// WriteMapData(text);
		}
		if (GUILayout.Button ("Load")) {
			// mapConstructor.Load (LoadMapData());
			// TODO: confirm windows
		}
		if (GUILayout.Button ("Reset")) {
			ResetData();
			mc.ApplyModifiedProperties();	// ????

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
				// CreateTowerPoint(hit.point);
			} else {
				// check there is any path
				// CreateWayPoint(0, hit.point);	// TODO: get current selected path
			}
		}
	}

	private void CreatePath () {
		mapConstructor.paths.Add(new PathData(mapConstructor.paths.Count, new List<WayPointData> ()));
	}

	private void ClearPaths () {
		mapConstructor.paths.Clear ();
	}

	private void CreateWayPoint ( int pathIndex, Vector3 position) {
		mapConstructor.paths[pathIndex].Points.Add(new WayPointData(position));
	}

	private void ClearWayPoints(int pathIndex) {
		mapConstructor.paths[pathIndex].Points.Clear();
	}

	private void AlignWayPoints(int pathIndex) {
		for (int j = 0; j < mapConstructor.paths[pathIndex].Points.Count; j++)
		{
			mapConstructor.paths[pathIndex].Points[j].WayPointPos = new Vector3(mapConstructor.paths[pathIndex].Points[j].WayPointPos.x, 0f, mapConstructor.paths[pathIndex].Points[j].WayPointPos.z);
		}
	}

	private void CreateTowerPoint (int index, Vector3 position) {
		mapConstructor.towerPoints.Add (new TowerPointData("t" + index, position));
	}

	private void ClearTowerPoints() {
		mapConstructor.towerPoints.Clear ();
	}
	private void AlignTowerPoints() {
		for (int i = 0; i < mapConstructor.towerPoints.Count; i++)
		{
			mapConstructor.towerPoints[i].TowerPointPos = new Vector3(mapConstructor.towerPoints[i].TowerPointPos.x, 0f, mapConstructor.towerPoints[i].TowerPointPos.z);
		}
	}
	private void CreateWave(){
		mapConstructor.waves.Add (new WaveData(mapConstructor.waves.Count, new List<WaveGroup> ()));
	}
	private void ClearWaves() {
		mapConstructor.waves.Clear();
		enemyIndexes.Clear();
		pathIndexes.Clear();
	}

	private void ResetData() {
		ClearPaths();
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

	private string ConvertDataToJson () {
		string json = JsonUtility.ToJson(mapConstructor);

		Debug.Log (json);
		return json;
	}
	#endregion database
}
