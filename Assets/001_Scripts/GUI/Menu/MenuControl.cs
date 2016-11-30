using UnityEngine;
using System.Collections;

public class MenuControl : MonoBehaviour {
	public CanvasGroup pnlMaps;

	void Awake(){
		Messenger.AddListener (Events.Loading.DONE_INIT, OnInitDone);
	}

	void OnDestroy(){
		Messenger.RemoveListener (Events.Loading.DONE_INIT, OnInitDone);
	}

	public void ToggleMapsPnl(bool state){
		if (state) {
			pnlMaps.alpha = 1f;
			pnlMaps.interactable = true;
			pnlMaps.blocksRaycasts = true;
		} else {
			pnlMaps.alpha = 0f;
			pnlMaps.interactable = false;
			pnlMaps.blocksRaycasts = false;
		}
	}

	void OnInitDone(){
		ToggleMapsPnl (false);
	}
}
