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
		_groupTowerReady = _pool.GetGroup (Matcher.AllOf (Matcher.Active, Matcher.Tower, Matcher.Target).NoneOf(Matcher.AttackCooldown, Matcher.Channeling, Matcher.Attacking));
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
			if(!tower.target.e.hasEnemy){
				tower.RemoveTarget ();
				continue;
			}

			//attack
			if (!tower.hasCoroutine) {
				tower.AddCoroutine (Attack (tower));
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

	IEnumerator Attack(Entity tower){
		tower.IsAttacking (true);

		float time = 0f;
		while(time < tower.attackTime.value){
			if (!_pool.isGamePause) {
				time += Time.deltaTime;
			}
			if (tower.view.Anim != null) {
				tower.view.Anim.Play (AnimState.Fire, 0, time / tower.attackTime.value);
			}
			yield return null;
		}
			
		if (tower.view.Anim != null) {
			tower.view.Anim.Play (AnimState.Idle);
		}

		if (tower.hasTarget) {
			AttackNow (tower, tower.target.e);
		}
		tower.IsAttacking (false);
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