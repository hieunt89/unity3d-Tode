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
			if (tower.view.Anim != null) {
				tower.IsAttacking (true);
				tower.view.Anim.SetTrigger ("fire");
				tower.AddCoroutine (StartAttack (tower, tower.target.e));
			} else {
				Debug.Log ("tower " + tower.id.value + " does not have attack animator");
				AttackNow (tower, tower.target.e);
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

	IEnumerator StartAttack(Entity tower, Entity target){
		while(tower.isAttacking){
			yield return null;
		}

		AttackNow (tower, target);
	}

	void AttackNow(Entity tower, Entity target){
		tower.AddAttackCooldown(tower.attackSpeed.value);
		_pool.CreateProjectile (
			tower.projectile.projectileId,
			tower.position.value + Vector3.up,
			tower.attack.attackType,
			tower.attackDamage.minDamage,
			tower.attackDamage.maxDamage,
			tower.aoe.value,
			target).AddOrigin(tower);
	}
}
