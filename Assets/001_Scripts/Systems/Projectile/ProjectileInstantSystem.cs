using UnityEngine;
using System.Collections;
using Entitas;

public class ProjectileInstantSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Group _groupPrjInstant;
	public void SetPool (Pool pool)
	{
		_groupPrjInstant = pool.GetGroup (Matcher.AllOf (Matcher.ProjectileInstant, Matcher.Target).NoneOf (Matcher.ReachedEnd));
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if (_groupPrjInstant.count <= 0) {
			return;
		}

		var ens = _groupPrjInstant.GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			var e = ens [i];

			e.IsReachedEnd (true);
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
