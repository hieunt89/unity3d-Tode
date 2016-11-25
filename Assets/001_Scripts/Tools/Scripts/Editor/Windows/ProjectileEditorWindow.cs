using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ProjectileEditorWindow : EditorWindow {

	public ProjectileList projectileList;
	ProjectileData projectile;

	int projectileIndex = 1;
	int viewIndex = 0;
	bool toggleEditMode = false;
	Vector2 scrollPosition;
	bool isSelectedAll = false;
	List<bool> selectedIndexes;

	[MenuItem("Tode/Projectile Editor &P")]
	public static void ShowWindow()
	{
		var projectileEditorWindow = EditorWindow.GetWindow <ProjectileEditorWindow> ("Projectile Editor", true);
		projectileEditorWindow.minSize = new Vector2 (400, 600); 
	}

	void OnEnable () {
		
		projectileList = AssetDatabase.LoadAssetAtPath (ConstantString.ProjectileDataPath, typeof(ProjectileList)) as ProjectileList;
		if (projectileList == null) {
			CreateNewItemList ();
		}
		selectedIndexes = new List<bool> ();
		for (int i = 0; i < projectileList.projectiles.Count ; i++) {
			selectedIndexes.Add (false);
		}
	}

	void OnGUI()
	{
		switch (viewIndex) {
		case 0:
			DrawProjectileList ();
			break;
		case 1:
			DrawSelectedProjectile ();
			break;
		default:
			break;
		}
	}

	void DrawProjectileList () {
		EditorGUILayout.BeginHorizontal ("box", GUILayout.Height (25));
		if (GUILayout.Button ("Add")) {
			AddProjectileData ();
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
							projectileList.projectiles.RemoveAt (i);
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
		for (int i = 0; i < projectileList.projectiles.Count; i++) {
			EditorGUILayout.BeginHorizontal ();

			var btnLabel = projectileList.projectiles[i].intId + " - " + projectileList.projectiles[i].Name;
			if (GUILayout.Button (btnLabel)) {
				projectileIndex = i;
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

	void DrawSelectedProjectile () {
		GUI.SetNextControlName ("DummyFocus");
		GUI.Button (new Rect (0,0,0,0), "", GUIStyle.none);

		GUILayout.BeginHorizontal ("box");

		if (GUILayout.Button("<", GUILayout.ExpandWidth(false))) 
		{
			if (projectileIndex > 1)
			{	
				projectileIndex --;
				GUI.FocusControl ("DummyFocus");
			}

		}
		if (GUILayout.Button(">", GUILayout.ExpandWidth(false))) 
		{
			if (projectileIndex < projectileList.projectiles.Count) 
			{
				projectileIndex ++;
				GUI.FocusControl ("Dummy");
			}
		}

		GUILayout.Space(100);

		if (GUILayout.Button("Add", GUILayout.ExpandWidth(false))) 
		{
			AddProjectileData();
		}
		if (GUILayout.Button("Delete", GUILayout.ExpandWidth(false))) 
		{
			DeleteProjectileData (projectileIndex - 1);
		}

		GUILayout.FlexibleSpace ();

		if (GUILayout.Button("Back", GUILayout.ExpandWidth(false))) 
		{
			viewIndex = 0;
		}
		GUILayout.EndHorizontal ();


		if (projectileList.projectiles == null)
			Debug.Log("wtf");
		if (projectileList.projectiles.Count > 0) 
		{
			GUILayout.BeginHorizontal ();
			projectileIndex = Mathf.Clamp (EditorGUILayout.IntField ("Current Item", projectileIndex, GUILayout.ExpandWidth(false)), 1, projectileList.projectiles.Count);
			EditorGUILayout.LabelField ("of   " +  projectileList.projectiles.Count.ToString() + "  items", "", GUILayout.ExpandWidth(false));
			GUILayout.EndHorizontal ();
			GUILayout.Space(10);

			projectile = projectileList.projectiles[projectileIndex-1];
			EditorGUILayout.LabelField ("id",  projectile.intId.ToString());
			projectile.Name = EditorGUILayout.TextField ("Name", projectile.Name);
			projectile.View = (GameObject) EditorGUILayout.ObjectField ("Projectile GO", projectile.View, typeof(GameObject), true);
			projectile.Type = (ProjectileType) EditorGUILayout.EnumPopup ("Type", projectile.Type);

			if (projectile.Type == ProjectileType.homing) {
				projectile.Duration = 0f;
			}

			GUI.enabled = projectile.Type == ProjectileType.homing;
			projectile.TravelSpeed = EditorGUILayout.FloatField ("Travel Speed", projectile.TravelSpeed);
			GUI.enabled = true;

			GUI.enabled = projectile.Type == ProjectileType.throwing || projectile.Type == ProjectileType.laser;
			projectile.Duration = EditorGUILayout.FloatField ("Duration", projectile.Duration);
			GUI.enabled = true;

			GUI.enabled = projectile.Type == ProjectileType.laser;
			projectile.MaxDmgBuildTime = EditorGUILayout.FloatField ("Time to reach maxDmg", projectile.MaxDmgBuildTime);
			projectile.TickInterval = EditorGUILayout.FloatField ("Tick interval", projectile.TickInterval);
			GUI.enabled = true;

			GUILayout.Space(10);

		} 
		else 
		{
			GUILayout.Label ("This Projectile List is Empty.");
		}
	}

	void OnDestroy () {

	}

	void OpenItemList () {

	}

	void CreateNewItemList () {
		projectileIndex = 1;
		projectileList = CreateProjectileList();
		if (projectileList) 
		{
			projectileList.projectiles = new List<ProjectileData>();
		}
	}

	[MenuItem("Assets/Create/Inventory Item List")]
	public static ProjectileList  CreateProjectileList()
	{
		ProjectileList asset = ScriptableObject.CreateInstance<ProjectileList>();

		AssetDatabase.CreateAsset(asset, ConstantString.ProjectileDataPath);
		AssetDatabase.SaveAssets();
		return asset;
	}

	void DeleteProjectileData (int index) 
	{
		if (EditorUtility.DisplayDialog ("Are you sure?", 
			"Do you want to delete " + projectileList.projectiles[index].intId + " data?",
			"Yes", "No")) {
			projectileList.projectiles.RemoveAt (index);
			selectedIndexes.RemoveAt (index);
		}
	}

	void AddProjectileData () {
		ProjectileData newProjectileData = new ProjectileData();
		int projectileId = 0;
		if (projectileList.projectiles.Count > 0){
			projectileId = projectileList.projectiles [projectileList.projectiles.Count - 1].intId + 1;
		}else {
			projectileId = 0;
		}
		newProjectileData.intId = projectileId;
		projectileList.projectiles.Add (newProjectileData);
		selectedIndexes.Add (false);
		projectileIndex = projectileList.projectiles.Count;
	}

}
