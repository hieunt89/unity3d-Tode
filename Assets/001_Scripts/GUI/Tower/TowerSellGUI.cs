using UnityEngine;
using System.Collections;
using Entitas;
using UnityEngine.UI;
using Lean;

public class TowerSellGUI : MonoBehaviour {
	public GameObject prefab;

	void OnEnable(){
		Messenger.AddListener (Events.Input.EMPTY_SELECT, HandleEmptyClick);
		Messenger.AddListener (Events.Input.ENTITY_SELECT, HandleEntityClick);
		Messenger.AddListener (Events.Input.ENTITY_DESELECT, HandleEmptyClick);
	}

	void OnDisable(){
		Messenger.RemoveListener (Events.Input.EMPTY_SELECT, HandleEmptyClick);
		Messenger.RemoveListener (Events.Input.ENTITY_SELECT, HandleEntityClick);
		Messenger.RemoveListener (Events.Input.ENTITY_DESELECT, HandleEmptyClick);
	}

	void HandleEntityClick(){
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

	void HandleEmptyClick(){
		for (int i = 0; i < transform.childCount; i++) {
			LeanPool.Despawn (transform.GetChild (i).gameObject);
		}
	}
}
