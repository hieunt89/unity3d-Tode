using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;

public interface IGenericTree {
	void CreateTree (Type _type);
	void UpdateTreeUI (Event _e, Rect _viewRect, GUISkin _viewSkin);
}

public class GenericTree <T> : IGenericTree {

	public Tree <T> treeData;
	public List<NodeUI> nodes;
	private NodeUI lastNodeUI;

	public bool showNodeProperties = false;
	public bool wantsConnection = false;

//	public GenericTree ()
//	{
//		this.treeData = new Tree <T> ();
//	}
	
	public void CreateTree (Type _type)
	{
		this.treeData = new Tree <T> ();
		Debug.Log ("Create tree");
	}

	public void UpdateTreeUI (Event _e, Rect _viewRect, GUISkin _viewSkin)
	{
//		Debug.Log ("Update Tree UI");
		ProcessEvents (_e, _viewRect);

//		if (treeData != null && nodes.Count == 0) {
//			GenerateNodes(this);
//		}
//
//		if (nodes.Count > 0) {
//			for (int i = 0; i < nodes.Count; i++) {
//				nodes [i].UpdateNodeUI (i, _e, _viewRect, _viewSkin);
//			}
//		}
	}

	private void ProcessEvents (Event _e, Rect _viewRect) {
		if (_viewRect.Contains (_e.mousePosition)) {
			if (_e.button == 0) {
				if (_e.type == EventType.MouseDown) {
					showNodeProperties = false;

					if (wantsConnection) {
						wantsConnection = false;
					}
				}
			}
		}
	}

	private void GenerateNodes (TreeUI _currentTree) {
		NodeUI rootNode = new NodeUI ("Root Node", NodeType.RootNode, _currentTree.treeData.Root, null, new List<NodeUI> (), _currentTree);

		if (rootNode != null) {
			rootNode.InitNode (new Vector2 (50f, 50f));
			_currentTree.nodes.Add (rootNode);
			lastNodeUI = rootNode;
			GenerateNodes (_currentTree, rootNode);
		}

	}

	private void GenerateNodes (TreeUI _currentTree, NodeUI _parentNode) {
		for (int i = 0; i < _parentNode.nodeData.children.Count; i++) {
			NodeUI newNode = new NodeUI ("Node", NodeType.Node, _parentNode.nodeData.children[i], _parentNode, new List<NodeUI> (), _currentTree);
			if (newNode != null) {
				_parentNode.childNodes.Add (newNode);
				newNode.InitNode (new Vector2 ((_parentNode.nodeRect.x + _parentNode.nodeRect.width) + _parentNode.nodeRect.width / 2, lastNodeUI.nodeRect.y +(_parentNode.nodeRect.height * 2 * i)));
				_currentTree.nodes.Add (newNode);
				lastNodeUI = newNode;
				GenerateNodes (_currentTree, newNode);
			}
		}

	}
}

public class GenericTreeUI {

	object genericTree = null;

	Type _type = null;

	public Type Type {
		get { return _type.GetGenericArguments()[0]; }
		set {
			_type = typeof(GenericTree <>).MakeGenericType(value);

			genericTree = Activator.CreateInstance(_type);

		}
	}

