using UnityEngine;
using System.Collections;

public interface IInput {
	float GetXMove ();
	float GetYMove ();
	float GetRotationAngle ();
	float GetZoomAmount ();
}
