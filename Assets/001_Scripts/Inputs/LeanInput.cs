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
			if (LeanTouch.GuiInUse) {
				deltaX = 0f;
				deltaY = 0f;
				return;
			}

			if (Mathf.Abs (fg.DeltaScreenPosition.x) > 1f) {
				deltaX = -fg.DeltaScreenPosition.x;
			} else {
				deltaX = 0f;
			}

			if (Mathf.Abs (fg.DeltaScreenPosition.y) > 1f) {
				deltaY = -fg.DeltaScreenPosition.y;
			} else {
				deltaY = 0f;
			}
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
		return deltaX/10;
	}

	public float GetYMove ()
	{
		return deltaY/10;
	}
	#endregion
}
