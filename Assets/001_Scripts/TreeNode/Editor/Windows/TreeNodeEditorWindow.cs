using UnityEngine;
using UnityEditor;
using System;

public class TreeNodeEditorWindow <T> : EditorWindow {

	public static TreeNodeEditorWindow <T> currentWindow;
	public TreeNodeWorkView <string> workView;
	public TreeNodePropertiesView <string> propertiesView;

	public TreeUI currentTree = null;

	private float viewPercentage = .75f;

	public static void InitTowerNodeEditorWindow () {
		currentWindow = (TreeNodeEditorWindow <T>)EditorWindow.GetWindow <TreeNodeEditorWindow<T>> ();
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
		workView.UpdateView (position, new Rect (0f, 0f, viewPercentage, 1f), e, currentTree);
		propertiesView.UpdateView (new Rect (position.width, position.y, position.width, position.height), 
									new Rect (viewPercentage, 0f, 1f - viewPercentage, 1f), e, currentTree);

		Repaint ();
	}

	private static void CreateViews () {
		if (currentWindow != null) {
			Type typeParameterType = typeof(T);
			Debug.Log (typeParameterType);


//			currentWindow.workView = new TreeNodeWorkView <T> ();
//			currentWindow.propertiesView = new TreeNodePropertiesView <T> ();

			TreeNodePopupWindow<string>.InitTreeNodePopup ();
		} else {
			currentWindow = (TreeNodeEditorWindow<T>)EditorWindow.GetWindow <TreeNodeEditorWindow<T>> ();
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
