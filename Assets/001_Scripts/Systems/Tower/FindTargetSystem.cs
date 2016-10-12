using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class FindTargetSystem : IReactiveSystem, ISetPool {
	Pool _pool;
	Group _groupActiveAttacker;
	Group _groupActiveEnemy;
	#region ISetPool implementation

	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupActiveAttacker = _pool.GetGroup (Matcher.AllOf (Matcher.Active).AnyOf(Matcher.Tower, Matcher.Skill).NoneOf(Matcher.Target, Matcher.AttackCooldown));
		_groupActiveEnemy = _pool.GetGroup (Matcher.AllOf (Matcher.Enemy, Matcher.Active));
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (List<Entity> entities)
	{
		if(_groupActiveAttacker.count <= 0 || _groupActiveEnemy.count <= 0){
			return;
		}

		var attackerEns = _groupActiveAttacker.GetEntities ();
		var enemyEns = _groupActiveEnemy.GetEntities ();
		for (int i = 0; i < attackerEns.Length; i++) {
			var origin = attackerEns [i].hasSkill ? attackerEns [i].origin.e : attackerEns [i];
			var target = CombatUtility.FindTargetInRange (origin, enemyEns, attackerEns[i].attackRange.value);
			if (target != null) {
				attackerEns [i].AddTarget (target);
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
