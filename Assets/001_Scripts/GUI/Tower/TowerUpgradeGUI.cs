using UnityEngine;
using System.Collections;
using Entitas;
using UnityEngine.UI;
using System.Collections.Generic;
using Lean;

public class TowerUpgradeGUI : MonoBehaviour{
	public GameObject prefab;

	void OnEnable(){
		Messenger.AddListener (Events.Input.EMPTY_SELECT, HandleEmptyClick);
		Messenger.AddListener (Events.Input.ENTITY_SELECT, HandleEntityClick);
		Messenger.AddListener (Events.Input.ENTITY_DESELECT, HandleEmptyClick);
	}

	void OnDisable(){
		Messenger.RemoveListener (Events.Input.EMPTY_SELECT, HandleEmptyClick);
		Messenger.RemoveListener (Events.Input.ENTITY_SELECT, HandleEntityClick);
		Messenger.RemoveListener (Events.Input.ENTITY_DESELECT, HandleEmptyClick);
	}

	void HandleEntityClick(){
		var e = Pools.pool.currentSelected.e;
		if (!e.hasTower && !e.isTowerBase) {
			HandleEmptyClick ();
			return;
		} else {
			HandleEmptyClick ();
		}

		List<Node<string>> upgrades;

		if (e.isTowerBase) {
			upgrades = DataManager.Instance.GetTowerRoots();
		} else {
			upgrades = e.tower.currentNode.Children;
		}

		if(upgrades == null){
			return;
		}

		for (int i = 0; i < upgrades.Count; i++) {
			CreateTowerUpgradeBtn (upgrades [i]);
		}
	}

	void CreateTowerUpgradeBtn(Node<string> towerNode){
		var go = LeanPool.Spawn (prefab);
		go.transform.SetParent (this.transform, false);
		go.GetComponent<TowerUpgradeBtn> ().RegisterUpgradeBtn(towerNode);
	}

	void HandleEmptyClick(){
		for (int i = 0; i < this.transform.childCount; i++) {
			LeanPool.Despawn (this.transform.GetChild (i).gameObject, 0.001f);
		}
	}
}
