using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class TreeNodeUtils <T> where T : class{

	public static void CreateTree (TreeType treeType, string treeName) {
		
		TreeUI currentTree = new TreeUI(treeType, treeName); //, new Tree<string> (""));

		if (currentTree != null) {

			TreeNodeEditorWindow <T> currentWindow = (TreeNodeEditorWindow<T>)EditorWindow.GetWindow <TreeNodeEditorWindow<T>> ();
			if (currentWindow != null) {
				currentWindow.currentTree = currentTree;
			}

			// add root node
			AddNode (currentTree, NodeType.RootNode, new Vector2(10f, 10f));

		} else {
			EditorUtility.DisplayDialog ("Tree Node Message", "Unable to create new tree", "OK");
		}

	}

	public static  void LoadTree () {
		Tree<string> treeData = null;
		string treePath = EditorUtility.OpenFilePanel ("Load Tree", Application.dataPath + TreeNodeConstants.DatabasePath, "");
		Debug.Log(treePath);
		int appPathLength = Application.dataPath.Length;
		string finalPath = treePath.Substring (appPathLength - TreeNodeConstants.DataExtension.Length);
		Debug.Log (finalPath);

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (finalPath, FileMode.Open);
		treeData = (Tree<string>) bf.Deserialize (file);
		file.Close ();
		if (treeData != null) {
			TreeNodeEditorWindow<T> currentWindow = (TreeNodeEditorWindow<T>)EditorWindow.GetWindow<TreeNodeEditorWindow<T>> ();
			if (currentWindow != null) {
//				currentWindow.currentTree.treeData = treeData;
			}
		} else {
			EditorUtility.DisplayDialog ("Tree Node Message", "Unable to load selected tree!", "OK");
		}
	}

	public static void GenerateTreeUI (Tree<string> treeData) {
		// TODO: generate tree ui + node ui base on tree data
	}

	public static void UnloadTree () {
		TreeNodeEditorWindow<T> currentWindow = (TreeNodeEditorWindow<T>)EditorWindow.GetWindow <TreeNodeEditorWindow<T>> ();
		if (currentWindow != null) {
			currentWindow.currentTree = null;
		}
	}

	public static void SaveTree (Tree<string> _treeData, TreeUI _currentTree) {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.dataPath + TreeNodeConstants.DatabasePath + _currentTree.treeName + ".tree");
		bf.Serialize (file, _treeData);
		file.Close ();
		AssetDatabase.Refresh ();
	}



	public static void AddNode (TreeUI _currentTree, NodeType _nodeType, Vector2 _position) {
		if (_currentTree != null) {
			NodeUI newNode = null;
			List <NodeUI> outputNodes = new List<NodeUI> ();
			switch (_nodeType) {
			case NodeType.RootNode:
				newNode = new NodeUI ("Root Node", _nodeType, new Node<string> (_currentTree.existIds [0]), null, outputNodes);
//				_currentTree.treeData.Root = newNode.nodeData;
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
