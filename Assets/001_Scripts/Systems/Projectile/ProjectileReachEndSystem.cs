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

			if (prj.hasAttackDamage) {
				ProjectileDamage (prj);
			}

			if (prj.hasSkillCombat) {
				ProjectileSkill (prj);
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

	void ProjectileDamage(Entity prj){
		if (prj.hasAoe) {
			var enemies = _groupActiveEnemy.GetEntities ();
			var targets = CombatUtility.FindTargetsInRange (prj, enemies, prj.aoe.value);
			for (int j = 0; j < targets.Count; j++) {
				ApplyDamage (prj.attackDamage.dmg, prj, targets [j]);
			}
		} else {
			ApplyDamage (prj.attackDamage.dmg, prj, prj.target.e);
		}	
	}

	void ApplyDamage(int damage, Entity prj, Entity target){
		if (target.isAttackable) {
			var reduceTo = CombatUtility.GetDamageReduction (prj.attack.attackType, target.armor.armorList);
			target.BeDamaged (CombatUtility.GetDamageAfterReduction (damage, reduceTo));
		}
	}

	void ProjectileSkill(Entity prj){
		if (prj.hasAoe) {
			var enemies = _groupActiveEnemy.GetEntities ();
			var targets = CombatUtility.FindTargetsInRange (prj, enemies, prj.aoe.value);
			for (int i = 0; i < targets.Count; i++) {
				ApplyEffect (prj, targets [i]);
			}
		} else {
			ApplyEffect (prj, prj.target.e);	
		}
	}

	void ApplyEffect(Entity prj, Entity target){
		if (target.isAttackable && !target.hasSkillEffects) {
			target.AddSkillEffects (prj.skillCombat.effects);	
		}
	}
}
