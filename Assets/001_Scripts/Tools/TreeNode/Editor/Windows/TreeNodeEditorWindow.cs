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

	bool toggleListView;

	void OnGUI () {
		// Get and process the current event
		if (workView == null)
			workView = new TreeNodeWorkView ();
		if (currentTree != null) {
			Event e = Event.current;
			ProcessEvents (e);

			scrollPosition = GUI.BeginScrollView (new Rect (0, 0, 2000, 2000), scrollPosition, new Rect (0, 0, 2000, 2000));
			BeginWindows ();
			workView.UpdateView (new Rect (0, 0, position.width, position.height), e, currentTree);
			EndWindows ();
			GUI.EndScrollView ();
		}

		GUILayout.BeginHorizontal (EditorStyles.toolbar);
		toggleListView = GUILayout.Toggle (toggleListView, "Trees", EditorStyles.toolbarButton, GUILayout.Width (50));

		GUILayout.FlexibleSpace ();

		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		if (toggleListView) {
			if (position.width > listViewWidth + 50f) {
				GUILayout.BeginVertical ("box", GUILayout.Width (listViewWidth), GUILayout.Height (position.height));
				DrawListView ();
				GUILayout.EndVertical ();
			}
		}
		GUILayout.FlexibleSpace ();

		if (currentTree != null) {
			if (currentTree.showNodeProperties) {
				if (position.width > listViewWidth + propertiesViewWidth + 50f) {
					GUILayout.BeginVertical ("box", GUILayout.Width (propertiesViewWidth), GUILayout.Height (position.height));
					EditorGUILayout.LabelField ("Show selected node properties");
					GUILayout.EndVertical ();
				}
			}
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

		treeList = ScriptableObject.CreateInstance<TreeList>();

		if (treeList) 
		{
			treeList.trees = new List<Tree<string>>();
		}
		dataUtils.CreateData <TreeList> (treeList);
	}

	string treeName;
	TreeType treeType;

	void DrawListView () {
		EditorGUILayout.LabelField ("Create New Tree");

		treeType = (TreeType) EditorGUILayout.EnumPopup (treeType);
		treeName = EditorGUILayout.TextField (treeName);
		GUI.enabled = !string.IsNullOrEmpty (treeName) && treeType != TreeType.None;
		if (GUILayout.Button ("Create")) {
//			AddTreeData (treeName, treeType);
			TreeEditorUtils.CreateTree (treeType, treeName);
		}
		GUI.enabled = true;	

		GUILayout.Space (10);

		EditorGUILayout.LabelField ("Tree List");
		for (int i = 0; i < treeList.trees.Count; i++) {
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button (treeList.trees[i].name)) {
				treeIndex = i;

				TreeEditorUtils.ConstructTree (treeList.trees [treeIndex]);

				toggleListView = false;
			}
			GUILayout.EndHorizontal ();
		}
	}

//	void AddTreeData (string treeName, TreeType treeType) {
//		Tree<string> newTreeData = new Tree<string>();
//
//		newTreeData.id =
//		newTreeData.name = treeName;
//		newTreeData.treeType = treeType;
//		newTreeData.Root = new Node<string> ("test");

//		Tree<string> newTree = new Tree<string> ( Guid.NewGuid().ToString(), treeType, treeName, new Node<string> ("test"));
//		treeList.trees.Add (newTree);
//		selectedTowerIndexes.Add (false);
//		treeIndex = treeList.trees.Count;
//	}
}
