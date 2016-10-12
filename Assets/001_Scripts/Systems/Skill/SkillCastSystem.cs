﻿using UnityEngine;
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
			if (!origin.isActive || origin.isAttacking || origin.isChanneling) {
				continue;
			}

			origin.AddCoroutine (CastSkill (origin, skill));
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

		CastNow (origin, skill);
		origin.IsAttacking (false);
	}

	void CastNow(Entity origin, Entity skill){
		skill.AddAttackCooldown (skill.attackSpeed.value);
		if (skill.hasSkillCombat) {
			CastCombatSkill(origin, skill);
		}else if (skill.hasSkillSummon) {
			
		}
	}

	void CastCombatSkill(Entity origin, Entity skill){
		var e = _pool.CreateProjectile (skill.projectile.projectileId, origin, skill.target.e)
			.AddSkillCombat(skill.skillCombat.effectList)
		;
		if(skill.aoe.value > 0){
			e.AddAoe (skill.aoe.value);
		}
	}
}