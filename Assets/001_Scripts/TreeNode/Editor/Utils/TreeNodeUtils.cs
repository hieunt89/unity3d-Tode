using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class TreeNodeUtils {

	public static void CreateTree (TreeType _treeType, string _treeName) {
		
		TreeUI currentTree = new TreeUI(_treeType, _treeName);

		if (currentTree != null) {
			TreeNodeEditorWindow currentWindow = (TreeNodeEditorWindow)EditorWindow.GetWindow <TreeNodeEditorWindow> ();
			if (currentWindow != null) {
				currentWindow.currentTree = currentTree;
			}

			// add root node
			AddNode (currentTree, NodeType.RootNode, new Vector2(50f, 50f));

		} else {
			EditorUtility.DisplayDialog ("Tree Node Message", "Unable to create new tree", "OK");
		}

	}

	public static void SaveTree (TreeUI _currentTree) {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.dataPath + TreeNodeConstants.DatabasePath + _currentTree.treeData.treeName + ".tree");
		bf.Serialize (file, _currentTree.treeData);
		file.Close ();
		AssetDatabase.Refresh ();
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
			TreeNodeEditorWindow currentWindow = (TreeNodeEditorWindow)EditorWindow.GetWindow<TreeNodeEditorWindow> ();
			if (currentWindow != null) {
				TreeUI currentTree = new TreeUI(treeData.treeType, treeData.treeName);
				currentTree.treeData = treeData;
				currentWindow.currentTree = currentTree;
			}
		} else {
			EditorUtility.DisplayDialog ("Tree Node Message", "Unable to load selected tree!", "OK");
		}
	}

	public static void GenerateNodeUI (TreeUI _currentTree) {
		Debug.Log ("generate node ui");
		NodeUI rootNode = new NodeUI ("Root Node", NodeType.RootNode, _currentTree.treeData.Root, null, new List<NodeUI> ());

		if (rootNode != null) {
			rootNode.InitNode (new Vector2 (50f, 50f));
			rootNode.currentTree = _currentTree;
			_currentTree.nodes.Add (rootNode);
		}

		for (int i = 0; i < rootNode.nodeData.children.Count; i++) {
			// TODO: how to recursive :(

			NodeUI newNode = new NodeUI ("Node", NodeType.Node, rootNode.nodeData.children[i], rootNode, rootNode.childNodes);

			if (newNode != null) {
				newNode.InitNode (new Vector2 ((_currentTree.nodes.Count + 1) * 100f, (i+1) * 50f));
				newNode.currentTree = _currentTree;
				_currentTree.nodes.Add (newNode);
			}

		}

	}

	private static void DeQuy () {

	}

	public static void UnloadTree () {
		TreeNodeEditorWindow currentWindow = (TreeNodeEditorWindow)EditorWindow.GetWindow <TreeNodeEditorWindow> ();
		if (currentWindow != null) {
			currentWindow.currentTree = null;
		}
	}

	public static void AddNode (TreeUI _currentTree, NodeType _nodeType, Vector2 _position) {
		if (_currentTree != null) {
			NodeUI newNode = null;
			List <NodeUI> outputNodes = new List<NodeUI> ();
			switch (_nodeType) {
			case NodeType.RootNode:
				newNode = new NodeUI ("Root Node", _nodeType, new Node<string> (_currentTree.existIds [0]), null, outputNodes);
				// assign data to root node
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
			}
		}

	}

	public static void RemoveNode (int _nodeId, TreeUI _currentTree) {
		if (_currentTree != null) {
			if(_currentTree.nodes.Count >= _nodeId) {
				NodeUI selectecNode = _currentTree.nodes[_nodeId];
				if(selectecNode != null) {
//					var deleteNodeData = _currentTree.treeData.Root.FindChildNodeByData (_currentTree.nodes [_nodeId].nodeData.data);	// it does not work with duplicate data

					//TODO: remove connection

					// TODO: delete node data ?
					_currentTree.nodes[_nodeId].nodeData = null;
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
					_currentTree.nodes [_nodeId].nodeData.parent = null;
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
