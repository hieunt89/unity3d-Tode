using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class NodeGUI {
	public Node<string> nodeData;
	public NodeGUI parentNode;
	public List<NodeGUI> childNodes;
	public Rect nodeRect;
	public TreeGUI currentTree;
	public bool isRoot;

	protected GUISkin nodeSkin;

	private int popUpSelectedIndex;
	private float nodeWidth = 100f;
	private float nodeHeight = 400f;

	public NodeGUI (bool _isRoot, Node<string> _nodeData, Rect _nodeRect, NodeGUI _parentNode, TreeGUI _currentTree) {
		this.isRoot = _isRoot;
		this.nodeData = _nodeData;
		this.nodeRect = _nodeRect;
		this.parentNode = _parentNode;
		this.childNodes = new List<NodeGUI>();
		this.currentTree = _currentTree;

		currentTree.nodes.Add (this);

		for (int i = 0; i < currentTree.existIds.Count; i++) {
			if (nodeData.data.Equals (currentTree.existIds [i])) {
				popUpSelectedIndex = i;
			}
		}
	}

	public void InitNodeGUI (Vector2 position) {
		nodeRect = new Rect (position.x, position.y, nodeWidth, nodeHeight);
	}

	public void UpdateNode (Event _e, Rect _viewRect) {
	}

	public void UpdateNodeUI (int nodeIndex, Event _e, Rect _viewRect) {
		if (nodeData == null) return;


		nodeRect = GUI.Window (nodeIndex, nodeRect, NodeWindow, isRoot ? "Root" : "Node");

		DrawNodeConnection ();

//		EditorUtility.SetDirty (this);
	}

	void NodeWindow (int _windowId) {
		Event e = Event.current;
		ProcessEvent (e, _windowId);

		popUpSelectedIndex = EditorGUILayout.Popup (popUpSelectedIndex, currentTree.existIds.ToArray());
		nodeData.data = currentTree.existIds [popUpSelectedIndex];
		GUI.DragWindow ();
	}

	public void DrawNodeProperties (TreeGUI _currentTree) {
		EditorGUILayout.BeginVertical ();
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Space (30);

		EditorGUI.BeginChangeCheck ();
		popUpSelectedIndex = EditorGUILayout.Popup (popUpSelectedIndex, _currentTree.existIds.ToArray());
		if (EditorGUI.EndChangeCheck ()) {
			nodeData.data = _currentTree.existIds [popUpSelectedIndex];
		}

		GUILayout.Space (30);
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.EndVertical ();
	}
	private void ProcessEvent (Event _e, int _windowId) {
		if (_e.button == 1) {
			if (_e.type == EventType.MouseDown) {
				ProcessNodeContextMenu (_e);
			}
		}
		if (_e.button == 0) {
			if (_e.type == EventType.MouseDown) {

				// check mouse over any node
				if (currentTree.startConnectionNode != null) {
					
					if (currentTree.nodes[_windowId] != currentTree.startConnectionNode && !currentTree.nodes[_windowId].isRoot && currentTree.nodes[_windowId].parentNode == null) {
						// add mouse over node to startconnection children node
						currentTree.startConnectionNode.nodeData.AddChild (currentTree.nodes [_windowId].nodeData);

						// assign mouse over node ui to start connection node ui
						currentTree.startConnectionNode.childNodes.Add(currentTree.nodes[_windowId]);
						currentTree.nodes[_windowId].parentNode = currentTree.startConnectionNode;
					}
				}

				currentTree.wantsConnection = false;
				currentTree.startConnectionNode = null;
			}
		}

	}

	private void ProcessNodeContextMenu (Event _e) {
		GenericMenu menu = new GenericMenu ();
		menu.AddItem (new GUIContent ("Add Child"), false, OnClickContextCallback, "0");
		if (parentNode != null)
			menu.AddItem (new GUIContent ("Remove Parent"), false, OnClickContextCallback, "1");
		menu.AddItem (new GUIContent ("Remove Node"), false, OnClickContextCallback, "2");

		menu.ShowAsContext ();
		_e.Use ();
	}
	private void OnClickContextCallback (object obj) {
		switch (obj.ToString ()) {
		case "0":
			if (currentTree.wantsConnection == false && currentTree.startConnectionNode == null) {
				currentTree.wantsConnection = true;
				currentTree.startConnectionNode = this;
			}
			break;
		case "1":
			TreeEditorUtils.RemoveParentNode (this);
			break;
		case "2":
			TreeEditorUtils.RemoveNode (this);
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

//		Vector3 startPos = new Vector3(start.x + start.width * vStartPercentage.x, start.y + start.height * vStartPercentage.y, 0);
//		Vector3 startTan = startPos + Vector3.right * (-fHandleDistance + fHandleDistanceDouble * vStartPercentage.x) + Vector3.up * (-fHandleDistance + fHandleDistanceDouble * vStartPercentage.y);

		Vector3 endPos = new Vector3(end.x + end.width * vEndPercentage.x, end.y + end.height * vEndPercentage.y, 0);
		Vector3 endTan = endPos + Vector3.right * (-fHandleDistance + fHandleDistanceDouble * vEndPercentage.x) + Vector3.up * (-fHandleDistance + fHandleDistanceDouble * vEndPercentage.y);

		float dy = endTan.y - endPos.y;
		float dx = endTan.x - endPos.x;

		Vector3 vDelta = endTan - endPos;
		Vector3 vNormal = new Vector3 ( -dy, dx, 0f ).normalized;

		Vector3 vArrowHeadEnd1 = endPos + vDelta.normalized * fLength + vNormal.normalized * fWidth;
		Vector3 vArrowHeadEnd2 = endPos + vDelta.normalized * fLength + vNormal.normalized * -fWidth;

		Vector3 vHalfwayPoint = endPos  + vDelta.normalized * fLength * 0.5f;

//		Color shadowCol = new Color(0, 0, 0, 0.06f);

//		for (int i = 0; i < 3; i++) // Draw a shadow
//			Handles.DrawBezier(endPos, vArrowHeadEnd1, endPos, vHalfwayPoint, shadowCol, null, (i + 1) * 5);
		Handles.DrawBezier(endPos, vArrowHeadEnd1, endPos, vHalfwayPoint, Color.black, null, 2);

//		for (int i = 0; i < 3; i++) // Draw a shadow
//			Handles.DrawBezier(endPos, vArrowHeadEnd2, endPos, vHalfwayPoint, shadowCol, null, (i + 1) * 5);
		Handles.DrawBezier(endPos, vArrowHeadEnd2, endPos, vHalfwayPoint, Color.black, null, 2);
	}
}
