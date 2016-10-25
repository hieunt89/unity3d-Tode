using UnityEngine;
using System.Collections;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;

public class GenericTree <T> {

	public Tree<T> treeData;
	public List<GenericNode<T>> nodes;

	public GenericNode<T> lastNodeUI;
	public GenericNode<T> selectedNode;
	public GenericNode<T> startConnectionNode;
	public bool wantsConnection = false;

	public List<T> existData;
	public List<string> existIds;

	public GenericTree (string _treeName) {
		existData = DataManager.Instance.LoadAllData <T> ();
		if (existData.Count > 0) {
			existIds = new List<string> ();
			for (int i = 0; i < existData.Count; i++) {
				FieldInfo field = typeof(T).GetField ("id", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				string id = (string)field.GetValue (existData [i]);
				existIds.Add (id);
			}
		} else {
			// do popup craete rootnode data
			GenericTreePopupWindow.InitGenericTreePopup(typeof(T));
			return;
		}

		treeData = new Tree<T> (_treeName, null);

		var rootNode = new GenericNode<T> ("Root Node", new Node<T> (existData [0]), null, new List<GenericNode<T>> (), this);
		nodes = new List<GenericNode<T>> ();
		nodes.Add(rootNode);

	}

	public void UpdateGenericTree <T> (Event _e, Rect _viewRect, GUISkin _viewSkin) {
		ProcessEvents (_e, _viewRect);

		if (treeData != null && nodes.Count == 0) {
			GenerateNodes();
		}

		if (nodes.Count > 0) {
			for (int i = 0; i < nodes.Count; i++) {
				nodes [i].UpdateGenericNode <T> (i, _e, _viewRect, _viewSkin);
			}
		}

		if (wantsConnection) {
			if (startConnectionNode != null) {
				DrawConnectionToMouse (_e.mousePosition);
			}
		} else {

		}

		if (_e.type == EventType.Layout) {
			if (selectedNode != null) {
//				showNodeProperties = true;
			}
		}
	}

	private void ProcessEvents (Event _e, Rect _viewRect) {
		if (_viewRect.Contains (_e.mousePosition)) {
			if (_e.button == 0) {
				if (_e.type == EventType.MouseDown) {

				}
			}
		}
	}

	private void DrawConnectionToMouse (Vector2 _mousePosition) {
		bool isRight = _mousePosition.x >= startConnectionNode.nodeRect.x + startConnectionNode.nodeRect.width * 0.5f;

		var startPos = new Vector3(isRight ? startConnectionNode.nodeRect.x + startConnectionNode.nodeRect.width :  startConnectionNode.nodeRect.x, 
			startConnectionNode.nodeRect.y + startConnectionNode.nodeRect.height +  startConnectionNode.nodeRect.height * .5f, 
			0);
		var endPos = new Vector3(_mousePosition.x, _mousePosition.y, 0);

		float mnog = Vector3.Distance(startPos,endPos);
		Vector3 startTangent = startPos + (isRight ? Vector3.right : Vector3.left) * (mnog / 3f) ;
		Vector3 endTangent = endPos + (isRight ? Vector3.left : Vector3.right) * (mnog / 3f);

		Handles.BeginGUI ();
		Handles.DrawBezier(startPos, endPos, startTangent, endTangent,Color.white, null, 2f);
		Handles.EndGUI ();
	}

	private void GenerateNodes () {
//		NodeUI rootNode = new NodeUI ("Root Node", NodeType.RootNode, _currentTree.treeData.Root, null, new List<NodeUI> (), _currentTree);
//		if (rootNode != null) {
//			rootNode.InitNode (new Vector2 (50f, 50f));
//			_currentTree.nodes.Add (rootNode);
//			lastNodeUI = rootNode;
//			GenerateNodes (_currentTree, rootNode);
//		}
	}

	private void GenerateNodes (GenericNode <T> _parentNode) {
		for (int i = 0; i < _parentNode.nodeData.children.Count; i++) {
//			NodeUI newNode = new NodeUI ("Node", NodeType.Node, _parentNode.nodeData.children[i], _parentNode, new List<NodeUI> (), _currentTree);
//			if (newNode != null) {
//				_parentNode.childNodes.Add (newNode);
//				newNode.InitNode (new Vector2 ((_parentNode.nodeRect.x + _parentNode.nodeRect.width) + _parentNode.nodeRect.width / 2, lastNodeUI.nodeRect.y +(_parentNode.nodeRect.height * 2 * i)));
//				_currentTree.nodes.Add (newNode);
//				lastNodeUI = newNode;
//				GenerateNodes (_currentTree, newNode);
//			}
		}

	}
}

public class TreeUI {

	public Tree<string> treeData;
	public List<NodeUI> nodes;
	public NodeUI selectedNode;
	public bool wantsConnection = false;
	public NodeUI startConnectionNode;
	public bool showNodeProperties = false;

	public List<string> existIds;
	List<TowerData> towerData;
	List<CombatSkillData> combatSkillData;
	List<SummonSkillData> summonSkillData;

