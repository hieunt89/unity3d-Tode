using UnityEngine;
using System.Collections;
using Lean;
using System.Collections.Generic;
using Entitas;

public class TowerSkillUpgradeGUI : HandleEnitityGUI {

	public GameObject prefabGroup;
	public GameObject prefab;

	public override void HandleEntityClick ()
	{
		var e = Pools.pool.currentSelected.e;
		if (!e.hasTower) {
			HandleEmptyClick ();
			return;
		} else {
			HandleEmptyClick ();
		}
			
		for (int i = 0; i < e.skillEntityList.skillEntities.Count; i++) {
			var skillEntity = e.skillEntityList.skillEntities [i];
			if (!skillEntity.isActive) {
				CreateTowerSkillUpgradeBtn (this.transform ,skillEntity, skillEntity.skill.skillNode);
			}else if (skillEntity.skill.skillNode.children.Count > 0) {
				var go = LeanPool.Spawn (prefabGroup);
				go.transform.SetParent (this.transform, false);
				for (int j = 0; j < skillEntity.skill.skillNode.children.Count; j++) {
					CreateTowerSkillUpgradeBtn (go.transform ,skillEntity, skillEntity.skill.skillNode.children[j]);
				}
			}
		}
	}

	void CreateTowerSkillUpgradeBtn(Transform parent, Entity skillEntity, Node<string> upgrade){
		var go = LeanPool.Spawn (prefab);
		go.transform.SetParent (parent, false);
		go.GetComponent<TowerSkillUpgradeBtn> ().RegisterBtn(skillEntity, upgrade);
	}

	public override void HandleEmptyClick ()
	{
		for (int i = 0; i < this.transform.childCount; i++) {
			GameObject.Destroy (this.transform.GetChild(i).gameObject);
		}
	}
}
