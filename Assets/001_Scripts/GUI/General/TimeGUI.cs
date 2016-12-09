using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeGUI : MonoBehaviour {
	Text txtValue;
	void Start(){
		Messenger.AddListener<float> (Events.Game.TIME_CHANGE, OnTimeChange);
		txtValue = GetComponent<Text> ();
	}

	void OnTimeChange(float life){
		txtValue.text = life + "";
	}
}
