using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class MapEditorWindow : EditorWindow {
	
	private MapData map;
	private Event currentEvent;

	private float pointSize;
	private float maxPointSize;
	private Color baseColor;
	private Color pathColor;
	private Color wayPointColor;
	private Color towerPointColor;

	private bool quickCreationMode = false;
	private bool resetTool = false;

	private List<bool> togglePaths;
	private List<bool> toggleWaves;

	List<MapData> existMaps;
	List<CharacterData> existEnemies;

	List<List<int>> enemyPopupIndexes;
	List<List<int>> pathPopupIndexes;

	List<string> enemyIds;
	List<string> pathIds;
    
    [MenuItem("Window/Map Editor")]
    public static void ShowWindow()
    {
//        EditorWindow.GetWindow(typeof(MapConstructorWindow));
		EditorWindow.GetWindow <MapEditorWindow> ("Map Editor", true);
    }
    
	#region MONO
	void OnEnable () {
		map = new MapData("", 0, 0, new List<PathData>(), new List<TowerPointData>(), new List<WaveData>());
		LoadExistData ();


		CreateToggles ();
				CreatePopupIndexes ();
		GenerateStyle ();

		map.Id =  "map" + existMaps.Count;;

		pointSize = 1f;
		maxPointSize = 2f;
		baseColor = Color.white;
		pathColor = Color.white;
		wayPointColor = Color.white;
		towerPointColor = Color.white;

	}
	void OnFocus () {
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
		SceneView.onSceneGUIDelegate += this.OnSceneGUI;
	}
	void OnDestroy () {
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
	}
	#endregion MONO
   
	#region Custom Inspector
	void OnGUI (){
		// DrawDefaultInspector();	// test

		if(map == null)
			return;

		EditorGUILayout.LabelField ("MAP CONSTRUCTOR", headerStyle, GUILayout.MinHeight (40));

		EditorGUILayout.BeginVertical ();
		EditorGUILayout.Space();

		EditorGUI.BeginChangeCheck();
		var id = EditorGUILayout.TextField ("Map Id", map.Id);
		var initGold = EditorGUILayout.IntField ("Init Gold", map.InitGold);
		var initLife = EditorGUILayout.IntField ("Init Life", map.InitLife);
		var pointSize = EditorGUILayout.Slider("Point Size", this.pointSize, 0f, this.maxPointSize);
		var baseColor = EditorGUILayout.ColorField("Base Color", this.baseColor);
		var pathColor = EditorGUILayout.ColorField("Path Color", this.pathColor);
		var wayPointColor = EditorGUILayout.ColorField("Way Points Color", this.wayPointColor);
		var towerPointColor = EditorGUILayout.ColorField("Tower Points Color", this.towerPointColor);
		if(EditorGUI.EndChangeCheck()){
			this.map.Id = id;
			this.map.InitGold = initGold;
			this.map.initLife = initLife;
			this.pointSize = pointSize;	
			this.baseColor = baseColor;
			this.pathColor = pathColor;
			this.wayPointColor = wayPointColor;
			this.towerPointColor = towerPointColor;
			EditorUtility.SetDirty(this);
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

		GUI.enabled = (map.Paths != null && map.Paths.Count > 0);
		OnWaveInspectorGUI();
		GUI.enabled = true;

		if (map != null)
			OnDataInspectorGUI ();
		EditorGUILayout.EndVertical ();


		EditorGUILayout.LabelField ("fdj", footerStyle, GUILayout.MinHeight (20));
		EditorGUILayout.Space();

		Repaint ();
	}


	public void OnSceneGUI (SceneView _sceneView){
		if (map == null)
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
		Handles.color = baseColor;
//		Handles.Disc (map.transform.rotation, map.transform.position, Vector3.up, 1f, false, 1f);
//		Handles.Label (map.transform.position + new Vector3(map.pointSize, map.pointSize, map.pointSize), map.name);

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
		this.pointSize = GUILayout.HorizontalSlider (this.pointSize, 0f, this.maxPointSize, GUILayout.MinWidth(100));
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		Handles.EndGUI(); 

		// render waypoints on scene view
		if (map.Paths != null && map.Paths.Count > 0){
			for (int i = 0; i < map.Paths.Count; i++)
			{
				if (map.Paths[i].Points.Count > 0){
					for (int j = 0; j < map.Paths[i].Points.Count; j++)
					{
						Vector3 point = map.Paths[i].Points[j];

						Handles.Label (point + new Vector3(pointSize * .5f, pointSize * .5f, pointSize * .5f), i+"-"+j, titleAStyle);
						if (j == 0){
							Handles.color = Color.green;
						} else if (j == map.Paths[i].Points.Count - 1) {
							Handles.color = Color.red;
						} else {
							Handles.color = baseColor;
						}

						Handles.SphereCap (j, point, Quaternion.identity, pointSize * .5f);
						// draw line between way points
						if(j < map.Paths[i].Points.Count - 1) {
							Handles.color = pathColor;
							Vector3 newPoint = map.Paths[i].Points[j + 1];
							Handles.DrawLine (point, newPoint);
							Handles.color = baseColor;
						}

						map.Paths[i].Points[j] = Handles.FreeMoveHandle (point, Quaternion.identity, pointSize * .5f, Vector3.one, Handles.CircleCap);
						//							point = Handles.PositionHandle (point, Quaternion.identity);
					}
				}
			}
		}
		// render towerpoints on scene view
		if (map.TowerPoints != null && map.TowerPoints.Count > 0){
			for (int i = 0; i < map.TowerPoints.Count; i++)
			{
				// GameObject towerPoint = mapConstructor.towerPoints[i].towerPointGo;
				Vector3 towerPoint = map.TowerPoints[i].TowerPointPos;

				Handles.Label (towerPoint + new Vector3(pointSize * .5f, pointSize * .5f, pointSize * .5f), "t"+i, titleAStyle);
				Handles.color = towerPointColor;
				Handles.CubeCap (0, towerPoint, Quaternion.identity, pointSize * .5f);
				Handles.color = baseColor;

				map.TowerPoints[i].TowerPointPos = Handles.FreeMoveHandle (towerPoint, Quaternion.identity, pointSize * .5f, Vector3.one, Handles.RectangleCap);

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

		GUI.enabled = (map.Paths != null && map.Paths.Count > 0);
		if (GUILayout.Button ("Clear Paths", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
			ClearPaths();
		}
		GUI.enabled = true;

		EditorGUILayout.EndHorizontal();
		GUILayout.Space (5);

		var wpIdWidth = EditorGUIUtility.currentViewWidth * .1f;
		var posWidth = EditorGUIUtility.currentViewWidth * .6f;
		var btnWidth = EditorGUIUtility.currentViewWidth * .1f;

		if (map.Paths != null && map.Paths.Count > 0) {
			for (int i = 0; i < map.Paths.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				var pIdWidth = EditorGUIUtility.currentViewWidth * .3f;
				EditorGUIUtility.labelWidth = pIdWidth * .5f;
				EditorGUILayout.LabelField ("Path Id", map.Paths[i].Id, GUILayout.Width (pIdWidth));

				if(GUILayout.Button("Remove", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
					map.Paths.RemoveAt (i);
					pathIds.RemoveAt (i);

					GetPathIds ();
					continue;
				}
				if(GUILayout.Button("Add WP", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
					CreateWayPoint (i, new Vector3(map.Paths[i].Points.Count, 0f, i + 1));
				}
				GUI.enabled = (map.Paths[i].Points.Count > 0);
				if(GUILayout.Button("Clear WPs", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
					ClearWayPoints(i);	
				}
				GUI.enabled = true;
				if(GUILayout.Button("Align WPs", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
					AlignWayPoints(i);	
				}
				EditorGUILayout.EndHorizontal();



				if (map.Paths[i].Points.Count > 0) {
					togglePaths[i] = EditorGUILayout.Foldout(togglePaths[i], "Waypoint");
					if (togglePaths[i]) {
						EditorGUI.indentLevel++;
						for (int j = 0; j < map.Paths[i].Points.Count; j++)
						{
							EditorGUILayout.BeginHorizontal();


							EditorGUILayout.LabelField ("Id", "p" + j, GUILayout.Width (wpIdWidth));
							EditorGUI.BeginChangeCheck();
							var position = EditorGUILayout.Vector3Field("Pos", map.Paths[i].Points[j], GUILayout.Width(posWidth));
							if(EditorGUI.EndChangeCheck()){
								map.Paths[i].Points[j] = position;
								EditorUtility.SetDirty(this);
							}
							if (GUILayout.Button("Remove", GUILayout.Width (btnWidth))) {
								map.Paths[i].Points.RemoveAt(j);
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
			if (map.TowerPoints == null)
				map.TowerPoints = new List<TowerPointData> ();
			CreateTowerPoint(new Vector3(map.TowerPoints.Count, 0f, 0f));
		}

		GUI.enabled = (map.TowerPoints != null && map.TowerPoints.Count > 0);
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

		if (map.TowerPoints != null && map.TowerPoints.Count > 0){
			EditorGUI.indentLevel++;
			for (int i = 0; i < map.TowerPoints.Count; i++)
			{	
				EditorGUI.BeginChangeCheck();
				GUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("Id", "t" + i, GUILayout.Width (wpIdWidth));

				var pos = EditorGUILayout.Vector3Field ("Pos", map.TowerPoints[i].TowerPointPos, GUILayout.Width (posWidth));
				if (EditorGUI.EndChangeCheck ()) {
					map.TowerPoints[i].TowerPointPos = pos;
					EditorUtility.SetDirty(this);
				}

				if (GUILayout.Button("Remove", GUILayout.Width (btnWidth))) {
					map.TowerPoints.RemoveAt(i);
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
//		var posWidth = EditorGUIUtility.currentViewWidth * .6f;
//		var btnWidth = EditorGUIUtility.currentViewWidth * .1f;

		// render wave data
		if (map.Waves!= null && map.Waves.Count > 0) {
			for (int i = 0; i < map.Waves.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField ("Id",  map.Waves[i].Id);
				if(GUILayout.Button("Remove", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
					map.Waves.RemoveAt (i);
					continue;
				}
				GUILayout.Space (10);
				if(GUILayout.Button("Add Group", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
					var g = new WaveGroupData("g" + map.Waves[i].Groups.Count, enemyIds[0], pathIds[0]);
					map.Waves[i].Groups.Add(g);
					CreatePopupIndexes ();
				}
				GUI.enabled = (map.Waves[i].Groups.Count > 0);
				if(GUILayout.Button("Clear Groups", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {	
					map.Waves[i].Groups.Clear();
				}
				GUI.enabled = true;
				EditorGUILayout.EndHorizontal();

				if (map.Waves[i].Groups != null && map.Waves[i].Groups.Count > 0) {
					toggleWaves[i] = EditorGUILayout.Foldout(toggleWaves[i], "Group");
					if (toggleWaves[i]) {
						EditorGUI.indentLevel++;
						for (int j = 0; j < map.Waves[i].Groups.Count; j++)
						{
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField ("Id ", map.Waves[i].Groups[j].Id, GUILayout.Width (elementWidth));

							EditorGUI.BeginChangeCheck();
							enemyPopupIndexes[i][j] = EditorGUILayout.Popup (enemyPopupIndexes[i][j], enemyIds.ToArray(), GUILayout.Width (elementWidth));
							var amount = EditorGUILayout.IntField (map.Waves[i].Groups[j].Amount,  GUILayout.Width (elementWidth));
							var spawnInterval = EditorGUILayout.FloatField (map.Waves[i].Groups[j].SpawnInterval,  GUILayout.Width (elementWidth));
							var waveDelay = EditorGUILayout.FloatField (map.Waves[i].Groups[j].GroupDelay, GUILayout.Width (elementWidth));
							pathPopupIndexes[i][j] = EditorGUILayout.Popup (pathPopupIndexes[i][j], pathIds.ToArray(),  GUILayout.Width (elementWidth));
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

							if(GUILayout.Button("Remove", GUILayout.MinWidth (85), GUILayout.MaxWidth (85))) {
								map.Waves[i].Groups.RemoveAt(j);
								enemyPopupIndexes[i].RemoveAt(j);
								pathPopupIndexes[i].RemoveAt(j);
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
			DataManager.Instance.SaveData(map);
			//			LoadExistData ();
		}
		GUI.enabled = true;
		if (GUILayout.Button ("Load")) {
			map = DataManager.Instance.LoadData <MapData> ();
			if (map != null) {
				CreateToggles ();
				CreatePopupIndexes ();

				GetPathIds ();
			}
		}
		if (GUILayout.Button ("Reset")) {
			ResetData();
//			mc.ApplyModifiedProperties();	// ????

			// TODO: confirm windows
		}
		EditorGUILayout.EndHorizontal();
	}

	private bool CheckFields () {
		var mapIdInput = !String.IsNullOrEmpty (map.Id);

		var pathInput = map.Paths != null && map.Paths.Count > 0;

		var towerPointInput = map.TowerPoints != null && map.TowerPoints.Count > 0;

		var waveInput = map.Waves != null && map.Waves.Count > 0;

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
		if (map.Paths == null)
			map.Paths = new List<PathData> ();
		PathData pathData = new PathData("p" + map.Paths.Count, new List<Vector3> ()) ;
		map.Paths.Add(pathData);

		if (pathIds == null) 
			pathIds = new List<string> ();		

		pathIds.Add(pathData.Id);
		togglePaths.Add(false);
	}

	private void ClearPaths () {
		map.Paths.Clear ();
		if (pathIds != null && pathIds.Count > 0)
			pathIds.Clear();
		ClearWaves ();
	}

	private void CreateWayPoint (int pathIndex, Vector3 position) {
		map.Paths[pathIndex].Points.Add(position);
	}

	private void ClearWayPoints(int pathIndex) {
		map.Paths[pathIndex].Points.Clear();
	}

	private void AlignWayPoints(int pathIndex) {
		for (int j = 0; j < map.Paths[pathIndex].Points.Count; j++)
		{
			map.Paths[pathIndex].Points[j] = new Vector3(map.Paths[pathIndex].Points[j].x, 0f, map.Paths[pathIndex].Points[j].z);
		}
	}

	private void CreateTowerPoint (Vector3 position) {
		map.TowerPoints.Add (new TowerPointData("t" + map.TowerPoints.Count, position));
	}

	private void ClearTowerPoints() {
		map.TowerPoints.Clear ();
	}
	private void AlignTowerPoints() {
		for (int i = 0; i < map.TowerPoints.Count; i++){
			map.TowerPoints[i].TowerPointPos = new Vector3(map.TowerPoints[i].TowerPointPos.x, 0f, map.TowerPoints[i].TowerPointPos.z);
		}
	}
	private void CreateWave(){
		if (map.Waves == null)
			map.Waves = new List<WaveData> ();
		map.Waves.Add (new WaveData("w" + map.Waves.Count, new List<WaveGroupData> ()));
		toggleWaves.Add(false);
	}

	private void ClearWaves() {
		if (map.Waves != null)
			map.Waves.Clear();
		enemyPopupIndexes.Clear();
		pathPopupIndexes.Clear();
	}

	private void ResetData() {
		ClearPaths();
		ClearTowerPoints();
		ClearWaves();

		pathIds.Clear ();
	}

	void LoadExistData () {
		existMaps = DataManager.Instance.LoadAllData <MapData> ();
		GetPathIds ();

		existEnemies = DataManager.Instance.LoadAllData <CharacterData> ();
		GetEnemyIds ();
	}

	private void GetEnemyIds () {
		if (existEnemies.Count > 0) {
			enemyIds = new List<string> ();
			for (int i = 0; i < existEnemies.Count; i++) {
				enemyIds.Add(existEnemies[i].Id);
			}
		}
	}

	private void GetPathIds () {
		if (map.Paths.Count > 0) {
			pathIds = new List<string> ();
			for (int i = 0; i < map.Paths.Count; i++) {
				pathIds.Add(map.Paths[i].Id);		
			}
		}
	}

	private void CreateToggles () {
		togglePaths = new List <bool> ();
		toggleWaves = new List <bool> ();

		if (map.Paths != null && map.Paths.Count > 0) {
			togglePaths = new List<bool> (map.Paths.Count);
			for (int i = 0; i < map.Paths.Count; i++)
			{
				togglePaths.Add(false);
			}
		}
		if (map.Waves != null && map.Waves.Count > 0) {
			toggleWaves = new List<bool> (map.Waves.Count);
			for (int i = 0; i < map.Waves.Count; i++)
			{
				toggleWaves.Add(false);
			}
		}
	}

	private void CreatePopupIndexes () {
		enemyPopupIndexes = new List<List<int>> ();
		pathPopupIndexes = new List<List<int>> ();
		if (map.Waves != null && map.Waves.Count > 0) {
			for (int i = 0; i < map.Waves.Count; i++)
			{
				enemyPopupIndexes.Add(new List<int> ());
				pathPopupIndexes.Add (new List<int> ());
				for (int j = 0; j < map.Waves[i].Groups.Count; j++)
				{
					enemyPopupIndexes[i].Add (map.Waves[i].Groups[j].EnemyIdIndex);
					pathPopupIndexes[i].Add (map.Waves[i].Groups[j].PathIdIndex);
				}
			}
		}
	}


	#endregion private methods
}
