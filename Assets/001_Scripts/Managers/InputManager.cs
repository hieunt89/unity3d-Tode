using UnityEngine;
using Lean;

public class InputManager : MonoBehaviour {

	void Start(){
		LeanTouch.OnFingerTap += HandleFingerTap;
	}

	void HandleFingerTap (LeanFinger fg){
		RaycastHit hitInfo;
		Ray ray = fg.GetRay (Camera.main);
		if (Physics.Raycast (ray, out hitInfo)) {
			Pools.pool.CreateEntity().AddTapInput(hitInfo.collider.gameObject.name);
		}
	}
}