	private NodeUI lastNodeUI;
	public TreeUI (string _treeName) {
		treeData = new Tree<string> (_treeName, null);

		if (nodes == null) {
			nodes = new List<NodeUI> ();
		}

		// TODO: chuyen exist data ra global ?
		// load exist data based on tree type
//		switch (_treeType) {
//		case TreeType.Towers:
////			Type x = typeof(TowerData);
////			Type listType = typeof(List<>).MakeGenericType(x);
////			var existData = Activator.CreateInstance(listType);
//
////			var fooList = Activator
////				.CreateInstance(typeof(List<>)
////					.MakeGenericType(TowerData.GetType()));
//			
//			towerData = DataManager.Instance.LoadAllData <TowerData> ();
//			if (towerData != null){
//				existIds = new List<string> ();
//				for (int i = 0; i < towerData.Count; i++) {
//					existIds.Add(towerData[i].Id);
//				}
//			}
//			break;
//		case TreeType.CombatSkills:
//			combatSkillData = DataManager.Instance.LoadAllData <CombatSkillData> ();
//			if (combatSkillData != null){
//				existIds = new List<string> ();
//				for (int i = 0; i < combatSkillData.Count; i++) {
//					existIds.Add(combatSkillData[i].id);
//				}
//			}
//			break;
//		case TreeType.SummonSkills:
//			summonSkillData = DataManager.Instance.LoadAllData <SummonSkillData> ();
//			if (summonSkillData != null){
//				existIds = new List<string> ();
//				for (int i = 0; i < summonSkillData.Count; i++) {
//					existIds.Add(summonSkillData[i].id);
//				}
//			}
//			break;
//		default:
//			break;
//		}
	}


//	public void UpdateTreeUI (Event _e, Rect _viewRect, GUISkin _viewSkin) {
//		ProcessEvents (_e, _viewRect);
//		
//		if (treeData != null && nodes.Count == 0) {
//			GenerateNodes(this);
//		}
//
//		if (nodes.Count > 0) {
//			for (int i = 0; i < nodes.Count; i++) {
//				nodes [i].UpdateNodeUI (i, _e, _viewRect, _viewSkin);
//			}
//		}
//
//
//		if (wantsConnection) {
//			if (startConnectionNode != null) {
//				DrawConnectionToMouse (_e.mousePosition);
//			}
//		} else {
//
//		}
//
//		if (_e.type == EventType.Layout) {
//			if (selectedNode != null) {
//				showNodeProperties = true;
//			}
//		}
//
////		EditorUtility.SetDirty (this);
//	}

//	private void ProcessEvents (Event _e, Rect _viewRect) {
//		if (_viewRect.Contains (_e.mousePosition)) {
//			if (_e.button == 0) {
//				if (_e.type == EventType.MouseDown) {
////					DeselectAllNodes ();
////
//					showNodeProperties = false;
////					bool setNode = false;
////					selectedNode = null;
////
////					for (int i = 0; i < nodes.Count; i++) {
////						if (nodes[i].nodeRect.Contains (_e.mousePosition)) {
////							nodes[i].isSelected = true;
////							selectedNode = nodes[i];
////							setNode = true;
////						}
////					}
////
////					if (!setNode) {
////						DeselectAllNodes ();
////					}
//
//					if (wantsConnection) {
//						wantsConnection = false;
//					}
//				}
//			}
//		}
//	}
//
//	private void DrawConnectionToMouse (Vector2 _mousePosition) {
//		bool isRight = _mousePosition.x >= startConnectionNode.nodeRect.x + startConnectionNode.nodeRect.width * 0.5f;
//
//		var startPos = new Vector3(isRight ? startConnectionNode.nodeRect.x + startConnectionNode.nodeRect.width :  startConnectionNode.nodeRect.x, 
//			startConnectionNode.nodeRect.y + startConnectionNode.nodeRect.height +  startConnectionNode.nodeContentRect .height * .5f, 
//									0);
//		var endPos = new Vector3(_mousePosition.x, _mousePosition.y, 0);
//
//		float mnog = Vector3.Distance(startPos,endPos);
//		Vector3 startTangent = startPos + (isRight ? Vector3.right : Vector3.left) * (mnog / 3f) ;
//		Vector3 endTangent = endPos + (isRight ? Vector3.left : Vector3.right) * (mnog / 3f);
//
//		Handles.BeginGUI ();
//		Handles.DrawBezier(startPos, endPos, startTangent, endTangent,Color.white, null, 2f);
//		Handles.EndGUI ();
//	}

//	private void DeselectAllNodes () {
//		for (int i = 0; i < nodes.Count; i++) {
//			nodes [i].isSelected = false;
//		}
//	}

//	private void GenerateNodes (TreeUI _currentTree) {
//		NodeUI rootNode = new NodeUI ("Root Node", NodeType.RootNode, _currentTree.treeData.Root, null, new List<NodeUI> (), _currentTree);
//
//		if (rootNode != null) {
//			rootNode.InitNode (new Vector2 (50f, 50f));
//			_currentTree.nodes.Add (rootNode);
//			lastNodeUI = rootNode;
//			GenerateNodes (_currentTree, rootNode);
//		}
//
//	}
//
//	private void GenerateNodes (TreeUI _currentTree, NodeUI _parentNode) {
//		for (int i = 0; i < _parentNode.nodeData.children.Count; i++) {
//			NodeUI newNode = new NodeUI ("Node", NodeType.Node, _parentNode.nodeData.children[i], _parentNode, new List<NodeUI> (), _currentTree);
//			if (newNode != null) {
//				_parentNode.childNodes.Add (newNode);
//				newNode.InitNode (new Vector2 ((_parentNode.nodeRect.x + _parentNode.nodeRect.width) + _parentNode.nodeRect.width / 2, lastNodeUI.nodeRect.y +(_parentNode.nodeRect.height * 2 * i)));
//				_currentTree.nodes.Add (newNode);
//				lastNodeUI = newNode;
//				GenerateNodes (_currentTree, newNode);
//			}
//		}
//
//	}
}
