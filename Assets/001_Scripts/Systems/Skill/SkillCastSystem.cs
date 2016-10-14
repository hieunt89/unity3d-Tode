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
			if (!origin.isActive || origin.hasCoroutine || origin.isAttacking || origin.isChanneling) {
				continue;
			}

			origin.AddCoroutine (CastSkill (origin, skill, skill.target.e));
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

	IEnumerator CastSkill(Entity origin, Entity skill, Entity target){
		origin.IsAttacking (true);

		float time = 0f;
		while (time < skill.attackTime.value) {
			if (!_pool.isGamePause) {
				time += Time.deltaTime;
			}
			if (origin.view.Anim != null) {
				origin.view.Anim.Play (AnimState.Cast, 0, time / skill.attackTime.value);
			}
			yield return null;
		}

		if (origin.view.Anim != null) {
			origin.view.Anim.Play (AnimState.Idle);
		}

		CastNow (origin, skill, target);
		origin.IsAttacking (false);
	}

	void CastNow(Entity origin, Entity skill, Entity target){
		skill.AddAttackCooldown (skill.attackSpeed.value);
		if (skill.hasSkillCombat) {
			CastCombatSkill(origin, skill, target);
		}else if (skill.hasSkillSummon) {
			
		}
	}

	void CastCombatSkill(Entity origin, Entity skill, Entity target){
		var e = _pool.CreateProjectile (skill.projectile.projectileId, origin, target)
			.AddSkillCombat(skill.skillCombat.effectList)
//			.AddAttackDamage(skill
		;
		if(skill.aoe.value > 0){
			e.AddAoe (skill.aoe.value);
		}
	}
}
