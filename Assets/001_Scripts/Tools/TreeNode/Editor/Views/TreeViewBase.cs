using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

public class TreeViewBase {

	public string viewTitle;
	public Rect viewRect;

	protected GUISkin viewSkin;
	protected TreeGUI currentTree;

	protected IDataUtils binaryUtils;

	public TreeViewBase () {

		GetEditorSkin ();
	}

	public virtual void UpdateView (Rect _editorRect, Rect _percentageRect, Event _e, TreeGUI _currentTree) {
		if (viewSkin == null) {
			GetEditorSkin ();
			return;
		}

		this.currentTree = _currentTree;
		if (currentTree != null && currentTree.treeData != null) {
			viewTitle = TreeEditorUtils.UppercaseFirst(currentTree.treeData.id);
		} else {
			viewTitle = "No";
		}

		viewRect = new Rect (
			_editorRect.x * _percentageRect.x,
			_editorRect.y * _percentageRect.y,
			_editorRect.width * _percentageRect.width,
			_editorRect.height * _percentageRect.height
		);
	}

	public virtual void ProcessEvent (Event e) {
	}

	protected void GetEditorSkin () {
		binaryUtils = new BinaryUtils () as IDataUtils;
		viewSkin = (GUISkin)Resources.Load ("EditorSkins/TreeNodeEditorSkin");
	}
}
