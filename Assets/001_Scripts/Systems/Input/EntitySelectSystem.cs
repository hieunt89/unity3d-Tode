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

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		var e = entities.SingleEntity ().currentSelected.e;

		if (e != null) {
			Messenger.Broadcast<Entity> (Events.Input.ENTITY_SELECT, e);
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
		Messenger.AddListener <GameObject> (Events.Input.CLICK_HIT_SOMETHING, ClickedSomeThing);
		Messenger.AddListener (Events.Input.CLICK_HIT_NOTHING, ClickedNotThing);

		_pool.SetCurrentSelected (null);
	}
	#endregion

	#region ITearDownSystem implementation

	public void TearDown ()
	{
		Messenger.RemoveListener <GameObject> (Events.Input.CLICK_HIT_SOMETHING, ClickedSomeThing);
		Messenger.RemoveListener (Events.Input.CLICK_HIT_NOTHING, ClickedNotThing);
	}

	#endregion

	void ClickedNotThing(){
		if (_pool.currentSelected.e != null) {
			_pool.currentSelected.e.IsSelected (false);
		}
		_pool.ReplaceCurrentSelected (null);
	}

	void ClickedSomeThing(GameObject go){
		Entity e = EntityLink.GetEntity (go);
		if (e != null && e.isInteractable) {
			if (e != _pool.currentSelected.e) {
				if (_pool.currentSelected.e != null) {
					_pool.currentSelected.e.IsSelected (false);
				}
				e.IsSelected (true);
				_pool.ReplaceCurrentSelected (e);
			}
		} else {
			ClickedNotThing ();
		}
	}
}
