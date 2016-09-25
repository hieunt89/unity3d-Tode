using UnityEngine;
using System;

[Serializable]
public class TreeNodeWorkView : ViewBase {
	private Vector2 mousePosition;

	public TreeNodeWorkView () : base ("Tree View") {
	}

	public override void UpdateView (Rect _editorRect, Rect _percentageRect, Event _e, NodeGraph _currentGraph)
	{
		base.UpdateView (_editorRect, _percentageRect, _e, _currentGraph);

		GUI.Box (viewRect, viewTitle, viewSkin.GetStyle("ViewBg"));

		// draw grid
//		TreeNodeUtils.DrawGrid (viewRect, 60f, 0.15f, Color.white);
//		TreeNodeUtils.DrawGrid (viewRect, 30f, 0.05f, Color.white);

//		GUILayout.BeginArea (viewRect);
//
//
//		GUILayout.EndArea ();

		ProcessEvent (_e);
	}

	public override void ProcessEvent (Event e)
	{
		base.ProcessEvent (e);
	}

	private void ProcessContextMenu (Event e, int contextId) {

	}
}
