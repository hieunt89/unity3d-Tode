using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBarGUI : MonoBehaviour{
	public GameObject sldHealthBarPrefab;

	public Slider CreateHealthBar(){
		GameObject go = GameObject.Instantiate (sldHealthBarPrefab);
		go.transform.SetParent (transform, false);
		return go.GetComponentInParent<Slider>();
	}
}
