using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;

public enum TreeType {
	Tower,
	Skill
}

[Serializable]
public class TreeUI {
	public Tree<string> treeData;
//	public List<NodeBase> nodes;
	public NodeUI selectedNode;
	public bool wantsConnection = false;
	public NodeUI connectionNode;	// ???
	public bool showProperties = false;


	public TreeUI (Tree<string> _treeData) {
		this.treeData = _treeData;
	}

	void OnEnable () {
		Debug.Log ("tree ui on enable");

		treeData = new Tree <string> ();
//		if (nodes == null) {
//			nodes = new List<NodeBase> ();
//		}
	}

	public void InitTree () {
		treeData = new Tree <string> ();
//		if (nodes.Count > 0) {
//			for (int i = 0; i < nodes.Count; i++) {
//				nodes [i].InitNode ();
//			}
//		}
	}

	public void UpdateTree() {
//		if (nodes.Count > 0) {
//
//		}
	}

	public void UpdateTreeGUI (Event e, Rect viewRect, GUISkin viewSkin) {
//		if (nodes.Count > 0) {
//			ProcessEvents (e, viewRect);
//			for (int i = 0; i < nodes.Count; i++) {
//				nodes [i].UpdateNodeGUI (e, viewRect, viewSkin);
//			}
//		}
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

//		EditorUtility.SetDirty (this);
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
//		for (int i = 0; i < nodes.Count; i++) {
//			nodes [i].isSelected = false;
//		}
	}
}
