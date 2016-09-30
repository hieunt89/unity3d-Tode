using UnityEngine;
using UnityEditor;

public static class TreeNodeMenu {

	[MenuItem ("Tree Node Editor/Tower Node Editor")]
	public static void InitTowerNodeEditor () {
		TreeNodeEditorWindow<string>.InitTowerNodeEditorWindow ();
	}

	[MenuItem ("Tree Node Editor/Skill Node Editor")]
	public static void InitSkillNodeEditor () {

	}
}
