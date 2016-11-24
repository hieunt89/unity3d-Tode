using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using System;

public class TowerEditorWindow : EditorWindow {
	static TowerEditorWindow towerEditorWindow;
	public TowerList towerList;
	TowerData tower;
	int towerIndex = 1;
	int viewIndex = 0;
	bool toggleEditMode = false;
	Vector2 scrollPosition;
	bool isSelectedAll = false;
	List<bool> selectedIndexes;

	List<ProjectileData> existProjectiles;

	bool toggleSkillTrees;
	List<string> existSkillTreeIDs;
	List<int> skillTreeIndexes;
	List<Tree<string>> existTrees;

	[MenuItem("Tode/Tower Editor &T")]
	public static void ShowWindow()
	{
		towerEditorWindow = EditorWindow.GetWindow <TowerEditorWindow> ("Tower Editor", true);
		towerEditorWindow.minSize = new Vector2 (400, 600);
	}

	void OnEnable () {

		towerList = AssetDatabase.LoadAssetAtPath (ConstantString.TowerDataPath, typeof(TowerList)) as TowerList;
		if (towerList == null) {
			CreateNewItemList ();
		}
		selectedIndexes = new List<bool> ();
		for (int i = 0; i < towerList.towers.Count ; i++) {
			selectedIndexes.Add (false);
		}
	}

