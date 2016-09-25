using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

[Serializable]
public class ViewBase {

	public string viewTitle;
	public Rect viewRect;

	protected GUISkin viewSkin;
	protected NodeGraph currentGraph;

	public ViewBase (string _viewTitle) {
		this.viewTitle = _viewTitle;
		GetEditorSkin ();
	}

	public virtual void UpdateView (Rect _editorRect, Rect _percentageRect, Event _e, NodeGraph _currentGraph) {
		if (viewSkin == null) {
			GetEditorSkin ();
			return;
		}

		this.currentGraph = _currentGraph;
		if (currentGraph != null) {
			viewTitle = currentGraph.graphName;
		} else {
			viewTitle = "No graph";
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
		viewSkin = (GUISkin)Resources.Load ("EditorSkins/TreeNodeEditorSkin");
	}
}
