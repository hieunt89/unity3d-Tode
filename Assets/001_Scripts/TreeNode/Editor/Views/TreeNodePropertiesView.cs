using UnityEngine;
using UnityEditor;
using System;

public class PropertiesView : ViewBase {

	public bool showProperties = false;
	public PropertiesView () : base () {
	}

	public override void UpdateView<T> (Rect _editorRect, Rect _percentageRect, Event _e, GenericTree<T> _currentTree)
	{
		base.UpdateView (_editorRect, _percentageRect, _e, _currentTree);
	}
	public override void UpdateView (Rect _editorRect, Rect _percentageRect, Event _e, TreeUI _currentTree)
	{
		
		base.UpdateView (_editorRect, _percentageRect, _e, _currentTree);

//		GUI.Box (viewRect, viewTitle + " Properties", viewSkin.GetStyle ("ViewBg"));

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

	public override void ProcessEvent (Event _event)
	{
		base.ProcessEvent (_event);
		if (viewRect.Contains (_event.mousePosition)) {
			if (_event.button == 0) {
				if (_event.type == EventType.MouseDown) {

				}
			}
			if (_event.button == 1) {
				if (_event.type == EventType.MouseDown) {

				}
			}
		}
	}


}
