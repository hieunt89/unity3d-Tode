using UnityEngine;
using System.Collections;
using Entitas;
using UnityEngine.UI;
using Lean;

public class TowerSellGUI : MonoBehaviour {
	public GameObject prefab;
	Entity currentSelected = null;

	void OnEnable(){
		Messenger.AddListener (Events.Input.EMPTY_CLICK, HandleEmptyClick);
		Messenger.AddListener <Entity> (Events.Input.ENTITY_CLICK, HandleEntityClick);
		Messenger.AddListener (Events.Input.TOWER_SELL_BTN_CLICK, SellTower);
		Messenger.AddListener (Events.Input.TOWER_UI_CLEAR, HandleEmptyClick);
	}

	void OnDisable(){
		Messenger.RemoveListener (Events.Input.EMPTY_CLICK, HandleEmptyClick);
		Messenger.RemoveListener <Entity> (Events.Input.ENTITY_CLICK, HandleEntityClick);
		Messenger.RemoveListener (Events.Input.TOWER_SELL_BTN_CLICK, SellTower);
		Messenger.RemoveListener (Events.Input.TOWER_UI_CLEAR, HandleEmptyClick);
	}

	void HandleEntityClick(Entity e){
		if (!e.hasTower) {
			HandleEmptyClick ();
			return;
		} else if (e == currentSelected) {
			return;
		} else {
			HandleEmptyClick ();
		}

		currentSelected = e;

		var go = LeanPool.Spawn (prefab);
		go.transform.SetParent (this.transform, false);
		go.GetComponent<TowerSellBtn> ().RegisterSellBtn (e);
	}

	void SellTower(){
		if(currentSelected != null){
			currentSelected.IsMarkedForSell (true);
		}
		Messenger.Broadcast (Events.Input.TOWER_UI_CLEAR);
	}

	void HandleEmptyClick(){
		currentSelected = null;
		for (int i = 0; i < transform.childCount; i++) {
			LeanPool.Despawn (transform.GetChild (i).gameObject);
		}
	}
}
