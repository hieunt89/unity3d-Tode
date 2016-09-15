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
		Entity e;
		Ray ray = fg.GetRay (Camera.main);
		if (Physics.Raycast (ray, out hitInfo)) {
			e = Pools.pool.GetEntityById (hitInfo.collider.gameObject.name);
			if(e != null){
				Messenger.Broadcast<Entity> (Events.Input.ENTITY_CLICK, e);
			}
		} else {
			Messenger.Broadcast (Events.Input.EMPTY_CLICK);
		}
	}
}