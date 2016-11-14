using UnityEngine;
using System.Collections;
using Entitas;
using Lean;

public class TowerRangeGUI : HandleEnitityGUI {
	public GameObject rangeObject;

	void Start(){
		HandleEmptyClick ();
	}

	public override void HandleEmptyClick(){
		rangeObject.SetActive (false);
	}

	public override void HandleEntityClick(){
		var e = Pools.sharedInstance.pool.currentSelected.e;
		if (!e.hasTower) {
			HandleEmptyClick ();
			return;
		} else {
			HandleEmptyClick ();
		}

		CreateTowerRange (e);
	}

	void CreateTowerRange(Entity e){
		rangeObject.SetActive (true);
		rangeObject.transform.position = e.position.value;
		rangeObject.transform.localScale = new Vector3 (e.attackRange.value*2, 0, e.attackRange.value*2);
	}
}
