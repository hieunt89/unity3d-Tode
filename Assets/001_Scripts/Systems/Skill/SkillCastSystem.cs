using UnityEngine;
using System.Collections;
using Entitas;

public class SkillCastSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Pool _pool;
	Group _groupSkillCastable;
	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupSkillCastable = _pool.GetGroup (Matcher.AllOf(Matcher.Skill, Matcher.Active, Matcher.Target).NoneOf(Matcher.AttackCooldown));
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if (_groupSkillCastable.count <= 0) {
			return;
		}

		var skillEns = _groupSkillCastable.GetEntities ();
		for (int i = 0; i < skillEns.Length; i++) {
			var skill = skillEns [i];
			var origin = skill.origin.e;
			if (!origin.isActive || origin.hasAttacking || origin.isChanneling) {
				continue;
			}
				
			origin.AddCoroutineTask (CastSkill (origin, skill));
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

	IEnumerator CastSkill(Entity origin, Entity skill){
		origin.ReplaceAttackingParams (AnimState.Cast, skill.attackTime.value);
		origin.AddAttacking (0f);

		while (origin.attacking.spentTime < skill.attackTime.value) {
			origin.ReplaceAttacking (origin.attacking.spentTime + _pool.tick.change);
			yield return null;
		}

		if (skill.hasTarget) {
			CastNow (origin, skill, skill.target.e);
		}
		origin.RemoveAttacking ();
	}

	void CastNow(Entity origin, Entity skill, Entity target){
		if (!skill.hasAttackCooldown) {
			skill.AddAttackCooldown (skill.attackSpeed.value);
		}
		if (skill.hasSkillCombat) {
			CastCombatSkill(origin, skill, target);
		}else if (skill.hasSkillSummon) {
			
		}
	}

	void CastCombatSkill(Entity origin, Entity skill, Entity target){
		var e = _pool.CreateProjectile (skill.projectile.projectileId, origin, target)
			.AddSkillCombat(skill.skillCombat.effects)
			.AddAttackDamage(skill.attackDamage.dmg)
			.AddAttack(skill.attack.attackType)
		;
		if(skill.aoe.value > 0){
			e.AddAoe (skill.aoe.value);
		}
	}
}
