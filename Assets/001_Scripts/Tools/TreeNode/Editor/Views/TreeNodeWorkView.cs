using UnityEngine;
using System.Collections;
using UnityEditor;

public class TreeNodeWorkView : TreeViewBase {

	private Vector2 mousePosition;

	public override void UpdateView (Rect _rect, Event _e, TreeGUI _currentTree)
	{
		base.UpdateView (_rect, _e, _currentTree);

		TreeEditorUtils.DrawGrid (_rect, 20f, 20f, Color.gray);

		currentTree.UpdateTreeUI (_e, viewRect);

		ProcessEvents (_e);
	}

	public override void ProcessEvents (Event _e)
	{
		base.ProcessEvents (_e);

		if (viewRect.Contains (_e.mousePosition)) {
			if (_e.button == 1) {
				if (_e.type == EventType.MouseDown) {
					mousePosition = _e.mousePosition;

					ProcessContextMenu(_e, 0);
				}
			}
		}
	}

	private void ProcessContextMenu (Event _e, int _contextId) {
		GenericMenu menu = new GenericMenu ();

		switch (_contextId) {
		case 0:
			menu.AddItem (new GUIContent ("Add Node"), false, OnClickContextCallback, "2");
//			menu.AddItem (new GUIContent("Save Tree"), false, OnClickContextCallback, "3");
			menu.AddSeparator ("");
			menu.AddItem (new GUIContent("Unload Tree"), false, OnClickContextCallback, "4");
			break;
		}

		menu.ShowAsContext ();
		_e.Use ();
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
