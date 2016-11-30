using UnityEngine;
using System.Collections;
using Lean;

public class MenuMapGUI : MonoBehaviour {
	public GameObject prefab;
	void Awake(){
		Messenger.AddListener (Events.Loading.DONE_INIT, OnInitDone);
	}

	void OnDestroy(){
		Messenger.RemoveListener (Events.Loading.DONE_INIT, OnInitDone);
	}

	void OnInitDone(){
		var maps = DataManager.Instance.GetMapsData ();

		foreach (var item in maps) {
			CreateMenuMapSelectBtn (item.Value);
		}
	}

	void CreateMenuMapSelectBtn(MapData map){
		var go = LeanPool.Spawn (prefab);
		go.transform.SetParent (this.transform, false);
		go.GetComponent<MenuMapSelectBtn> ().RegisterBtn(map.Id, "");
	}
}
