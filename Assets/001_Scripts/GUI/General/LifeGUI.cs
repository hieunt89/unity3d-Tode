using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LifeGUI : MonoBehaviour {
	Text txtValue;
	void Start(){
		Messenger.AddListener<int> (Events.Game.LIFE_CHANGE, OnLifeChange);
		txtValue = GetComponent<Text> ();
	}

	void OnLifeChange(int life){
		txtValue.text = life + "";
	}
}
