using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

public class TreeViewBase {
	protected static TreeNodeEditorWindow currentWindow;

	public string viewTitle;
	public Rect viewRect;

	protected GUISkin viewSkin;
	protected TreeGUI currentTree;
	protected IDataUtils dataAssetUtils;

	public TreeViewBase () {
		currentWindow = (TreeNodeEditorWindow) EditorWindow.GetWindow <TreeNodeEditorWindow> ();
		dataAssetUtils = new GameAssetUtils () as IDataUtils;
		GetEditorSkin ();
	}

	public virtual void UpdateView (Rect _editorWindowRect, Rect _percentageRect, Event _e, TreeGUI _currentTree) {
		if (viewSkin == null) {
			GetEditorSkin ();
			return;
		}

		this.currentTree = _currentTree;

		viewRect = new Rect (
			_editorWindowRect.x * _percentageRect.x,
			_editorWindowRect.y * _percentageRect.y,
			_editorWindowRect.width * _percentageRect.width,
			_editorWindowRect.height * _percentageRect.height
		);
	}

	public virtual void ProcessEvent (Event e) {
	}

	protected void GetEditorSkin () {
		viewSkin = (GUISkin)Resources.Load ("EditorSkins/TreeNodeEditorSkin");
	}
}
