using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Entitas;

public class TowerSkillUpgradeBtn : MonoBehaviour {
	Button _button;
	float _goldRequire;

	public void RegisterBtn (Entity skillEntity, Node<string> upgrade)
	{
		if (_button == null) {
			_button = GetComponent<Button> ();
		}

		var data = DataManager.Instance.GetTowerData (upgrade.data);
		if (data != null) {
			_button.onClick.AddListener (() => {
				//Todo add skill upgrade

				Messenger.Broadcast (Events.Input.ENTITY_DESELECT);
			});
			GetComponentInChildren<Text> ().text = "upgrade to " + data.Name + " for " + data.GoldRequired + " gold";

			_goldRequire = data.GoldRequired;
			HandleGoldChange (Pools.pool.goldPlayer.value);
			Messenger.AddListener<int> (Events.Game.GOLD_CHANGE, HandleGoldChange);
		} else if(GameManager.debug){
			Debug.Log (upgrade.data + " is null");
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
