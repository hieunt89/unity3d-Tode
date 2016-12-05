using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public enum ViewIndex {
	List,
	Detail
}
public class TreeNodeEditorWindow : EditorWindow {

	public static TreeNodeEditorWindow currentWindow;

	public TreeNodeListView listView;
	public TreeNodeDetailView detailView;

	public TreeGUI currentTree = null;
	public TreeType workingType = TreeType.None;

	IDataUtils dataUtils;
	public TreeList treeList;

	public ViewIndex viewIndex = ViewIndex.List;

	public static void InitTowerNodeEditorWindow () {
		currentWindow = (TreeNodeEditorWindow)EditorWindow.GetWindow <TreeNodeEditorWindow> ("Tree Editor", true);
		currentWindow.minSize = new Vector2 (500f, 400f);
	}

	void OnEnable () {
		dataUtils = DIContainer.GetModule <IDataUtils> ();
		treeList = dataUtils.LoadData <TreeList> ();
		if (treeList == null) {
			CreateTreeList ();
		}
		if (treeList.trees == null) {
			treeList.trees = new List<Tree<string>>();
		}
		CreateViews ();
	}

	void OnFocus () {

	}

	void CreateTreeList () {
//		towerIndex = 1;

		treeList = ScriptableObject.CreateInstance<TreeList>();
		if (treeList) 
		{
			treeList.trees = new List<Tree<string>>();
		}
		dataUtils.CreateData <TreeList> (treeList);
	}
	void OnGUI () {
		if (listView == null || detailView == null) {
			CreateViews ();
			return;
		}

		Event e = Event.current;

		ProcessEvent (e);

		BeginWindows ();
		switch (viewIndex) {
		case ViewIndex.List:
			listView.UpdateView (new Rect (position.x, position.y, position.width, position.height), new Rect (0f, 0f, 1f, 1f), e, currentTree);
			break;

		case ViewIndex.Detail:
			detailView.UpdateView (new Rect (position.x, position.y, position.width, position.height), new Rect (0f, 0f, 1f, 1f), e, currentTree);
			break;
		}
		EndWindows ();
//		if (Event.current.type == EventType.Layout || Event.current.type == EventType.Repaint)
//		{
//			Repaint ();
//		}
	}

	private static void CreateViews () {
		if (currentWindow != null) {
			currentWindow.listView = new TreeNodeListView ();
			currentWindow.detailView = new TreeNodeDetailView ();
		} else {
			currentWindow = (TreeNodeEditorWindow)EditorWindow.GetWindow <TreeNodeEditorWindow> ();
		}
	}

	private void ProcessEvent (Event _e) {
		if (_e.type == EventType.ScrollWheel) {
		}
//		if (_e.type == EventType.KeyDown && _e.keyCode == KeyCode.LeftArrow) {
//			viewPercentage -= 0.01f;
//		}
//		if (_e.type == EventType.KeyDown && _e.keyCode == KeyCode.RightArrow) {
//			viewPercentage += 0.01f;
//		}
	}
}
