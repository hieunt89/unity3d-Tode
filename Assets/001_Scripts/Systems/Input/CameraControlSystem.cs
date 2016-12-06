using UnityEngine;
using System.Collections;
using Entitas;
using Lean;

public class CameraControlSystem : IInitializeSystem, ITearDownSystem, ISetPool {
	Transform camera;
	Entity mover;
	float maxPanStep = 0.5f;
	float maxRotateStep = 5f;
	float camZoomStep = 2f;
	float maxCamHeightScale = 2.5f;
	float minCamHeightScale = 0.5f;
	float maxCamHeight;
	float minCamHeight;

	#region ISetPool implementation

	public void SetPool (Pool pool)
	{
		mover = pool.CreateEntity ();
	}

	#endregion

	#region IInitializeSystem implementation
	public void Initialize ()
	{
		Messenger.AddListener<float> (Events.Input.PAN_CAM_X, PanCamX);
		Messenger.AddListener<float> (Events.Input.PAN_CAM_Y, PanCamY);
		Messenger.AddListener<float> (Events.Input.ROTATE_CAM, RotateCam);
		Messenger.AddListener<float> (Events.Input.ZOOM_CAM, ZoomCam);
		camera = Camera.main.transform;
		maxCamHeight = camera.position.y * maxCamHeightScale;
		minCamHeight = camera.position.y * minCamHeightScale;
	}
	#endregion

	#region ITearDownSystem implementation

	public void TearDown ()
	{
		Messenger.RemoveListener<float> (Events.Input.PAN_CAM_X, PanCamX);
		Messenger.RemoveListener<float> (Events.Input.PAN_CAM_Y, PanCamY);
		Messenger.RemoveListener<float> (Events.Input.ROTATE_CAM, RotateCam);
		Messenger.RemoveListener<float> (Events.Input.ZOOM_CAM, ZoomCam);
	}

	#endregion

	#region Pan

	bool PanCamCheck(Vector3 newPos){
		RaycastHit hit;
		if (GameManager.debug) {
			Debug.DrawRay (newPos, camera.forward * 100f, Color.red, 3f);
		}
		if (Physics.Raycast (newPos, camera.forward, out hit)) {
			return true;
		} else {
			return false;
		}
	}

	void PanCamX(float value){
		if (camera != null) {
			var a = camera.rotation.eulerAngles.y;
			if (Mathf.Abs(value) > maxPanStep) {
				value = value > 0 ? maxPanStep : -maxPanStep;
			}
			var addPosX = new Vector3 (value * Mathf.Cos(a * Mathf.Deg2Rad), 0f, -value * Mathf.Sin(a * Mathf.Deg2Rad));

			var newPos = camera.position + addPosX;
			if (PanCamCheck (newPos)) {
				if (mover.hasCoroutine) {
					mover.RemoveCoroutine ();
				}
				camera.position = newPos;
			} else if(!mover.hasCoroutine){
				mover.AddCoroutine (PanSmooth(newPos));
			}
		}
	}

	void PanCamY(float value){
		if (camera != null) {
			var a = camera.rotation.eulerAngles.y;
			if (Mathf.Abs(value) > maxPanStep) {
				value = value > 0 ? maxPanStep : -maxPanStep;
			}
			var addPosY = new Vector3 (value * Mathf.Sin (a * Mathf.Deg2Rad), 0f, value * Mathf.Cos (a * Mathf.Deg2Rad));

			var newPos = camera.position + addPosY;
			if (PanCamCheck (newPos)) {
				if (mover.hasCoroutine) {
					mover.RemoveCoroutine ();
				}
				camera.position = newPos;
			} else if(!mover.hasCoroutine){
				mover.AddCoroutine (PanSmooth(newPos));
			}
		}
	}

	IEnumerator PanSmooth(Vector3 toward){
		while(PanCamCheck(Vector3.MoveTowards (camera.position, toward, 0.05f))){
			camera.position = Vector3.MoveTowards (camera.position, toward, 0.05f);
			yield return null;
		}
	}

	#endregion

	#region Rotate

	void RotateCam(float angle){
		RaycastHit hit;
		if (GameManager.debug) {
			Debug.DrawRay (camera.position, camera.forward * 100f, Color.red, 3f);
		}
		if (Physics.Raycast (camera.position, camera.forward, out hit)) {
			if (Mathf.Abs(angle) > maxRotateStep) {
				angle = angle > 0 ? maxRotateStep : -maxRotateStep;
			}
			camera.RotateAround (hit.point, Vector3.up, angle);
		}
	}

	#endregion

	#region Zoom

	void ZoomCam(float amount){
		RaycastHit hit;
		if (GameManager.debug) {
			Debug.DrawRay (camera.position, camera.forward * 100f, Color.red, 3f);
		}
		if (Physics.Raycast (camera.position, camera.forward, out hit)) {
			amount = amount > 0 ? camZoomStep : -camZoomStep;
			if (ZoomCamCheck(Vector3.MoveTowards (camera.position, hit.point, amount))) {
				camera.position = Vector3.MoveTowards (camera.position, hit.point, amount);
			}
		}
	}

	bool ZoomCamCheck(Vector3 newPos){
		if (newPos.y >= minCamHeight && newPos.y <= maxCamHeight) {
			return true;
		} else {
			return false;
		}
	}

	#endregion
}
