using UnityEngine;
using UnityEditor;

public static class TreeEditorMenu {

	[MenuItem ("Tree Editor/Open")]
	public static void InitTowerNodeEditor () {
		TreeEditorWindow.InitTowerNodeEditorWindow ();
	}
}
