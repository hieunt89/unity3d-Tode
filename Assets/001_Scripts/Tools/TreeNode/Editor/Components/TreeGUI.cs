using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;

public class TreeGUI : IInjectDataUtils {
	public Tree<string> treeData;
	public List<NodeGUI> nodes;
	public NodeGUI selectedNode;
	private NodeGUI lastNodeUI;
	public NodeGUI startConnectionNode;
	public bool wantsConnection = false;
	public bool showNodeProperties = false;
	public List<string> existIds;

	List<TowerData> towerData;
	List<CombatSkillData> combatSkillData;
	List<SummonSkillData> summonSkillData;

	IDataUtils dataUtils;
	public void SetDataUtils (IDataUtils dataUtils)
	{
		this.dataUtils = dataUtils;
	}

	public TreeGUI (Tree<string> _data) {
		SetDataUtils (DIContainer.GetModule<IDataUtils> ());

		treeData = new Tree<string> (_data.id, _data.treeType, _data.name, _data.Root);
		nodes = new List<NodeGUI> ();

		// TODO: chuyen exist data ra global ?
		// load exist data based on tree type
		switch (_data.treeType) {
		case TreeType.Towers:
			towerData = dataUtils.LoadAllData <TowerData> ();
			if (towerData != null){
				existIds = new List<string> ();
				for (int i = 0; i < towerData.Count; i++) {
					existIds.Add(towerData[i].Id);
				}
			}
			break;
		case TreeType.CombatSkills:
			combatSkillData = dataUtils.LoadAllData <CombatSkillData> ();
			if (combatSkillData != null){
				existIds = new List<string> ();
				for (int i = 0; i < combatSkillData.Count; i++) {
					existIds.Add(combatSkillData[i].id);
				}
			}
			break;
		case TreeType.SummonSkills:
			summonSkillData = dataUtils.LoadAllData <SummonSkillData> ();
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

	public void UpdateTreeUI (Event _e, Rect _rect) {
//		ProcessEvents (_e, _rect);

		if (existIds == null) return;

		if (treeData != null && nodes.Count == 0) {
			GenerateNodes();
		}

		if (nodes.Count > 0) {
			for (int i = 0; i < nodes.Count; i++) {
				nodes [i].UpdateNodeUI (i, _e, _rect);
			}
		}

		if (wantsConnection) {
			if (startConnectionNode != null) {
				DrawConnectionToMouse (_e.mousePosition);
			}
		}

		if (_e.type == EventType.Layout) {
			if (selectedNode != null) {
				showNodeProperties = true;
			}
		}

//		EditorUtility.SetDirty (this);
	}

	private void ProcessEvents (Event _e, Rect _viewRect) {
		
	}

	private void DrawConnectionToMouse (Vector2 _mousePosition) {
		bool isRight = _mousePosition.x >= startConnectionNode.nodeRect.x + startConnectionNode.nodeRect.width * 0.5f;

		var startPos = new Vector3(isRight ? startConnectionNode.nodeRect.x + startConnectionNode.nodeRect.width :  startConnectionNode.nodeRect.x, 
									startConnectionNode.nodeRect.y + startConnectionNode.nodeRect.height * .5f, 
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
		var nodeRect = new Rect(200f, 50f, 100f, 40f);
		NodeGUI rootNode = new NodeGUI (true, this.treeData.Root, nodeRect, null, this);
		if (rootNode != null) {
			lastNodeUI = rootNode;
			GenerateNodes (rootNode);
		}
	}

	private void GenerateNodes (NodeGUI _parentNode) {
		for (int i = 0; i < _parentNode.nodeData.children.Count; i++) {
			var nodeRect = new Rect ((_parentNode.nodeRect.x + _parentNode.nodeRect.width) + _parentNode.nodeRect.width / 2, lastNodeUI.nodeRect.y +(_parentNode.nodeRect.height * 2 * i), 100f, 40f);
			NodeGUI newNode = new NodeGUI (false, _parentNode.nodeData.children[i], nodeRect, _parentNode, this);
			if (newNode != null) {
				_parentNode.childNodes.Add (newNode);
				lastNodeUI = newNode;
				GenerateNodes (newNode);
			}
		}

	}

	void DeselectAllNodes () {
		for (int i = 0; i < nodes.Count; i++) {
			nodes [i].isSelected = false;
		}
	}
}
