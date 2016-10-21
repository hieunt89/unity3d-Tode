using UnityEngine;
using System.Collections;
using Lean;
using System.Collections.Generic;
using Entitas;

public class TowerSkillUpgradeGUI : HandleEnitityGUI {

	public GameObject prefab;

	public override void HandleEmptyClick ()
	{
		var e = Pools.pool.currentSelected.e;
		if (!e.hasTower) {
			HandleEmptyClick ();
			return;
		} else {
			HandleEmptyClick ();
		}

		List<Node<string>> skills;

		for (int i = 0; i < e.skillEntityList.skillEntities.Count; i++) {
			var skillEntity = e.skillEntityList.skillEntities [i];
			if (!skillEntity.isActive) {
				CreateTowerSkillUpgradeBtn (skillEntity);
			}
		}
	}

	void CreateTowerSkillUpgradeBtn(Entity skillEntity){
		var go = LeanPool.Spawn (prefab);
		go.transform.SetParent (this.transform, false);
//		go.GetComponent<TowerSkillUpgradeBtn> ().RegisterBtn(towerNode);
	}

	public override void HandleEntityClick ()
	{
		for (int i = 0; i < this.transform.childCount; i++) {
			LeanPool.Despawn (this.transform.GetChild (i).gameObject, 0.001f);
		}
	}
}
