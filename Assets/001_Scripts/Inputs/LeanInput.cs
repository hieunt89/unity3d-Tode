using UnityEngine;
using System.Collections;
using Lean;

public class LeanInput : IInput {
	float deltaX;
	float deltaY;
	LeanFinger panFg;

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
		if (panFg == null) {
			panFg = fg;
		}
	}

	void OnFingerSet(LeanFinger fg){
		if (fg == panFg) {
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
		if (fg == panFg) {
			deltaX = 0f;
			deltaY = 0f;
			panFg = null;
		}
	}

	#region IInput implementation
	public float GetXMove ()
	{
		return deltaX;
	}

	public float GetYMove ()
	{
		return deltaY;
	}

	public float GetRotationAngle()
	{
		return LeanTouch.TwistDegrees;
	}

	public float GetZoomAmount (){
		return LeanTouch.PinchScale - 1;
	}
	#endregion
}
