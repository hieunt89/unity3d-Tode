using UnityEngine;
using System.Collections;
using Entitas;

public class CheckCloseCombatSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Pool _pool;
	Group _groupCloseCombating;
	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupCloseCombating = pool.GetGroup (Matcher.AllOf(Matcher.Active, Matcher.CloseCombat));
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if (_groupCloseCombating.count <= 0) {
			return;
		}

		var ens = _groupCloseCombating.GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			var e = ens [i];

			if (!e.closeCombat.opponent.isTargetable || Vector3.Distance(e.position.value, e.closeCombat.opponent.position.value) > ConstantData.CLOSE_COMBAT_RANGE) {
				e.RemoveCloseCombat ();
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
