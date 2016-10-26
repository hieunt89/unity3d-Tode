using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class TreeNodeUtils {

	public static void CreateTree (TreeType _treeType, string _treeName) {
		
		TreeGUI currentTree = new TreeGUI(_treeType, _treeName);

		if (currentTree != null) {
			TreeEditorWindow currentWindow = (TreeEditorWindow)EditorWindow.GetWindow <TreeEditorWindow> ();
			if (currentWindow != null) {
				currentWindow.currentTree = currentTree;
			}

			// add root node
			AddNode (currentTree, NodeType.RootNode, new Vector2(50f, 50f));

		} else {
			EditorUtility.DisplayDialog ("Tree Node Message", "Unable to create new tree", "OK");
		}

	}

	public static void SaveTree (TreeGUI _currentTree) {
		BinaryFormatter bf = new BinaryFormatter ();
		if (!Directory.Exists (Application.dataPath + TreeNodeConstants.DatabasePath + _currentTree.treeData.treeType.ToString())) {
			Directory.CreateDirectory (Application.dataPath + TreeNodeConstants.DatabasePath + _currentTree.treeData.treeType.ToString());
		}
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
			TreeEditorWindow currentWindow = (TreeEditorWindow)EditorWindow.GetWindow<TreeEditorWindow> ();
			if (currentWindow != null) {
				TreeGUI currentTree = new TreeGUI(treeData.treeType, treeData.treeName);
				currentTree.treeData = treeData;
				currentWindow.currentTree = currentTree;
			}
		} else {
			EditorUtility.DisplayDialog ("Tree Node Message", "Unable to load selected tree!", "OK");
		}
	}

//	public static void GenerateNodes (TreeUI _currentTree) {
//		NodeUI rootNode = new NodeUI ("Root Node", NodeType.RootNode, _currentTree.treeData.Root, null, new List<NodeUI> (), _currentTree);
//
//		if (rootNode != null) {
//			rootNode.InitNode (new Vector2 (50f, 50f));
//			_currentTree.nodes.Add (rootNode);
//			GenerateNodes (_currentTree, rootNode);
//		}
//
//	}
//
//	private static void GenerateNodes (TreeUI _currentTree, NodeUI _parentNode) {
//		for (int i = 0; i < _parentNode.nodeData.children.Count; i++) {
//			NodeUI newNode = new NodeUI ("Node", NodeType.Node, _parentNode.nodeData.children[i], _parentNode, new List<NodeUI> (), _currentTree);
//			if (newNode != null) {
//				_parentNode.childNodes.Add (newNode);
//
//				newNode.InitNode(new Vector2((_parentNode.nodeRect.x + _parentNode.nodeRect.width) + _parentNode.nodeRect.width / 2, _parentNode.nodeRect.y + (_parentNode.nodeRect.height * 4 * i)));
//
//				_currentTree.nodes.Add (newNode);
//				GenerateNodes (_currentTree, newNode);
//			}
//		}
//
//	}

	public static void UnloadTree () {
		TreeEditorWindow currentWindow = (TreeEditorWindow)EditorWindow.GetWindow <TreeEditorWindow> ();
		if (currentWindow != null) {
			currentWindow.currentTree = null;
		}
	}

	public static void AddNode (TreeGUI _currentTree, NodeType _nodeType, Vector2 _position) {
		if (_currentTree != null) {
			var nodeRect = new Rect (_position.x, _position.y, 100f, 40f);
			var newNode = new NodeGUI (new Node<string> (_currentTree.existIds [0]), nodeRect, null, _currentTree);
			if (_nodeType == NodeType.RootNode) {
				_currentTree.treeData.Root = newNode.nodeData;
			}
		}
	}

	public static void RemoveNode (NodeGUI _node) {
		for (int i = 0; i < _node.nodeData.children.Count; i++) {
			_node.nodeData.children[i].parent = null;
		}
		if (_node.nodeData.parent != null)
			_node.nodeData.parent.children.Remove (_node.nodeData);
		_node.nodeData.parent = null;
		_node.nodeData = null;

		for (int i = 0; i < _node.childNodes.Count; i++) {
			_node.childNodes[i].parentNode = null;
		}
		_node.currentTree.nodes.Remove (_node);
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

	public static void RemoveParentNode (NodeGUI _node) {
		_node.parentNode.nodeData.children.Remove(_node.nodeData);
		// remove this node's parent data
		_node.nodeData.parent = null;

		_node.parentNode = null;
	}
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

//	public static void DrawGrid (Rect _viewRect, float _gridSpacing, float _gridOpacity, Color _gridColor) {
//		int widthDivs = Mathf.CeilToInt (_viewRect.width - _gridSpacing);
//		int heightDivs = Mathf.CeilToInt (_viewRect.height - _gridSpacing);
//		Handles.BeginGUI ();
//		Handles.color = new Color (_gridColor.r, _gridColor.g, _gridColor.b, _gridOpacity);
//
//		for (int x = 0; x < widthDivs; x++) {
//			Handles.DrawLine (new Vector3 (_gridSpacing * x, 0f, 0f), new Vector3 (_gridSpacing * x, _viewRect.height, 0f));
//		}
//
//		for (int y = 0; y < heightDivs; y++) {
//			Handles.DrawLine (new Vector3 (0f, _gridSpacing * y, 0f), new Vector3 (_viewRect.width, _gridSpacing * y, 0f));
//		}
//		Handles.color = Color.white;
//		Handles.EndGUI ();
//	}

	public static string UppercaseFirst(string s)
	{
		// Check for empty string.
		if (string.IsNullOrEmpty(s))
		{
			return string.Empty;
		}
		// Return char and concat substring.
		return char.ToUpper(s[0]) + s.Substring(1);
	}
}