	void OnGUI()
	{
		switch (viewIndex) {
		case 0:
			DrawTowerList ();
			break;
		case 1:
			DrawTowerDetail ();
			break;
		default:
			break;
		}
	}
	void DrawTowerList() {
		EditorGUILayout.BeginHorizontal ("box", GUILayout.Height (25));
		if (GUILayout.Button ("Add")) {
			AddTowerData ();
		}

		GUILayout.FlexibleSpace ();

		if (GUILayout.Button (toggleEditMode ? "Done" : "Edit Mode")) {
			toggleEditMode = !toggleEditMode;
		}
		if (toggleEditMode) {
			if (GUILayout.Button (isSelectedAll ? "Deselect All" : "Select All")) {
				isSelectedAll = !isSelectedAll;
				for (int i = 0; i < selectedIndexes.Count; i++) {
					selectedIndexes[i] = isSelectedAll;
				}
			}
			if (GUILayout.Button ("Delete Selected")) {
				if (EditorUtility.DisplayDialog ("Are you sure?", 
					"Do you want to delete all selected data?",
					"Yes", "No")) {
					for (int i = selectedIndexes.Count - 1; i >= 0; i--) {
						if (selectedIndexes[i]) {
							towerList.towers.RemoveAt (i);
							selectedIndexes.RemoveAt (i);
						}
					}
					isSelectedAll = false;
					toggleEditMode = false;
				}
			}
		}

		EditorGUILayout.EndHorizontal ();


		EditorGUILayout.BeginVertical ();
		scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition, GUILayout.Height (position.height - 40));
		for (int i = 0; i < towerList.towers.Count; i++) {
			EditorGUILayout.BeginHorizontal ();

			var btnLabel = towerList.towers[i].intId + " - " + towerList.towers[i].Name;
			if (GUILayout.Button (btnLabel)) {
				towerIndex = i;
				viewIndex = 1;
			}
			GUI.enabled = toggleEditMode;
			selectedIndexes[i] = EditorGUILayout.Toggle (selectedIndexes[i], GUILayout.Width (30));
			GUI.enabled = true;
			EditorGUILayout.EndHorizontal ();

		}
		EditorGUILayout.EndScrollView ();
		EditorGUILayout.EndVertical ();
	}

	void DrawTowerDetail () {
		GUI.SetNextControlName ("DummyFocus");
		GUI.Button (new Rect (0,0,0,0), "", GUIStyle.none);

		GUILayout.BeginHorizontal ("box");

		if (GUILayout.Button("<", GUILayout.ExpandWidth(false))) 
		{
			if (towerIndex > 1)
			{	
				towerIndex --;
				GUI.FocusControl ("DummyFocus");
			}

		}
		if (GUILayout.Button(">", GUILayout.ExpandWidth(false))) 
		{
			if (towerIndex < towerList.towers.Count) 
			{
				towerIndex ++;
				GUI.FocusControl ("Dummy");
			}
		}

		GUILayout.Space(100);

		if (GUILayout.Button("Add", GUILayout.ExpandWidth(false))) 
		{
			AddTowerData();
		}
		if (GUILayout.Button("Delete", GUILayout.ExpandWidth(false))) 
		{
			DeleteTowerData (towerIndex - 1);
		}

		GUILayout.FlexibleSpace ();

		if (GUILayout.Button("Back", GUILayout.ExpandWidth(false))) 
		{
			viewIndex = 0;
		}
		GUILayout.EndHorizontal ();


		if (towerList.towers == null)
			Debug.Log("wtf");
		if (towerList.towers.Count > 0) {
			GUILayout.BeginHorizontal ();
			towerIndex = Mathf.Clamp (EditorGUILayout.IntField ("Current Tower", towerIndex, GUILayout.ExpandWidth(false)), 1, towerList.towers.Count);	// important
			//Mathf.Clamp (viewIndex, 1, inventoryItemList.itemList.Count);
			EditorGUILayout.LabelField ("of   " +  towerList.towers.Count.ToString() + "  items", "", GUILayout.ExpandWidth(false));
			GUILayout.EndHorizontal ();
			GUILayout.Space(10);

			tower = towerList.towers [towerIndex - 1];
			tower.Id = EditorGUILayout.TextField ("Id", tower.Id);
			tower.Name = EditorGUILayout.TextField ("Name", tower.Name);
	
			tower.View = (GameObject) EditorGUILayout.ObjectField ("Tower GO", tower.View, typeof(GameObject), true);
//			GUI.enabled = towerGo == null && tower.Id.Length > 0;
//				if (GUILayout.Button ("Create Tower GO")) {
//					towerGo = new GameObject (tower.Id);
//				}
//			GUI.enabled = true;
	
//			towerIndex = EditorGUILayout.Popup ("Projectile", towerIndex, projectileIds.ToArray());
//			tower.ProjectileId = projectileIds[projectileIndex];
//	
//			GUILayout.BeginVertical ("box");
//			toggleProjectile = EditorGUILayout.Foldout (toggleProjectile, "Projectile Detail");
//			if (toggleProjectile) {
//				EditorGUILayout.LabelField ("Name", existProjectiles[projectileIndex].Name);
//				EditorGUILayout.LabelField ("Type", existProjectiles[projectileIndex].Type.ToString ());
//				EditorGUILayout.LabelField ("TravelSpeed", existProjectiles[projectileIndex].TravelSpeed.ToString ());
//				EditorGUILayout.LabelField ("Duration", existProjectiles[projectileIndex].Duration.ToString ());
//				EditorGUILayout.LabelField ("MaxDmgBuildTime", existProjectiles[projectileIndex].MaxDmgBuildTime.ToString ());
//				EditorGUILayout.LabelField ("TickInterval", existProjectiles[projectileIndex].TickInterval.ToString ());
//			}
//			GUILayout.EndVertical ();
	
			tower.AtkType =  (AttackType) EditorGUILayout.EnumPopup ("Attack Type", tower.AtkType);
			tower.AtkRange = EditorGUILayout.FloatField ("Tower Range", tower.AtkRange);
			tower.MinDmg = EditorGUILayout.IntField ("Min Damage", tower.MinDmg);
			tower.MaxDmg = EditorGUILayout.IntField ("Max Damage", tower.MaxDmg);
			tower.AtkSpeed = EditorGUILayout.FloatField ("Attack Speed", tower.AtkSpeed);
			tower.AtkTime = EditorGUILayout.FloatField ("Attack Time", tower.AtkTime);
			tower.TurnSpeed = EditorGUILayout.FloatField ("Turn Speed", tower.TurnSpeed);
			tower.AtkPoint = EditorGUILayout.Vector3Field ("Attack Point", tower.AtkPoint);
			tower.GoldRequired = EditorGUILayout.IntField ("Gold Cost", tower.GoldRequired);
			tower.BuildTime = EditorGUILayout.FloatField ("Build Time", tower.BuildTime);
			tower.Aoe = EditorGUILayout.FloatField ("AOE", tower.Aoe);
	
			toggleSkillTrees = EditorGUILayout.Foldout (toggleSkillTrees, "Skill Trees " + tower.TreeSkillNames.Count);
			if (toggleSkillTrees) {
//				for (int skillTreeIndex = 0; skillTreeIndex < tower.TreeSkillNames.Count; skillTreeIndex++) {
//					GUILayout.BeginHorizontal ();
//					skillTreeIndexes[skillTreeIndex] = EditorGUILayout.Popup (skillTreeIndexes[skillTreeIndex], existSkillTreeIDs.ToArray ());
//					tower.TreeSkillNames[skillTreeIndex] = existSkillTreeIDs [skillTreeIndexes[skillTreeIndex]];
//					if (GUILayout.Button ("Remove")) {
//						tower.TreeSkillNames.RemoveAt (skillTreeIndex);
//						skillTreeIndexes.RemoveAt (skillTreeIndex);
//						continue;
//					}
//					GUILayout.EndHorizontal ();
//				}
//				GUI.enabled = existSkillTreeIDs.Count > 0;
//				if (GUILayout.Button ("Add Skill Tree")) {
//					tower.TreeSkillNames.Add ("new");
//					skillTreeIndexes.Add (0);
//				}
//				GUI.enabled = true;
			}
			
		} else {
			GUILayout.Label ("This Tower List is Empty.");
		}
	}

	void CreateNewItemList () {
		towerIndex = 1;
		towerList = CreateTowerList();
		if (towerList) 
		{
			towerList.towers = new List<TowerData>();
		}
	}

	[MenuItem("Assets/Create/Inventory Item List")]
	public static TowerList CreateTowerList()
	{
		TowerList asset = ScriptableObject.CreateInstance<TowerList>();

		AssetDatabase.CreateAsset(asset, ConstantString.TowerDataPath);
		AssetDatabase.SaveAssets();
		return asset;
	}

	void AddTowerData () {
		TowerData newTowerData = new TowerData();
		int towerId = 0;
		if (towerList.towers.Count > 0){
			towerId =towerList.towers [towerList.towers.Count - 1].intId + 1;
		}else {
			towerId = 0;
		}
		newTowerData.intId = towerId;
		towerList.towers.Add (newTowerData);
		selectedIndexes.Add (false);
		towerIndex = towerList.towers.Count;
	}

	void DeleteTowerData (int index) {
		if (EditorUtility.DisplayDialog ("Are you sure?", 
			"Do you want to delete " + towerList.towers[index].intId + " data?",
			"Yes", "No")) {
			towerList.towers.RemoveAt (index);
			selectedIndexes.RemoveAt (index);
		}
	}

