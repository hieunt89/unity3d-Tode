using UnityEngine;
using Lean;
using Entitas;

public class InputManager : MonoBehaviour {
	public bool debug = true;

	void OnEnable(){
		Pools.pool.SetCurrentSelected (null);
		LeanTouch.OnFingerTap += HandleFingerTap;
		Messenger.AddListener (Events.Input.ENTITY_DESELECT, HandleEntityDeselect);
	}

	void OnDisable(){
		LeanTouch.OnFingerTap -= HandleFingerTap;
		Messenger.RemoveListener (Events.Input.ENTITY_DESELECT, HandleEntityDeselect);
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
			e = EntityLink.GetEntity (hitInfo.collider.gameObject);
			if(e != null && e.isInteractable && e != Pools.pool.currentSelected.e){
				if (debug) {
					Debug.Log ("hit entity " + e.id.value);
				}
				Pools.pool.ReplaceCurrentSelected (e);
				Messenger.Broadcast (Events.Input.ENTITY_SELECT);
			}
		} else {
			if (debug) {
				Debug.Log ("hit nothing interactable");
			}
			Pools.pool.ReplaceCurrentSelected (null);
			Messenger.Broadcast (Events.Input.EMPTY_SELECT);
		}
	}

	void HandleEntityDeselect(){
		Pools.pool.ReplaceCurrentSelected (null);
	}
}