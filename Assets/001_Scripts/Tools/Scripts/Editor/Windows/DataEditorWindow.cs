using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public enum GameDataType {
	Tower,
	Character,
	Projectile
}

public class DataEditorWindow : EditorWindow {

	public static DataEditorWindow dataEditorWindow;
	static ActionBarView actionBarView;
	static ItemListView itemListView; 
	static ProjectileDatalView projectileDetailView; 

	public TowerList towerList;
	public CharacterList characterList;
	public ProjectileList projectileList;

	public GameDataType selectedDataType;
	int viewIndex = 0;

	[MenuItem("Tode/Data Editor &D")]
	public static void DisplayWindow()
	{
		dataEditorWindow = EditorWindow.GetWindow <DataEditorWindow> ("Data Editor", true);
		dataEditorWindow.minSize = new Vector2 (400, 600); 
		CreateViews ();
	}

	void OnEnable () {
//		Debug.Log (actionBarView);
		// actionbar view is null -> onenable call before displaywindow
	}


	void OnGUI()
	{
		GUILayout.Space (10);
		selectedDataType = (GameDataType) EditorGUILayout.EnumPopup ("Game Data Type", selectedDataType);
		GUILayout.Space (20);

		switch (selectedDataType) {
		case GameDataType.Tower:
			towerList = AssetDatabase.LoadAssetAtPath (ConstantString.TowerDataPath, typeof(TowerList)) as TowerList;
			if (towerList == null) {
				CreateNewTowerList ();
				return;
			}
			break;

		case GameDataType.Character:
			characterList = AssetDatabase.LoadAssetAtPath (ConstantString.CharacterDataPath, typeof(CharacterList)) as CharacterList;
			if (characterList == null) {
				
				CreateNewCharacterList ();
				return;
			}
			break;

		case GameDataType.Projectile:
			projectileList = AssetDatabase.LoadAssetAtPath (ConstantString.ProjectileDataPath, typeof(ProjectileList)) as ProjectileList;
			if (projectileList == null) {
				CreateNewProjectileList ();
				return;
			}
			break;
		}

		if (actionBarView == null || itemListView == null || projectileDetailView == null) {
			CreateViews ();
			return;
		}

		BeginWindows ();

		actionBarView.UpdateView ();

		switch (viewIndex) {
		case 0:
			itemListView.UpdateView ();
			break;

		case 1:
			switch (selectedDataType) {
			case GameDataType.Tower:
//				projectileDetailView.UpdateView ();
				break;
			case GameDataType.Character:
//				projectileDetailView.UpdateView ();
				break;
			case GameDataType.Projectile:
				projectileDetailView.UpdateView ();
				break;
			}
			break;

		default:
			break;
		}
		EndWindows ();
	}

	void DrawListView () {

	}

	void DrawDetailView () {

	}

	static void CreateViews () {
		if (dataEditorWindow != null) {
			actionBarView = new ActionBarView ();
			itemListView = new ItemListView ();
			projectileDetailView = new ProjectileDatalView ();
		} else {
			dataEditorWindow = EditorWindow.GetWindow <DataEditorWindow> ("Data Editor", true);
			dataEditorWindow.minSize = new Vector2 (400, 600); 
		}
	}


	void CreateNewTowerList () {
//		projectileindex = 1;
		towerList = CreateAssetData <TowerList> (ConstantString.TowerDataPath);
		if (towerList) 
		{
			towerList.towers = new List<TowerData>();
		}

	}

	void CreateNewCharacterList () {
		//		projectileindex = 1;
		characterList = CreateAssetData <CharacterList> (ConstantString.CharacterDataPath);
		if (characterList) 
		{
			characterList.characters = new List<CharacterData>();
		}

	}
	void CreateNewProjectileList () {
		//		projectileindex = 1;
		projectileList = CreateAssetData <ProjectileList> (ConstantString.ProjectileDataPath);
		if (projectileList) 
		{
			projectileList.projectiles = new List<ProjectileData>();
		}
	}

//	[MenuItem("Assets/Create/Inventory Item List")]
	public static T CreateAssetData <T> (string path) where T : ScriptableObject
	{
		T asset = ScriptableObject.CreateInstance<T>();
		AssetDatabase.CreateAsset(asset, path);
		AssetDatabase.SaveAssets();
		return asset;
	}


}