using UnityEngine;
using System.Collections;
using Entitas;
using UnityEngine.UI;
using System.Collections.Generic;
using Lean;

public class TowerUpgradeGUI : HandleEnitityGUI{
	public GameObject prefab;

	public override void HandleEntityClick(){
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
			upgrades = e.tower.towerNode.children;
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
		go.GetComponent<TowerUpgradeBtn> ().RegisterBtn(towerNode);
	}

	public override void HandleEmptyClick(){
		for (int i = 0; i < this.transform.childCount; i++) {
			LeanPool.Despawn (this.transform.GetChild (i).gameObject, 0.001f);
		}
	}
}
