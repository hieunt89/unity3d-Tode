using UnityEngine;
using UnityEditor;
using System;

public class TreeNodeEditorWindow : EditorWindow {

	public static TreeNodeEditorWindow currentWindow;
	public TreeNodeWorkView workView;
	public TreeNodePropertiesView propertiesView;

	public TreeUI currentTree = null;

	private float viewPercentage = 1f;
	private Vector2 scrollPosition;
	private Rect virtualRect;
	private float virtualPadding = 20f;
	private float virtualX, virtualY, virtualWidth, virtualHeight;

	public static void InitTowerNodeEditorWindow () {
		currentWindow = (TreeNodeEditorWindow)EditorWindow.GetWindow <TreeNodeEditorWindow> ();
		currentWindow.titleContent = new GUIContent ("Tree Node");
		CreateViews ();
	}
	void OnEnable () {
		virtualRect = new Rect (0f, 0f, position.width, position.height);
		virtualX = virtualY = virtualWidth = virtualHeight = 0f;
	}

	void OnGUI () {
		if (workView == null || propertiesView == null) {
			CreateViews ();
			return;
		}

		Event e = Event.current;

		ProcessEvent (e);

		scrollPosition =  GUI.BeginScrollView(new Rect(0f, 0f, position.width, position.height), scrollPosition, virtualRect); // <-- need to customize this viewrect (expandable by nodes + offset)
		BeginWindows ();
		workView.UpdateView (position, new Rect (0f, 0f, viewPercentage, 1f), e, currentTree);
		EndWindows ();
		GUI.EndScrollView ();

//		propertiesView.UpdateView (new Rect (position.width, position.y, position.width, position.height), 
//			new Rect (viewPercentage, 0f, 1f - viewPercentage, 1f), e, currentTree);

		UpdateVirtualRect ();
		Repaint ();
	}

	private void UpdateVirtualRect () {
		if (currentTree != null) {
			if (currentTree.nodes.Count > 0) {
				virtualX = currentTree.nodes [0].nodeRect.x;
				virtualY = currentTree.nodes [0].nodeRect.y;

				for (int i = 0; i < currentTree.nodes.Count; i++) {
					if (currentTree.nodes[i].nodeRect.x < virtualX)
						virtualX = currentTree.nodes [i].nodeRect.x;
					if (currentTree.nodes [i].nodeRect.y < virtualY)
						virtualY = currentTree.nodes [i].nodeRect.y;
					if (currentTree.nodes [i].nodeRect.x + currentTree.nodes [i].nodeRect.width > virtualWidth)
						virtualWidth = currentTree.nodes [i].nodeRect.x + currentTree.nodes [i].nodeRect.width;
					if (currentTree.nodes [i].nodeRect.y + currentTree.nodes [i].nodeRect.height > virtualHeight)
						virtualHeight = currentTree.nodes [i].nodeRect.y + currentTree.nodes [i].nodeRect.height;
				}
				virtualRect = new Rect (virtualX - virtualPadding, virtualY - virtualPadding, virtualWidth - virtualX + virtualPadding, virtualHeight - virtualY + virtualPadding);
			}
		}
	}

	private static void CreateViews () {
		if (currentWindow != null) {
			currentWindow.workView = new TreeNodeWorkView ();
			currentWindow.propertiesView = new TreeNodePropertiesView ();
		} else {
			currentWindow = (TreeNodeEditorWindow)EditorWindow.GetWindow <TreeNodeEditorWindow> ();
		}
	}

	private void ProcessEvent (Event _e) {
		// TODO: toggle properties or something with hotkey

//		if (_e.type == EventType.KeyDown && _e.keyCode == KeyCode.LeftArrow) {
//			viewPercentage -= 0.01f;
//		}
//		if (_e.type == EventType.KeyDown && _e.keyCode == KeyCode.RightArrow) {
//			viewPercentage += 0.01f;
//		}
	}
}
