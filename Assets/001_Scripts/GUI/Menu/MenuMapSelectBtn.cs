using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuMapSelectBtn : MonoBehaviour {
	Button _button;
	public void RegisterBtn(string mapId, string mapName){
		if (_button == null) {
			_button = GetComponent<Button> ();
		}

		_button.onClick.AddListener (() => {
			UserManager.Instance.userData.SetSelectedMap(mapId);
			Tode.SceneManager.Instance.LoadScene(Scene.game);
		});

		GetComponentInChildren<Text> ().text = mapId + " " + mapName;
	}

	void OnDisable(){
		_button.onClick.RemoveAllListeners ();
	}
}
