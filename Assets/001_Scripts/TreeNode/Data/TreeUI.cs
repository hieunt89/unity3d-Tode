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
//	public Tree<string> treeData;

	public List<NodeUI> nodes;
	public NodeUI selectedNode;
	public bool wantsConnection = false;
	public NodeUI startConnectionNode;	// ???
	public bool showNodeProperties = false;

	public List<string> existIds;

	public TreeUI (TreeType _treeType, string _treeName) { //, Tree<string> _treeData) {
		this.treeType = _treeType;
		this.treeName = _treeName;
//		this.treeData = _treeData;

		if (nodes == null) {
			nodes = new List<NodeUI> ();
		}

		// load exist data based on tree type
		// TODO: chuyen exist data ra global ?
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

	public void UpdateTreeUI (Event _e, Rect _viewRect, GUISkin _viewSkin) {

		ProcessEvents (_e, _viewRect);

		if (nodes.Count > 0) {
			for (int i = 0; i < nodes.Count; i++) {
				nodes [i].UpdateNodeUI (_e, _viewRect, _viewSkin);
			}
		}

		if (wantsConnection) {
			if (startConnectionNode != null) {
				DrawConnectionToMouse (_e.mousePosition);
			}
		} else {

		}

		if (_e.type == EventType.Layout) {
			if (selectedNode != null) {
				showNodeProperties = true;
			}
		}

//		EditorUtility.SetDirty (this);
	}

	public void DrawTreeProperties () {
		EditorGUILayout.BeginVertical ();
		EditorGUILayout.LabelField ("Name", treeName);
		EditorGUILayout.LabelField ("Name", nodes.Count.ToString ());
		EditorGUILayout.EndVertical ();

		// TODO : draw all nodes data :D
	}

	private void ProcessEvents (Event _e, Rect _viewRect) {
		if (_viewRect.Contains (_e.mousePosition)) {
			if (_e.button == 0) {
				if (_e.type == EventType.MouseDown) {
					DeselectAllNodes ();

					showNodeProperties = false;
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
		bool isRight = _mousePosition.x >= startConnectionNode.nodeRect.x + startConnectionNode.nodeRect.width * 0.5f;

		var startPos = new Vector3(isRight ? startConnectionNode.nodeRect.x + startConnectionNode.nodeRect.width :  startConnectionNode.nodeRect.x, 
			startConnectionNode.nodeRect.y + startConnectionNode.nodeRect.height +  startConnectionNode.nodeContentRect .height * .5f, 
									0);
		var endPos = new Vector3(_mousePosition.x, _mousePosition.y, 0);

		float mnog = Vector3.Distance(startPos,endPos);
		Vector3 startTangent = startPos + (isRight ? Vector3.right : Vector3.left) * (mnog / 3f) ;
		Vector3 endTangent = endPos + (isRight ? Vector3.left : Vector3.right) * (mnog / 3f);

		Handles.BeginGUI ();
		Handles.DrawBezier(startPos, endPos, startTangent, endTangent,Color.white, null, 2f);
		Handles.EndGUI ();
	}

	private void DeselectAllNodes () {
		for (int i = 0; i < nodes.Count; i++) {
			nodes [i].isSelected = false;
		}
	}
}
