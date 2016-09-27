using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class TreeNodeWorkView : ViewBase {
	private Vector2 mousePosition;
	private int deleteNodeId = 0;

	public TreeNodeWorkView () : base ("Tree View") {
	}

	public override void UpdateView (Rect _editorRect, Rect _percentageRect, Event _e, TreeUI _currentTree)
	{
		base.UpdateView (_editorRect, _percentageRect, _e, _currentTree);

		if (_currentTree != null) {
			viewTitle = _currentTree.treeName;
		} else {
			viewTitle = "No Tree";
		}

		GUI.Box (viewRect, viewTitle, viewSkin.GetStyle("ViewBg"));

		// draw grid
		TreeNodeUtils.DrawGrid (viewRect, 60f, 0.015f, Color.white);
//		TreeNodeUtils.DrawGrid (viewRect, 20f, 0.1f, Color.white);

		GUILayout.BeginArea (viewRect);
		if (currentTree != null) {
			currentTree.UpdateTreeUI (_e, viewRect, viewSkin);
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
					deleteNodeId = 0;
					if (currentTree != null) {
						if (currentTree.nodes.Count > 0) {
							for (int i = 0; i < currentTree.nodes.Count; i++) {
								if (currentTree.nodes[i].nodeRect.Contains (mousePosition)) {
									mouseOverNode = true;
									deleteNodeId = i;
								}
							}
						}
					}

					if (!mouseOverNode) {
						ProcessContextMenu(e, 0);
					} else {
						ProcessContextMenu(e, 1);
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
				menu.AddItem (new GUIContent ("Remove Node"), false, OnClickContextCallback, "4");
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
			Debug.Log ("Load Tree");
			break;
		case "2":
			TreeNodeUtils.UnloadTree ();
			break;
		case "3":
			TreeNodeUtils.AddNode (currentTree, NodeType.Node, mousePosition);
			break;
		case "4":
			TreeNodeUtils.RemoveNode(deleteNodeId, currentTree);
			break;
		}
	}


}
