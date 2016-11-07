using UnityEngine;
using System.Collections;
using Entitas;

public class DyingSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Pool _pool;
	Group _groupDying;
	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupDying = _pool.GetGroup (Matcher.Dying);
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if (true) {
			
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
