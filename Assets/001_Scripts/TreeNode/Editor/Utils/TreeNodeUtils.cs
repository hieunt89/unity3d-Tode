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
		FileStream file = File.Create (Application.dataPath + TreeNodeConstants.DatabasePath + _currentTree.treeData.treeType.ToString() + "/" + _currentTree.treeData.treeName + ".bytes");
		bf.Serialize (file, _currentTree.treeData);
		file.Close ();
		AssetDatabase.Refresh ();
	}

	public static  void LoadTree () {
		Tree<string> treeData = null;
		string treePath = EditorUtility.OpenFilePanel ("Load Tree", Application.dataPath + TreeNodeConstants.DatabasePath, "");
		int appPathLength = Application.dataPath.Length;
		if (string.IsNullOrEmpty(treePath)) return;

		string finalPath = treePath.Substring (appPathLength - TreeNodeConstants.DataExtension.Length);

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

	public static void GenerateNodes (TreeUI _currentTree) {
		NodeUI rootNode = new NodeUI ("Root Node", NodeType.RootNode, _currentTree.treeData.Root, null, new List<NodeUI> ());

		if (rootNode != null) {
			rootNode.InitNode (new Vector2 (50f, 50f));
			rootNode.currentTree = _currentTree;
			_currentTree.nodes.Add (rootNode);
			GenerateNodes (_currentTree, rootNode);
		}

	}

	private static void GenerateNodes (TreeUI _currentTree, NodeUI _parentNode) {
		for (int i = 0; i < _parentNode.nodeData.children.Count; i++) {
			NodeUI newNode = new NodeUI ("Node", NodeType.Node, _parentNode.nodeData.children[i], _parentNode, new List<NodeUI> ());
			if (newNode != null) {
				_parentNode.childNodes.Add (newNode);

				newNode.InitNode(new Vector2((_parentNode.nodeRect.x + _parentNode.nodeRect.width) + _parentNode.nodeRect.width / 2, _parentNode.nodeRect.y + (_parentNode.nodeRect.height * 4 * i)));
				newNode.currentTree = _currentTree;
				_currentTree.nodes.Add (newNode);
				GenerateNodes (_currentTree, newNode);
			}
		}

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

//	public static void RemoveNode (int _nodeId, TreeUI _currentTree) {
//		if (_currentTree != null) {
//			if(_currentTree.nodes.Count >= _nodeId) {
//				NodeUI selectecNode = _currentTree.nodes[_nodeId];
//				if(selectecNode != null) {
//					// remove this node (parent) from its children node
//					for (int i = 0; i < selectecNode.nodeData.children.Count; i++) {
//						selectecNode.nodeData.children[i].parent = null;
//					}
//					// remove this node from its parent node
//					if (selectecNode.nodeData.parent != null)
//						selectecNode.nodeData.parent.children.Remove (selectecNode.nodeData);
//
//					// remove its parent node data
//					selectecNode.nodeData.parent = null;
//
//					// remove this node data ?
//					selectecNode.nodeData = null;
//
//					// remove node ui parent from child node ui
//					for (int i = 0; i < selectecNode.childNodes.Count; i++) {
//						selectecNode.childNodes[i].parentNode = null;
//					}
//
//					// remove this node ui from list 
//					_currentTree.nodes.RemoveAt (_nodeId);
//				}
//			}
//		}
//	}

//	public static void RemoveParentNode (int _nodeId, TreeUI _currentTree) {
//		if (_currentTree != null) {
//			if(_currentTree.nodes.Count >= _nodeId) {
//				NodeUI selectecNode = _currentTree.nodes[_nodeId];
//				if(selectecNode != null) {
//					// remove this node data from parent node's children data list
//					_currentTree.nodes[_nodeId].parentNode.nodeData.children.Remove(_currentTree.nodes[_nodeId].nodeData);
//					// remove this node's parent data
//					_currentTree.nodes[_nodeId].nodeData.parent = null;
//
//					_currentTree.nodes[_nodeId].parentNode = null;
//				}
//			}
//		}
//	}

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