	public void UpdateTreeUI (Event _e, Rect _viewRect, GUISkin _viewSkin) {
		if (genericTree == null) {
			if (GUILayout.Button("TowerData")) {
//				Type = typeof(TowerData);
				_type = typeof(GenericTree <>).MakeGenericType(typeof(TowerData));
				genericTree = Activator.CreateInstance(_type);
				(genericTree as IGenericTree).CreateTree (Type);
			}
			return;
		}
		(genericTree as IGenericTree).UpdateTreeUI (_e, _viewRect, _viewSkin);
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
	public TreeUI (TreeType _treeType, string _treeName) {
		treeData = new Tree<string> (_treeType, _treeName, null);

		if (nodes == null) {
			nodes = new List<NodeUI> ();
		}

		// TODO: chuyen exist data ra global ?
		// load exist data based on tree type
		switch (_treeType) {
		case TreeType.Towers:
//			Type x = typeof(TowerData);
//			Type listType = typeof(List<>).MakeGenericType(x);
//			var existData = Activator.CreateInstance(listType);

//			var fooList = Activator
//				.CreateInstance(typeof(List<>)
//					.MakeGenericType(TowerData.GetType()));
			
			towerData = DataManager.Instance.LoadAllData <TowerData> ();
			if (towerData != null){
				existIds = new List<string> ();
				for (int i = 0; i < towerData.Count; i++) {
					existIds.Add(towerData[i].Id);
				}
			}
			break;
		case TreeType.CombatSkills:
			combatSkillData = DataManager.Instance.LoadAllData <CombatSkillData> ();
			if (combatSkillData != null){
				existIds = new List<string> ();
				for (int i = 0; i < combatSkillData.Count; i++) {
					existIds.Add(combatSkillData[i].id);
				}
			}
			break;
		case TreeType.SummonSkills:
			summonSkillData = DataManager.Instance.LoadAllData <SummonSkillData> ();
			if (summonSkillData != null){
				existIds = new List<string> ();
				for (int i = 0; i < summonSkillData.Count; i++) {
					existIds.Add(summonSkillData[i].id);
				}
			}
			break;
		default:
			break;
		}
	}

	public void UpdateTreeUI (Event _e, Rect _viewRect, GUISkin _viewSkin) {
		ProcessEvents (_e, _viewRect);
		
		if (treeData != null && nodes.Count == 0) {
			GenerateNodes(this);
		}

		if (nodes.Count > 0) {
			for (int i = 0; i < nodes.Count; i++) {
				nodes [i].UpdateNodeUI (i, _e, _viewRect, _viewSkin);
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
				showNodeProperties = true;
			}
		}

//		EditorUtility.SetDirty (this);
	}

	private void ProcessEvents (Event _e, Rect _viewRect) {
		if (_viewRect.Contains (_e.mousePosition)) {
			if (_e.button == 0) {
				if (_e.type == EventType.MouseDown) {
//					DeselectAllNodes ();
//
					showNodeProperties = false;
//					bool setNode = false;
//					selectedNode = null;
//
//					for (int i = 0; i < nodes.Count; i++) {
//						if (nodes[i].nodeRect.Contains (_e.mousePosition)) {
//							nodes[i].isSelected = true;
//							selectedNode = nodes[i];
//							setNode = true;
//						}
//					}
//
//					if (!setNode) {
//						DeselectAllNodes ();
//					}

					if (wantsConnection) {
						wantsConnection = false;
					}
				}
			}
		}
	}

	private void DrawConnectionToMouse (Vector2 _mousePosition) {
		bool isRight = _mousePosition.x >= startConnectionNode.nodeRect.x + startConnectionNode.nodeRect.width * 0.5f;

		var startPos = new Vector3(isRight ? startConnectionNode.nodeRect.x + startConnectionNode.nodeRect.width :  startConnectionNode.nodeRect.x, 
			startConnectionNode.nodeRect.y + startConnectionNode.nodeRect.height +  startConnectionNode.nodeContentRect .height * .5f, 
									0);
		var endPos = new Vector3(_mousePosition.x, _mousePosition.y, 0);

		float mnog = Vector3.Distance(startPos,endPos);
		Vector3 startTangent = startPos + (isRight ? Vector3.right : Vector3.left) * (mnog / 3f) ;
		Vector3 endTangent = endPos + (isRight ? Vector3.left : Vector3.right) * (mnog / 3f);

		Handles.BeginGUI ();
		Handles.DrawBezier(startPos, endPos, startTangent, endTangent,Color.white, null, 2f);
		Handles.EndGUI ();
	}

//	private void DeselectAllNodes () {
//		for (int i = 0; i < nodes.Count; i++) {
//			nodes [i].isSelected = false;
//		}
//	}

	private void GenerateNodes (TreeUI _currentTree) {
		NodeUI rootNode = new NodeUI ("Root Node", NodeType.RootNode, _currentTree.treeData.Root, null, new List<NodeUI> (), _currentTree);

		if (rootNode != null) {
			rootNode.InitNode (new Vector2 (50f, 50f));
			_currentTree.nodes.Add (rootNode);
			lastNodeUI = rootNode;
			GenerateNodes (_currentTree, rootNode);
		}

	}

	private void GenerateNodes (TreeUI _currentTree, NodeUI _parentNode) {
		for (int i = 0; i < _parentNode.nodeData.children.Count; i++) {
			NodeUI newNode = new NodeUI ("Node", NodeType.Node, _parentNode.nodeData.children[i], _parentNode, new List<NodeUI> (), _currentTree);
			if (newNode != null) {
				_parentNode.childNodes.Add (newNode);
				newNode.InitNode (new Vector2 ((_parentNode.nodeRect.x + _parentNode.nodeRect.width) + _parentNode.nodeRect.width / 2, lastNodeUI.nodeRect.y +(_parentNode.nodeRect.height * 2 * i)));
				_currentTree.nodes.Add (newNode);
				lastNodeUI = newNode;
				GenerateNodes (_currentTree, newNode);
			}
		}

	}
}
