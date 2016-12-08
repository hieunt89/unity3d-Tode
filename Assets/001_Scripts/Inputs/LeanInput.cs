using UnityEngine;
using System.Collections;
using Lean;

public class LeanInput : IInput {
	float deltaX;
	float deltaY;
	LeanFinger firstFg;
	LeanFinger tapFg;

	public LeanInput(){
		LeanTouch.OnFingerDown += OnFingerDown;
		LeanTouch.OnFingerSet += OnFingerSet;
		LeanTouch.OnFingerUp += OnFingerUp;
	}

	~LeanInput(){
		LeanTouch.OnFingerDown -= OnFingerDown;
		LeanTouch.OnFingerSet -= OnFingerSet;
		LeanTouch.OnFingerUp -= OnFingerUp;
	}

	void OnFingerDown(LeanFinger fg){
		if (firstFg == null) {
			firstFg = fg;
		}
	}

	void OnFingerSet(LeanFinger fg){
		if (fg == firstFg) {
			if (LeanTouch.GuiInUse || LeanTouch.Fingers.Count > 1) {
				deltaX = 0f;
				deltaY = 0f;
				return;
			}
			deltaX = -fg.DeltaScreenPosition.x;
			deltaY = -fg.DeltaScreenPosition.y;
		}
	}

	void OnFingerUp(LeanFinger fg){
		if (fg == firstFg) {
			if (firstFg.Tap) {
				tapFg = firstFg;
			}

			deltaX = 0f;
			deltaY = 0f;
			firstFg = null;
		}
	}

	#region IInput implementation
	public float GetXMove ()
	{
		return deltaX/10;
	}

	public float GetYMove ()
	{
		return deltaY/10;
	}

	public float GetRotationAngle()
	{
		return LeanTouch.TwistDegrees;
	}

	public float GetZoomAmount (){
		return LeanTouch.PinchScale - 1;
	}

	public IEnumerator StartRecordingClick (System.Action<Ray> callbackRay){
		while(true){
			if (tapFg != null) {
				if (!LeanTouch.GuiInUse) {
					callbackRay (tapFg.GetRay());
				}
				tapFg = null;
			}
			yield return null;
		}
	}
	#endregion
}
