using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.EventSystems;

public class UnityInput : IInput {
	
	//This only works with default unity inputs setting (Edit -> Project Settings -> Inputs)
	#region IInput implementation
	public float GetXMove ()
	{
		if (EventSystem.current.IsPointerOverGameObject()) {
			return 0f;
		}

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
		if (EventSystem.current.IsPointerOverGameObject()) {
			return 0f;
		}

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
		if (EventSystem.current.IsPointerOverGameObject()) {
			return 0f;
		}

		if (Input.GetMouseButton (2)) {
			return Input.GetAxis ("Mouse X");
		} else {
			return 0f;
		}
	}

	public float GetZoomAmount (){
		if (EventSystem.current.IsPointerOverGameObject()) {
			return 0f;
		}

		return Input.GetAxis ("Mouse ScrollWheel");
	}


	public IEnumerator StartRecordingClick (System.Action<Ray> callbackRay){
		float timer = Mathf.Infinity;
		while (true) {
			if (Input.GetMouseButtonDown(0)) {
				timer = 0f;
			}

			if (Input.GetMouseButtonUp(0)) {
				if (timer < 0.5f && !EventSystem.current.IsPointerOverGameObject()) {
					callbackRay (Camera.main.ScreenPointToRay(Input.mousePosition));
				}
				timer = Mathf.Infinity;
			}

			timer += Time.deltaTime;
			yield return null;
		}
	}

	#endregion
}
