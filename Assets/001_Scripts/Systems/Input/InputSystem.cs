using UnityEngine;
using System.Collections;
using Entitas;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputSystem : IInitializeSystem, IExecuteSystem, ISetPool {
	#region ISetPool implementation
	Entity clickRecorder;
	public void SetPool (Pool pool)
	{
		clickRecorder = pool.CreateEntity ();
	}

	#endregion

	#region IInitializeSystem implementation
	IInput input;

	public void Initialize ()
	{
//		input = new LeanInput ();
		input = new UnityInput ();

		clickRecorder.AddCoroutine (input.StartRecordingClick(CheckClick));
	}

	#endregion

	#region IExecuteSystem implementation
	public void Execute ()
	{
		CheckPanInputs ();
		CheckRotateInputs ();
		CheckZoomInputs ();
	}
	#endregion

	void CheckPanInputs(){
		var x = input.GetXMove();

		if (x != 0) {
			Messenger.Broadcast<float> (Events.Input.PAN_CAM_X, x);
		}

		var y = input.GetYMove();

		if (y != 0) {
			Messenger.Broadcast<float> (Events.Input.PAN_CAM_Y, y);
		}
	}

	void CheckRotateInputs(){
		var angle = input.GetRotationAngle ();

		if (angle != 0) {
			Messenger.Broadcast<float> (Events.Input.ROTATE_CAM, angle);
		}
	}

	void CheckZoomInputs(){
		var amount = input.GetZoomAmount ();

		if (amount != 0) {
			Messenger.Broadcast<float> (Events.Input.ZOOM_CAM, amount);
		}
	}

	void CheckClick(Ray ray){
		RaycastHit hitInfo;
		if (Physics.Raycast (ray, out hitInfo)) {
			Messenger.Broadcast<GameObject> (Events.Input.CLICK_HIT_SOMETHING, hitInfo.collider.gameObject);
			Messenger.Broadcast<Vector3> (Events.Input.CLICK_HIT_POS, hitInfo.point);
		}else{
			Messenger.Broadcast (Events.Input.CLICK_HIT_NOTHING);
		}
	}
}
