using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public class TreeViewBase {
	protected static TreeNodeEditorWindow currentWindow;

	public Rect viewRect;

	protected GUISkin viewSkin;
	protected TreeGUI currentTree;

	protected int selectedTreeIndex;


	public TreeViewBase () {
		if (currentWindow == null) 
			currentWindow = (TreeNodeEditorWindow) EditorWindow.GetWindow <TreeNodeEditorWindow> ();

		GetEditorSkin ();
	}

	public virtual void UpdateView (Rect _rect, Event _e, TreeGUI _currentTree) {
		if (viewSkin == null) {
			GetEditorSkin ();
			return;
		}

		this.currentTree = _currentTree;
		viewRect = _rect;
	}

	public virtual void ProcessEvents (Event e) {
	}

	protected void GetEditorSkin () {
		viewSkin = (GUISkin)Resources.Load ("EditorSkins/TreeNodeEditorSkin");
	}
}
