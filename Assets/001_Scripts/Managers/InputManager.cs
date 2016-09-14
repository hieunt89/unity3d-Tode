using UnityEngine;
using Lean;
using Entitas;

public class InputManager : MonoBehaviour {

	void Start(){
		LeanTouch.OnFingerTap += HandleFingerTap;
	}

	void HandleFingerTap (LeanFinger fg){
		if(LeanTouch.GuiInUse){
			return;
		}

		RaycastHit hitInfo;
		Ray ray = fg.GetRay (Camera.main);
		if (Physics.Raycast (ray, out hitInfo)) {
			Messenger.Broadcast<Entity> (Events.Input.ENTITY_CLICK, Pools.pool.GetEntityById(hitInfo.collider.gameObject.name));
		} else {
			Messenger.Broadcast (Events.Input.EMPTY_CLICK);
		}
	}
}