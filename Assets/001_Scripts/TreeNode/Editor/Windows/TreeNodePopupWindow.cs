using UnityEngine;
using UnityEditor;

public class TreeNodePopupWindow : EditorWindow {

	static TreeNodePopupWindow currentPopup;
	string treeName = "Enter tree name ...";
	TreeType treeType;

	public static void InitTreeNodePopup () {
		currentPopup = (TreeNodePopupWindow) EditorWindow.GetWindow <TreeNodePopupWindow> ();
		currentPopup.titleContent = new GUIContent ("Tree Node Popup");
	}

	private void OnGUI () {
//		GUILayout.Space (20);
		GUILayout.BeginHorizontal ();
		GUILayout.BeginVertical ();

		EditorGUILayout.LabelField ("Create New Tree", EditorStyles.label);
		treeType = (TreeType) EditorGUILayout.EnumPopup ("Tree Type", treeType);
		treeName = EditorGUILayout.TextField ("Tree name", treeName);
		GUILayout.Space (10);

		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Create Tree", GUILayout.Height(40))){
			if (!string.IsNullOrEmpty (treeName) && treeName != "Enter tree name ...") {
				TreeNodeUtils.CreateTree (treeType, treeName);

//				currentPopup.Close ();
			} else {
				EditorUtility.DisplayDialog ("Tree Node Message", "Please enter a valid tree name", "OK");
			}
		}
		GUILayout.EndHorizontal ();
		GUILayout.EndVertical ();
		GUILayout.EndHorizontal ();
	}


}
