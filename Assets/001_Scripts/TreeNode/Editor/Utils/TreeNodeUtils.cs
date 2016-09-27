using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public static class TreeNodeUtils {

	public static void CreateTree (TreeType treeType, string treeName) {
		
		TreeUI currentTree = new TreeUI(treeType, treeName, new Tree<string> (""));

		if (currentTree != null) {
			// add root node
			AddNode (currentTree, NodeType.RootNode, new Vector2(10f, 10f));

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

		// TODO: load tree from resource

		if (currentTree != null) {

		} else {
			EditorUtility.DisplayDialog ("Tree Node Message", "Unable to load selected tree!", "OK");
		}
	}

	public static void UnloadTree () {
		TreeNodeEditorWindow currentWindow = (TreeNodeEditorWindow)EditorWindow.GetWindow <TreeNodeEditorWindow> ();
		if (currentWindow != null) {
			currentWindow.currentTree = null;
		}
	}

	public static void AddNode (TreeUI currentTree, NodeType nodeType, Vector2 position) {
		if (currentTree != null) {
			NodeUI newNode = null;
			List <NodeUI> outputNodes = new List<NodeUI> ();
			switch (nodeType) {
			case NodeType.RootNode:
				newNode = new NodeUI(new Node<string> ("root node"), null, outputNodes);
				break;
			case NodeType.Node:
				newNode = new NodeUI(new Node<string> ("node"), null, outputNodes);
				break;
			default:
				break;
			}

			if (newNode != null) {
				newNode.InitNode (position);

				newNode.currentTree = currentTree;
				currentTree.nodes.Add (newNode);

				// TODO: save node 
			}
		}

	}

	public static void RemoveNode (int nodeId, TreeUI currentTree) {
		if (currentTree != null) {
			if(currentTree.nodes.Count >= nodeId) {
				NodeUI selectecNode = currentTree.nodes[nodeId];
				if(selectecNode != null) {
					currentTree.nodes.RemoveAt (nodeId);

					// TODO: save data after remove
				}
			}
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
