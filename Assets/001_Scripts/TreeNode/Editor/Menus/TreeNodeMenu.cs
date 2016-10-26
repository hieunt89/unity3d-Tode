using UnityEngine;
using UnityEditor;

public static class TreeNodeMenu {

	[MenuItem ("Tree Node Editor/Tree Node Editor")]
	public static void InitTowerNodeEditor () {
		TreeEditorWindow.InitTowerNodeEditorWindow ();
	}
}
