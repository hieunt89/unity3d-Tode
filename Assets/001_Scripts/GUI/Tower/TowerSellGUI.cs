using UnityEngine;
using System.Collections;
using Entitas;

public class TowerSellGUI : MonoBehaviour {


	void Start(){
		Messenger.AddListener (Events.Input.EMPTY_CLICK, ClearTowerSellBtn);
		Messenger.AddListener <Entity> (Events.Input.ENTITY_CLICK, RegisterTowerSellBtn);
	}

	void OnDestroy(){
		Messenger.RemoveListener (Events.Input.EMPTY_CLICK, ClearTowerSellBtn);
		Messenger.RemoveListener <Entity> (Events.Input.ENTITY_CLICK, RegisterTowerSellBtn);
	}

	public void ClearTowerSellBtn(){
		
	}

	public void RegisterTowerSellBtn(Entity e){
		
	}
}
