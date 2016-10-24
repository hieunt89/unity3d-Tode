using UnityEngine;
using UnityEditor;
using System;

public class WorkView : ViewBase {
	private Vector2 mousePosition;
//	private int selectedNodeId = 0;
//	private Vector2 scrollPosition;
	private TreeType treeType;
	private string treeName = "Enter tree name ...";
	public WorkView () : base () {
	}

	public override void UpdateView<T> (Rect _viewRect, Rect _percentageRect, Event _event, GenericTree<T> _currentTree)
	{
		base.UpdateView (_viewRect, _percentageRect, _event, _currentTree);

		if (_currentTree != null) {
			GUI.Box (viewRect, viewTitle + " Tree", viewSkin.GetStyle("ViewBg"));

			_currentTree.UpdateGenericTree <T> (_event, _percentageRect, viewSkin);

			ProcessEvent (_event);

		}
	}

	public override void UpdateView (Rect _editorRect, Rect _percentageRect, Event _e, TreeUI _currentTree)
	{
		base.UpdateView (_editorRect, _percentageRect, _e, _currentTree);
//		GUI.Label (_editorRect, viewTitle + " Tree");
		// draw grid
//		TreeNodeUtils.DrawGrid (viewRect, 60f, 0.15f, Color.white);
//		TreeNodeUtils.DrawGrid (viewRect, 20f, 0.1f, Color.white);

		GUILayout.BeginArea (viewRect);

		if (_currentTree != null) {
			GUI.Box (viewRect, viewTitle + " Tree", viewSkin.GetStyle("ViewBg"));
//			currentTree.UpdateTreeUI (_e, viewRect, viewSkin);
			ProcessEvent (_e);
		} else {
			var newTreeRect = new Rect (viewRect.width / 2 - 200f, viewRect.height / 2 - 100f, 400, 200);
			GUI.Box (newTreeRect, "Create New Tree", viewSkin.GetStyle("ViewBg"));

			GUILayout.BeginArea (newTreeRect);

			GUILayout.BeginHorizontal ();
			GUILayout.Space (12);

			GUILayout.BeginVertical ();
			GUILayout.Space (60);
			treeType = (TreeType) EditorGUILayout.EnumPopup ("Tree Type", treeType);
			treeName = EditorGUILayout.TextField ("Tree name", treeName);
			GUILayout.Space (40);

			GUILayout.BeginHorizontal ();
			GUI.enabled = !string.IsNullOrEmpty (treeName) && treeName != "Enter tree name ...";
			if (GUILayout.Button ("Create Tree", GUILayout.Height(40))){
				TreeEditorUtils.CreateTree (treeType, treeName);
			}
			GUI.enabled = true;
			if (GUILayout.Button ("Load Tree", GUILayout.Height(40))){
				TreeEditorUtils.LoadTree ();
			}
			GUILayout.EndHorizontal ();
			GUILayout.EndVertical ();
			GUILayout.Space (12);
			GUILayout.EndHorizontal ();
			GUILayout.EndArea ();
		}

		GUILayout.EndArea ();

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

//					bool mouseOverNode = false;
//					selectedNodeId = 0;
//					if (currentTree != null) {
//						if (currentTree.nodes.Count > 0) {
//							for (int i = 0; i < currentTree.nodes.Count; i++) {
//								if (currentTree.nodes[i].nodeRect.Contains (mousePosition)) {
//									mouseOverNode = true;
//									selectedNodeId = i;
//								}
//							}
//						}
//					}

//					if (!mouseOverNode) {
						ProcessContextMenu(e, 0);
//					} else {
//						if (currentTree.nodes[selectedNodeId].nodeType != NodeType.RootNode) {
//							ProcessContextMenu(e, 1);
//						}
//					}
				}
			}
		}
	}

	private void ProcessContextMenu (Event e, int contextId) {
		GenericMenu menu = new GenericMenu ();

		switch (contextId) {
		case 0:
//			if (currentTree == null) {
//				menu.AddItem (new GUIContent ("Create Tree"), false, OnClickContextCallback, "0");
//				menu.AddItem (new GUIContent ("Load Tree"), false, OnClickContextCallback, "1");
//			} else {
				menu.AddItem (new GUIContent ("Add Node"), false, OnClickContextCallback, "2");
				menu.AddItem (new GUIContent("Save Tree"), false, OnClickContextCallback, "3");
				menu.AddSeparator ("");
				menu.AddItem (new GUIContent("Unload Tree"), false, OnClickContextCallback, "4");
//			}
			break;

//		case 1:
//			if (currentTree != null){
//				if (currentTree.nodes[selectedNodeId].parentNode != null)
//					menu.AddItem (new GUIContent ("Remove Parent"), false, OnClickContextCallback, "5");
//				if (currentTree.nodes[selectedNodeId].nodeType != NodeType.RootNode)
//					menu.AddItem (new GUIContent ("Remove Node"), false, OnClickContextCallback, "6");
//			}
//			break;
		}

		menu.ShowAsContext ();
		e.Use ();
	}

	private void OnClickContextCallback (object obj) {
		switch(obj.ToString()) {
//		case "0":
//			TreeNodePopupWindow.InitTreeNodePopup ();
//			break;
//		case "1":
//			TreeNodeUtils.LoadTree ();
//			break;
		case "2":
//			TreeNodeUtils.AddNode (currentTree, NodeType.Node, mousePosition);
			break;
		case "3":
//			TreeNodeUtils.SaveTree (currentTree);
			break;
		case "4":
//			TreeNodeUtils.UnloadTree ();
			break;
//		case "5":
//			TreeNodeUtils.RemoveParentNode (selectedNodeId, currentTree);
//			break;
//		case "6":
//			TreeNodeUtils.RemoveNode(selectedNodeId, currentTree);
//			break;
		}
	}


}
