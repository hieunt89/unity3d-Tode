using UnityEngine;
using System.Collections;

public class TreeNodeWorkView : TreeViewBase {

	public override void UpdateView (Rect _editorRect, Rect _percentageRect, Event _e, TreeGUI _currentTree)
	{
		base.UpdateView (_editorRect, _percentageRect, _e, _currentTree);
		GUI.Box (viewRect, "tree");

	}

	public override void ProcessEvent (Event e)
	{
		base.ProcessEvent (e);
	}
}