//	void LoadExistData () {
//		existTowers = dataUtils.LoadAllData <TowerData>();
//		existProjectiles = dataUtils.LoadAllData <ProjectileData>();
//		existTrees = binartyUtils.LoadAllData <Tree<string>> ();
//	}
//
//	void SetupProjectileIDs () {
//		if (existProjectiles.Count > 0) {
//			projectileIds = new List<string> ();
//			for (int i = 0; i < existProjectiles.Count; i++) {
//				projectileIds.Add (existProjectiles [i].Id);
//				if (tower.ProjectileId.Equals (existProjectiles [i].Id)) {
//					projectileIndex = i;
//				}
//			}				
//		}
//	}

	void LoadExistData () {
//		existProjectiles = dataUtils.LoadAllData <ProjectileData>();
//		existTrees = binartyUtils.LoadAllData <Tree<string>> ();
	}

	void SetupProjectileIDs () {
		if (existProjectiles.Count > 0) {
//			projectileIds = new List<string> ();
//			for (int i = 0; i < existProjectiles.Count; i++) {
//				projectileIds.Add (existProjectiles [i].Id);
//				if (tower.ProjectileId.Equals (existProjectiles [i].Id)) {
//					projectileIndex = i;
//				}
//			}				
		}
	}

	void SetupSkillTreeIndexes () {
		
		existSkillTreeIDs = new List<string> ();
		for (int i = 0; i < existTrees.Count; i++) {
			if (existTrees[i].treeType == TreeType.CombatSkills || existTrees[i].treeType == TreeType.SummonSkills ) 
				existSkillTreeIDs.Add(existTrees[i].id);
		}
		skillTreeIndexes.Clear();
		if (existSkillTreeIDs.Count > 0) {
			for (int i = 0; i < tower.TreeSkillNames.Count; i++) {
				skillTreeIndexes.Add (GetSkillTreeIndex (tower.TreeSkillNames[i]));
			}
		} else {
			tower.TreeSkillNames.Clear ();
		}
	}
	int GetSkillTreeIndex (string towerValue) {
		for (int i = 0; i < existSkillTreeIDs.Count; i++) {
			if (existSkillTreeIDs [i] != null) {
				if (towerValue.Equals (existSkillTreeIDs [i])) {
					return i;
				}
			}
		}
		return 0;
	}
