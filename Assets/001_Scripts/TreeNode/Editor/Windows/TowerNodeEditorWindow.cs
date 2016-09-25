using UnityEngine;
using UnityEditor;

public class TowerNodeEditorWindow : EditorWindow {

	public static TowerNodeEditorWindow currentWindow;
	public TreeNodeWorkView workView;
	public TreeNodePropertiesView propertiesView;

	public NodeGraph currentGraph = null;

	private float viewPercentage = 0.75f;

	public static void InitTowerNodeEditorWindow () {
		currentWindow = (TowerNodeEditorWindow)EditorWindow.GetWindow <TowerNodeEditorWindow> ();
		currentWindow.titleContent = new GUIContent ("Tower Tree");
		CreateViews ();
	}


	void OnGUI () {
		if (workView == null || propertiesView == null) {
			CreateViews ();
			return;
		}

		Event e = Event.current;
		ProcessEvent (e);

		workView.UpdateView (position, new Rect (0f, 0f, viewPercentage, 1f), e, currentGraph);
		propertiesView.UpdateView (new Rect (position.width, position.y, position.width, position.height), 
			new Rect (viewPercentage, 0f, 1f - viewPercentage, 1f), e, currentGraph);

		Repaint ();
	}

	private static void CreateViews () {
		if (currentWindow != null) {
			currentWindow.workView = new TreeNodeWorkView ();
			currentWindow.propertiesView = new TreeNodePropertiesView ();
		} else {
			currentWindow = (TowerNodeEditorWindow)EditorWindow.GetWindow <TowerNodeEditorWindow> ();
		}
	}

	private void ProcessEvent (Event e) {

	}
}
