using UnityEngine;
using System.Collections;
using Lean;

public class InputManager : MonoBehaviour {
	IInput input;
	public float panSpeed = 0.5f;
	public bool useUnityInput = true;
	// Use this for initialization
	void Awake(){
		if (useUnityInput) {
			input = new UnityInput ();
		}else{
			input = new LeanInput ();
		}
	}

	// Update is called once per frame
	void Update () {
		CheckPanInputs ();
	}

	void CheckPanInputs(){
		var x = input.GetXMove();
		var y = input.GetYMove();

		if (x != 0 || y != 0) {
			Messenger.Broadcast<float, float> (Events.Input.PAN_CAM, x * panSpeed, y * panSpeed);
		}
	}
}
