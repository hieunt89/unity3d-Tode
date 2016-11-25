using UnityEngine;
using System.Collections;

public static class FloatExtension {
	public static float ReverseAndClamp01(this float value){
		if (value > 1) {
			value = Mathf.Clamp(100f - value, 1f, 100f) / 100f;
		} else {
			value = Mathf.Clamp(1f - value, 0.1f, 1f);
		}
		return value;
	}
}