//
//	void OnEnable () {
//		prefabUtils = DIContainer.GetModule <IPrefabUtils> ();
//		dataUtils = DIContainer.GetModule <IDataUtils> ();
//		binartyUtils = new BinaryUtils () as IDataUtils;
//
//		LoadExistData ();
//
//		tower = new TowerData("tower" + existTowers.Count);
//
//		SetupProjectileIDs ();
//		SetupSkillTreeIndexes ();
//
//
//	}
//
//	void OnFocus () {
//		LoadExistData ();
//		SetupProjectileIDs ();
//		SetupSkillTreeIndexes ();
//
//		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
//		SceneView.onSceneGUIDelegate += this.OnSceneGUI;
//	}
//
//	void OnDestroy () {
//		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
//	}
//
//
//	void OnGUI()
//	{
//		//		EditorGUI.BeginChangeCheck ();
//
//		tower.Id = EditorGUILayout.TextField ("Id", tower.Id);
//		tower.Name = EditorGUILayout.TextField ("Name", tower.Name);
//
//		towerGo = (GameObject) EditorGUILayout.ObjectField ("Tower GO", towerGo, typeof(GameObject), true);
//		GUI.enabled = towerGo == null && tower.Id.Length > 0;
//			if (GUILayout.Button ("Create Tower GO")) {
//				towerGo = new GameObject (tower.Id);
//			}
//		GUI.enabled = true;
//
//		projectileIndex = EditorGUILayout.Popup ("Projectile", projectileIndex, projectileIds.ToArray());
//		tower.ProjectileId = projectileIds[projectileIndex];
//
//		GUILayout.BeginVertical ("box");
//		toggleProjectile = EditorGUILayout.Foldout (toggleProjectile, "Projectile Detail");
//		if (toggleProjectile) {
//			EditorGUILayout.LabelField ("Name", existProjectiles[projectileIndex].Name);
//			EditorGUILayout.LabelField ("Type", existProjectiles[projectileIndex].Type.ToString ());
//			EditorGUILayout.LabelField ("TravelSpeed", existProjectiles[projectileIndex].TravelSpeed.ToString ());
//			EditorGUILayout.LabelField ("Duration", existProjectiles[projectileIndex].Duration.ToString ());
//			EditorGUILayout.LabelField ("MaxDmgBuildTime", existProjectiles[projectileIndex].MaxDmgBuildTime.ToString ());
//			EditorGUILayout.LabelField ("TickInterval", existProjectiles[projectileIndex].TickInterval.ToString ());
//		}
//		GUILayout.EndVertical ();
//
//		tower.AtkType =  (AttackType) EditorGUILayout.EnumPopup ("Attack Type", tower.AtkType);
//		tower.AtkRange = EditorGUILayout.FloatField ("Tower Range",tower.AtkRange);
//		tower.MinDmg = EditorGUILayout.IntField ("Min Damage", tower.MinDmg);
//		tower.MaxDmg = EditorGUILayout.IntField ("Max Damage", tower.MaxDmg);
//		tower.AtkSpeed = EditorGUILayout.FloatField ("Attack Speed", tower.AtkSpeed);
//		tower.AtkTime = EditorGUILayout.FloatField ("Attack Time", tower.AtkTime);
//		tower.TurnSpeed = EditorGUILayout.FloatField ("Turn Speed", tower.TurnSpeed);
//		tower.AtkPoint = EditorGUILayout.Vector3Field ("Attack Point", tower.AtkPoint);
//		tower.GoldRequired = EditorGUILayout.IntField ("Gold Cost", tower.GoldRequired);
//		tower.BuildTime = EditorGUILayout.FloatField ("Build Time", tower.BuildTime);
//		tower.Aoe = EditorGUILayout.FloatField ("AOE", tower.Aoe);
//
//		toggleSkillTrees = EditorGUILayout.Foldout (toggleSkillTrees, "Skill Trees " + tower.TreeSkillNames.Count);
//		if (toggleSkillTrees) {
//			for (int skillTreeIndex = 0; skillTreeIndex < tower.TreeSkillNames.Count; skillTreeIndex++) {
//				GUILayout.BeginHorizontal ();
//				skillTreeIndexes[skillTreeIndex] = EditorGUILayout.Popup (skillTreeIndexes[skillTreeIndex], existSkillTreeIDs.ToArray ());
//				tower.TreeSkillNames[skillTreeIndex] = existSkillTreeIDs [skillTreeIndexes[skillTreeIndex]];
//				if (GUILayout.Button ("Remove")) {
//					tower.TreeSkillNames.RemoveAt (skillTreeIndex);
//					skillTreeIndexes.RemoveAt (skillTreeIndex);
//					continue;
//				}
//				GUILayout.EndHorizontal ();
//			}
//			GUI.enabled = existSkillTreeIDs.Count > 0;
//			if (GUILayout.Button ("Add Skill Tree")) {
//				tower.TreeSkillNames.Add ("new");
//				skillTreeIndexes.Add (0);
//			}
//			GUI.enabled = true;
//		}
//
//		GUILayout.Space(5);
//
////		GUILayout.BeginHorizontal ();
//		GUI.enabled = CheckInputFields ();
//		if (GUILayout.Button("Save")){
//			tower.AtkPoint = towerGo.transform.InverseTransformPoint (tower.AtkPoint);
//
//			dataUtils.CreateData (tower);
//			prefabUtils.CreatePrefab (towerGo as GameObject);
//		}
//		GUI.enabled = true;
//
//		if (GUILayout.Button("Load")){
//			tower = dataUtils.LoadData <TowerData> ();
//			if(tower == null){
//				tower = new TowerData("tower" + existTowers.Count);
//			} else {
//				SetupProjectileIDs ();
//				SetupSkillTreeIndexes ();
//			}
//
//			if (towerGo) {
//				DestroyImmediate (towerGo);
//			}
//
//			towerGo = prefabUtils.InstantiatePrefab (ConstantString.PrefabPath + tower.Id + ".prefab");
//			if (towerGo != null) {
//				tower.AtkPoint = towerGo.transform.TransformPoint (tower.AtkPoint);
//			}
//		}
//
//		if (GUILayout.Button("Reset")){
//			if (EditorUtility.DisplayDialog ("Are you sure?", 
//				"Do you want to reset " + tower.Id + " data?",
//				"Yes", "No")) {
//				tower = new TowerData ("tower" + existTowers.Count);
//				if (towerGo) {
//					DestroyImmediate (towerGo);
//				}
//				skillTreeIndexes.Clear ();
//			}
//		}
//
//		if (GUILayout.Button("Delete")){
//			if (EditorUtility.DisplayDialog ("Are you sure?", 
//				"Do you want to delete " + tower.Id + " data?",
//				"Yes", "No")) {
//				if (towerGo) {
//					DestroyImmediate (towerGo);
//				}
//				dataUtils.DeleteData (ConstantString.DataPath + tower.GetType().Name + "/" + tower.Id + ".json");
//				prefabUtils.DeletePrefab (ConstantString.PrefabPath + tower.Id + ".prefab");
//				tower = new TowerData ("tower" + existTowers.Count);
//			}
//		}
////		GUILayout.EndHorizontal();
//
//		Repaint ();
//	}
//
//
//
//	public void OnSceneGUI (SceneView _sceneView){
//		Handles.color = Color.red;
//		tower.AtkPoint = Handles.FreeMoveHandle(tower.AtkPoint, Quaternion.identity, .1f, Vector3.one, Handles.SphereCap);
//	}
//
//	private bool CheckInputFields () {
//		var nameInput = !String.IsNullOrEmpty (tower.Name);
//
//		return towerGo && nameInput;
//	}
//

}
