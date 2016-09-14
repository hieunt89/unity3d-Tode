using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProgressBarGUI : MonoBehaviour {
	public GameObject sldProgressBarPrefab;

	public Slider CreateProgressBar(){
		GameObject go = GameObject.Instantiate (sldProgressBarPrefab);
		go.transform.SetParent (transform, false);
		return go.GetComponentInParent<Slider>();
	}
}
