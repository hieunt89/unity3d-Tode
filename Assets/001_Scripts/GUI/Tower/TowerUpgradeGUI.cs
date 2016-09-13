using UnityEngine;
using System.Collections;
using Entitas;
using UnityEngine.UI;

public class TowerUpgradeGUI : MonoBehaviour{

	public GameObject btnTowerUpgradePrefab;
	Entity currentSelected = null;

	void Start(){
		Messenger.AddListener (Events.Input.EMPTY_CLICK, ClearTowerUpgradeBtns);
		Messenger.AddListener<Entity> (Events.Input.ENTITY_CLICK, CreateTowerUpgradeBtns);
		Messenger.AddListener<string> (Events.Input.TOWER_UPGRADE_BTN_CLICK, CreateTowerUpgradeEntity);
	}

	void OnDestroy(){
		Messenger.RemoveListener (Events.Input.EMPTY_CLICK, ClearTowerUpgradeBtns);
		Messenger.RemoveListener<Entity> (Events.Input.ENTITY_CLICK, CreateTowerUpgradeBtns);
		Messenger.RemoveListener<string> (Events.Input.TOWER_UPGRADE_BTN_CLICK, CreateTowerUpgradeEntity);
	}

	void CreateTowerUpgradeBtns(Entity e){
		ClearTowerUpgradeBtns ();

		if (e == currentSelected || !e.hasTower) {
			return;
		}

		currentSelected = e;

		var upgrades = e.towerNextUpgrade.upgradeIds;
		if(upgrades == null){
			return;
		}

		for (int i = 0; i < upgrades.Count; i++) {
			var data = DataManager.Instance.GetTowerData (upgrades [i]);
			if(data != null){
				var go = GameObject.Instantiate (btnTowerUpgradePrefab);
				go.transform.SetParent (this.transform);

				go.AddComponent<TowerUpgradeBtn> ().RegisterUpgradeBtn(upgrades[i], data.goldRequired);
				go.GetComponentInChildren<Text> ().text = "upgrade to " + upgrades [i] + " for " + data.goldRequired + " gold";
			}
		}
	}

	void CreateTowerUpgradeEntity (string id){
		var data = DataManager.Instance.GetTowerData (id);
		if(data != null){
			currentSelected.AddTowerUpgrade (data.buildTime, id);
		}
	}

	void ClearTowerUpgradeBtns(){
		currentSelected = null;
		for (int i = 0; i < transform.childCount; i++) {
			GameObject.Destroy (transform.GetChild (i).gameObject);
		}
	}
}
