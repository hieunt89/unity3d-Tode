using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Entitas;

public class TowerSkillUpgradeBtn : MonoBehaviour, IGoldChangeListener {
	Button _button;
	float _goldRequire;

	public void RegisterBtn (Entity skillEntity, Node<string> upgrade)
	{
		if (_button == null) {
			_button = GetComponent<Button> ();
		}

		var data = DataManager.Instance.GetSkillData (upgrade.data);
		if (data != null) {
			_button.onClick.AddListener (() => {
				//Todo add skill upgrade
				skillEntity.AddSkillUpgrade(upgrade);
			});
			GetComponentInChildren<Text> ().text = "upgrade to skill " + data.name + " for " + data.goldCost + " gold";

			_goldRequire = data.goldCost;
			OnGoldChange (Pools.sharedInstance.pool.goldPlayer.value);
			Messenger.AddListener<int> (Events.Game.GOLD_CHANGE, OnGoldChange);
		} else if(GameManager.ShowDebug){
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
