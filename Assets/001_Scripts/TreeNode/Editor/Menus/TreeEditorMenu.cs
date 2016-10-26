using UnityEngine;
using UnityEditor;

public static class TreeEditorMenu {

	[MenuItem ("Tree/Open Editor")]
	public static void InitTowerNodeEditor () {
		TreeEditorWindow.InitTowerNodeEditorWindow ();
	}
}
