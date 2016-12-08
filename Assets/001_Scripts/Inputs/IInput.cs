using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public interface IInput {
	float GetXMove ();
	float GetYMove ();
	float GetRotationAngle ();
	float GetZoomAmount ();

	IEnumerator StartRecordingClick (System.Action<Ray> callbackRay);
}
