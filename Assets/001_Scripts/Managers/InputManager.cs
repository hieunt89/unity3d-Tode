using UnityEngine;
using System.Collections;
using Lean;

public class InputManager : MonoBehaviour {
	IInput input;
	public float panSpeed = 0.5f;
	public float rotateSpeed = 4f;
	public float zoomSpeed = 2f;
	public bool useLeanInput = true;
	// Use this for initialization
	void Awake(){
		if (useLeanInput) {
			input = new LeanInput ();
		}else{
			input = new UnityInput ();
		}
	}

	// Update is called once per frame
	void Update () {
		CheckPanInputs ();
		CheckRotateInputs ();
		CheckZoomInputs ();
	}

	void CheckPanInputs(){
		var x = input.GetXMove();

		if (x != 0) {
			Messenger.Broadcast<float> (Events.Input.PAN_CAM_X, x * panSpeed);
		}

		var y = input.GetYMove();

		if (y != 0) {
			Messenger.Broadcast<float> (Events.Input.PAN_CAM_Y, y * panSpeed);
		}
	}

	void CheckRotateInputs(){
		var angle = input.GetRotationAngle ();

		if (angle != 0) {
			Messenger.Broadcast<float> (Events.Input.ROTATE_CAM, angle * rotateSpeed);
		}
	}

	void CheckZoomInputs(){
		var amount = input.GetZoomAmount ();

		if (amount != 0) {
			Messenger.Broadcast<float> (Events.Input.ZOOM_CAM, amount * zoomSpeed);
		}
	}
}
