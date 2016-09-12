using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TowerUpgradeBtn : MonoBehaviour {

	Button _button;
	float _goldRequire;

	public void RegisterUpgradeBtn (string id, float goldRequire)
	{
		_button = GetComponent<Button> ();
		if (_button) {
			_button.onClick.AddListener (() => {
				Messenger.Broadcast<string> (Events.Input.TOWER_UPGRADE_BTN_CLICK, id);
			});
			_goldRequire = goldRequire;
			Messenger.AddListener<int> (Events.Game.GOLD_CHANGE, HandleGoldChange);
		}
	}

	void HandleGoldChange(int gold){
		if (gold < _goldRequire) {
			_button.interactable = false;
		} else {
			_button.interactable = true;
		}
	}
}
