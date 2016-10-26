using UnityEngine;
using System.Collections;
using Entitas;

public class AttackCooldownSystem : IReactiveSystem, ISetPool {
	Pool _pool;
	Group _groupOnCooldown;
	#region ISetPool implementation

	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupOnCooldown = _pool.GetGroup (Matcher.AllOf(Matcher.AttackCooldown, Matcher.Active).NoneOf(Matcher.Channeling));
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if(_groupOnCooldown.count <= 0){
			return;
		}

		var ens = _groupOnCooldown.GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			var e = ens [i];
			//check if tower attack is on cooldown
			if (e.hasAttackCooldown) {
				if (e.attackCooldown.time > 0) {
					e.ReplaceAttackCooldown (e.attackCooldown.time - Time.deltaTime);
				} else {
					e.RemoveAttackCooldown ();
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
