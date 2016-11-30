using UnityEngine;
using System.Collections;
using Entitas;
using Lean;

public class TowerRangeGUI : HandleTowerSelectGUI {
	public GameObject rangeObject;

	public override void HandleEmptyClick(){
		rangeObject.SetActive (false);
	}

	public override void HandleTowerClick(Entity e){
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
