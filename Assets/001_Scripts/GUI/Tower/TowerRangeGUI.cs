using UnityEngine;
using System.Collections;
using Entitas;
using Lean;

public class TowerRangeGUI : MonoBehaviour {
	public GameObject rangeObject;

	void OnEnable(){
		Messenger.AddListener (Events.Input.EMPTY_SELECT, HandleEmptyClick);
		Messenger.AddListener (Events.Input.ENTITY_SELECT, HandleEntityClick);
		Messenger.AddListener (Events.Input.ENTITY_DESELECT, HandleEmptyClick);
		HandleEmptyClick ();
	}

	void OnDisable(){
		Messenger.RemoveListener (Events.Input.EMPTY_SELECT, HandleEmptyClick);
		Messenger.RemoveListener (Events.Input.ENTITY_SELECT, HandleEntityClick);
		Messenger.RemoveListener (Events.Input.ENTITY_DESELECT, HandleEmptyClick);
	}

	public void HandleEmptyClick(){
		rangeObject.SetActive (false);
	}

	public void HandleEntityClick(){
		var e = Pools.pool.currentSelected.e;
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
