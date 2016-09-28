using UnityEngine;
using System.Collections;
using Entitas;

public class ProjectileLaserSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Pool _pool;
	Group _groupPrjInstant;
	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupPrjInstant = _pool.GetGroup (Matcher.AllOf(Matcher.ProjectileMark, Matcher.Target, Matcher.ProjectileLaser).NoneOf(Matcher.ReachedEnd));
	}
	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if(_groupPrjInstant.count <= 0){
			return;
		}

		Entity e;
		var ens = _groupPrjInstant.GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			e = ens [i];


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
