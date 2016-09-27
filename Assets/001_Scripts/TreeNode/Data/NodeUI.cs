using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public enum NodeType {
	RootNode,
	Node
}

[Serializable]
public class NodeUI  {
	public Node<string> nodeData;
	public bool isSelected = false;
	public Rect nodeRect;
	public TreeUI currentTree;

	public NodeUI inputNode;
	public List<NodeUI> outputNodes;

	protected GUISkin nodeSkin;

	public NodeUI (Node<string> _nodeData) {
		this.nodeData = _nodeData;
	}

	public NodeUI (Node<string> _nodeData, NodeUI _inputNode, List<NodeUI> _outputNodes) {
		this.nodeData = _nodeData;
		this.inputNode = _inputNode;
		this.outputNodes = _outputNodes;
	}

	public void InitNode () {
		nodeRect = new Rect (10f, 10f, 100f, 40f);
	}

	public void UpdateNode (Event _e, Rect _viewRect) {
		ProcessEvent (_e, _viewRect);
	}

	public void UpdateNodeUI (Event _e, Rect _viewRect, GUISkin _viewSkin) {
		ProcessEvent (_e, _viewRect);

		if (!isSelected) {
			GUI.Box (nodeRect, "nodeData.Data", _viewSkin.GetStyle ("NodeDefault"));
		} else {
			GUI.Box (nodeRect, "nodeData.Data", _viewSkin.GetStyle ("NodeSelected"));
		}
			
//		if (GUI.Button (new Rect(nodeRect.x, nodeRect.y + (nodeRect.height * 0.5f), nodeRect.width, nodeRect.height * 0.5f), "C")) {
//			if (currentTree != null) {
//				currentTree.wantsConnection = true;
//				currentTree.startConnectionNode = this;
//			}
//		}


		DrawInputConnection ();
	}

	public void DrawNodeProperties () {
		// TODO: display node properties 
	}

	private void ProcessEvent (Event _e, Rect _viewRect) {
		if (isSelected) {
			if (_viewRect.Contains (_e.mousePosition)) {
//				if (_e.type == EventType.MouseDrag) {
//					nodeRect.x += _e.delta.x;
//					nodeRect.y += _e.delta.y;
//				}
				if (_e.type == EventType.MouseDown) {
					Debug.Log ("Node mouse down");
					if (currentTree != null) {
						if (currentTree.startConnectionNode == null) {
							currentTree.wantsConnection = true;
							currentTree.startConnectionNode = this;
						} else {
							inputNode = currentTree.startConnectionNode;
							currentTree.wantsConnection = false;
							currentTree.startConnectionNode = null;
						}
					}
				}
				if (_e.type == EventType.MouseUp) {
					Debug.Log ("Node mouse up + add connection here " + nodeData.Data); 
					// check this node if it has valid conditions
					if (currentTree !=null) {
						
					}
				}

			}
		}
	}

	private void DrawInputConnection () {
		if (inputNode != null) {
			bool isRight = nodeRect.x >= inputNode.nodeRect.x + (inputNode.nodeRect.width * 0.5f);

			var startPos = new Vector3 (inputNode.nodeRect.x + inputNode.nodeRect.width, 
				inputNode.nodeRect.y + inputNode.nodeRect.height * 0.75f, 0f);
			var endPos = new Vector3(nodeRect.x, nodeRect.y + (nodeRect.height * 0.75f), 0f);

			float mnog = Vector3.Distance(startPos,endPos);
			Vector3 startTangent = startPos + (isRight ? Vector3.right : Vector3.left) * (mnog / 3f) ;
			Vector3 endTangent = endPos + (isRight ? Vector3.left : Vector3.right) * (mnog / 3f);

			Handles.BeginGUI ();
			Handles.DrawBezier(startPos, endPos, startTangent, endTangent,Color.white, null, 2f);
			Handles.EndGUI ();

		} else {
			inputNode = null;
		}


	}
}
