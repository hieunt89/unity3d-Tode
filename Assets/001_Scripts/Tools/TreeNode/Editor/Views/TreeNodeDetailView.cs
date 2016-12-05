using UnityEngine;
using System.Collections;

public class TreeNodeDetailView : TreeViewBase {

	public override void UpdateView (Rect _editorRect, Rect _percentageRect, Event _e, TreeGUI _currentTree)
	{
		base.UpdateView (_editorRect, _percentageRect, _e, _currentTree);
		if (currentTree != null) {
			GUI.Box (viewRect, "Tree", viewSkin.GetStyle("ViewBg"));
			GUILayout.BeginArea (viewRect);
			if (GUILayout.Button ("Back")) {
				currentWindow.viewIndex = ViewIndex.List;
			}
			GUILayout.EndArea ();
			currentTree.UpdateTreeUI (_e, viewRect, viewSkin);
			ProcessEvent (_e);
		}
	}

	public override void ProcessEvent (Event e)
	{
		base.ProcessEvent (e);
	}
}
