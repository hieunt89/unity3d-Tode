using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class TreeNodePropertiesView : ViewBase {

	public bool showProperties = false;
	public TreeNodePropertiesView () : base ("Properties View") {
	}

	public override void UpdateView (Rect _editorRect, Rect _percentageRect, Event _e, NodeGraph _currentGraph)
	{
		base.UpdateView (_editorRect, _percentageRect, _e, _currentGraph);

		GUI.Box (viewRect, viewTitle, viewSkin.GetStyle ("ViewBg"));

//		GUILayout.BeginArea (viewRect);
//		GUILayout.Space (30);
//		GUILayout.BeginHorizontal ();
//		GUILayout.Space (30);
//		if (!
//		GUILayout.EndHorizontal ();

	}


}
