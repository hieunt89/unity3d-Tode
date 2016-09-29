using UnityEngine;
using System.Collections;
using Entitas;
using UnityEngine.UI;
using System.Collections.Generic;
using Lean;

public class TowerUpgradeGUI : MonoBehaviour{
	public GameObject prefab;
	Entity currentSelected = null;

	void OnEnable(){
		Messenger.AddListener (Events.Input.EMPTY_CLICK, HandleEmptyClick);
		Messenger.AddListener <Entity> (Events.Input.ENTITY_CLICK, HandleEntityClick);
		Messenger.AddListener <Node<string>> (Events.Input.TOWER_UPGRADE_BTN_CLICK, CreateTowerUpgradeEntity);
		Messenger.AddListener (Events.Input.TOWER_UI_CLEAR, HandleEmptyClick);
	}

	void OnDisable(){
		Messenger.RemoveListener (Events.Input.EMPTY_CLICK, HandleEmptyClick);
		Messenger.RemoveListener <Entity> (Events.Input.ENTITY_CLICK, HandleEntityClick);
		Messenger.RemoveListener <Node<string>> (Events.Input.TOWER_UPGRADE_BTN_CLICK, CreateTowerUpgradeEntity);
		Messenger.RemoveListener (Events.Input.TOWER_UI_CLEAR, HandleEmptyClick);
	}

	void HandleEntityClick(Entity e){
		if (!e.hasTower && !e.isTowerBase) {
			HandleEmptyClick ();
			return;
		} else if (e == currentSelected) {
			return;
		} else {
			HandleEmptyClick ();
		}

		currentSelected = e;
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
		var data = DataManager.Instance.GetTowerData (towerNode.Data);
		if(data != null){
			var go = LeanPool.Spawn (prefab);
			go.transform.SetParent (this.transform, false);
			go.GetComponent<TowerUpgradeBtn> ().RegisterUpgradeBtn(towerNode, data.GoldRequired);
			go.GetComponentInChildren<Text> ().text = "upgrade to " + towerNode.Data + " for " + data.GoldRequired + " gold";
		}
	}

	void CreateTowerUpgradeEntity (Node<string> upgrade){
		var data = DataManager.Instance.GetTowerData (upgrade.Data);
		if(data != null && currentSelected != null){
			currentSelected.AddTowerUpgrade (data.BuildTime, upgrade);
		}
		Messenger.Broadcast (Events.Input.TOWER_UI_CLEAR);
	}

	void HandleEmptyClick(){
		currentSelected = null;
		for (int i = 0; i < transform.childCount; i++) {
			LeanPool.Despawn (transform.GetChild (i).gameObject);
		}
	}
}
