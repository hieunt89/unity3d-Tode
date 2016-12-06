using UnityEngine;
using System.Collections;
using Entitas;
using Lean;

public class CameraControlSystem : IInitializeSystem, ITearDownSystem, ISetPool {
	#region vars

	Transform camera;
	float minPanStep = 0.1f;
	float maxPanStep = 0.5f; //in unity unit
	float maxRotateStep = 5f; //in degrees
	float camZoomStep = 1.5f; //in unity unit
	float maxCamHeightScale = 2.5f; //max zoom out height scale compare to init position
	float minCamHeightScale = 0.5f; //max zoom in height scale compare to init position
	float maxCamHeight;
	float minCamHeight;

	#endregion

	#region ISetPool implementation
	Entity mover;
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

		//vars set up
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

	bool MoveIfValid(Vector3 newPos){
		if (PanCamCheck (newPos)) {
			if (mover.hasCoroutine) {
				mover.RemoveCoroutine ();
			}
			camera.position = newPos;
			return true;
		} else {
			return false;
		}
	}

	IEnumerator PanSmooth(Vector3 toward){
		while(PanCamCheck(Vector3.MoveTowards (camera.position, toward, 0.01f))){
			Debug.Log ("moving now");
			camera.position = Vector3.MoveTowards (camera.position, toward, 0.01f);
			yield return null;
		}
	}
	
	void PanCamX(float value){
		var a = camera.rotation.eulerAngles.y;
		if (Mathf.Abs(value) > maxPanStep) {
			value = value > 0 ? maxPanStep : -maxPanStep;
		}
		Vector3 addPos;

		//move along x axis first
		addPos = new Vector3 (value * Mathf.Cos(a * Mathf.Deg2Rad), 0f, 0f);
		if (!MoveIfValid (camera.position + addPos)) {
			if(!mover.hasCoroutine){
				mover.AddCoroutine (PanSmooth(camera.position + addPos));
			}
		}

		//then move along z axis
		addPos = new Vector3 (0f, 0f, -value * Mathf.Sin (a * Mathf.Deg2Rad));
		if (!MoveIfValid (camera.position + addPos)) {
			if(!mover.hasCoroutine){
				mover.AddCoroutine (PanSmooth(camera.position + addPos));
			}
		}
	}

	void PanCamY(float value){
		var a = camera.rotation.eulerAngles.y;
		if (Mathf.Abs(value) > maxPanStep) {
			value = value > 0 ? maxPanStep : -maxPanStep;
		}
		Vector3 addPos;

		//move along x axis first
		addPos = new Vector3 (value * Mathf.Sin (a * Mathf.Deg2Rad), 0f, 0f);
		if (!MoveIfValid (camera.position + addPos)) {
			if(!mover.hasCoroutine){
				mover.AddCoroutine (PanSmooth(camera.position + addPos));
			}
		}

		//then move along z axis
		addPos = new Vector3 (0f, 0f, value * Mathf.Cos (a * Mathf.Deg2Rad));
		if (!MoveIfValid (camera.position + addPos)) {
			if(!mover.hasCoroutine){
				mover.AddCoroutine (PanSmooth(camera.position + addPos));
			}
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
