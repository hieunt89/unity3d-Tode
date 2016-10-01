using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TowerUpgradeBtn : MonoBehaviour {

	Button _button;
	float _goldRequire;

	public void RegisterUpgradeBtn (Node<string> upgrade)
	{
		if (_button == null) {
			_button = GetComponent<Button> ();
		}

		var data = DataManager.Instance.GetTowerData (upgrade.Data);
		if (data != null) {
			_button.onClick.AddListener (() => {
				var e = Pools.pool.currentSelected.e;
				if(e != null){
					e.AddTowerUpgrade (data.BuildTime, upgrade);
				}
				Messenger.Broadcast (Events.Input.ENTITY_DESELECT);
			});
			GetComponentInChildren<Text> ().text = "upgrade to " + data.Name + " for " + data.GoldRequired + " gold";

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

	void OnDisable(){
		Messenger.RemoveListener<int> (Events.Game.GOLD_CHANGE, HandleGoldChange);
		_button.onClick.RemoveAllListeners ();
	}
}