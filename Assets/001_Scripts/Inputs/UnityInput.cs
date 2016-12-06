using UnityEngine;
using System.Collections;
using UnityEditor;

public class UnityInput : IInput {
	
	//This only works with default unity inputs setting (Edit -> Project Settings -> Inputs)
	#region IInput implementation
	public float GetXMove ()
	{
		if (Input.GetAxis ("Horizontal") != 0) {
			return Input.GetAxis ("Horizontal");
		}

		if (Input.GetMouseButton (0)) {
			return -Input.GetAxis ("Mouse X");
		}

		return 0f;
	}

	public float GetYMove ()
	{
		if (Input.GetAxis ("Vertical") != 0) {
			return Input.GetAxis ("Vertical");
		}

		if (Input.GetMouseButton (0)) {
			return -Input.GetAxis ("Mouse Y");
		}

		return 0f;
	}

	public float GetRotationAngle()
	{
		if (Input.GetMouseButton (2)) {
			return Input.GetAxis ("Mouse X");
		} else {
			return 0f;
		}
	}

	public float GetZoomAmount (){
		return Input.GetAxis ("Mouse ScrollWheel");
	}
	#endregion
}
