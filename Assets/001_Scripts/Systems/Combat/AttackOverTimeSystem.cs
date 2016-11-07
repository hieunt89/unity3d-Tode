using UnityEngine;
using System.Collections;
using Entitas;

public class AttackOverTimeSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Pool _pool;
	Group _groupDOT;
	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupDOT = _pool.GetGroup (Matcher.AllOf(Matcher.AttackOverTime));
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if(_groupDOT.count <= 0){
			return;
		}

		var tickEn = entities.SingleEntity ();
		var ens	= _groupDOT.GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			var e = ens [i];

			if (e.hasAttackCooldown) {
				if (e.attackCooldown.time <= 0) {
					e.RemoveAttackCooldown ();
				} else {
					e.ReplaceAttackCooldown (e.attackCooldown.time -= tickEn.tick.change);
				}
				continue;
			} else if (e.target.e.isTargetable) {
				e.target.e.BeDamaged (e.attackOverTime.damage);
				e.AddAttackCooldown (e.attackOverTime.tickInterval);
			} else {
				e.RemoveAttackOverTime ();
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
