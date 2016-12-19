using UnityEngine;
using System.Collections;
using Entitas;

public class CheckEngageSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Pool _pool;
	Group _groupEngaging;
	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupEngaging = pool.GetGroup (Matcher.AllOf(Matcher.Active, Matcher.Engage));
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

			if (!e.engage.target.isTargetable) {
				if (e.hasEnemy) {
					var newOpp = _pool.FindEngageOpponent (e);
					if (newOpp == null) {
						e.RemoveEngage ();
					} else {
						e.ReplaceEngage (newOpp);
					}
				} else {
					e.RemoveEngage ();
				}
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
