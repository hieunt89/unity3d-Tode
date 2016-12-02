using UnityEngine;
using UnityEditor;
using System;

public class TreeNodeMenuView : TreeViewBase {
	private TreeType treeType;

	GUIStyle menuStyle;
	public TreeNodeMenuView ()
	{
		menuStyle = new GUIStyle ();
		menuStyle.padding = new RectOffset (16, 16, 16, 16);
		menuStyle.alignment = TextAnchor.UpperCenter;
	}
	

	public override void UpdateView (Rect _editorRect, Rect _percentageRect, Event _e, TreeGUI _currentTree)
	{
		base.UpdateView (_editorRect, _percentageRect, _e, _currentTree);
//		
		var newRect = new Rect (viewRect.width / 2 - 200f, viewRect.height / 2 - 100f, 400, 200);
		GUILayout.BeginArea (newRect);
		GUILayout.Label ("Menu");
		GUILayout.Space (10);

		if (GUILayout.Button (TreeType.Towers.ToString ())) {
			currentWindow.workType = TreeType.Towers;
			currentWindow.viewIndex = ViewIndex.List;
		}

		if (GUILayout.Button (TreeType.CombatSkills.ToString ())) {
			currentWindow.viewIndex = ViewIndex.List;
			currentWindow.workType = TreeType.CombatSkills;
		}

		if (GUILayout.Button (TreeType.SummonSkills.ToString ())) {
			currentWindow.viewIndex = ViewIndex.List;
			currentWindow.workType = TreeType.SummonSkills;
		}
		GUILayout.EndArea ();
	}

	public override void ProcessEvent (Event e)
	{
		base.ProcessEvent (e);
	}
}
