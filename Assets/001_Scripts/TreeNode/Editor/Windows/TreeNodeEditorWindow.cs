using UnityEngine;
using UnityEditor;

public class TreeNodeEditorWindow : EditorWindow {

	public static TreeNodeEditorWindow currentWindow;
	public TreeNodeWorkView workView;
	public TreeNodePropertiesView propertiesView;

	public TreeUI currentTree = null;

	private float viewPercentage = .75f;

	public static void InitTowerNodeEditorWindow () {
		currentWindow = (TreeNodeEditorWindow)EditorWindow.GetWindow <TreeNodeEditorWindow> ();
		currentWindow.titleContent = new GUIContent ("Tree Node");
		CreateViews ();
	}


	void OnGUI () {
		if (workView == null) { //|| propertiesView == null) {
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
			currentWindow.workView = new TreeNodeWorkView ();
			currentWindow.propertiesView = new TreeNodePropertiesView ();

			TreeNodePopupWindow.InitTreeNodePopup ();
		} else {
			currentWindow = (TreeNodeEditorWindow)EditorWindow.GetWindow <TreeNodeEditorWindow> ();
		}
	}

	private void ProcessEvent (Event _e) {
		if (_e.type == EventType.KeyDown && _e.keyCode == KeyCode.LeftArrow) {
			viewPercentage -= 0.01f;
		}
		if (_e.type == EventType.KeyDown && _e.keyCode == KeyCode.LeftArrow) {
			viewPercentage += 0.01f;
		}
	}
}
