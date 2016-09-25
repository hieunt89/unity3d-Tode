using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TowerUpgradeBtn : MonoBehaviour {

	Button _button;
	float _goldRequire;

	public void RegisterUpgradeBtn (Node<string> upgrade, float goldRequire)
	{
		_button = GetComponent<Button> ();
		if (_button) {
			_button.onClick.AddListener (() => {
				Messenger.Broadcast<Node<string>> (Events.Input.TOWER_UPGRADE_BTN_CLICK, upgrade);
			});
			_goldRequire = goldRequire;
			HandleGoldChange (Pools.pool.goldPlayer.value);
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

	void OnDestroy(){
		Messenger.RemoveListener<int> (Events.Game.GOLD_CHANGE, HandleGoldChange);
	}
}
