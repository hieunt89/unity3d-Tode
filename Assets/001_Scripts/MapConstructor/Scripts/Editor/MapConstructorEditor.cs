using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;

[CustomEditor (typeof(MapConstructor))]
public class MapConstructorEditor : Editor {

	private MapConstructor mapConstructor;
	private SerializedObject mc;
	private Event currentEvent;
	private bool quickCreationMode = false;
	private bool resetTool = false;

	string mapId;

	private List<bool> togglePaths;
	private List<bool> toggleWaves;

	List<List<int>> enemyIndexes;
	List<List<int>> pathIndexes;

	List<MapData> existMaps;
	List<EnemyData> existEnemies;
//	List<TowerData> existTowers;

//	List<string> towerIds;
	List<string> enemyIds;
	List<string> pathIds;

	#region MONO
	void OnEnable () {
		mapConstructor = (MapConstructor) target as MapConstructor;
		mc = new SerializedObject(mapConstructor);

		if (mapConstructor.Map == null)
			mapConstructor.Map = new MapData("", new List<PathData>(), new List<TowerPointData>(), new List<WaveData>());
		CreateToggles ();

		LoadExistData ();
		CreateIndexes ();
		GenerateStyle ();

		mapId = "map" + existMaps.Count;
		mapConstructor.Map.Id  = mapId;
	}
	#endregion MONO


