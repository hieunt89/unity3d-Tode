using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class TreeNodeEditorWindow : EditorWindow {

	public static TreeNodeEditorWindow currentWindow;
	public TreeNodeListView listView;
	public TreeNodeWorkView treeView;
	public TreeGUI treeGUI = null;
	public TreeType workingType;
	public TreeList treeList;

	IDataUtils dataUtils;

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
		}
		if (treeList) {
			if (treeList.trees == null) 
				treeList.trees = new List<Tree<string>> ();
		}

		CreateViews ();
	}

	void OnGUI () {
		if (listView == null || treeView == null) {
			CreateViews ();
			return;
		}


		// Get and process the current event
		Event e = Event.current;
		ProcessEvents (e);
		//		currentGraph = new NodeGraph ();
		listView.UpdateView (position, new Rect(0f,0f,viewPercentage,1f), e, treeGUI); // 
		treeView.UpdateView (new Rect(position.width, position.y, position.width, position.height), new Rect(viewPercentage, 0f, 1-viewPercentage, 1f), e, treeGUI);
		Repaint ();
	}

	static void CreateViews () {
		if (currentWindow != null) {
			currentWindow.listView = new TreeNodeListView ();
			currentWindow.treeView = new TreeNodeWorkView ();
		} else {
			currentWindow = (TreeNodeEditorWindow) EditorWindow.GetWindow<TreeNodeEditorWindow> ();
		}
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
//		towerIndex = 1;
		//		towerList = CreateTowerList();

		treeList = ScriptableObject.CreateInstance<TreeList>();

		if (treeList) 
		{
			treeList.trees = new List<Tree<string>>();
		}
		dataUtils.CreateData <TreeList> (treeList);
	}
}
