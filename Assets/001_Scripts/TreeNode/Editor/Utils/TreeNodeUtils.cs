using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class TreeNodeUtils {

	public static void CreateTree (TreeType treeType, string treeName) {
		
		TreeUI currentTree = new TreeUI(treeType, treeName, new Tree<string> (""));

		if (currentTree != null) {

			// TODO: Save tree data to binary file

			TreeNodeEditorWindow currentWindow = (TreeNodeEditorWindow)EditorWindow.GetWindow <TreeNodeEditorWindow> ();
			if (currentWindow != null) {
				currentWindow.currentTree = currentTree;
			}

			// add root node
			AddNode (currentTree, NodeType.RootNode, new Vector2(10f, 10f));

		} else {
			EditorUtility.DisplayDialog ("Tree Node Message", "Unable to create new tree", "OK");
		}

	}

//	public static void LoadTree () {
//		TreeUI currentTree = null;
//
//		string treePath = EditorUtility.OpenFilePanel ("Load Tree", Application.dataPath + TreeNodeConstants.DatabasePath, "");
//		int appPathLength = Application.dataPath.Length;
//		string finalPath = treePath.Substring (appPathLength - TreeNodeConstants.DataExtension.Length);
//
////		currentTree = (TreeUI)AssetDatabase.LoadAssetAtPath (finalPath, typeof(TreeUI));
//
//		if (currentTree != null) {
//			TreeNodeEditorWindow currentWindow = (TreeNodeEditorWindow)EditorWindow.GetWindow<TreeNodeEditorWindow> ();
//			if (currentWindow != null) {
//				currentWindow.currentTree = currentTree;
//			}
//		} else {
//			EditorUtility.DisplayDialog ("Tree Node Message", "Unable to load selected tree!", "OK");
//		}
//	}

	public static void UnloadTree () {
		TreeNodeEditorWindow currentWindow = (TreeNodeEditorWindow)EditorWindow.GetWindow <TreeNodeEditorWindow> ();
		if (currentWindow != null) {
			currentWindow.currentTree = null;
		}
	}

	public static void SaveTree (TreeUI _currentTree) {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.dataPath + TreeNodeConstants.DatabasePath + _currentTree.treeName + ".gd");
		bf.Serialize (file, _currentTree.treeData);
		file.Close ();
		AssetDatabase.Refresh ();
	}

	public static Tree<string> LoadTree (string treeName) {
		if (File.Exists (Application.dataPath + TreeNodeConstants.DatabasePath + treeName + ".gd")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.dataPath + TreeNodeConstants.DatabasePath + treeName + ".gd", FileMode.Open);
			Tree<string> result = (Tree<string>) bf.Deserialize (file);

			file.Close ();
			if (result != null)
				return result;
			else
				return null;
		} else {
			return null;
		}
	}

	public static void AddNode (TreeUI _currentTree, NodeType _nodeType, Vector2 _position) {
		if (_currentTree != null) {
			NodeUI newNode = null;
			List <NodeUI> outputNodes = new List<NodeUI> ();
			switch (_nodeType) {
			case NodeType.RootNode:
				newNode = new NodeUI ("Root Node", _nodeType, new Node<string> (_currentTree.existIds [0]), null, outputNodes);
				_currentTree.treeData.Root = newNode.nodeData;
				break;
			case NodeType.Node:
				newNode = new NodeUI("Node", _nodeType, new Node<string> (_currentTree.existIds[0]), null, outputNodes);
				break;
			default:
				break;
			}

			if (newNode != null) {
				newNode.InitNode (_position);

				newNode.currentTree = _currentTree;
				_currentTree.nodes.Add (newNode);

				// TODO: save node 
//				AssetDatabase.AddObjectToAsset (newNode, _currentTree);
//				AssetDatabase.SaveAssets ();
//				AssetDatabase.Refresh ();
			}
		}

	}

	public static void RemoveNode (int _nodeId, TreeUI _currentTree) {
		if (_currentTree != null) {
			if(_currentTree.nodes.Count >= _nodeId) {
				NodeUI selectecNode = _currentTree.nodes[_nodeId];
				if(selectecNode != null) {
					_currentTree.nodes.RemoveAt (_nodeId);

					// TODO: save data after remove
				}
			}
		}
	}

	public static void RemoveParentNode (int _nodeId, TreeUI _currentTree) {
		if (_currentTree != null) {
			if(_currentTree.nodes.Count >= _nodeId) {
				NodeUI selectecNode = _currentTree.nodes[_nodeId];
				if(selectecNode != null) {
					_currentTree.nodes[_nodeId].parentNode = null;

					// TODO: save data after remove
				}
			}
		}
	}

	public static void DrawGrid (Rect _viewRect, float _gridSpacing, float _gridOpacity, Color _gridColor) {
		int widthDivs = Mathf.CeilToInt (_viewRect.width - _gridSpacing);
		int heightDivs = Mathf.CeilToInt (_viewRect.height - _gridSpacing);

		Handles.BeginGUI ();
		Handles.color = new Color (_gridColor.r, _gridColor.g, _gridColor.b, _gridOpacity);

		for (int x = 0; x < widthDivs; x++) {
			Handles.DrawLine (new Vector3 (_gridSpacing * x, 0f, 0f), new Vector3 (_gridSpacing * x, _viewRect.height, 0f));
		}

		for (int y = 0; y < heightDivs; y++) {
			Handles.DrawLine (new Vector3 (0f, _gridSpacing * y, 0f), new Vector3 (_viewRect.width, _gridSpacing * y, 0f));
		}
		Handles.color = Color.white;
		Handles.EndGUI ();
	}
}
