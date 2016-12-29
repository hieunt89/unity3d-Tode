using UnityEngine;
using System.Collections;
using Entitas;

public class AllyUpdateEngageDestinationSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Group _groupEngaging;
	public void SetPool (Pool pool)
	{
		_groupEngaging = pool.GetGroup (Matcher.AllOf(Matcher.Ally, Matcher.Active, Matcher.Engage));
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if (_groupEngaging.count <= 0) {
			return;
		}

		var ens = _groupEngaging.GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			var e = ens [i];

			e.ReplaceDestination (e.engage.target.position.value);
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
