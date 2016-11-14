using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Entitas;

public class TowerUpgradeBtn : MonoBehaviour, IGoldChangeListener {

	Button _button;
	float _goldRequire;
	public void RegisterBtn (Node<string> upgrade, Entity e)
	{
		if (_button == null) {
			_button = GetComponent<Button> ();
		}

		var data = DataManager.Instance.GetTowerData (upgrade.data);
		if (data != null) {
			_button.onClick.AddListener (() => {
				if(e != null){
					e.AddTowerUpgrade (data.BuildTime, upgrade);
				}
				Messenger.Broadcast (Events.Input.ENTITY_DESELECT);
			});
			GetComponentInChildren<Text> ().text = "upgrade to " + data.Name + " for " + data.GoldRequired + " gold";

			_goldRequire = data.GoldRequired;
			OnGoldChange (Pools.sharedInstance.pool.goldPlayer.value);
			Messenger.AddListener<int> (Events.Game.GOLD_CHANGE, OnGoldChange);
		} else if(GameManager.debug){
			Debug.Log (upgrade.data + " is null");
		}
	}

	#region IGoldChangeListener implementation

	public void OnGoldChange (int amount)
	{
		if (amount < _goldRequire) {
			_button.interactable = false;
		} else {
			_button.interactable = true;
		}
	}

	#endregion

	void OnDisable(){
		Messenger.RemoveListener<int> (Events.Game.GOLD_CHANGE, OnGoldChange);
		_button.onClick.RemoveAllListeners ();
	}
}