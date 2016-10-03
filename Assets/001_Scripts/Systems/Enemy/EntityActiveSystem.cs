using UnityEngine;
using System.Collections;
using Entitas;

public class EntityActiveSystem : IReactiveSystem, ISetPool {
	Pool _pool;
	Group _groupActivable;
	#region ISetPool implementation

	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupActivable = _pool.GetGroup (Matcher.MarkedForActive);
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if(_groupActivable.count <= 0){
			return;
		}

		var eTime = entities.SingleEntity ();
		var eActivable = _groupActivable.GetEntities ();
		for (int i = 0; i < eActivable.Length; i++) {
			var e = eActivable [i];
			if(eTime.tick.time >= e.markedForActive.delayTime){
				e.RemoveMarkedForActive ().IsActive(true);
			}
		}
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.Tick.OnEntityAdded ();
		}
	}

	#endregion
}
