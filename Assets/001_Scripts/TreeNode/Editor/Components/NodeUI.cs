using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class NodeUI {
	public string nodeTitle = "Node";
	public NodeType nodeType;
	public Node<string> nodeData;
	public int contentSelectedIndex;
	public bool isSelected = false;
	public Rect nodeRect;
	public Rect nodeContentRect;
	public TreeUI currentTree;

	public NodeUI parentNode;
	public List<NodeUI> childNodes;

	protected GUISkin nodeSkin;

	public NodeUI (string _nodeTitle, NodeType _nodeType, Node<string> _nodeData, NodeUI _parentNode, List<NodeUI> _childNodes) {
		this.nodeTitle = _nodeTitle;
		this.nodeType = _nodeType;
		this.nodeData = _nodeData;
		this.parentNode = _parentNode;
		this.childNodes = _childNodes;
	}

	public void InitNode (Vector2 position) {
		nodeRect = new Rect (position.x, position.y, 100f, 40f);
	}

	public void UpdateNode (Event _e, Rect _viewRect) {
//		ProcessEvent (_e, _viewRect);
	}

	public void UpdateNodeUI (int nodeIndex, Event _e, Rect _viewRect, GUISkin _viewSkin) {
//		ProcessEvent (_e, _viewRect);
		if (nodeData == null) return;



//		if (!isSelected) {
//			GUI.Box (nodeRect, nodeTitle, _viewSkin.GetStyle ("NodeDefault"));
		nodeRect = GUI.Window (nodeIndex, nodeRect, NodeWindow, nodeTitle);
//		} else {
//			GUI.Box (nodeRect, nodeTitle, _viewSkin.GetStyle ("NodeSelected"));
//		}
//			
//		nodeContentRect = new Rect(nodeRect.x, nodeRect.y + nodeRect.height, nodeRect.width, nodeRect.height);
//		GUI.Box (nodeContentRect, nodeData.data, _viewSkin.GetStyle ("ContentDefault"));

//		GUILayout.BeginArea (nodeContentRect);
//		GUILayout.EndArea ();

		DrawNodeConnection ();

//		EditorUtility.SetDirty (this);
	}

	void NodeWindow (int windowId) {
		Event e = Event.current;
		if (e.button == 1) {
			if (e.type == EventType.MouseDown) {
				ProcessNodeContextMenu (e);
			}

		}
		if (e.button == 0) {
			if (e.type == EventType.MouseDown) {
				// check mouse over any node
				if (currentTree.startConnectionNode != null) {
//					if (this.nodeRect.Contains (e.mousePosition)) {
						if (currentTree.nodes[windowId] != currentTree.startConnectionNode && currentTree.nodes[windowId].nodeType != NodeType.RootNode && currentTree.nodes[windowId].parentNode == null) {
							// add mouse over node to startconnection children node
							currentTree.startConnectionNode.nodeData.AddChild (currentTree.nodes [windowId].nodeData);

							// assign mouse over node ui to start connection node ui
							currentTree.startConnectionNode.childNodes.Add(currentTree.nodes[windowId]);
							currentTree.nodes[windowId].parentNode = currentTree.startConnectionNode;
						}
//					}
				}

				currentTree.wantsConnection = false;
				currentTree.startConnectionNode = null;
			}
		}

		EditorGUI.BeginChangeCheck ();
		contentSelectedIndex = EditorGUILayout.Popup (contentSelectedIndex, currentTree.existIds.ToArray());
		if (EditorGUI.EndChangeCheck ()) {
			nodeData.data = currentTree.existIds [contentSelectedIndex];
		}
		GUI.DragWindow ();
	}

	public void DrawNodeProperties (TreeUI _currentTree) {
		EditorGUILayout.BeginVertical ();
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Space (30);

		EditorGUI.BeginChangeCheck ();
		contentSelectedIndex = EditorGUILayout.Popup (contentSelectedIndex, _currentTree.existIds.ToArray());
		if (EditorGUI.EndChangeCheck ()) {
			nodeData.data = _currentTree.existIds [contentSelectedIndex];
		}

		GUILayout.Space (30);
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.EndVertical ();
	}

//	private void ProcessEvent (Event _e, Rect _viewRect) {
//		if (isSelected) {
//			if (_viewRect.Contains (_e.mousePosition)) {
//				if (_e.type == EventType.MouseDrag) {
//					nodeRect.x += _e.delta.x;
//					nodeRect.y += _e.delta.y;
//				}
//			}
//		}
//		if (nodeContentRect.Contains (_e.mousePosition)) {
//			if (_e.type == EventType.MouseDrag && currentTree.selectedNode == null) {
//				if (currentTree.wantsConnection == false && currentTree.startConnectionNode == null) {
//					currentTree.wantsConnection = true;
//					currentTree.startConnectionNode = this;
//				}
//			}
//		} else {
//			if (_e.type == EventType.MouseUp) {
//				// check mouse over any node
//				if (currentTree.startConnectionNode != null) {
//					for (int i = 0; i < currentTree.nodes.Count; i++) {
//						if (currentTree.nodes[i].nodeContentRect.Contains (_e.mousePosition)){
//							if (currentTree.nodes[i] != currentTree.startConnectionNode && currentTree.nodes[i].nodeType != NodeType.RootNode && currentTree.nodes[i].parentNode == null) {
//								// add mouse over node to startconnection children node
//								currentTree.startConnectionNode.nodeData.AddChild (currentTree.nodes [i].nodeData);
//
//								// assign mouse over node ui to start connection node ui
//								currentTree.startConnectionNode.childNodes.Add(currentTree.nodes[i]);
//								currentTree.nodes[i].parentNode = currentTree.startConnectionNode;
//							}
//						}
//					}
//				}
//
//				currentTree.wantsConnection = false;
//				currentTree.startConnectionNode = null;
//			}
//		}
//	}

	private void ProcessNodeContextMenu (Event _e) {
		GenericMenu menu = new GenericMenu ();
		menu.AddItem (new GUIContent ("Add Child"), false, OnClickContextCallback, "0");
		menu.AddItem (new GUIContent ("Remove Parent"), false, OnClickContextCallback, "1");
		menu.AddItem (new GUIContent ("Remove Node"), false, OnClickContextCallback, "2");

		menu.ShowAsContext ();
		_e.Use ();
	}
	private void OnClickContextCallback (object obj) {
		switch (obj.ToString ()) {
		case "0":
			Debug.Log ("Add child");
			if (currentTree.wantsConnection == false && currentTree.startConnectionNode == null) {
				currentTree.wantsConnection = true;
				currentTree.startConnectionNode = this;
			}
			break;
		case "1":
			Debug.Log ("Remove Parent");
			this.parentNode.nodeData.children.Remove(this.nodeData);
			// remove this node's parent data
			this.nodeData.parent = null;

			this.parentNode = null;
			break;
		case "2":
			Debug.Log ("Remove Node");
			for (int i = 0; i < this.nodeData.children.Count; i++) {
				this.nodeData.children[i].parent = null;
			}
			if (this.nodeData.parent != null)
				this.nodeData.parent.children.Remove (this.nodeData);
			this.nodeData.parent = null;
			this.nodeData = null;

			for (int i = 0; i < this.childNodes.Count; i++) {
				this.childNodes[i].parentNode = null;
			}
			currentTree.nodes.Remove (this);
			break;
		}
	}

	//TODO: draw better bezier
	private void DrawNodeConnection () {
		if (parentNode != null) {
			bool isRight = nodeRect.x >= parentNode.nodeRect.x + (parentNode.nodeRect.width * 0.5f);

			var startPos = new Vector3 (parentNode.nodeRect.x + parentNode.nodeRect.width, 
				parentNode.nodeRect.y + parentNode.nodeRect.height * 0.5f, 0f);
			var endPos = new Vector3 (nodeRect.x, nodeRect.y + nodeRect.height * 0.5f, 0f);

			float mnog = Vector3.Distance(startPos,endPos);
			Vector3 startTangent = startPos + (isRight ? Vector3.right : Vector3.left) * (mnog / 3f) ;
			Vector3 endTangent = endPos + (isRight ? Vector3.left : Vector3.right) * (mnog / 3f);

			Handles.BeginGUI ();
			Handles.DrawBezier(startPos, endPos, startTangent, endTangent,Color.black, null, 2f);
			DrawArrowhead (parentNode.nodeRect, nodeRect, new Vector2 (.25f, 1f), new Vector2 (0f, .5f), 10f, 10f, 5f);
			Handles.EndGUI ();

		}
	}

	void DrawArrowhead(Rect start, Rect end, Vector2 vStartPercentage, Vector2 vEndPercentage, float fHandleDistance, float fLength, float fWidth)
	{
		float fHandleDistanceDouble = fHandleDistance * 2;

		Vector3 startPos = new Vector3(start.x + start.width * vStartPercentage.x, start.y + start.height * vStartPercentage.y, 0);
		Vector3 startTan = startPos + Vector3.right * (-fHandleDistance + fHandleDistanceDouble * vStartPercentage.x) + Vector3.up * (-fHandleDistance + fHandleDistanceDouble * vStartPercentage.y);

		Vector3 endPos = new Vector3(end.x + end.width * vEndPercentage.x, end.y + end.height * vEndPercentage.y, 0);
		Vector3 endTan = endPos + Vector3.right * (-fHandleDistance + fHandleDistanceDouble * vEndPercentage.x) + Vector3.up * (-fHandleDistance + fHandleDistanceDouble * vEndPercentage.y);

		float dy = endTan.y - endPos.y;
		float dx = endTan.x - endPos.x;

		Vector3 vDelta = endTan - endPos;
		Vector3 vNormal = new Vector3 ( -dy, dx, 0f ).normalized;

		Vector3 vArrowHeadEnd1 = endPos + vDelta.normalized * fLength + vNormal.normalized * fWidth;
		Vector3 vArrowHeadEnd2 = endPos + vDelta.normalized * fLength + vNormal.normalized * -fWidth;

		Vector3 vHalfwayPoint = endPos  + vDelta.normalized * fLength * 0.5f;

		Color shadowCol = new Color(0, 0, 0, 0.06f);

//		for (int i = 0; i < 3; i++) // Draw a shadow
//			Handles.DrawBezier(endPos, vArrowHeadEnd1, endPos, vHalfwayPoint, shadowCol, null, (i + 1) * 5);
		Handles.DrawBezier(endPos, vArrowHeadEnd1, endPos, vHalfwayPoint, Color.black, null, 2);

//		for (int i = 0; i < 3; i++) // Draw a shadow
//			Handles.DrawBezier(endPos, vArrowHeadEnd2, endPos, vHalfwayPoint, shadowCol, null, (i + 1) * 5);
		Handles.DrawBezier(endPos, vArrowHeadEnd2, endPos, vHalfwayPoint, Color.black, null, 2);
	}
}
