using UnityEngine;
using System.Collections;
using Entitas;
using Lean;

public class InputSystem : IInitializeSystem, ITearDownSystem, ISetPool {
	#region ISetPool implementation
	Pool _pool;
	public void SetPool (Pool pool)
	{
		_pool = pool;
	}

	#endregion

	#region ITearDownSystem implementation

	public void TearDown ()
	{
		LeanTouch.OnFingerTap -= HandleFingerTap;
		Messenger.RemoveListener (Events.Input.ENTITY_DESELECT, HandleEntityDeselect);
	}

	#endregion

	#region IInitializeSystem implementation
	public void Initialize ()
	{
		_pool.SetCurrentSelected (null);
		LeanTouch.OnFingerTap += HandleFingerTap;
		Messenger.AddListener (Events.Input.ENTITY_DESELECT, HandleEntityDeselect);
	}
	#endregion

	void HandleFingerTap (LeanFinger fg){
		if(LeanTouch.GuiInUse){
			return;
		}

		RaycastHit hitInfo;
		Entity e;
		Ray ray = fg.GetRay (Camera.main);

		if (Physics.Raycast (ray, out hitInfo)) {
			e = EntityLink.GetEntity (hitInfo.collider.gameObject);
			if(e != null && e.isInteractable && e != _pool.currentSelected.e){
				_pool.ReplaceCurrentSelected (e);
				Messenger.Broadcast <Entity> (Events.Input.ENTITY_SELECT, e);
			}
		} else {
			_pool.ReplaceCurrentSelected (null);
			Messenger.Broadcast (Events.Input.EMPTY_SELECT);
		}
	}

	void HandleEntityDeselect(){
		Pools.sharedInstance.pool.ReplaceCurrentSelected (null);
	}
}
