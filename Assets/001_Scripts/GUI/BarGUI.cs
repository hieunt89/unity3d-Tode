using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarGUI : MonoBehaviour{
	public GameObject sldHealthBarPrefab;
	public GameObject sldProgressBarPrefab;

	public Slider CreateHealthBar(){
		return CreateBar (sldHealthBarPrefab);
	}
		
	public Slider CreateProgressBar(){
		return CreateBar (sldProgressBarPrefab);
	}

	public Slider CreateBar(GameObject pref){
		GameObject go = Lean.LeanPool.Spawn (pref);
		go.transform.SetParent (transform, false);
		return go.GetComponentInParent<Slider>();
	}
}