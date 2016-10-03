using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Entitas;

public class TowerSellBtn : MonoBehaviour {
	Button _button;

	public void RegisterSellBtn(Entity e){
		_button = GetComponent<Button> ();
		if(_button){
			_button.onClick.AddListener (() => {
				e.IsMarkedForSell (true);
				Messenger.Broadcast (Events.Input.ENTITY_DESELECT);
			});
		}
		GetComponentInChildren<Text> ().text = "Sell tower for " + e.gold.value + " gold";
	}

	void OnDisable(){
		if(_button){
			_button.onClick.RemoveAllListeners ();
		}
	}
}
