using UnityEngine;
using System.Collections;
using Entitas;

public class AutoAttackSystem : IReactiveSystem, ISetPool {
	Pool _pool;
	Group _groupAttackable;
	#region ISetPool implementation

	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupAttackable = _pool.GetGroup (Matcher
			.AllOf (Matcher.Active, Matcher.Attackable)
			.AnyOf (Matcher.Target, Matcher.CloseCombat)
			.NoneOf(Matcher.AttackCooldown, Matcher.Channeling, Matcher.Attacking, Matcher.Coroutine)
		);
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if(_groupAttackable.count <= 0){
			return;
		}
			
		var attackers = _groupAttackable.GetEntities ();
		for (int i = 0; i < attackers.Length; i++) {
			var attacker = attackers [i];

			//ensure enemy exist
			if(attacker.hasTarget && !attacker.target.e.isTargetable){
				attacker.RemoveTarget ();
				continue;
			}

			//attack
			attacker.AddCoroutineTask (Attack (attacker));
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

	IEnumerator Attack(Entity attacker){
		attacker.AddAttacking (0f);
		attacker.ReplaceAttackingParams (AnimState.Fire, attacker.attackTime.value);

		while(attacker.attacking.timeSpent < attacker.attackTime.value){
			if (!attacker.hasTarget && !attacker.hasCloseCombat) {
				break;
			}
			attacker.ReplaceAttacking(attacker.attacking.timeSpent + _pool.tick.change);
			yield return null;
		}

		if (attacker.hasTarget) {
			AttackNow (attacker, attacker.target.e);
		}else if (attacker.hasCloseCombat) {
			AttackNow (attacker, attacker.closeCombat.opponent);
		}

		attacker.ReplaceAttacking (0f);
		attacker.ReplaceAttackingParams (AnimState.PostFire, attacker.attackTime.value);

		while(attacker.attacking.timeSpent < attacker.attackTime.value){
			if (!attacker.hasTarget && !attacker.hasCloseCombat) {
				break;
			}
			attacker.ReplaceAttacking(attacker.attacking.timeSpent + _pool.tick.change);
			yield return null;
		}

		attacker.RemoveAttacking ();
	}

	void AttackNow(Entity attacker, Entity target){
		attacker.AddAttackCooldown(attacker.attackSpeed.value);
		var damage = CombatUtility.RandomDamage (
			attacker.attackDamageRange.maxDmg,
			attacker.attackDamageRange.minDmg
		);

		var prjId = attacker.hasProjectile ? attacker.projectile.projectileId : null;
		var e = _pool.CreateProjectile (prjId, attacker, target)
			.AddAttack (attacker.attack.attackType)
			.AddAttackDamage (damage)
			;
		if(attacker.hasAoe && attacker.aoe.value > 0){
			e.AddAoe (attacker.aoe.value);
		}
	}
}