using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

[CustomEditor (typeof(MapConstructor))]
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
        
		toggleWayPoints = new List<bool> (mapConstructor.Paths.Count);
		for (int i = 0; i < mapConstructor.Paths.Count; i++)
		{
			toggleWayPoints.Add(false);
		}

		toggleWaveGroups = new List<bool> (mapConstructor.Waves.Count);
		for (int i = 0; i < mapConstructor.Waves.Count; i++)
		{
			toggleWaveGroups.Add(false);
		}
		CreateIndexes ();
		GenerateStyle ();
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
		var mapId = EditorGUILayout.IntField ("Map Id", mapConstructor.MapId);
		var pointSize = EditorGUILayout.Slider("Point Size", mapConstructor.PointSize, 0f, mapConstructor.MaxPointSize);
		var baseColor = EditorGUILayout.ColorField("Base Color", mapConstructor.BaseColor);
		var pathColor = EditorGUILayout.ColorField("Path Color", mapConstructor.PathColor);
		var wayPointColor = EditorGUILayout.ColorField("Way Points Color", mapConstructor.WayPointColor);
		var towerPointColor = EditorGUILayout.ColorField("Tower Points Color", mapConstructor.TowerPointColor);
		if(EditorGUI.EndChangeCheck()){
			mapConstructor.MapId = mapId;
			mapConstructor.PointSize = pointSize;
			mapConstructor.BaseColor = baseColor;
			mapConstructor.PathColor = pathColor;
			mapConstructor.WayPointColor = wayPointColor;
			mapConstructor.TowerPointColor = towerPointColor;
			EditorUtility.SetDirty(mapConstructor);
		}

