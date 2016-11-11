using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public static class EntityExtension {
	public static Entity AddCoroutineTask(this Entity e, IEnumerator task, bool priority = false){
		if (!e.hasCoroutine) {
			e.AddCoroutine (task);
		} else {
			if (!e.hasCoroutineQueue) {
				e.AddCoroutineQueue (new System.Collections.Generic.Queue<IEnumerator> ());
			}

			if (priority) {
				e.coroutineQueue.Queue.Enqueue (e.coroutine.task);
				e.ReplaceCoroutine (task);
			} else {
				e.coroutineQueue.Queue.Enqueue (task);
			}
		}
		return e;
	}

	public static Entity BeDamaged(this Entity e, int damage){
		if (e.hasHp && e.hasHpTotal) {
			int hpLeft = Mathf.Clamp(e.hp.value - damage, 0, e.hpTotal.value);
			e.ReplaceHp (hpLeft);
		}

		return e;
	}

	public static Entity ReplaceSkillStats(this Entity e, SkillData data){
		if (e.hasSkill && data != null) {
			e.ReplaceAttackSpeed (data.cooldown)
			.ReplaceAttackRange (data.castRange)
			.ReplaceAttackTime (data.castTime)
			.ReplaceGold (data.goldCost)
			.ReplaceAttackCooldown (data.cooldown);

			if (data is CombatSkillData) {
				CombatSkillData s = data as CombatSkillData;
				e.ReplaceSkillCombat (s.effectList)
					.ReplaceAttack (s.attackType)
					.ReplaceAttackDamage (s.damage)
					.ReplaceAoe (s.aoe)
					.ReplaceProjectile (s.projectileId);
			} else if (data is SummonSkillData) {
				SummonSkillData s = data as SummonSkillData;
				e.ReplaceSkillSummon (s.summonId, s.summonCount)
					.ReplaceDuration (s.duration);
			}
		} else {
			e.IsActive (false);
		}

		return e;
	}

	public static Entity ReplaceTowerStats(this Entity e, TowerData data){
		if (e.hasTower && data != null) {
			e.ReplaceProjectile (data.ProjectileId)
				.ReplaceAttack (data.AtkType)
				.ReplaceAttackRange (data.AtkRange)
				.ReplaceAttackDamageRange (data.MinDmg, data.MaxDmg)
				.ReplaceAttackSpeed (data.AtkSpeed)
				.ReplaceAttackTime (data.AtkTime)
				.ReplaceTurnSpeed(data.TurnSpeed)
				.ReplaceAoe (data.Aoe)
				.ReplaceAttackCooldown (data.AtkSpeed)
				.ReplaceSkillList (DataManager.Instance.GetSkillTrees ("fireball_tree", "fireball_tree"))
				.ReplacePointAttack (data.AtkPoint)
				.IsActive(true);
		} else {
			e.IsActive (false);
		}

		return e;
	}

	public static Entity AddViewLookAtComponent(this Entity e){
		if (e.hasView) {
			var markedComp = e.view.go.GetComponentsInChildren<MarkedForLookAtTarget> ();
			var markedTran = new List<Transform> ();

			for (int i = 0; i < markedComp.Length; i++) {
				markedTran.Add (markedComp [i].transform);
			}

			e.ReplaceViewLookAt (markedTran);
		}

		return e;
	}
}
