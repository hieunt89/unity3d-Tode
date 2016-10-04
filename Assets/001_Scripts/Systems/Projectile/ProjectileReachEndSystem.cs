using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class ProjectileReachEndSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Pool _pool;
	Group _groupActiveEnemy;
	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupActiveEnemy = _pool.GetGroup (Matcher.AllOf (Matcher.Enemy, Matcher.Active));
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var prj = entities [i];

			var damage = CombatUtility.RandomDamage (
				prj.attackDamage.maxDamage,
				prj.attackDamage.minDamage
			);
			float reduceTo = 1f;

			if (prj.hasAttackRange) {
				var enemies = _groupActiveEnemy.GetEntities ();
				var targets = CombatUtility.FindTargets (prj, enemies);
				for (int j = 0; j < targets.Count; j++) {
					reduceTo = CombatUtility.GetDamageReduction (prj.attack.attackType, targets [j].armor.armorList);
					targets [j].AddDamage (CombatUtility.GetDamageAfterReduction(damage, reduceTo));
				}
			} else if(prj.target.e.hasEnemy){
				reduceTo = CombatUtility.GetDamageReduction (prj.attack.attackType, prj.target.e.armor.armorList);
				prj.target.e.AddDamage (CombatUtility.GetDamageAfterReduction(damage, reduceTo));
			}	
		prj.IsMarkedForDestroy (true);
		}
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.AllOf (Matcher.ProjectileMark, Matcher.ReachedEnd).OnEntityAdded();
		}
	}

	#endregion
}
