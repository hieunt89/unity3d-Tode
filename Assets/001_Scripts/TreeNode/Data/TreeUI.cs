using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;

public enum TreeType {
	Tower
}

[Serializable]
public class TreeUI {
	public TreeType treeType;
	public string treeName;
	public Tree<string> treeData;

	public List<NodeUI> nodes;
	public NodeUI selectedNode;
	public bool wantsConnection = false;
	public NodeUI startConnectionNode;	// ???
	public bool showProperties = false;

	public List<string> existIds;

	public TreeUI (TreeType _treeType, string _treeName, Tree<string> _treeData) {
		this.treeType = _treeType;
		this.treeName = _treeName;
		this.treeData = _treeData;

		if (nodes == null) {
			nodes = new List<NodeUI> ();
		}

		// load exist data based on tree type
		switch (_treeType) {
		case TreeType.Tower:
			var data = DataManager.Instance.LoadAllData <TowerData> ();
			if (data != null){
				existIds = new List<string> ();
				for (int i = 0; i < data.Count; i++) {
					existIds.Add(data[i].Id);
				}
			}
			break;
		default:
			break;
		}
	}

//	void OnEnable () {
//		Debug.Log ("tree ui on enable");
//
//		treeData = new Tree <string> ();
//		if (nodes == null) {
//			nodes = new List<NodeBase> ();
//		}
//	}

//	public void InitTree () {
//		treeData = new Tree <string> ();
//		if (nodes.Count > 0) {
//			for (int i = 0; i < nodes.Count; i++) {
//				nodes [i].InitNode ();
//			}
//		}
//	}

//	public void UpdateTree() {
//		if (nodes.Count > 0) {
//
//		}
//	}

	public void UpdateTreeUI (Event e, Rect viewRect, GUISkin viewSkin) {

		ProcessEvents (e, viewRect);

		if (nodes.Count > 0) {
			for (int i = 0; i < nodes.Count; i++) {
				nodes [i].UpdateNodeUI (e, viewRect, viewSkin);
			}
		}

		if (wantsConnection) {
			if (startConnectionNode != null) {
				DrawConnectionToMouse (e.mousePosition);
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
					DeselectAllNodes ();

					showProperties = false;
					bool setNode = false;
					selectedNode = null;

					for (int i = 0; i < nodes.Count; i++) {
						if (nodes[i].nodeRect.Contains (_e.mousePosition)) {
							nodes[i].isSelected = true;
							selectedNode = nodes[i];
							setNode = true;
						}
					}

					if (!setNode) {
						wantsConnection = false;
						startConnectionNode = null;
						DeselectAllNodes ();
					}

					if (wantsConnection) {
						wantsConnection = false;
					}
				}
			}
		}
	}

	private void DrawConnectionToMouse (Vector2 _mousePosition) {
		bool isRight = _mousePosition.x >= startConnectionNode.nodeRect.x + (startConnectionNode.nodeRect.width * 0.5f);

		var startPos = new Vector3(isRight ? startConnectionNode.nodeRect.x + startConnectionNode.nodeRect.width :  startConnectionNode.nodeRect.x, 
									startConnectionNode.nodeRect.y + startConnectionNode.nodeRect.height * 0.75f, 
									0);
		var endPos = new Vector3(_mousePosition.x, _mousePosition.y, 0);

		float mnog = Vector3.Distance(startPos,endPos);
		Vector3 startTangent = startPos + (isRight ? Vector3.right : Vector3.left) * (mnog / 3f) ;
		Vector3 endTangent = endPos + (isRight ? Vector3.left : Vector3.right) * (mnog / 3f);

		Handles.BeginGUI ();
		Handles.DrawBezier(startPos, endPos, startTangent, endTangent,Color.white, null, 2f);
		Handles.EndGUI ();
	}

	void DeselectAllNodes () {
		for (int i = 0; i < nodes.Count; i++) {
			nodes [i].isSelected = false;
		}
	}
}
