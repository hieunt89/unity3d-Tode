﻿using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class TreeNodeWorkView : ViewBase {
	private Vector2 mousePosition;
	private int selectedNodeId = 0;

	public TreeNodeWorkView () : base () {
	}

	public override void UpdateView (Rect _editorRect, Rect _percentageRect, Event _e, TreeUI _currentTree)
	{
		base.UpdateView (_editorRect, _percentageRect, _e, _currentTree);

		GUI.Box (viewRect, viewTitle + " Tree", viewSkin.GetStyle("ViewBg"));

		// draw grid
		TreeNodeUtils.DrawGrid (viewRect, 60f, 0.15f, Color.white);
		TreeNodeUtils.DrawGrid (viewRect, 20f, 0.1f, Color.white);

		GUILayout.BeginArea (viewRect);
		if (_currentTree != null) {
			_currentTree.UpdateTreeUI (_e, viewRect, viewSkin);

			GUILayout.BeginHorizontal ();
			if (GUI.Button (new Rect (viewRect.x + 10f, viewRect.y + viewRect.height - 30, 100f, 20f), "Save")) {
				Debug.Log ("Save Tree");
				TreeNodeUtils.SaveTree (currentTree);
			}
			EditorGUI.LabelField (new Rect (viewRect.x + 130f, viewRect.y + viewRect.height - 30, 100f, 20f), currentTree.nodes.Count + "nodes");
		}
		GUILayout.EndArea ();

		ProcessEvent (_e);
	}

	public override void ProcessEvent (Event e)
	{
		base.ProcessEvent (e);

		if (viewRect.Contains (e.mousePosition)) {
//			if (e.button == 0) {
//				if (e.type == EventType.MouseDown) {
//					
//				}
//				if (e.type == EventType.MouseDrag) {
//					
//				}
//				if (e.type == EventType.MouseUp) {
//					
//				}
//			}
			if (e.button == 1) {
				if (e.type == EventType.MouseDown) {
					mousePosition = e.mousePosition;

					bool mouseOverNode = false;
					selectedNodeId = 0;
					if (currentTree != null) {
						if (currentTree.nodes.Count > 0) {
							for (int i = 0; i < currentTree.nodes.Count; i++) {
								if (currentTree.nodes[i].nodeRect.Contains (mousePosition)) {
									mouseOverNode = true;
									selectedNodeId = i;
								}
							}
						}
					}

					if (!mouseOverNode) {
						ProcessContextMenu(e, 0);
					} else {
						if (currentTree.nodes[selectedNodeId].nodeType != NodeType.RootNode) {
							ProcessContextMenu(e, 1);
						}
					}
				}
			}
		}
	}

	private void ProcessContextMenu (Event e, int contextId) {
		GenericMenu menu = new GenericMenu ();

		switch (contextId) {
		case 0:
			if (currentTree == null) {
				menu.AddItem (new GUIContent ("Create Tree"), false, OnClickContextCallback, "0");
				menu.AddItem (new GUIContent ("Load Tree"), false, OnClickContextCallback, "1");
			} else {
				menu.AddSeparator ("");
				menu.AddItem (new GUIContent("Unload Tree"), false, OnClickContextCallback, "2");

				menu.AddSeparator ("");
				menu.AddItem (new GUIContent ("Add Node"), false, OnClickContextCallback, "3");
			}
			break;

		case 1:
			if (currentTree != null){
				if (currentTree.nodes[selectedNodeId].parentNode != null)
					menu.AddItem (new GUIContent ("Remove Parent"), false, OnClickContextCallback, "4");
				if (currentTree.nodes[selectedNodeId].nodeType != NodeType.RootNode)
					menu.AddItem (new GUIContent ("Remove Node"), false, OnClickContextCallback, "5");
			}
			break;
		}

		menu.ShowAsContext ();
		e.Use ();
	}

	private void OnClickContextCallback (object obj) {
		switch(obj.ToString()) {
		case "0":
			TreeNodePopupWindow.InitTreeNodePopup ();
			break;
		case "1":
			TreeNodeUtils.LoadTree ();
			break;
		case "2":
			TreeNodeUtils.UnloadTree ();
			break;
		case "3":
			TreeNodeUtils.AddNode (currentTree, NodeType.Node, mousePosition);
			break;
		case "4":
			TreeNodeUtils.RemoveParentNode (selectedNodeId, currentTree);
			break;
		case "5":
			TreeNodeUtils.RemoveNode(selectedNodeId, currentTree);
			break;
		}
	}


}