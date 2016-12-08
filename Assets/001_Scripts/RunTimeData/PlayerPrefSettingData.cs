using UnityEngine;
using System.Collections;

public class PlayerPrefSettingData : ISettingData {
	public float GetMouseSensitivity(){
		if (PlayerPrefs.HasKey (ConstantString.PPMouseSensitivity)) {
			return PlayerPrefs.GetFloat (ConstantString.PPMouseSensitivity);
		} else {
			return ConstantData.DEFAULT_MOUSE_SENSITIVITY;
		}
	}

	public void SetMouseSensitivity(float value){
		PlayerPrefs.SetFloat (ConstantString.PPMouseSensitivity, value);
	}
}
