using UnityEngine;
using System.Collections;

public static class FloatExtension {
	public static float ReverseAndClamp01(this float value){
		value = Mathf.Clamp (value, 0f, 100f);
		if (value > 1) {
			value = value / 100f;
		}
		value = Mathf.Clamp(1f - value, 0.1f, 1f);
		return value;
	}
}
