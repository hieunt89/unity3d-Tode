using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class TreeNodePropertiesView : ViewBase {

	public bool showProperties = false;
	public TreeNodePropertiesView () : base () {
	}

	public override void UpdateView (Rect _editorRect, Rect _percentageRect, Event _e, TreeUI _currentTree)
	{
		
		base.UpdateView (_editorRect, _percentageRect, _e, _currentTree);

		GUI.Box (viewRect, viewTitle + " Properties", viewSkin.GetStyle ("ViewBg"));

		GUILayout.BeginArea (viewRect);
		GUILayout.Space (30);
		GUILayout.BeginHorizontal ();
		GUILayout.Space (30);

		if (_currentTree != null) {
			if (_currentTree.showNodeProperties) {
				_currentTree.selectedNode.DrawNodeProperties (_currentTree);
			} else {
//				_currentTree.DrawTreeProperties ();
				EditorGUILayout.LabelField ("NONE");
			}
		}
		GUILayout.Space (30);
		GUILayout.EndHorizontal ();
		GUILayout.EndArea ();

		ProcessEvent (_e);
	}

	public override void ProcessEvent (Event _e)
	{
		base.ProcessEvent (_e);
		if (viewRect.Contains (_e.mousePosition)) {
			if (_e.button == 0) {
				if (_e.type == EventType.MouseDown) {

				}
			}
			if (_e.button == 1) {
				if (_e.type == EventType.MouseDown) {

				}
			}
		}
	}


}
