using UnityEngine;
using System.Collections;
using Entitas;

public static class EntityExtension {

	public static Entity BeDamaged(this Entity e, int damage){
		if (e.hasHp && e.hasHpTotal) {
			int hpLeft = Mathf.Clamp(e.hp.value - damage, 0, e.hpTotal.value);
			e.ReplaceHp (hpLeft);
		}
		return e;
	}

	public static Entity ReplaceSkillStats(this Entity e, SkillData data){
		if (e.hasSkill) {
			e
				.ReplaceAttackSpeed(data.cooldown)
				.ReplaceAttackRange(data.castRange)
				.ReplaceAttackTime(data.castTime)
				.ReplaceGold(data.goldCost)
				.ReplaceAttackCooldown(data.cooldown)
				;

			if(data is CombatSkillData){
				CombatSkillData s = data as CombatSkillData;
				e.ReplaceSkillCombat (s.effectList)
					.ReplaceAttack(s.attackType)
					.ReplaceAttackDamage(s.damage)
					.ReplaceAoe (s.aoe)
					.ReplaceProjectile (s.projectileId);
			}else if(data is SummonSkillData){
				SummonSkillData s = data as SummonSkillData;
				e.ReplaceSkillSummon (s.summonId, s.summonCount)
					.ReplaceDuration (s.duration);
			}
		}

		return e;
	}
}
