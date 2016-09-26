using UnityEngine;
using Lean;
using Entitas;

public class InputManager : MonoBehaviour {
	public bool debug = true;

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
			if(debug){
				Debug.Log ("hit " + hitInfo.collider.gameObject.name);
			}
			e = Pools.pool.GetEntityById (hitInfo.collider.gameObject.name);
			if(e != null && e.isInteractable){
				if (debug) {
					Debug.Log ("hit entity " + e.id.value);
				}
				Messenger.Broadcast<Entity> (Events.Input.ENTITY_CLICK, e);
			}
		} else {
			if (debug) {
				Debug.Log ("hit nothing interactable");
			}
			Messenger.Broadcast (Events.Input.EMPTY_CLICK);
		}
	}
}