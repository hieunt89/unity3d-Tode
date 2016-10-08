using UnityEngine;
using UnityEditor;
using System;

public class TreeNodeEditorWindow : EditorWindow {

	public static TreeNodeEditorWindow currentWindow;
	public TreeNodeWorkView workView;
	public TreeNodePropertiesView propertiesView;

	public TreeUI currentTree = null;

	private float viewPercentage = .75f;
	private Vector2 scrollPosition;

	public static void InitTowerNodeEditorWindow () {
		currentWindow = (TreeNodeEditorWindow)EditorWindow.GetWindow <TreeNodeEditorWindow> ();
		currentWindow.titleContent = new GUIContent ("Tree Node");
		CreateViews ();
	}

	void OnGUI () {
		if (workView == null || propertiesView == null) {
			CreateViews ();
			return;
		}

		Event e = Event.current;

		ProcessEvent (e);

		scrollPosition =  GUI.BeginScrollView(new Rect(0f, 0f, position.width, position.height), scrollPosition, new Rect(0f, 0f, 1000, 1000)); // <-- need to customize this viewrect (expandable by nodes + offset)
		BeginWindows ();
		workView.UpdateView (position, new Rect (0f, 0f, viewPercentage, 1f), e, currentTree);
		EndWindows ();
		GUI.EndScrollView ();

		propertiesView.UpdateView (new Rect (position.width, position.y, position.width, position.height), 
			new Rect (viewPercentage, 0f, 1f - viewPercentage, 1f), e, currentTree);


		Repaint ();
	}

//	void DoWindow (int windowId) {
//		GUILayout.Button("Hi");
//		GUI.DragWindow ();
//	}
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
