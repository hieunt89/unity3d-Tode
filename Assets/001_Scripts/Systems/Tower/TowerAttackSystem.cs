using UnityEngine;
using System.Collections;
using Entitas;

public class TowerAttackSystem : IReactiveSystem, ISetPool {
	Pool _pool;
	Group _groupTowerReady;
	#region ISetPool implementation

	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupTowerReady = _pool.GetGroup (Matcher.AllOf (Matcher.Active, Matcher.Tower, Matcher.Target).NoneOf(Matcher.AttackCooldown, Matcher.Channeling, Matcher.Attacking, Matcher.Coroutine));
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if(_groupTowerReady.count <= 0){
			return;
		}
			
		var towers = _groupTowerReady.GetEntities ();
		for (int i = 0; i < towers.Length; i++) {
			var tower = towers [i];

			//ensure enemy exist
			if(!tower.target.e.isTargetable){
				tower.RemoveTarget ();
				continue;
			}

			//attack
			tower.AddCoroutineTask (Attack (tower));
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

	IEnumerator Attack(Entity tower){
		tower.AddAttacking (0f);
		tower.ReplaceAttackingParams (AnimState.Fire, tower.attackTime.value);

		while(tower.attacking.timeSpent < tower.attackTime.value){
			tower.ReplaceAttacking(tower.attacking.timeSpent + _pool.tick.change);
			yield return null;
		}

		if (tower.hasTarget) {
			AttackNow (tower, tower.target.e);
		}

		tower.ReplaceAttacking (0f);
		tower.ReplaceAttackingParams (AnimState.PostFire, tower.attackTime.value);

		while(tower.attacking.timeSpent < tower.attackTime.value){
			tower.ReplaceAttacking(tower.attacking.timeSpent + _pool.tick.change);
			yield return null;
		}

		tower.RemoveAttacking ();
	}

	void AttackNow(Entity tower, Entity target){
		tower.AddAttackCooldown(tower.attackSpeed.value);
		var damage = CombatUtility.RandomDamage (
			tower.attackDamageRange.maxDmg,
			tower.attackDamageRange.minDmg
		);
		var e = _pool.CreateProjectile (tower.projectile.projectileId, tower, target)
			.AddAttack (tower.attack.attackType)
			.AddAttackDamage (damage)
			;
		if(tower.aoe.value > 0){
			e.AddAoe (tower.aoe.value);
		}
	}
}