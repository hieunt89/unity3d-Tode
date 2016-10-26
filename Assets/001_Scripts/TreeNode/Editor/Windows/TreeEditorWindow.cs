using UnityEngine;
using UnityEditor;
using System;

public class TreeEditorWindow : EditorWindow {

	public static TreeEditorWindow currentWindow;
	public TreeNodeWorkView workView;
	public TreeNodePropertiesView propertiesView;

	public TreeGUI currentTree = null;

	private float viewPercentage = 1f;
	private Vector2 scrollPosition;
	private Rect virtualRect;
	private float virtualPadding = 50f;
	private float minX, minY, maxX, maxY;

	public static void InitTowerNodeEditorWindow () {
		currentWindow = (TreeEditorWindow)EditorWindow.GetWindow <TreeEditorWindow> ("Tree Editor", true);
		currentWindow.minSize = new Vector2 (500f, 400f);
		CreateViews ();
	}

	void OnEnable () {
		virtualRect = new Rect (0f, 0f, position.width, position.height);
		minX = minY = maxX = maxY = 0f;
	}

	void OnGUI () {
		if (workView == null || propertiesView == null) {
			CreateViews ();
			return;
		}

		Event e = Event.current;

//		ProcessEvent (e);

		UpdateVirtualRect ();
//		Debug.Log (virtualRect);
		scrollPosition =  GUI.BeginScrollView(new Rect(0f, 0f, position.width, position.height), scrollPosition, virtualRect); // <-- need to customize this viewrect (expandable by nodes + offset)
		BeginWindows ();
		workView.UpdateView (virtualRect, new Rect (0f, 0f, viewPercentage, 1f), e, currentTree);
		EndWindows ();
		GUI.EndScrollView ();

//		propertiesView.UpdateView (new Rect (position.width, position.y, position.width, position.height), 
//			new Rect (viewPercentage, 0f, 1f - viewPercentage, 1f), e, currentTree);

		Repaint ();
	}

	private void UpdateVirtualRect () {

		//TODO: find length between nodes

		if (currentTree != null) {
			if (currentTree.nodes.Count > 0) {
				minX = currentTree.nodes [0].nodeRect.x;
				minY = currentTree.nodes [0].nodeRect.y;
				for (int i = 0; i < currentTree.nodes.Count; i++) {
					if (currentTree.nodes[i].nodeRect.x <= minX)
						minX = currentTree.nodes [i].nodeRect.x;
					if (currentTree.nodes [i].nodeRect.y <= minY)
						minY = currentTree.nodes [i].nodeRect.y;
					if (currentTree.nodes [i].nodeRect.x + currentTree.nodes [i].nodeRect.width >= maxX)
						maxX = currentTree.nodes [i].nodeRect.x + currentTree.nodes [i].nodeRect.width;
					if (currentTree.nodes [i].nodeRect.y + currentTree.nodes [i].nodeRect.height >= maxY)
						maxY = currentTree.nodes [i].nodeRect.y + currentTree.nodes [i].nodeRect.height;
				}
				virtualRect = Rect.MinMaxRect(minX - virtualPadding, minY - virtualPadding, maxX + virtualPadding, maxY + virtualPadding);
			}
		} else {
			virtualRect = new Rect (0f, 0f, position.width, position.height);
		}
	}

	private static void CreateViews () {
		if (currentWindow != null) {
			currentWindow.workView = new TreeNodeWorkView ();
			currentWindow.propertiesView = new TreeNodePropertiesView ();
		} else {
			currentWindow = (TreeEditorWindow)EditorWindow.GetWindow <TreeEditorWindow> ();
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
