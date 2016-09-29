﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public enum NodeType {
	RootNode,
	Node
}

[Serializable]
public class NodeUI : ScriptableObject {
	public string nodeTitle = "Node";
	public NodeType nodeType;
	public Node<string> nodeData;
	public int selectedIndex;
	public bool isSelected = false;
	public Rect nodeRect;
	public Rect nodeContentRect;
	public TreeUI currentTree;

	public NodeUI parentNode;
	public List<NodeUI> childNodes;

	protected GUISkin nodeSkin;

	private float nodeWidth = 100f;
	private float nodeHeight = 20f;

	public NodeUI (string _nodeTitle, NodeType _nodeType, Node<string> _nodeData, NodeUI _parentNode, List<NodeUI> _childNodes) {
		this.nodeTitle = _nodeTitle;
		this.nodeType = _nodeType;
		this.nodeData = _nodeData;
		this.parentNode = _parentNode;
		this.childNodes = _childNodes;
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
			
		nodeContentRect = new Rect(nodeRect.x, nodeRect.y + nodeRect.height, nodeRect.width, nodeRect.height);
		GUI.Box (nodeContentRect, nodeData.Data, _viewSkin.GetStyle ("ContentDefault"));

		GUILayout.BeginArea (nodeContentRect);
		EditorGUI.BeginChangeCheck ();
		selectedIndex = EditorGUILayout.Popup (selectedIndex, currentTree.existIds.ToArray(), GUILayout.Width(nodeContentRect.width * 0.8f));
		if (EditorGUI.EndChangeCheck ()) {
			nodeData.Data = currentTree.existIds [selectedIndex];
		}
		GUILayout.EndArea ();

		DrawInputConnection ();

		EditorUtility.SetDirty (this);
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

		if (nodeContentRect.Contains (_e.mousePosition)) {
			if (_e.type == EventType.MouseDrag) {
				if (currentTree.wantsConnection == false && currentTree.startConnectionNode == null) {
					currentTree.wantsConnection = true;
					currentTree.startConnectionNode = this;
				}
			}
		} else {
			if (_e.type == EventType.MouseUp) {
				// check mouse over any node
				if (currentTree.startConnectionNode != null) {
					for (int i = 0; i < currentTree.nodes.Count; i++) {
						if (currentTree.nodes[i].nodeContentRect.Contains (_e.mousePosition)){
							if (currentTree.nodes[i] != currentTree.startConnectionNode && currentTree.nodes[i].nodeType != NodeType.RootNode && currentTree.nodes[i].parentNode == null) {
								currentTree.nodes[i].parentNode = currentTree.startConnectionNode;
							}
						}
					}
				}

				currentTree.wantsConnection = false;
				currentTree.startConnectionNode = null;
			}
		}
	}

	//TODO: need to fix this bezier
	// start and end point base on position of node
	private void DrawInputConnection () {
		if (parentNode != null) {
			bool isRight = nodeRect.x >= parentNode.nodeRect.x + (parentNode.nodeRect.width * 0.5f);

			var startPos = new Vector3 (parentNode.nodeRect.x + parentNode.nodeRect.width, 
				parentNode.nodeRect.y + parentNode.nodeRect.height * 0.75f, 0f);
			var endPos = new Vector3(nodeRect.x, nodeRect.y + nodeRect.height * 0.75f, 0f);

			float mnog = Vector3.Distance(startPos,endPos);
			Vector3 startTangent = startPos + (isRight ? Vector3.right : Vector3.left) * (mnog / 3f) ;
			Vector3 endTangent = endPos + (isRight ? Vector3.left : Vector3.right) * (mnog / 3f);

			Handles.BeginGUI ();
			Handles.DrawBezier(startPos, endPos, startTangent, endTangent,Color.white, null, 2f);
			Handles.EndGUI ();

		}
	}
}