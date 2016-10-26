using UnityEngine;
using System.Collections;
using Entitas;
using UnityEngine.UI;
using Lean;

public class TowerSellGUI : HandleEnitityGUI {
	public GameObject prefab;

	public override void HandleEntityClick(){
		var e = Pools.pool.currentSelected.e;
		if (!e.hasTower) {
			HandleEmptyClick ();
			return;
		} else {
			HandleEmptyClick ();
		}

		var go = LeanPool.Spawn (prefab);
		go.transform.SetParent (this.transform, false);
		go.GetComponent<TowerSellBtn> ().RegisterSellBtn (e);
	}

	public override void HandleEmptyClick(){
		for (int i = 0; i < transform.childCount; i++) {
			LeanPool.Despawn (transform.GetChild (i).gameObject);
		}
	}
}
