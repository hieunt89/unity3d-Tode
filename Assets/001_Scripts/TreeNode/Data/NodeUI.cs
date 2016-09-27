using UnityEngine;
using UnityEditor;
using System;

public enum NodeType {
	RootNode,
	Node
}

[Serializable]
public class NodeUI  {

	public Node<string> nodeData;
	public bool isSelected = false;
	public Rect nodeRect;
	public TreeUI tree;

	protected GUISkin nodeSkin;

	public NodeUI (Node<string> _nodeData) {
		this.nodeData = _nodeData;
	}



	public void InitNode () {
		Debug.Log ("init node");
		// check node type ? rootNode : node
		nodeRect = new Rect (10f, 10f, 100f, 40f);
	}

	public void UpdateNode (Event _e, Rect _viewRect) {
		ProcessEvent (_e, _viewRect);
	}

	public void UpdateNodeGUI (Event _e, Rect _viewRect, GUISkin _viewSkin) {
		ProcessEvent (_e, _viewRect);

		if (!isSelected) {
			GUI.Box (nodeRect, nodeData.Data, _viewSkin.GetStyle ("NodeDefault"));
		} else {
			GUI.Box (nodeRect, nodeData.Data, _viewSkin.GetStyle ("NodeSelected"));
		}

		// draw bezier link from this node to mouse
		// TODO: draw button to link to other node 
		// check if the tree is not null
		// do the job
		// tree.wantsconnection = true;
		// tree.connectionNode = this node;
		if (GUI.Button (new Rect(nodeRect.x + nodeRect.width, nodeRect.y + (nodeRect.height * 0.5f) - 12f, 24f, 24f), "", _viewSkin.GetStyle ("NodeOutput"))) {
			if (tree != null) {
				tree.wantsConnection = true;
				tree.connectionNode = this;
			}
		}
	}

	public void DrawNodeProperties () {
		// TODO: display node properties 
	}

	private void ProcessEvent (Event _e, Rect _viewRect) {
		if (isSelected) {
			if (_viewRect.Contains (_e.mousePosition)) {
				if (_e.type == EventType.MouseDrag) {
					nodeRect.x += _e.delta.x;
					nodeRect.y += _e.delta.y;
				}
			}
		}
	}
}
