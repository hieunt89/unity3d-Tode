using UnityEngine;
using System.Collections;
using Entitas;

public class TowerAttackCooldownSystem : IReactiveSystem, ISetPool {
	Pool _pool;
	Group _groupTowerOnCooldown;
	#region ISetPool implementation

	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupTowerOnCooldown = _pool.GetGroup (Matcher.AllOf(Matcher.Tower, Matcher.AttackCooldown).NoneOf(Matcher.Channeling));
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if(_groupTowerOnCooldown.count <= 0){
			return;
		}

		var ens = _groupTowerOnCooldown.GetEntities ();
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
