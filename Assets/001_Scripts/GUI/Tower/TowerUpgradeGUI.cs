using UnityEngine;
using System.Collections;
using Entitas;
using UnityEngine.UI;
using System.Collections.Generic;

public class TowerUpgradeGUI : MonoBehaviour{

	public GameObject btnTowerUpgradePrefab;
	Entity currentSelected = null;

	void Start(){
		Messenger.AddListener (Events.Input.EMPTY_CLICK, ClearTowerUpgradeBtns);
		Messenger.AddListener <Entity> (Events.Input.ENTITY_CLICK, CreateTowerUpgradeBtns);
		Messenger.AddListener <Node<string>> (Events.Input.TOWER_UPGRADE_BTN_CLICK, CreateTowerUpgradeEntity);
	}

	void OnDestroy(){
		Messenger.RemoveListener (Events.Input.EMPTY_CLICK, ClearTowerUpgradeBtns);
		Messenger.RemoveListener <Entity> (Events.Input.ENTITY_CLICK, CreateTowerUpgradeBtns);
		Messenger.RemoveListener <Node<string>> (Events.Input.TOWER_UPGRADE_BTN_CLICK, CreateTowerUpgradeEntity);
	}

	void CreateTowerUpgradeBtns(Entity e){
		if (!e.hasTower && !e.isTowerBase) {
			ClearTowerUpgradeBtns ();
			return;
		} else if (e == currentSelected) {
			return;
		} else {
			ClearTowerUpgradeBtns ();
		}

		currentSelected = e;
		List<Node<string>> upgrades;

		if (e.isTowerBase) {
			upgrades = DataManager.Instance.GetTowerRoots();
		} else {
			upgrades = e.towerUpgradeCurrentNode.node.Children;
		}

		if(upgrades == null){
			return;
		}

		for (int i = 0; i < upgrades.Count; i++) {
			var data = DataManager.Instance.GetTowerData (upgrades[i].Data);
			if(data != null){
				var go = GameObject.Instantiate (btnTowerUpgradePrefab);
				go.transform.SetParent (this.transform, false);

				go.AddComponent<TowerUpgradeBtn> ().RegisterUpgradeBtn(upgrades[i], data.GoldRequired);
				go.GetComponentInChildren<Text> ().text = "upgrade to " + upgrades[i].Data + " for " + data.GoldRequired + " gold";
			}
		}
	}

	void CreateTowerUpgradeEntity (Node<string> upgrade){
		var data = DataManager.Instance.GetTowerData (upgrade.Data);
		if(data != null){
			currentSelected.AddTowerUpgrade (data.BuildTime, upgrade);
		}
		ClearTowerUpgradeBtns ();
	}

	void ClearTowerUpgradeBtns(){
		currentSelected = null;
		for (int i = 0; i < transform.childCount; i++) {
			GameObject.Destroy (transform.GetChild (i).gameObject);
		}
	}
}
