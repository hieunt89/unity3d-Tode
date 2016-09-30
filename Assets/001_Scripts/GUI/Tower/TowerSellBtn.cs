using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Entitas;

public class TowerSellBtn : MonoBehaviour {
	Button _button;

	public void RegisterSellBtn(Entity e){
		GetComponentInChildren<Text> ().text = "Sell tower for " + e.gold.value + " gold";
	}

	void OnEnable(){
		_button = GetComponent<Button> ();
		if(_button){
			_button.onClick.AddListener (() => {
				Messenger.Broadcast (Events.Input.TOWER_SELL_BTN_CLICK);
			});
		}
	}

	void OnDisable(){
		if(_button){
			_button.onClick.RemoveAllListeners ();
		}
	}
}
