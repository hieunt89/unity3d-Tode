using UnityEngine;
using System.Collections;
using Entitas;
using Lean;

public class EntitySelectSystem : IInitializeSystem, ITearDownSystem, ISetPool, IReactiveSystem {
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
		LeanTouch.OnFingerTap -= OnFingerTap;
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		var e = entities.SingleEntity ().currentSelected.e;

		if (e != null) {
			if (e.hasTower || e.isTowerBase || e.isTowerUpgrading) {
				Messenger.Broadcast<Entity> (Events.Input.TOWER_SELECT, e);
			}
		} else {
			Messenger.Broadcast (Events.Input.EMPTY_SELECT);
		}
	}
	#endregion

	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.CurrentSelected.OnEntityAdded ();
		}
	}
	#endregion

	#region IInitializeSystem implementation
	public void Initialize ()
	{
		LeanTouch.OnFingerTap += OnFingerTap;
		_pool.SetCurrentSelected (null);
	}
	#endregion

	void OnFingerTap (LeanFinger fg){
		if(LeanTouch.GuiInUse){
			return;
		}

		RaycastHit hitInfo;
		Entity e;
		Ray ray = fg.GetRay (Camera.main);

		if (Physics.Raycast (ray, out hitInfo)) {
			e = EntityLink.GetEntity (hitInfo.collider.gameObject);
			if (e != null && e.isInteractable) {
				if (e != _pool.currentSelected.e) {
					_pool.ReplaceCurrentSelected (e);
				}
				return;
			}
			Messenger.Broadcast<Vector3> (Events.Input.CLICK, hitInfo.point);
		}
		_pool.ReplaceCurrentSelected (null);
	}
}
