using UnityEngine;
using System.Collections;
using Entitas;

public class ProjectileInstantSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Pool _pool;
	Group _groupPrjInstant;
	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupPrjInstant = _pool.GetGroup (Matcher.AllOf(Matcher.ProjectileMark, Matcher.Target, Matcher.ProjectileInstant));
	}
	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		throw new System.NotImplementedException ();
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
