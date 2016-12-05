using UnityEngine;
using System.Collections;
using Entitas;
using Lean;

public class MapNavigationSystem : IInitializeSystem, ITearDownSystem, ISetPool {
	Transform camera;
	Entity mover;
	#region ISetPool implementation

	public void SetPool (Pool pool)
	{
		mover = pool.CreateEntity ();
	}

	#endregion

	#region IInitializeSystem implementation
	public void Initialize ()
	{
		Messenger.AddListener<float, float> (Events.Input.PAN_CAM, PanCam);
		camera = Camera.main.transform;
	}
	#endregion

	#region ITearDownSystem implementation

	public void TearDown ()
	{
		Messenger.RemoveListener<float, float> (Events.Input.PAN_CAM, PanCam);
	}

	#endregion

	bool CheckCamera(Vector3 newPos){
		RaycastHit hit;
		if (GameManager.debug) {
			Debug.DrawRay (newPos, camera.forward * 20f, Color.red, 3f);
		}
		if (Physics.Raycast (newPos, camera.forward, out hit)) {
			return true;
		} else {
			return false;
		}
	}

	void PanCam(float x, float y){
		if (camera != null) {
			var a = camera.rotation.eulerAngles.y;
			var addPosX = new Vector3 (x * Mathf.Cos(a * Mathf.Deg2Rad), 0f, -x * Mathf.Sin(a * Mathf.Deg2Rad));
			var addPosY = new Vector3 (y * Mathf.Sin (a * Mathf.Deg2Rad), 0f, y * Mathf.Cos (a * Mathf.Deg2Rad));

			var newPos = camera.position + addPosX + addPosY;
			if (CheckCamera (newPos)) {
				if (mover.hasCoroutine) {
					mover.RemoveCoroutine ();
				}
				camera.position = newPos;
			} else if(!mover.hasCoroutine){
				mover.AddCoroutine (SmoothPan(newPos));
			}
		}
	}

	IEnumerator SmoothPan(Vector3 toward){
		while(CheckCamera(camera.position)){
			camera.position = Vector3.MoveTowards (camera.position, toward, 0.1f);
			yield return null;
		}
	}
}