	#region Custom Inspector
	public override void OnInspectorGUI (){
		// DrawDefaultInspector();	// test

		if(mapConstructor == null)
			return;
		
		mc.Update();

		EditorGUILayout.LabelField ("MAP CONSTRUCTOR", headerStyle, GUILayout.MinHeight (40));

		EditorGUILayout.BeginVertical ();
		EditorGUILayout.Space();

		EditorGUI.BeginChangeCheck();
		var id = EditorGUILayout.TextField ("Map Id", mapConstructor.Map.Id);
		var pointSize = EditorGUILayout.Slider("Point Size", mapConstructor.pointSize, 0f, mapConstructor.maxPointSize);
		var baseColor = EditorGUILayout.ColorField("Base Color", mapConstructor.baseColor);
		var pathColor = EditorGUILayout.ColorField("Path Color", mapConstructor.pathColor);
		var wayPointColor = EditorGUILayout.ColorField("Way Points Color", mapConstructor.wayPointColor);
		var towerPointColor = EditorGUILayout.ColorField("Tower Points Color", mapConstructor.towerPointColor);
		if(EditorGUI.EndChangeCheck()){
			mapConstructor.Map.Id = id;
			mapConstructor.pointSize = pointSize;
			mapConstructor.baseColor = baseColor;
			mapConstructor.pathColor = pathColor;
			mapConstructor.wayPointColor = wayPointColor;
			mapConstructor.towerPointColor = towerPointColor;
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

		GUI.enabled = (mapConstructor.Map.Paths != null && mapConstructor.Map.Paths.Count > 0);
		OnWaveInspectorGUI();
		GUI.enabled = true;

		if (mapConstructor.Map != null)
			OnDataInspectorGUI ();
		EditorGUILayout.EndVertical ();
		

		EditorGUILayout.LabelField ("fdj", footerStyle, GUILayout.MinHeight (20));
		EditorGUILayout.Space();
		
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
		Handles.color = mapConstructor.baseColor;
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
		if (mapConstructor.Map.Paths != null && mapConstructor.Map.Paths.Count > 0){
			for (int i = 0; i < mapConstructor.Map.Paths.Count; i++)
			{
				if (mapConstructor.Map.Paths[i].Points.Count > 0){
					for (int j = 0; j < mapConstructor.Map.Paths[i].Points.Count; j++)
					{
						Vector3 point = mapConstructor.Map.Paths[i].Points[j];
						
						if (point != null) {
							// TODO: waypoint data need and id to set a label for it view
							Handles.Label (point + new Vector3(mapConstructor.pointSize * .5f, mapConstructor.pointSize * .5f, mapConstructor.pointSize * .5f), i+"-"+j, titleAStyle);
							if (j == 0){
								Handles.color = Color.green;
							} else if (j == mapConstructor.Map.Paths[i].Points.Count - 1) {
								Handles.color = Color.red;
							} else {
								Handles.color = mapConstructor.baseColor;
							}
							
							Handles.SphereCap (0, point, Quaternion.identity, mapConstructor.pointSize * .5f);
							// draw line between way points
							if(j < mapConstructor.Map.Paths[i].Points.Count - 1) {
								Handles.color = mapConstructor.pathColor;
								Vector3 newPoint = mapConstructor.Map.Paths[i].Points[j + 1];
								Handles.DrawLine (point, newPoint);
								Handles.color = mapConstructor.baseColor;
							}
														
							mapConstructor.Map.Paths[i].Points[j] = Handles.FreeMoveHandle (point, Quaternion.identity, mapConstructor.pointSize * .5f, Vector3.one, Handles.CircleCap);
//							point = Handles.PositionHandle (point, Quaternion.identity);
						}
					}
				}
			}
		}
		// render towerpoints on scene view
		if (mapConstructor.Map.TowerPoints != null && mapConstructor.Map.TowerPoints.Count > 0){
			for (int i = 0; i < mapConstructor.Map.TowerPoints.Count; i++)
			{
				// GameObject towerPoint = mapConstructor.towerPoints[i].towerPointGo;
				Vector3 towerPoint = mapConstructor.Map.TowerPoints[i].TowerPointPos;
				 
				if (towerPoint != null) {
					Handles.Label (towerPoint + new Vector3(mapConstructor.pointSize * .5f, mapConstructor.pointSize * .5f, mapConstructor.pointSize * .5f), "t"+i, titleAStyle);
					Handles.color = mapConstructor.towerPointColor;
					Handles.CubeCap (0, towerPoint, Quaternion.identity, mapConstructor.pointSize * .5f);
					Handles.color = mapConstructor.baseColor;
					
					mapConstructor.Map.TowerPoints[i].TowerPointPos = Handles.FreeMoveHandle (towerPoint, Quaternion.identity, mapConstructor.pointSize * .5f, Vector3.one, Handles.RectangleCap);
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
		Texture2D mBg = (Texture2D) Resources.Load ("Textures/map_constructor_bg");
		Texture2D mBg2 = (Texture2D) Resources.Load ("Textures/map_constructor_bg_2");
		Font mFont = (Font)Resources.Load("Fonts/HELVETICANEUEBOLD");

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
			togglePaths.Add (false);
		}

		GUI.enabled = (mapConstructor.Map.Paths != null && mapConstructor.Map.Paths.Count > 0);
		if (GUILayout.Button ("Clear Paths", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
			ClearPaths();
		}
		GUI.enabled = true;

		EditorGUILayout.EndHorizontal();
		GUILayout.Space (5);

		var wpIdWidth = EditorGUIUtility.currentViewWidth * .1f;
		var posWidth = EditorGUIUtility.currentViewWidth * .6f;
		var btnWidth = EditorGUIUtility.currentViewWidth * .1f;

		if (mapConstructor.Map.Paths != null && mapConstructor.Map.Paths.Count > 0) {
			for (int i = 0; i < mapConstructor.Map.Paths.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				var pIdWidth = EditorGUIUtility.currentViewWidth * .3f;
				EditorGUIUtility.labelWidth = pIdWidth * .5f;
				EditorGUILayout.LabelField ("Path Id", mapConstructor.Map.Paths[i].Id, GUILayout.Width (pIdWidth));

				if(GUILayout.Button("Remove", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
					mapConstructor.Map.Paths.RemoveAt (i);
					pathIds.RemoveAt (i);
					continue;
				}
				if(GUILayout.Button("Add WP", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
					CreateWayPoint (i, new Vector3(mapConstructor.Map.Paths[i].Points.Count, 0f, i + 1));
				}
				GUI.enabled = (mapConstructor.Map.Paths[i].Points.Count > 0);
				if(GUILayout.Button("Clear WPs", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
					ClearWayPoints(i);	
				}
				GUI.enabled = true;
				if(GUILayout.Button("Align WPs", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
					AlignWayPoints(i);	
				}
				EditorGUILayout.EndHorizontal();



				if (mapConstructor.Map.Paths[i].Points.Count > 0) {
					togglePaths[i] = EditorGUILayout.Foldout(togglePaths[i], "Waypoint");
					if (togglePaths[i]) {
						EditorGUI.indentLevel++;
						for (int j = 0; j < mapConstructor.Map.Paths[i].Points.Count; j++)
						{
							EditorGUILayout.BeginHorizontal();


							EditorGUILayout.LabelField ("Id", "p" + j, GUILayout.Width (wpIdWidth));
							EditorGUI.BeginChangeCheck();
							var position = EditorGUILayout.Vector3Field("Pos", mapConstructor.Map.Paths[i].Points[j], GUILayout.Width(posWidth));
							if(EditorGUI.EndChangeCheck()){
								mapConstructor.Map.Paths[i].Points[j] = position;
								EditorUtility.SetDirty(mapConstructor);
							}
							if (GUILayout.Button("Remove", GUILayout.Width (btnWidth))) {
								mapConstructor.Map.Paths[i].Points.RemoveAt(j);
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
			if (mapConstructor.Map.TowerPoints == null)
				mapConstructor.Map.TowerPoints = new List<TowerPointData> ();
			CreateTowerPoint(new Vector3(mapConstructor.Map.TowerPoints.Count, 0f, 0f));
		}

		GUI.enabled = (mapConstructor.Map.TowerPoints != null && mapConstructor.Map.TowerPoints.Count > 0);
		if (GUILayout.Button ("Clear TPs", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
			ClearTowerPoints();
		}
		GUI.enabled = true;

		if (GUILayout.Button ("Align TPs", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
			AlignTowerPoints();
		}
		EditorGUILayout.EndHorizontal();
		GUILayout.Space (5);
				
		var wpIdWidth = EditorGUIUtility.currentViewWidth * .1f;
		var posWidth = EditorGUIUtility.currentViewWidth * .6f;
		var btnWidth = EditorGUIUtility.currentViewWidth * .1f;

		if (mapConstructor.Map.TowerPoints != null && mapConstructor.Map.TowerPoints.Count > 0){
			EditorGUI.indentLevel++;
			for (int i = 0; i < mapConstructor.Map.TowerPoints.Count; i++)
			{	
				EditorGUI.BeginChangeCheck();
				GUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("Id", "t" + i, GUILayout.Width (wpIdWidth));

				var pos = EditorGUILayout.Vector3Field ("Pos", mapConstructor.Map.TowerPoints[i].TowerPointPos, GUILayout.Width (posWidth));
				if (EditorGUI.EndChangeCheck ()) {
					mapConstructor.Map.TowerPoints[i].TowerPointPos = pos;
					EditorUtility.SetDirty(mapConstructor);
				}

				if (GUILayout.Button("Remove", GUILayout.Width (btnWidth))) {
					mapConstructor.Map.TowerPoints.RemoveAt(i);
					continue;
				}
				GUILayout.EndHorizontal ();
			}
			EditorGUI.indentLevel--;
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
		}
		if (GUILayout.Button ("Clear Waves", GUILayout.MinWidth (100), GUILayout.MaxWidth (100))) {
			ClearWaves();
		}
		EditorGUILayout.EndHorizontal ();
		GUILayout.Space (5);

		var elementWidth = EditorGUIUtility.currentViewWidth * .14f;
		var posWidth = EditorGUIUtility.currentViewWidth * .6f;
		var btnWidth = EditorGUIUtility.currentViewWidth * .1f;

		// render wave data
		if (mapConstructor.Map.Waves!= null && mapConstructor.Map.Waves.Count > 0) {
			for (int i = 0; i < mapConstructor.Map.Waves.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField ("Id",  mapConstructor.Map.Waves[i].Id);
				if(GUILayout.Button("Remove", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
					mapConstructor.Map.Waves.RemoveAt (i);
					continue;
				}
				GUILayout.Space (10);
				if(GUILayout.Button("Add Group", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
					var g = new WaveGroupData("g" + mapConstructor.Map.Waves[i].Groups.Count, enemyIds.ToArray()[0], pathIds.ToArray()[0]);
					mapConstructor.Map.Waves[i].Groups.Add(g);
					CreateIndexes ();
				}
				GUI.enabled = (mapConstructor.Map.Waves[i].Groups.Count > 0);
				if(GUILayout.Button("Clear Groups", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {	
					mapConstructor.Map.Waves[i].Groups.Clear();
				}
				GUI.enabled = true;
				EditorGUILayout.EndHorizontal();

				if (mapConstructor.Map.Waves[i].Groups != null && mapConstructor.Map.Waves[i].Groups.Count > 0) {
					toggleWaves[i] = EditorGUILayout.Foldout(toggleWaves[i], "Group");
					if (toggleWaves[i]) {
						EditorGUI.indentLevel++;
						for (int j = 0; j < mapConstructor.Map.Waves[i].Groups.Count; j++)
						{
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField ("Id ", mapConstructor.Map.Waves[i].Groups[j].Id, GUILayout.Width (elementWidth));
							
							EditorGUI.BeginChangeCheck();
							enemyIndexes[i][j] = EditorGUILayout.Popup (enemyIndexes[i][j], enemyIds.ToArray(), GUILayout.Width (elementWidth));
							var amount = EditorGUILayout.IntField (mapConstructor.Map.Waves[i].Groups[j].Amount,  GUILayout.Width (elementWidth));
							var spawnInterval = EditorGUILayout.FloatField (mapConstructor.Map.Waves[i].Groups[j].SpawnInterval,  GUILayout.Width (elementWidth));
							var waveDelay = EditorGUILayout.FloatField (mapConstructor.Map.Waves[i].Groups[j].WaveDelay, GUILayout.Width (elementWidth));
							pathIndexes[i][j] = EditorGUILayout.Popup (pathIndexes[i][j], pathIds.ToArray(),  GUILayout.Width (elementWidth));
							if (EditorGUI.EndChangeCheck()){
								mapConstructor.Map.Waves[i].Groups[j].EId = enemyIndexes[i][j];
								mapConstructor.Map.Waves[i].Groups[j].EnemyId = enemyIds.ToArray()[enemyIndexes[i][j]];
								mapConstructor.Map.Waves[i].Groups[j].Amount = amount;
								mapConstructor.Map.Waves[i].Groups[j].SpawnInterval = spawnInterval;
								mapConstructor.Map.Waves[i].Groups[j].WaveDelay = waveDelay;
								mapConstructor.Map.Waves[i].Groups[j].PId = pathIndexes[i][j];
								mapConstructor.Map.Waves[i].Groups[j].PathId = pathIds.ToArray()[pathIndexes[i][j]];
								EditorUtility.SetDirty(mapConstructor);
							}

							if(GUILayout.Button("Remove", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
								mapConstructor.Map.Waves[i].Groups.RemoveAt(j);
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

	private void OnDataInspectorGUI () {
		EditorGUILayout.BeginHorizontal ();

		GUI.enabled = CheckFields();
		if (GUILayout.Button ("Save")) {
			DataManager.Instance.SaveData(mapConstructor.Map);
			LoadExistData ();
		}
		GUI.enabled = true;
		if (GUILayout.Button ("Load")) {
			mapConstructor.Map = DataManager.Instance.LoadData <MapData> ();
			if (mapConstructor.Map != null) {
				CreateToggles ();
				CreateIndexes ();
				if (pathIds == null) {
					pathIds = new List<string> ();
					for (int i = 0; i < mapConstructor.Map.Paths.Count; i++) {
						pathIds.Add(mapConstructor.Map.Paths[i].Id);		
					}
				}

			}
		}
		if (GUILayout.Button ("Reset")) {
			ResetData();
			mc.ApplyModifiedProperties();	// ????

			// TODO: confirm windows
		}
		EditorGUILayout.EndHorizontal();
	}

	private bool CheckFields () {
		var mapIdInput = !String.IsNullOrEmpty (mapConstructor.Map.Id);
	
		var pathInput = mapConstructor.Map.Paths != null && mapConstructor.Map.Paths.Count > 0;

		var towerPointInput = mapConstructor.Map.TowerPoints != null && mapConstructor.Map.TowerPoints.Count > 0;

		var waveInput = mapConstructor.Map.Waves != null && mapConstructor.Map.Waves.Count > 0;
		
		return mapIdInput && pathInput && towerPointInput && waveInput;

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
		if (mapConstructor.Map.Paths == null)
			mapConstructor.Map.Paths = new List<PathData> ();
		PathData pathData = new PathData("p" + mapConstructor.Map.Paths.Count, new List<Vector3> ()) ;
		mapConstructor.Map.Paths.Add(pathData);
		if (pathIds == null) {
			pathIds = new List<string> ();
		}
		pathIds.Add(pathData.Id);
		togglePaths.Add(false);
	}

	private void ClearPaths () {
		mapConstructor.Map.Paths.Clear ();
		if (pathIds != null && pathIds.Count > 0)
			pathIds.Clear();
		ClearWaves ();
	}

	private void CreateWayPoint (int pathIndex, Vector3 position) {
		mapConstructor.Map.Paths[pathIndex].Points.Add(position);
	}

	private void ClearWayPoints(int pathIndex) {
		mapConstructor.Map.Paths[pathIndex].Points.Clear();
	}

	private void AlignWayPoints(int pathIndex) {
		for (int j = 0; j < mapConstructor.Map.Paths[pathIndex].Points.Count; j++)
		{
			mapConstructor.Map.Paths[pathIndex].Points[j] = new Vector3(mapConstructor.Map.Paths[pathIndex].Points[j].x, 0f, mapConstructor.Map.Paths[pathIndex].Points[j].z);
		}
	}

	private void CreateTowerPoint (Vector3 position) {
		mapConstructor.Map.TowerPoints.Add (new TowerPointData("t" + mapConstructor.Map.TowerPoints.Count, position));
	}

	private void ClearTowerPoints() {
			mapConstructor.Map.TowerPoints.Clear ();
	}
	private void AlignTowerPoints() {
		for (int i = 0; i < mapConstructor.Map.TowerPoints.Count; i++)
		{
			mapConstructor.Map.TowerPoints[i].TowerPointPos = new Vector3(mapConstructor.Map.TowerPoints[i].TowerPointPos.x, 0f, mapConstructor.Map.TowerPoints[i].TowerPointPos.z);
		}
	}
	private void CreateWave(){
		if (mapConstructor.Map.Waves == null)
			mapConstructor.Map.Waves = new List<WaveData> ();
		mapConstructor.Map.Waves.Add (new WaveData("w" + mapConstructor.Map.Waves.Count, new List<WaveGroupData> ()));
		toggleWaves.Add(false);
	}

	private void ClearWaves() {
		if (mapConstructor.Map.Waves != null)
			mapConstructor.Map.Waves.Clear();
		enemyIndexes.Clear();
		pathIndexes.Clear();
	}

	private void ResetData() {
		ClearPaths();
		ClearTowerPoints();
		ClearWaves();

		pathIds.Clear ();
	}

	void LoadExistData () {
		existMaps = DataManager.Instance.LoadAllData <MapData> ();

	
		existEnemies = DataManager.Instance.LoadAllData <EnemyData> ();


		if (existEnemies.Count > 0) {
			enemyIds = new List<string> ();
			for (int i = 0; i < existEnemies.Count; i++) {
				enemyIds.Add(existEnemies[i].Id);
			}
		}
	}
	void CreateToggles () {
		togglePaths = new List <bool> ();
		toggleWaves = new List <bool> ();

		if (mapConstructor.Map.Paths != null && mapConstructor.Map.Paths.Count > 0) {
			togglePaths = new List<bool> (mapConstructor.Map.Paths.Count);
			for (int i = 0; i < mapConstructor.Map.Paths.Count; i++)
			{
				togglePaths.Add(false);
			}
		}
		if (mapConstructor.Map.Waves != null && mapConstructor.Map.Waves.Count > 0) {
			toggleWaves = new List<bool> (mapConstructor.Map.Waves.Count);
			for (int i = 0; i < mapConstructor.Map.Waves.Count; i++)
			{
				toggleWaves.Add(false);
			}
		}
	}
	private void CreateIndexes () {
		enemyIndexes = new List<List<int>> ();
		pathIndexes = new List<List<int>> ();
		if (mapConstructor.Map.Waves != null &&mapConstructor.Map.Waves.Count > 0) {
			for (int i = 0; i < mapConstructor.Map.Waves.Count; i++)
			{
				enemyIndexes.Add(new List<int> ());
				pathIndexes.Add (new List<int> ());
				for (int j = 0; j < mapConstructor.Map.Waves[i].Groups.Count; j++)
				{
					enemyIndexes[i].Add (mapConstructor.Map.Waves[i].Groups[j].EId);
					pathIndexes[i].Add (mapConstructor.Map.Waves[i].Groups[j].PId);
				}
			}
		}
	}


	#endregion private methods
}
