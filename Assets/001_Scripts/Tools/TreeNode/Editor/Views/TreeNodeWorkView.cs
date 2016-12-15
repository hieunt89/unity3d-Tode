using UnityEngine;
using System.Collections;
using UnityEditor;

public class TreeNodeWorkView : TreeViewBase {

	private Vector2 mousePosition;

	public override void UpdateView (Rect _rect, Event _e, TreeGUI _currentTree)
	{
		base.UpdateView (_rect, _e, _currentTree);

		_currentTree.UpdateTreeUI (_e, _rect);
		ProcessEvents (_e);
	}

	public override void ProcessEvents (Event e)
	{
		base.ProcessEvents (e);
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
			TreeEditorUtils.AddNode (currentTree, mousePosition);
			break;
		case "3":
			//			TreeEditorUtils.SaveTree (currentTree.treeData);
//			binaryUtils.CreateData <Tree<string>> (currentTree.treeData);
			break;
		case "4":
			TreeEditorUtils.UnloadTree ();
			break;
		}
	}
}
