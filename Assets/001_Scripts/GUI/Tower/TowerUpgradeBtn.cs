using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TowerUpgradeBtn : MonoBehaviour {

	Button _button;
	Text _text;
	float _goldRequire;
	Node<string> _upgrade;

	public void RegisterUpgradeBtn (Node<string> upgrade)
	{
		if (_button == null) {
			_button = GetComponent<Button> ();
			_text = GetComponentInChildren<Text> ();
		}
		_button.onClick.RemoveAllListeners ();

		_upgrade = upgrade;
		var data = DataManager.Instance.GetTowerData (upgrade.Data);
		if (data != null) {
			_button.onClick.AddListener (() => {
				Messenger.Broadcast<TowerUpgradeBtn>(Events.Input.TOWER_UPGRADE_BTN_RESET, this);
				RegisterUpgradeBtnConfirm (upgrade);
			});
			_text.text = "upgrade to " + data.Name + " for " + data.GoldRequired + " gold";

			_goldRequire = data.GoldRequired;
			HandleGoldChange (Pools.pool.goldPlayer.value);
			Messenger.AddListener<int> (Events.Game.GOLD_CHANGE, HandleGoldChange);
		} else if(GameManager.debug){
			Debug.Log (upgrade.Data + " is null");
		}
	}

	void HandleGoldChange(int gold){
		if (gold < _goldRequire) {
			_button.interactable = false;
		} else {
			_button.interactable = true;
		}
	}

	void OnEnable(){
		Messenger.AddListener<TowerUpgradeBtn> (Events.Input.TOWER_UPGRADE_BTN_RESET, ResetUpgradeBtn);
	}

	void OnDisable(){
		Messenger.RemoveListener<int> (Events.Game.GOLD_CHANGE, HandleGoldChange);
		Messenger.RemoveListener<TowerUpgradeBtn> (Events.Input.TOWER_UPGRADE_BTN_RESET, ResetUpgradeBtn);
		_button.onClick.RemoveAllListeners ();
	}

	void RegisterUpgradeBtnConfirm(Node<string> upgrade){
		_button.onClick.RemoveAllListeners ();
		_text.text = "Confirm?";
		_button.onClick.AddListener (() => {
			Messenger.Broadcast<Node<string>> (Events.Input.TOWER_UPGRADE_BTN_CLICK, upgrade);
		});
	}

	void ResetUpgradeBtn(TowerUpgradeBtn btn){
		if (btn != this) {
			RegisterUpgradeBtn (_upgrade);
		}
	}
}