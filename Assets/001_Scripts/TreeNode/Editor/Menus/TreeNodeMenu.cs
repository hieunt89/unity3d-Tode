using UnityEngine;
using UnityEditor;

public static class TreeNodeMenu {

	[MenuItem ("Node Editor/Node Editor")]
	public static void InitNodeEditor () {
		TreeNodeEditorWindow.InitNodeEditorWindow ();
	}
}
