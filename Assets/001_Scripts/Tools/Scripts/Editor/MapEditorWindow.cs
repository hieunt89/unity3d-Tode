using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class MapEditorWindow : EditorWindow {
	
	private MapData map;
	private Event currentEvent;

	private float handleSize;
	private float pickSize;
	private float maxHandleSize;

	private Color pathColor;
	private Color wayPointColor;
	private Color towerPointColor;

	private int selectedPathIndex;
	private int selectedWaypointIndex;
	private int selectedTowerpointIndex;

	private List<bool> togglePaths;
	private List<bool> toggleWaves;

	List<MapData> existMaps;
	List<CharacterData> existEnemies;
//	List<TowerData> existTowers;

	List<List<int>> enemyPopupIndexes;
	List<List<int>> pathPopupIndexes;

	List<string> enemyIds;
	List<string> pathIds;
    
	private int stepsPerCurve = 10;
	private float towerRange = 5;

	private bool isTerrainSetup;
	private UnityEngine.Object terrainGo;

	private GUISkin mapEditorSkin;

	IDataUtils dataUtils;
	IPrefabUtils prefabUtils;

    [MenuItem("Tode/Map Editor")]
    public static void ShowWindow()
    {
		MapEditorWindow mapEditorWindow = EditorWindow.GetWindow <MapEditorWindow> ("Map Editor", true);
		mapEditorWindow.minSize = new Vector2 (400, 600);
    }
    
	#region MONO
	void OnEnable () {
		dataUtils = DIContainer.GetModule <IDataUtils> ();
		prefabUtils = DIContainer.GetModule <IPrefabUtils> ();

		LoadExistData ();
		map = new MapData("map" + existMaps.Count);

		CreateToggles ();
		CreatePopupIndexes ();

		map.Id =  "map" + existMaps.Count;

		handleSize = .1f;
		pickSize = handleSize * 2f;
		maxHandleSize = handleSize * 2f;
		pathColor = Color.white;
		wayPointColor = Color.white;
		towerPointColor = Color.white;

		if (EditorPrefs.HasKey ("StepPerCurve")) {
			stepsPerCurve = EditorPrefs.GetInt ("StepPerCurve");
		} else {
			stepsPerCurve = 4;
		}

		if (EditorPrefs.HasKey ("TowerRange")) {
			towerRange = EditorPrefs.GetFloat ("TowerRange");
		} else {
			towerRange = 1;
		}

		selectedPathIndex = -1;
		selectedWaypointIndex = -1;
		selectedTowerpointIndex = -1;

	}

	void OnFocus () {
		LoadExistData ();
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
		SceneView.onSceneGUIDelegate += this.OnSceneGUI;
	}

	void OnDestroy () {
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
	}
	#endregion MONO
	protected void GetEditorSkin () {
		mapEditorSkin = (GUISkin)Resources.Load ("EditorSkins/MapEditorSkin");
	}
	#region Custom Inspector
	void OnGUI (){
		if (mapEditorSkin == null) {
			GetEditorSkin ();
		}
		// DrawDefaultInspector();	// test

		if(map == null)
			return;

		EditorGUILayout.LabelField ("MAP CONSTRUCTOR", mapEditorSkin.GetStyle("Header"), GUILayout.MinHeight (40));

		EditorGUILayout.BeginVertical ();

		EditorGUILayout.Space();

		this.map.Id = EditorGUILayout.TextField ("Map Id", map.Id);
		this.map.InitGold = EditorGUILayout.IntField ("Init Gold", map.InitGold);
		this.map.initLife = EditorGUILayout.IntField ("Init Life", map.InitLife);

		this.terrainGo = EditorGUILayout.ObjectField ("Terrain GO", this.terrainGo, typeof(GameObject), true);

		EditorGUILayout.Space();

		GUI.enabled = terrainGo == null && map.Id.Length > 0;
			if(GUILayout.Button("Create Terrain GO")) {
				terrainGo =	new GameObject (map.id);
			}
		GUI.enabled = true;

		EditorGUILayout.Space();

		OnPathInspectorGUI ();

		EditorGUILayout.Space();

		OnTowerPointInspectorGUI();

		EditorGUILayout.Space();

		GUI.enabled = (map.Paths != null && map.Paths.Count > 0);
		OnWaveInspectorGUI();
		GUI.enabled = true;

	

		if (map != null)
			OnDataInspectorGUI ();
		EditorGUILayout.EndVertical ();
		Repaint ();
	}

	private void OnPathInspectorGUI () {
		EditorGUILayout.BeginVertical("box");
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label("Path", mapEditorSkin.GetStyle("LabelA"));
		GUILayout.Label("(" + map.Paths.Count + " paths)", mapEditorSkin.GetStyle("LabelC"));

		GUILayout.FlexibleSpace ();

		if(GUILayout.Button("",  mapEditorSkin.GetStyle("AddButton"), GUILayout.MinWidth(16), GUILayout.MinHeight (16))) {
			CreatePath ();
			togglePaths.Add (false);
		}

		GUI.enabled = (map.Paths != null && map.Paths.Count > 0);
		if(GUILayout.Button("",  mapEditorSkin.GetStyle("ClearButton"), GUILayout.MinWidth(16), GUILayout.MinHeight (16))) {
			ClearPaths();
		}
		GUI.enabled = true;

		EditorGUILayout.EndHorizontal();

		GUILayout.Space (5);

//		EditorGUI.BeginChangeCheck ();
//		int _stepsPerCurve = Mathf.Clamp (EditorGUILayout.IntField ("Curve Smooth", stepsPerCurve), 4, 20);
//		if (EditorGUI.EndChangeCheck ()) {
//			this.stepsPerCurve = _stepsPerCurve;
//		}

		GUILayout.Space (5);
		if (map.Paths != null && map.Paths.Count > 0) {
			EditorGUILayout.BeginVertical("box");
			for (int pathIndex = 0; pathIndex < map.Paths.Count; pathIndex++)
			{
				EditorGUILayout.BeginHorizontal();
				
				if(GUILayout.Button("", mapEditorSkin.GetStyle("RemoveButton"), GUILayout.MinWidth(16), GUILayout.MinHeight (16))) {
					RemovePath (pathIndex);
					continue;
				}
				GUILayout.Label (map.Paths[pathIndex].Id, mapEditorSkin.GetStyle("LabelB"));
				GUILayout.Label ("(" + map.Paths[pathIndex].ControlPoints.Count + " points)",  mapEditorSkin.GetStyle("LabelC"));

				GUILayout.FlexibleSpace ();
				if(GUILayout.Button("",  mapEditorSkin.GetStyle("AddButton"), GUILayout.MinWidth(16), GUILayout.MinHeight (16))) {
//					AddWayPoint (i, new Vector3(map.Paths[i].ControlPoints.Count, 0f, i + 1));
					AddCurve (pathIndex);
				}
				GUI.enabled = (map.Paths[pathIndex].ControlPoints.Count > 0);
				if(GUILayout.Button("",  mapEditorSkin.GetStyle("ClearButton"), GUILayout.MinWidth(16), GUILayout.MinHeight (16))) {
					ClearWayPoints(pathIndex);	
				}
				GUI.enabled = true;
				if(GUILayout.Button("",  mapEditorSkin.GetStyle("AlignButton"), GUILayout.MinWidth(16), GUILayout.MinHeight (16))) {
					AlignWayPoints(pathIndex);	
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndVertical ();

			if (selectedPathIndex >= 0 && selectedPathIndex < map.Paths.Count) {
				if (map.Paths[selectedPathIndex].ControlPoints != null && map.Paths[selectedPathIndex].ControlPoints.Count > 0) {
					if (selectedWaypointIndex >= 0 && selectedWaypointIndex < map.Paths[selectedPathIndex].ControlPoints.Count) {
						EditorGUILayout.BeginVertical("box");
						EditorGUILayout.BeginHorizontal ();
						int selectedCurveIndex = selectedWaypointIndex == map.Paths[selectedPathIndex].ControlPointCount - 1 ? map.Paths[selectedPathIndex].CurveCount - 1 : selectedWaypointIndex / 3;
						if(GUILayout.Button("",  mapEditorSkin.GetStyle("RemoveButton"), GUILayout.MinWidth(16), GUILayout.MinHeight (16))) {
							RemoveCurve(selectedPathIndex, selectedCurveIndex);
							return;
						}
						GUILayout.Label ("Selected Curve",  mapEditorSkin.GetStyle("LabelB"));
						GUILayout.FlexibleSpace ();
						EditorGUILayout.EndHorizontal ();
						for (int i = 0; i <= 3; i++) {
							EditorGUILayout.BeginHorizontal ();

							GUILayout.Label ("path " + selectedPathIndex + " point " + (selectedCurveIndex * 3 + i), mapEditorSkin.GetStyle("LabelB")); 
							GUILayout.FlexibleSpace();
							EditorGUI.BeginChangeCheck();
							GUILayout.Label ("x");
							var posX = EditorGUILayout.FloatField (map.Paths[selectedPathIndex].ControlPoints[selectedCurveIndex * 3 + i].x, GUILayout.MaxWidth(60));
							GUILayout.Label ("y");
							var posY = EditorGUILayout.FloatField (map.Paths[selectedPathIndex].ControlPoints[selectedCurveIndex * 3 + i].y, GUILayout.MaxWidth(60));
							GUILayout.Label ("z");
							var posZ = EditorGUILayout.FloatField (map.Paths[selectedPathIndex].ControlPoints[selectedCurveIndex * 3 + i].z, GUILayout.MaxWidth(60));
							if(EditorGUI.EndChangeCheck()){
								map.Paths[selectedPathIndex].ControlPoints[selectedWaypointIndex] = new Vector3(posX, posY, posZ);
								EditorUtility.SetDirty(this);
							}
							EditorGUILayout.EndHorizontal ();
						}
						EditorGUILayout.EndVertical();
					}
				}
			}
		}
		EditorGUILayout.EndVertical();
	}

	private void OnTowerPointInspectorGUI () {
		EditorGUILayout.BeginVertical("box");

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label("Tower Point", mapEditorSkin.GetStyle("LabelA"));
		GUILayout.Label("(" + map.TowerPoints.Count + " points)", mapEditorSkin.GetStyle("LabelC"));

		GUILayout.FlexibleSpace ();

		if(GUILayout.Button("",  mapEditorSkin.GetStyle("AddButton"), GUILayout.MinWidth(16), GUILayout.MinHeight (16))) {
			if (map.TowerPoints == null)
				map.TowerPoints = new List<TowerPointData> ();
			AddTowerPoint(new Vector3(map.TowerPoints.Count, 0f, 0f));
		}

		GUI.enabled = (map.TowerPoints != null && map.TowerPoints.Count > 0);
		if(GUILayout.Button("",  mapEditorSkin.GetStyle("ClearButton"), GUILayout.MinWidth(16), GUILayout.MinHeight (16))) {
			ClearTowerPoints();
		}
		GUI.enabled = true;

		if(GUILayout.Button("",  mapEditorSkin.GetStyle("AlignButton"), GUILayout.MinWidth(16), GUILayout.MinHeight (16))) {
			AlignTowerPoints();
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Space (5);

		if (map.TowerPoints != null && map.TowerPoints.Count > 0){

			if (selectedTowerpointIndex >= 0 && selectedTowerpointIndex < map.TowerPoints.Count) {
				EditorGUILayout.BeginVertical("box");
				GUILayout.Label ("Selected Tower Point", mapEditorSkin.GetStyle("LabelB"));

				EditorGUILayout.BeginHorizontal ();
				if(GUILayout.Button("",  mapEditorSkin.GetStyle("RemoveButton"), GUILayout.MinWidth(16), GUILayout.MinHeight (16))) {
					RemoveTowerPoint (selectedTowerpointIndex);
				}
				GUILayout.Label ("tower" + selectedTowerpointIndex, mapEditorSkin.GetStyle("LabelB"));
				GUILayout.FlexibleSpace ();
				EditorGUI.BeginChangeCheck();
				GUILayout.Label ("x");
				var posX = EditorGUILayout.FloatField (map.TowerPoints[selectedTowerpointIndex].TowerPointPos.x, GUILayout.MaxWidth(60));
				GUILayout.Label ("y");
				var posY = EditorGUILayout.FloatField (map.TowerPoints[selectedTowerpointIndex].TowerPointPos.y, GUILayout.MaxWidth(60));
				GUILayout.Label ("z");
				var posZ = EditorGUILayout.FloatField (map.TowerPoints[selectedTowerpointIndex].TowerPointPos.z, GUILayout.MaxWidth(60));
				if (EditorGUI.EndChangeCheck ()) {
					map.TowerPoints[selectedTowerpointIndex].TowerPointPos = new Vector3(posX, posY, posZ);
					EditorUtility.SetDirty(this);
				}
				EditorGUILayout.EndHorizontal();

			

				EditorGUILayout.EndVertical();
			}
		}
		EditorGUILayout.EndVertical();
	}

	private void OnWaveInspectorGUI () {
		EditorGUILayout.BeginVertical("box");

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("Wave", mapEditorSkin.GetStyle("LabelA"));
		GUILayout.Label ("(" + map.Waves.Count + " waves)", mapEditorSkin.GetStyle("LabelC"));
		GUILayout.FlexibleSpace ();
		if(GUILayout.Button("",  mapEditorSkin.GetStyle("AddButton"), GUILayout.MinWidth(16), GUILayout.MinHeight (16))) {
			CreateWave();
		}
		if(GUILayout.Button("",  mapEditorSkin.GetStyle("ClearButton"), GUILayout.MinWidth(16), GUILayout.MinHeight (16))) {
			ClearWaves();
		}
		EditorGUILayout.EndHorizontal ();
		GUILayout.Space (5);

		// render wave data
		if (map.Waves!= null && map.Waves.Count > 0) {
			for (int i = 0; i < map.Waves.Count; i++)
			{
				EditorGUILayout.BeginVertical("box");
				EditorGUILayout.BeginHorizontal();
				if(GUILayout.Button("", mapEditorSkin.GetStyle("RemoveButton"), GUILayout.MinWidth(16), GUILayout.MinHeight (16))) {
					map.Waves.RemoveAt (i);
					continue;
				}
				GUILayout.Label (map.Waves[i].Id, mapEditorSkin.GetStyle("LabelB"));
				GUILayout.Label ("(" + map.Waves[i].Groups.Count + " groups)", mapEditorSkin.GetStyle("LabelC"));

				GUILayout.FlexibleSpace ();

				if(GUILayout.Button("", mapEditorSkin.GetStyle("AddButton"), GUILayout.MinWidth(16), GUILayout.MinHeight (16))) {
					var newGroup = new WaveGroupData("group" + map.Waves[i].Groups.Count, enemyIds[0], pathIds[0]);
					map.Waves[i].Groups.Add(newGroup);
					CreatePopupIndexes ();
				}
				GUI.enabled = (map.Waves[i].Groups.Count > 0);
				if(GUILayout.Button("", mapEditorSkin.GetStyle("ClearButton"), GUILayout.MinWidth(16), GUILayout.MinHeight (16))) {
					map.Waves[i].Groups.Clear();
				}
				GUI.enabled = true;
				EditorGUILayout.EndHorizontal();

				if (map.Waves[i].Groups != null && map.Waves[i].Groups.Count > 0) {
					toggleWaves[i] = EditorGUILayout.Foldout(toggleWaves[i], "Group");
					if (toggleWaves[i]) {
						for (int j = 0; j < map.Waves[i].Groups.Count; j++)
						{
							EditorGUILayout.BeginHorizontal();
							if(GUILayout.Button("", mapEditorSkin.GetStyle("RemoveButton"), GUILayout.MinWidth(16), GUILayout.MinHeight (16))) {
								map.Waves[i].Groups.RemoveAt(j);
								enemyPopupIndexes[i].RemoveAt(j);
								pathPopupIndexes[i].RemoveAt(j);
							}
							GUILayout.Label (map.Waves[i].Groups[j].Id, mapEditorSkin.GetStyle ("LabelC"));

							GUILayout.FlexibleSpace ();

							EditorGUI.BeginChangeCheck();
							enemyPopupIndexes[i][j] = EditorGUILayout.Popup (enemyPopupIndexes[i][j], enemyIds.ToArray());
							var amount = EditorGUILayout.IntField (map.Waves[i].Groups[j].Amount);
							var spawnInterval = EditorGUILayout.FloatField (map.Waves[i].Groups[j].SpawnInterval);
							var waveDelay = EditorGUILayout.FloatField (map.Waves[i].Groups[j].GroupDelay);
							pathPopupIndexes[i][j] = EditorGUILayout.Popup (pathPopupIndexes[i][j], pathIds.ToArray());
							if (EditorGUI.EndChangeCheck()){
								map.Waves[i].Groups[j].EnemyIdIndex = enemyPopupIndexes[i][j];
								map.Waves[i].Groups[j].EnemyId = enemyIds[enemyPopupIndexes[i][j]];
								map.Waves[i].Groups[j].Amount = amount;
								map.Waves[i].Groups[j].SpawnInterval = spawnInterval;
								map.Waves[i].Groups[j].GroupDelay = waveDelay;
								map.Waves[i].Groups[j].PathIdIndex = pathPopupIndexes[i][j];
								map.Waves[i].Groups[j].PathId = pathIds[pathPopupIndexes[i][j]];
								EditorUtility.SetDirty(this);
							}


							EditorGUILayout.EndHorizontal();
						}
					}
				}
				EditorGUILayout.EndHorizontal ();
			}
		}
		EditorGUILayout.EndVertical();
	}

	private void OnDataInspectorGUI () {
		EditorGUILayout.BeginHorizontal ();

		GUI.enabled = CheckFields();
		if (GUILayout.Button ("Save")) {
			dataUtils.CreateData(map);
			prefabUtils.CreatePrefab (terrainGo as GameObject);
			// TODO: Save map prefab;
		}
		GUI.enabled = true;
		if (GUILayout.Button ("Load")) {
			map = dataUtils.LoadData <MapData> ();
			if (map == null) {
				map = new MapData("map" + existMaps.Count);
			}

			if (terrainGo != null) {
				DestroyImmediate (terrainGo);
			}
			terrainGo = prefabUtils.InstantiatePrefab (ConstantString.PrefabPath + map.Id + ".prefab");

			CreateToggles ();
			CreatePopupIndexes ();

			UpdatePathID ();
		}
		if (GUILayout.Button ("Reset")) {
			if (EditorUtility.DisplayDialog ("Are you sure?", 
				"Do you want to reset map data?",
				"Yes", "No")) {
				ResetData();
			}
		}

		if (GUILayout.Button ("Delete")) {
			if (EditorUtility.DisplayDialog ("Are you sure?", 
				"Do you want to delete map data?",
				"Yes", "No")) {
				if (terrainGo) {
					DestroyImmediate (terrainGo);
				}
				dataUtils.DeleteData (ConstantString.DataPath + map.GetType().Name + "/" + map.Id + ".json");
				prefabUtils.DeletePrefab (ConstantString.PrefabPath + map.Id + ".prefab");
			}
		}
		EditorGUILayout.EndHorizontal();
	}

	private bool CheckFields () {
		var mapIdInput = !String.IsNullOrEmpty (map.Id);

		var pathInput = map.Paths != null && map.Paths.Count > 0;

		var towerPointInput = map.TowerPoints != null && map.TowerPoints.Count > 0;

		var waveInput = map.Waves != null && map.Waves.Count > 0;

		return terrainGo && mapIdInput && pathInput && towerPointInput && waveInput;

	}

	public void OnSceneGUI (SceneView _sceneView){
		if (map == null)
			return;

		// 2d gui on scene view
		Handles.BeginGUI();
		GUILayout.BeginArea(new Rect(10f, 10f, 250f, 120f), GUI.skin.box);
		GUILayout.Space(5f);
		this.handleSize = EditorGUILayout.Slider ("Point Size" ,this.handleSize, 0.01f, this.maxHandleSize);
		this.pathColor = EditorGUILayout.ColorField("Path Color", this.pathColor);
		this.wayPointColor = EditorGUILayout.ColorField("WP Color", this.wayPointColor);
		this.towerPointColor = EditorGUILayout.ColorField("TP Color", this.towerPointColor);

		EditorGUI.BeginChangeCheck ();
		int _stepsPerCurve = EditorGUILayout.IntSlider ("Step Per Curve", stepsPerCurve, 4 , 20);
		float _towerRange = EditorGUILayout.Slider ("Tower Range", towerRange, 1, 10);
		if (EditorGUI.EndChangeCheck ()) {
			this.stepsPerCurve = _stepsPerCurve;
			EditorPrefs.SetInt ("StepPerCurve", stepsPerCurve);

			this.towerRange = _towerRange;
			EditorPrefs.SetFloat ("TowerRange", towerRange);
		}
		GUILayout.EndArea();
		Handles.EndGUI(); 

		// render path on scene view
		if (map.Paths != null  && map.Paths.Count > 0){
			for (int pathIndex = 0; pathIndex < map.Paths.Count; pathIndex++){	
				if (map.Paths[pathIndex].ControlPointCount > 0) {
					Vector3 p0 = DrawWayPoint(pathIndex, 0);
					for (int pointIndex = 1; pointIndex < map.Paths[pathIndex].ControlPointCount; pointIndex += 3) {
						Vector3 p1 = DrawWayPoint(pathIndex, pointIndex);
						Vector3 p2 = DrawWayPoint(pathIndex, pointIndex + 1);
						Vector3 p3 = DrawWayPoint(pathIndex, pointIndex + 2);

						// draw tangent lines
						Handles.color = Color.gray;
						Handles.DrawLine(p0, p1);
						Handles.DrawLine(p2, p3);

						//			Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);

						// draw spline directions
						Vector3 lineStart = map.Paths[pathIndex].GetPoint(0f);
						Handles.color = Color.green;
						Handles.DrawLine(lineStart, lineStart + map.Paths[pathIndex].GetDirection(0f));

						int steps = stepsPerCurve * map.Paths[pathIndex].CurveCount;

						for (int stepIndex = 1; stepIndex <= steps; stepIndex++) {
							Vector3 lineEnd = map.Paths[pathIndex].GetPoint(stepIndex / (float)steps);
							// draw line between waypoint
							Handles.color = pathColor;
							Handles.DrawLine(lineStart, lineEnd);

							// draw waypoint direction
							Handles.color = Color.green;
							Handles.DrawLine(lineEnd, lineEnd +  map.Paths[pathIndex].GetDirection(stepIndex / (float)steps));
							lineStart = lineEnd;
						}

						p0 = p3;
					}
				}
			}	
		}

		// render towerpoints on scene view
		if (map.TowerPoints != null && map.TowerPoints.Count > 0){
			for (int towerIndex = 0; towerIndex < map.TowerPoints.Count; towerIndex++)
			{
				DrawTowerPoint (towerIndex);
			}
		}
		SceneView.RepaintAll();
	}

	private Vector3 DrawWayPoint (int pathIndex, int pointIndex) {
		Vector3 point = map.Paths[pathIndex].GetControlPoint(pointIndex);
		Handles.Label (point + new Vector3(handleSize, handleSize, handleSize), pathIndex+"-"+pointIndex, mapEditorSkin.GetStyle("LabelB"));
		Handles.color = wayPointColor;
		if (pointIndex == 0) {
			Handles.color = Color.green;
		}
		if (Handles.Button(point, Quaternion.identity, handleSize, pickSize, Handles.SphereCap)) {
			selectedPathIndex = pathIndex;
			selectedWaypointIndex = pointIndex;
			Repaint();
		}
		if (selectedPathIndex == pathIndex && selectedWaypointIndex == pointIndex) {
			EditorGUI.BeginChangeCheck();
			point = Handles.FreeMoveHandle(point, Quaternion.identity, handleSize, Vector3.one, Handles.CircleCap);
			if (EditorGUI.EndChangeCheck()) {
				map.Paths[pathIndex].SetControlPoint(pointIndex, point);
			}
		}
		return point;
	}

	private void DrawTowerPoint (int towerIndex) {
		Vector3 towerPoint = map.TowerPoints[towerIndex].TowerPointPos;

		Handles.Label (towerPoint +  V3Extension.FloatToVector3(handleSize), "t"+towerIndex, mapEditorSkin.GetStyle("LabelB"));

		Handles.color = towerPointColor;
//		if (Handles.Button (towerPoint, Quaternion.LookRotation(Vector3.up), handleSize, pickSize, Handles.CylinderCap)) {
		if (Handles.Button (towerPoint, Quaternion.identity, handleSize, pickSize, Handles.CubeCap)) {
			selectedTowerpointIndex = towerIndex;
			Repaint();
		}
		if (selectedTowerpointIndex == towerIndex) {
			Handles.DrawWireDisc (towerPoint, Vector3.up, towerRange);
			EditorGUI.BeginChangeCheck();
			towerPoint = Handles.FreeMoveHandle (towerPoint, Quaternion.identity, handleSize, Vector3.one, Handles.RectangleCap);
			if (EditorGUI.EndChangeCheck()) {
				map.TowerPoints[towerIndex].TowerPointPos = towerPoint;
			}
		}
	}
	#endregion Custom Inspector

	#region private methods
	private void CreatePath () {
		if (map.Paths == null)
			map.Paths = new List<PathData> ();
		PathData pathData = new PathData(map.Paths.Count, "path" + map.Paths.Count) ;
		map.Paths.Add(pathData);

		if (pathIds == null) 
			pathIds = new List<string> ();		

		pathIds.Add(pathData.Id);
		togglePaths.Add(false);
	}

	private void RemovePath (int _pathIndex) {
		map.Paths.RemoveAt (_pathIndex);	
		pathIds.RemoveAt (_pathIndex);

		UpdatePathID ();
	}

	private void ClearPaths () {
		map.Paths.Clear ();
		if (pathIds != null && pathIds.Count > 0)
			pathIds.Clear();
		ClearWaves ();
	}

	private void AddWayPoint (int pathIndex, Vector3 position) {
		map.Paths[pathIndex].ControlPoints.Add(position);
	}

	private void AddCurve (int pathIndex) {
		map.Paths[pathIndex].AddCurve (pathIndex);
	}

	private void RemoveCurve (int selectedPathIndex, int selectedCurveIndex) {
		if (map.Paths[selectedPathIndex].CurveCount > 1) {
			Debug.Log (selectedCurveIndex * 3 + " / " + 3);
			map.Paths[selectedPathIndex].ControlPoints.RemoveRange(selectedCurveIndex * 3 + 1, 3);//curveIndex * 3 + i);
		} else {
			RemovePath (selectedPathIndex);
		}
		selectedPathIndex = -1;
		selectedWaypointIndex = -1;
	}

	private void ClearWayPoints(int _pathIndex) {
		map.Paths[_pathIndex].ControlPoints.Clear();
	}

	private void AlignWayPoints(int _pathIndex) {
		for (int controlPointIndex = 0; controlPointIndex < map.Paths[_pathIndex].ControlPoints.Count; controlPointIndex++)
		{
			map.Paths[_pathIndex].ControlPoints[controlPointIndex] = new Vector3(map.Paths[_pathIndex].ControlPoints[controlPointIndex].x, 0f, map.Paths[_pathIndex].ControlPoints[controlPointIndex].z);
		}
	}

	private void AddTowerPoint (Vector3 position) {
		map.TowerPoints.Add (new TowerPointData("t" + map.TowerPoints.Count, position));
	}

	private void RemoveTowerPoint (int _selectedTowerpointIndex) {
		map.TowerPoints.RemoveAt(_selectedTowerpointIndex);
		selectedTowerpointIndex = -1;
	}

	private void ClearTowerPoints() {
		map.TowerPoints.Clear ();
	}

	private void AlignTowerPoints() {
		for (int towerPointIndex = 0; towerPointIndex < map.TowerPoints.Count; towerPointIndex++){
			map.TowerPoints[towerPointIndex].TowerPointPos = new Vector3(map.TowerPoints[towerPointIndex].TowerPointPos.x, 0f, map.TowerPoints[towerPointIndex].TowerPointPos.z);
		}
	}

	private void CreateWave(){
		if (map.Waves == null)
			map.Waves = new List<WaveData> ();
		map.Waves.Add (new WaveData("wave" + map.Waves.Count, new List<WaveGroupData> ()));
		toggleWaves.Add(false);
	}

	private void ClearWaves() {
		if (map.Waves != null)
			map.Waves.Clear();
		enemyPopupIndexes.Clear();
		pathPopupIndexes.Clear();
	}

	private void ResetData() {
		if (terrainGo) {
			DestroyImmediate (terrainGo);
		}
		ClearPaths();
		ClearTowerPoints();
		ClearWaves();

		pathIds.Clear ();
	}

	void LoadExistData () {
		existMaps = dataUtils.LoadAllData <MapData> ();
		UpdatePathID ();

		existEnemies = dataUtils.LoadAllData <CharacterData> ();
		GetEnemyIds ();

//		existTowers = DataManager.Instance.LoadAllData <TowerData> ();
	}

	private void GetEnemyIds () {
		enemyIds = new List<string> ();
		for (int index = 0; index < existEnemies.Count; index++) {
			enemyIds.Add(existEnemies[index].Id);
		}
	}

	private void UpdatePathID () {
		if (map != null) {
			pathIds = new List<string> ();
			for (int pathIndex = 0; pathIndex < map.Paths.Count; pathIndex++) {
				pathIds.Add(map.Paths[pathIndex].Id);		
			}
		}
	}

	private void CreateToggles () {
		togglePaths = new List <bool> ();
		toggleWaves = new List <bool> ();

		if (map.Paths != null && map.Paths.Count > 0) {
			togglePaths = new List<bool> (map.Paths.Count);
			for (int pathIndex = 0; pathIndex < map.Paths.Count; pathIndex++)
			{
				togglePaths.Add(false);
			}
		}
		if (map.Waves != null && map.Waves.Count > 0) {
			toggleWaves = new List<bool> (map.Waves.Count);
			for (int waveIndex = 0; waveIndex < map.Waves.Count; waveIndex++)
			{
				toggleWaves.Add(false);
			}
		}
	}

	private void CreatePopupIndexes () {
		enemyPopupIndexes = new List<List<int>> ();
		pathPopupIndexes = new List<List<int>> ();
		if (map.Waves != null && map.Waves.Count > 0) {
			for (int waveIndex = 0; waveIndex < map.Waves.Count; waveIndex++)
			{
				enemyPopupIndexes.Add(new List<int> ());
				pathPopupIndexes.Add (new List<int> ());
				for (int j = 0; j < map.Waves[waveIndex].Groups.Count; j++)
				{
					enemyPopupIndexes[waveIndex].Add (map.Waves[waveIndex].Groups[j].EnemyIdIndex);
					pathPopupIndexes[waveIndex].Add (map.Waves[waveIndex].Groups[j].PathIdIndex);
				}
			}
		}
	}
	#endregion private methods
}
