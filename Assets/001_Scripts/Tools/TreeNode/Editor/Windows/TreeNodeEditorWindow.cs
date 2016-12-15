using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class TreeNodeEditorWindow : EditorWindow {

	public static TreeNodeEditorWindow currentWindow;

	TreeNodeWorkView workView;

	public TreeGUI currentTree = null;
	public TreeType workingType;
	public TreeList treeList;


	IDataUtils dataUtils;

	private float listViewWidth = 200f;
	private float propertiesViewWidth = 200f;

	private Vector2 scrollPosition;
	private int treeIndex;
	private static float viewPercentage= 0.25f;
	public static void InitTreeNodeEditorWindow () {
		currentWindow = (TreeNodeEditorWindow)EditorWindow.GetWindow <TreeNodeEditorWindow> ();
		currentWindow.titleContent = new GUIContent ("Tree Node Editor");
	}

	void OnEnable () {
		dataUtils = DIContainer.GetModule<IDataUtils> ();
		treeList = dataUtils.LoadData <TreeList> ();
		if (treeList == null) {
			// Create new tree list
			CreateNewTreeList ();
		} else {
			if (treeList.trees == null) {
				treeList.trees = new List<Tree<string>> ();
			}
		}
//		CreateViews ();
	}

	bool createNewTree;

	void OnGUI () {
		// Get and process the current event
		if (workView == null)
			workView = new TreeNodeWorkView ();
		if (currentTree != null) {
			Event e = Event.current;
			ProcessEvents (e);

			scrollPosition = GUI.BeginScrollView (new Rect(0f, 0f, position.width, position.height), scrollPosition, new Rect (0, 0, 2000, 2000));
			BeginWindows ();
			workView.UpdateView (position, e, currentTree);
			EndWindows ();
			GUI.EndScrollView ();
		}

		GUILayout.BeginHorizontal (EditorStyles.toolbar);
		if (GUILayout.Button ("Back", EditorStyles.toolbarButton, GUILayout.Width (50))) {
			
		}
		if (GUILayout.Button ("Create", EditorStyles.toolbarButton, GUILayout.Width (50))) {
			createNewTree = true;
		}
		GUILayout.FlexibleSpace ();

		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		if (createNewTree) {
			if (position.width > listViewWidth + 50f) {
				GUILayout.BeginVertical ("box", GUILayout.Width (listViewWidth), GUILayout.Height (position.height));
			
//				if (GUILayout.Button ("Create", EditorStyles.toolbarButton, GUILayout.Width (50))) {
//					TreeEditorUtils.CreateTree (TreeType.Towers, "test");
//					createNewTree = false;
//				}

				DrawListView ();
				GUILayout.EndVertical ();
			}
		}
		GUILayout.FlexibleSpace ();

		if (position.width > listViewWidth + propertiesViewWidth + 50f) {
			GUILayout.BeginVertical ("box", GUILayout.Width (propertiesViewWidth), GUILayout.Height (position.height));
			EditorGUILayout.LabelField ("Test");
			GUILayout.EndVertical ();
		}
		GUILayout.EndHorizontal ();

		Repaint ();
	}

	void ProcessEvents (Event e)
	{
		if (e.type == EventType.KeyDown && e.keyCode == KeyCode.LeftArrow) {
			viewPercentage -= 0.01f;
		}
		if (e.type == EventType.KeyDown && e.keyCode == KeyCode.RightArrow) {
			viewPercentage += 0.01f;
		}
	}
	void CreateNewTreeList () {
		treeIndex = 1;
		//		towerList = CreateTowerList();

		treeList = ScriptableObject.CreateInstance<TreeList>();

		if (treeList) 
		{
			treeList.trees = new List<Tree<string>>();
		}
		dataUtils.CreateData <TreeList> (treeList);
	}

	void DrawListView () {
		EditorGUILayout.LabelField ("Tree List");

		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Add Tree")) {
			AddTowerData ();
		}
		GUILayout.EndHorizontal ();

		for (int i = 0; i < treeList.trees.Count; i++) {
//			EditorGUILayout.LabelField (treeList.trees[i].id);
			if (GUILayout.Button (treeList.trees[i].id)) {
				treeIndex = i;
				TreeEditorUtils.ConstructTree (treeList.trees [treeIndex]);
			}
		}
	}

	void AddTowerData () {
		Tree<string> newTreeData = new Tree<string>();
		newTreeData.id = Guid.NewGuid().ToString();
		treeList.trees.Add (newTreeData);
//		selectedTowerIndexes.Add (false);
		treeIndex = treeList.trees.Count;
	}
}
