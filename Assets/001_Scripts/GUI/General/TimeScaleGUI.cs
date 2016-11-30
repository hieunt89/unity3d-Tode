using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeScaleGUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Slider> ().onValueChanged.AddListener(OnValueChanged);
	}
	
	// Update is called once per frame
	void OnValueChanged (float value) {
		Time.timeScale = value;
	}
}
