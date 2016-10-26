using UnityEngine;
using UnityEditor;
using System;

public class TreeNodeWorkView : ViewBase {
	private Vector2 mousePosition;
	private TreeType treeType;
	private string treeName = "Enter tree name ...";
	public TreeNodeWorkView () : base () {
	}

	public override void UpdateView (Rect _editorRect, Rect _percentageRect, Event _e, TreeGUI _currentTree)
	{
		base.UpdateView (_editorRect, _percentageRect, _e, _currentTree);

		GUILayout.BeginArea (viewRect);

		if (currentTree != null) {
//			GUI.Box (viewRect, viewTitle + " Tree", viewSkin.GetStyle("ViewBg"));
			currentTree.UpdateTreeUI (_e, viewRect, viewSkin);
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
			if (e.button == 1) {
				if (e.type == EventType.MouseDown) {
					mousePosition = e.mousePosition;

					ProcessContextMenu(e, 0);
				}
			}
		}
	}

	private void ProcessContextMenu (Event e, int contextId) {
		GenericMenu menu = new GenericMenu ();

		switch (contextId) {
		case 0:
			menu.AddItem (new GUIContent ("Add Node"), false, OnClickContextCallback, "2");
			menu.AddItem (new GUIContent("Save Tree"), false, OnClickContextCallback, "3");
			menu.AddSeparator ("");
			menu.AddItem (new GUIContent("Unload Tree"), false, OnClickContextCallback, "4");
			break;
		}

		menu.ShowAsContext ();
		e.Use ();
	}

	private void OnClickContextCallback (object obj) {
		switch(obj.ToString()) {
		case "2":
			TreeEditorUtils.AddNode (currentTree, NodeType.Node, mousePosition);
			break;
		case "3":
			TreeEditorUtils.SaveTree (currentTree);
			break;
		case "4":
			TreeEditorUtils.UnloadTree ();
			break;
		}
	}


}