//		quickCreationMode = GUILayout.Toggle(quickCreationMode, "Interactive Creation", GUI.skin.button);
//		if (quickCreationMode) {
//			if (mapConstructor.gameObject.GetComponent <BoxCollider> () == null)
//				mapConstructor.gameObject.AddComponent <BoxCollider> ().size = new Vector3(50f, 0.01f, 50f);
//			EditorGUILayout.HelpBox ("- Left click on the plane to create a new WAY POINT.\n- Shift + left click on the plane to create a new TOWER POINT.\n-You should turn off Interactive Creation Mode to drag the points.", MessageType.Info, true);
//		} 		
		EditorGUILayout.Space();

		OnPathInspectorGUI ();
		// OnWayPointInspectorGUI();
		EditorGUILayout.Space();
		
		OnTowerPointInspectorGUI();
		EditorGUILayout.Space();

		if (mapConstructor.Paths.Count > 0) {
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
		Handles.color = mapConstructor.BaseColor;
		Handles.Disc (mapConstructor.transform.rotation, mapConstructor.transform.position, Vector3.up, 1f, false, 1f);
		Handles.Label (mapConstructor.transform.position + new Vector3(mapConstructor.PointSize, mapConstructor.PointSize, mapConstructor.PointSize), mapConstructor.name);

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
		mapConstructor.PointSize = GUILayout.HorizontalSlider (mapConstructor.PointSize, 0f, mapConstructor.MaxPointSize, GUILayout.MinWidth(100));
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		Handles.EndGUI(); 

		// render waypoints on scene view
		if (mapConstructor.Paths.Count > 0){
			for (int i = 0; i < mapConstructor.Paths.Count; i++)
			{
				if (mapConstructor.Paths[i].Points.Count > 0){
					for (int j = 0; j < mapConstructor.Paths[i].Points.Count; j++)
					{
						Vector3 point = mapConstructor.Paths[i].Points[j];
						
						if (point != null) {
							// TODO: waypoint data need and id to set a label for it view
							Handles.Label (point + new Vector3(mapConstructor.PointSize * .5f, mapConstructor.PointSize * .5f, mapConstructor.PointSize * .5f), i+"-"+j, titleAStyle);
							if (j == 0){
								Handles.color = Color.green;
							} else if (j == mapConstructor.Paths[i].Points.Count - 1) {
								Handles.color = Color.red;
							} else {
								Handles.color = mapConstructor.BaseColor;
							}
							
							Handles.SphereCap (0, point, Quaternion.identity, mapConstructor.PointSize * .5f);
							// draw line between way points
							if(j < mapConstructor.Paths[i].Points.Count - 1) {
								Handles.color = mapConstructor.PathColor;
								Vector3 newPoint = mapConstructor.Paths[i].Points[j + 1];
								Handles.DrawLine (point, newPoint);
								Handles.color = mapConstructor.BaseColor;
							}
														
							mapConstructor.Paths[i].Points[j] = Handles.FreeMoveHandle (point, Quaternion.identity, mapConstructor.PointSize * .5f, Vector3.one, Handles.CircleCap);
//							point = Handles.PositionHandle (point, Quaternion.identity);
						}
					}
				}
			}
		}
		// render towerpoints on scene view
		if (mapConstructor.TowerPoints.Count > 0){
			for (int i = 0; i < mapConstructor.TowerPoints.Count; i++)
			{
				// GameObject towerPoint = mapConstructor.towerPoints[i].towerPointGo;
				Vector3 towerPoint = mapConstructor.TowerPoints[i].TowerPointPos;
				 
				if (towerPoint != null) {
					Handles.Label (towerPoint + new Vector3(mapConstructor.PointSize * .5f, mapConstructor.PointSize * .5f, mapConstructor.PointSize * .5f), "t"+i, titleAStyle);
					Handles.color = mapConstructor.TowerPointColor;
					Handles.CubeCap (0, towerPoint, Quaternion.identity, mapConstructor.PointSize * .5f);
					Handles.color = mapConstructor.BaseColor;
					
					mapConstructor.TowerPoints[i].TowerPointPos = Handles.FreeMoveHandle (towerPoint, Quaternion.identity, mapConstructor.PointSize * .5f, Vector3.one, Handles.RectangleCap);
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

		if (mapConstructor.Paths != null && mapConstructor.Paths.Count > 0) {
			
			for (int i = 0; i < mapConstructor.Paths.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField ("Path " + mapConstructor.Paths[i].PathId);
				if(GUILayout.Button("Add WP", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
					CreateWayPoint (i, new Vector3(mapConstructor.Paths[i].Points.Count, 0f, mapConstructor.Paths[i].PathId + 1));
				}
				if(GUILayout.Button("Clear WPs", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
					ClearWayPoints(i);	
				}
				if(GUILayout.Button("Align WPs", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
					AlignWayPoints(i);	
				}
				EditorGUILayout.EndHorizontal();
				if (mapConstructor.Paths[i].Points.Count > 0) {
					toggleWayPoints[i] = EditorGUILayout.Foldout(toggleWayPoints[i], "Waypoints");
					if (toggleWayPoints[i]) {
						EditorGUI.indentLevel++;
						for (int j = 0; j < mapConstructor.Paths[i].Points.Count; j++)
						{
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField ("p" + j, GUILayout.MinWidth (60), GUILayout.MaxWidth (100));
							
							EditorGUI.BeginChangeCheck();
							var position = EditorGUILayout.Vector3Field("Pos", mapConstructor.Paths[i].Points[j]);
							if(EditorGUI.EndChangeCheck()){
								mapConstructor.Paths[i].Points[j] = position;
								EditorUtility.SetDirty(mapConstructor);
							}
							if (GUILayout.Button("Remove", GUILayout.MinWidth (175), GUILayout.MaxWidth (175))) {
								mapConstructor.Paths[i].Points.RemoveAt(j);
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
			CreateTowerPoint(mapConstructor.TowerPoints.Count, new Vector3(mapConstructor.TowerPoints.Count, 0f, 0f));
		}
		if (GUILayout.Button ("Clear TPs", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
			ClearTowerPoints();
		}
		if (GUILayout.Button ("Align TPs", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
			AlignTowerPoints();
		}
		EditorGUILayout.EndHorizontal();
		GUILayout.Space (5);
				
		if (mapConstructor.TowerPoints != null && mapConstructor.TowerPoints.Count > 0){
			for (int i = 0; i < mapConstructor.TowerPoints.Count; i++)
			{	
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.LabelField ("t" + i);
				
				var pos = EditorGUILayout.Vector3Field ("Pos", mapConstructor.TowerPoints[i].TowerPointPos);
				if (EditorGUI.EndChangeCheck ()) {
					mapConstructor.TowerPoints[i].TowerPointPos = pos;
					EditorUtility.SetDirty(mapConstructor);
				}

				if (GUILayout.Button("Remove",GUILayout.MinWidth (175), GUILayout.MaxWidth (175))) {
					mapConstructor.TowerPoints.RemoveAt(i);
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
		if (mapConstructor.Waves!= null && mapConstructor.Waves.Count > 0) {
			for (int i = 0; i < mapConstructor.Waves.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField ("Wave " + mapConstructor.Waves[i].WaveId);
				if(GUILayout.Button("Remove", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
					mapConstructor.Waves.RemoveAt (i);
					continue;
				}
				if(GUILayout.Button("Add Group", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
					var g = new WaveGroup(enemyIdOptions[0], pathIdOptions[0]);
					mapConstructor.Waves[i].Groups.Add(g);
					CreateIndexes ();
				}
				if(GUILayout.Button("Clear Groups", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {	
					mapConstructor.Waves[i].Groups.Clear();
				}
				EditorGUILayout.EndHorizontal();

				if (mapConstructor.Waves[i].Groups != null && mapConstructor.Waves[i].Groups.Count > 0) {
					toggleWaveGroups[i] = EditorGUILayout.Foldout(toggleWaveGroups[i], "Groups");
					if (toggleWaveGroups[i]) {
						EditorGUI.indentLevel++;
						for (int j = 0; j < mapConstructor.Waves[i].Groups.Count; j++)
						{
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField ("Group " + j, GUILayout.MinWidth (80), GUILayout.MaxWidth (80));
							
							EditorGUI.BeginChangeCheck();

							Debug.Log (i + " / " + j + " / index count: " + enemyIndexes.Count);
							Debug.Log("sub index count: " + enemyIndexes[i].Count);
							Debug.Log (enemyIndexes[i][j]);
							enemyIndexes[i][j] = EditorGUILayout.Popup (enemyIndexes[i][j], enemyIdOptions, GUILayout.MinWidth (100), GUILayout.MaxWidth (100));
							var amount = EditorGUILayout.IntField (mapConstructor.Waves[i].Groups[j].Amount, GUILayout.MinWidth (100), GUILayout.MaxWidth (100));
							var spawnInterval = EditorGUILayout.FloatField (mapConstructor.Waves[i].Groups[j].SpawnInterval, GUILayout.MinWidth (100), GUILayout.MaxWidth (100));
							var waveDelay = EditorGUILayout.FloatField (mapConstructor.Waves[i].Groups[j].WaveDelay, GUILayout.MinWidth (100), GUILayout.MaxWidth (100));
							pathIndexes[i][j] = EditorGUILayout.Popup (pathIndexes[i][j], pathIdOptions, GUILayout.MinWidth (100), GUILayout.MaxWidth (100));
							if (EditorGUI.EndChangeCheck()){
								mapConstructor.Waves[i].Groups[j].EId = enemyIndexes[i][j];
								mapConstructor.Waves[i].Groups[j].Amount = amount;
								mapConstructor.Waves[i].Groups[j].SpawnInterval = spawnInterval;
								mapConstructor.Waves[i].Groups[j].WaveDelay = waveDelay;
								mapConstructor.Waves[i].Groups[j].PId = pathIndexes[i][j];
								EditorUtility.SetDirty(mapConstructor);
							}

							if(GUILayout.Button("Remove", GUILayout.MinWidth (175), GUILayout.MaxWidth (175))) {
								mapConstructor.Waves[i].Groups.RemoveAt(j);
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
			DataManager.Instance.SaveMapData(mapConstructor);
		}
		if (GUILayout.Button ("Load")) {
			DataManager.Instance.LoadMapData();
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
		mapConstructor.Paths.Add(new PathData(mapConstructor.Paths.Count, new List<Vector3> ()));
	}

	private void ClearPaths () {
		mapConstructor.Paths.Clear ();
	}

	private void CreateWayPoint ( int pathIndex, Vector3 position) {
		mapConstructor.Paths[pathIndex].Points.Add(position);
	}

	private void ClearWayPoints(int pathIndex) {
		mapConstructor.Paths[pathIndex].Points.Clear();
	}

	private void AlignWayPoints(int pathIndex) {
		for (int j = 0; j < mapConstructor.Paths[pathIndex].Points.Count; j++)
		{
			mapConstructor.Paths[pathIndex].Points[j] = new Vector3(mapConstructor.Paths[pathIndex].Points[j].x, 0f, mapConstructor.Paths[pathIndex].Points[j].z);
		}
	}

	private void CreateTowerPoint (int index, Vector3 position) {
		mapConstructor.TowerPoints.Add (new TowerPointData("t" + index, position));
	}

	private void ClearTowerPoints() {
		mapConstructor.TowerPoints.Clear ();
	}
	private void AlignTowerPoints() {
		for (int i = 0; i < mapConstructor.TowerPoints.Count; i++)
		{
			mapConstructor.TowerPoints[i].TowerPointPos = new Vector3(mapConstructor.TowerPoints[i].TowerPointPos.x, 0f, mapConstructor.TowerPoints[i].TowerPointPos.z);
		}
	}
	private void CreateWave(){
		mapConstructor.Waves.Add (new WaveData(mapConstructor.Waves.Count, new List<WaveGroup> ()));
	}

	private void CreateIndexes () {
		if (mapConstructor.Waves.Count > 0) {
			for (int i = 0; i < mapConstructor.Waves.Count; i++)
			{
				enemyIndexes.Add(new List<int> ());
				pathIndexes.Add (new List<int> ());
				for (int j = 0; j < mapConstructor.Waves[i].Groups.Count; j++)
				{
					enemyIndexes[i].Add (mapConstructor.Waves[i].Groups[j].EId);
					pathIndexes[i].Add (mapConstructor.Waves[i].Groups[j].PId);
				}
			}
		}
	}
	private void ClearWaves() {
		mapConstructor.Waves.Clear();
		enemyIndexes.Clear();
		pathIndexes.Clear();
	}

	private void ResetData() {
		ClearPaths();
		ClearTowerPoints();
		ClearWaves();
	}
	#endregion private methods
}
