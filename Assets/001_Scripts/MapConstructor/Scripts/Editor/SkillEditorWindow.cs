using UnityEngine;
using UnityEditor;


public class SkillEditorWindow : EditorWindow {



	[MenuItem ("Window/Skill Editor &S")]
	public static void ShowWindow () {
		EditorWindow.GetWindow(typeof(SkillEditorWindow));
	}
}
