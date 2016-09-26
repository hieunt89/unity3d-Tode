using UnityEngine;
using UnityEditor;

public static class TreeNodeUtils {

	public static void CreateTree (TreeType treeType, string treeName) {
//		Tree <string> currentTree = new Tree<string> ();
		TreeUI currentTree = new TreeUI(new Tree<string> ());

		if (currentTree != null) {
			currentTree.treeData.treeType = treeType;
			currentTree.treeData.treeName = treeName;

//			currentTree.
//			currentTree.Root = new Node<string> ("tower0");
//			Debug.Log (currentTree.treeType + " / " + currentTree.treeName + " / " + currentTree.Root);

			// TODO: save current tree to resource folders

			TreeNodeEditorWindow currentWindow = (TreeNodeEditorWindow)EditorWindow.GetWindow <TreeNodeEditorWindow> ();
			if (currentWindow != null) {
				currentWindow.currentTree = currentTree;
			}
		} else {
			EditorUtility.DisplayDialog ("Tree Node Message", "Unable to create new tree", "OK");
		}

	}

	public static void LoadTree () {
		TreeUI currentTree = null;
//		string treePath = EditorUtility.OpenFilePanel ("Load Tree", Application.dataPath + "/Database/", "");
//		int appPathLength = Application.dataPath.Length;
//		string finalPath = treePath.Substring (appPathLength - 6);

//		currentTree = (TreeUI) DataManager.Load
		if (currentTree != null) {

		} else {
			EditorUtility.DisplayDialog ("Tree Node Message", "Unable to load selected tree!", "OK");
		}
	}

	public static void UnloadTree () {
		TreeNodeEditorWindow currentWindow = (TreeNodeEditorWindow)EditorWindow.GetWindow <TreeNodeEditorWindow> ();
		if (currentWindow.currentTree != null) {
			currentWindow.currentTree = null;
		}
	}

	public static void DrawGrid (Rect viewRect, float gridSpacing, float gridOpacity, Color gridColor) {
		int widthDivs = Mathf.CeilToInt (viewRect.width - gridSpacing);
		int heightDivs = Mathf.CeilToInt (viewRect.height - gridSpacing);

		Handles.BeginGUI ();
		Handles.color = gridColor;

		for (int x = 0; x < widthDivs; x++) {
			Handles.DrawLine (new Vector3 (gridSpacing * x, 0f, 0f), new Vector3 (gridSpacing * x, viewRect.height, 0f));
		}

		for (int y = 0; y < heightDivs; y++) {
			Handles.DrawLine (new Vector3 (0f, gridSpacing * y, 0f), new Vector3 (viewRect.width, gridSpacing * y, 0f));
		}
		Handles.color = Color.white;
		Handles.EndGUI ();
	}
}
