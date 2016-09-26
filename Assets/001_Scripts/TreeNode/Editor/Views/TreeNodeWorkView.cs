using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class TreeNodeWorkView : ViewBase {
	private Vector2 mousePosition;

	public TreeNodeWorkView () : base ("Tree View") {
	}

	public override void UpdateView (Rect _editorRect, Rect _percentageRect, Event _e, Tree _currentTree)
	{
		base.UpdateView (_editorRect, _percentageRect, _e, _currentTree);

		if (_currentTree != null) {
			viewTitle = _currentTree.treeName;
		} else {
			viewTitle = "No Tree";
		}

		GUI.Box (viewRect, viewTitle, viewSkin.GetStyle("ViewBg"));

		// draw grid
//		TreeNodeUtils.DrawGrid (viewRect, 60f, 0.15f, Color.white);
//		TreeNodeUtils.DrawGrid (viewRect, 30f, 0.05f, Color.white);


		GUILayout.BeginArea (viewRect);

		GUILayout.EndArea ();

		ProcessEvent (_e);
	}

	public override void ProcessEvent (Event e)
	{
		base.ProcessEvent (e);

		if (viewRect.Contains (e.mousePosition)) {
			if (e.button == 0) {
				if (e.type == EventType.MouseDown) {
					
				}
				if (e.type == EventType.MouseDrag) {
					
				}
				if (e.type == EventType.MouseUp) {
					
				}
			}
			if (e.button == 1) {
				if (e.type == EventType.MouseDown) {
					CreateContextMenu (e);
				}
			}
		}
	}

	private void CreateContextMenu (Event e) {
		GenericMenu menu = new GenericMenu ();
		menu.AddItem (new GUIContent ("Create Tree"), false, OnClickContextCallback, "0");
		menu.AddItem (new GUIContent ("Load Tree"), false, OnClickContextCallback, "1");

		if (currentTree != null) {
			menu.AddSeparator ("");
			menu.AddItem (new GUIContent("Unload Tree"), false, OnClickContextCallback, "2");

			menu.AddSeparator ("");
			menu.AddItem (new GUIContent ("Add Node"), false, OnClickContextCallback, "3");
			menu.AddItem (new GUIContent ("Remove Node"), false, OnClickContextCallback, "4");
		}

		menu.ShowAsContext ();
		e.Use ();

	}

	private void OnClickContextCallback (object obj) {
		switch(obj.ToString()) {
		case "0":
			TreeNodePopupWindow.InitTreeNodePopup ();
			break;
		case "1":
			Debug.Log ("Load Tree");
			break;
		case "2":
			Debug.Log ("Unload Tree");
			break;
		case "3":
			Debug.Log ("Add Node");
			break;
		case "4":
			Debug.Log ("Remove Node");
			break;
		}
	}
	private void ProcessContextMenu (Event e, int contextId) {

	}
}
