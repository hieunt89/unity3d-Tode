using UnityEngine;
using System.Collections;
using Entitas;
using Lean;

public class TowerRangeGUI : MonoBehaviour {
	public GameObject rangeObject;
	Entity currentSelected = null;

	void OnEnable(){
		Messenger.AddListener (Events.Input.EMPTY_CLICK, HandleEmptyClick);
		Messenger.AddListener <Entity> (Events.Input.ENTITY_CLICK, HandleEntityClick);
		Messenger.AddListener (Events.Input.TOWER_UI_CLEAR, HandleEmptyClick);
		HandleEmptyClick ();
	}

	void OnDisable(){
		Messenger.RemoveListener (Events.Input.EMPTY_CLICK, HandleEmptyClick);
		Messenger.RemoveListener <Entity> (Events.Input.ENTITY_CLICK, HandleEntityClick);
		Messenger.RemoveListener (Events.Input.TOWER_UI_CLEAR, HandleEmptyClick);
	}

	public void HandleEmptyClick(){
		currentSelected = null;
		rangeObject.SetActive (false);
	}

	public void HandleEntityClick(Entity e){
		if (!e.hasTower) {
			HandleEmptyClick ();
			return;
		} else if (e == currentSelected) {
			return;
		} else {
			HandleEmptyClick ();
		}
	
		currentSelected = e;

		CreateTowerRange (currentSelected);
	}

	void CreateTowerRange(Entity e){
		rangeObject.SetActive (true);
		rangeObject.transform.position = e.position.value;
		rangeObject.transform.localScale = new Vector3 (e.attackRange.value*2, 0, e.attackRange.value*2);
	}
}
