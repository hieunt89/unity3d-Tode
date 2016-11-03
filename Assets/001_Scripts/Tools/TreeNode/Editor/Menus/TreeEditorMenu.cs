using UnityEngine;
using UnityEditor;

public static class TreeEditorMenu {

	[MenuItem ("Tode/Tree Data Editor")]
	public static void InitTowerNodeEditor () {
		TreeEditorWindow.InitTowerNodeEditorWindow ();
	}
}
