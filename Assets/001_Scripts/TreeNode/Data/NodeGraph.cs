using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;

[Serializable]
public class NodeGraph : ScriptableObject {
	public string graphName = "New Graph";
	public List<NodeBase> nodes;
	public NodeBase selectedNode;
	public bool wantsConnection = false;
	public NodeBase connectionNode;	// ???
	public bool showProperties = false;

	void OnEnable () {
		if (nodes == null) {
			nodes = new List<NodeBase> ();
		}
	}

	public void InitGraph () {
		if (nodes.Count > 0) {
			for (int i = 0; i < nodes.Count; i++) {
				nodes [i].InitNode ();
			}
		}
	}

	public void UpdateGraph() {
		if (nodes.Count > 0) {

		}
	}

	public void UpdateGraphGUI (Event e, Rect viewRect, GUISkin viewSkin) {
		if (nodes.Count > 0) {
			ProcessEvents (e, viewRect);
			for (int i = 0; i < nodes.Count; i++) {
				nodes [i].UpdateNodeGUI (e, viewRect, viewSkin);
			}
		}
		if (wantsConnection) {
			if (connectionNode != null) {
				// draw conntection bezier
			}
		}

		if (e.type == EventType.Layout) {
			if (selectedNode != null) {
				showProperties = true;
			}
		}

		EditorUtility.SetDirty (this);
	}

	private void ProcessEvents (Event _e, Rect _viewRect) {
		if (_viewRect.Contains (_e.mousePosition)) {
			if (_e.button == 0) {
				if (_e.type == EventType.MouseDown) {

				}
			}
		}
	}

	private void DrawConnectionToMouse (Vector2 _mousePosition) {

	}

	void DeselectAllNodes () {
		for (int i = 0; i < nodes.Count; i++) {
			nodes [i].isSelected = false;
		}
	}
}
