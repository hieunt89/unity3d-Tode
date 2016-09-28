﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public enum NodeType {
	RootNode,
	Node
}

[Serializable]
public class NodeUI  {
	public string nodeTitle = "Node";
	public NodeType nodeType;
	public Node<string> nodeData;
	public int selectedIndex;
	public bool isSelected = false;
	public Rect nodeRect;
	public TreeUI currentTree;

	public NodeUI inputNode;
	public List<NodeUI> outputNodes;

	protected GUISkin nodeSkin;

	private float nodeWidth = 100f;
	private float nodeHeight = 40f;

	public NodeUI (string _nodeTitle, NodeType _nodeType, Node<string> _nodeData, NodeUI _inputNode, List<NodeUI> _outputNodes) {
		this.nodeTitle = _nodeTitle;
		this.nodeType = _nodeType;
		this.nodeData = _nodeData;
		this.inputNode = _inputNode;
		this.outputNodes = _outputNodes;
	}

	public void InitNode (Vector2 position) {
		nodeRect = new Rect (position.x, position.y, nodeWidth, nodeHeight);
	}

	public void UpdateNode (Event _e, Rect _viewRect) {
		ProcessEvent (_e, _viewRect);
	}

	public void UpdateNodeUI (Event _e, Rect _viewRect, GUISkin _viewSkin) {
		ProcessEvent (_e, _viewRect);

		if (nodeData == null) return;
		
		if (!isSelected) {
			GUI.Box (nodeRect, nodeTitle, _viewSkin.GetStyle ("NodeDefault"));
		} else {
			GUI.Box (nodeRect, nodeTitle, _viewSkin.GetStyle ("NodeSelected"));
		}

		if (GUI.Button (new Rect (nodeRect.x, nodeRect.y + nodeRect.height * 2f, nodeRect.width, nodeRect.height * .5f), nodeData.Data, _viewSkin.GetStyle ("ContentDefault"))) {
			if (currentTree != null) {
				if (currentTree.startConnectionNode == null) {
					currentTree.wantsConnection = true;
					currentTree.startConnectionNode = this;
				} else {
					// TODO: kiem tra truoc khi add connection
					// khong la chinh no
					// input node chua co parent ????
					if (currentTree.startConnectionNode != this) {
						inputNode = currentTree.startConnectionNode;
						currentTree.wantsConnection = false;
						currentTree.startConnectionNode = null;
					} else {
						currentTree.wantsConnection = false;
						currentTree.startConnectionNode = null;
					}
				}
			}
		}
			
		DrawInputConnection ();
	}

	public void DrawNodeProperties () {
		EditorGUILayout.BeginVertical ();
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Space (30);
		EditorGUI.BeginChangeCheck ();
		selectedIndex = EditorGUILayout.Popup (selectedIndex, currentTree.existIds.ToArray());
		if (EditorGUI.EndChangeCheck ()) {
			nodeData.Data = currentTree.existIds [selectedIndex];
		}

		GUILayout.Space (30);
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.EndVertical ();


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

	//TODO: need to fix this bezier
	// start and end point base on position of node
	private void DrawInputConnection () {
		if (inputNode != null) {
			bool isRight = nodeRect.x >= inputNode.nodeRect.x + (inputNode.nodeRect.width * 0.5f);

			var startPos = new Vector3 (inputNode.nodeRect.x + inputNode.nodeRect.width, 
				inputNode.nodeRect.y + inputNode.nodeRect.height * 0.75f, 0f);
			var endPos = new Vector3(nodeRect.x, nodeRect.y + nodeRect.height * 0.75f, 0f);

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
