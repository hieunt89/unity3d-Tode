using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GoldGUI : MonoBehaviour {
	Text txtValue;
	void Start(){
		Messenger.AddListener<int> (Events.Game.GOLD_CHANGE, OnGoldChange);
		txtValue = GetComponent<Text> ();
	}

	void OnGoldChange(int gold){
		txtValue.text = gold + "";
	}
}
