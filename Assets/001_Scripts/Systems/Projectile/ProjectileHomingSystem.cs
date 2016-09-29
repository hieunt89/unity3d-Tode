using UnityEngine;
using System.Collections;
using Entitas;

public class ProjectileHomingSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Pool _pool;
	Group _groupPrjHoming;
	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupPrjHoming = _pool.GetGroup (Matcher.AllOf (Matcher.ProjectileMark, Matcher.Target, Matcher.ProjectileHoming).NoneOf (Matcher.ReachedEnd));
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if(_groupPrjHoming.count <= 0){
			return;
		}

		Entity e;
		var projectiles = _groupPrjHoming.GetEntities ();
		for (int i = 0; i < projectiles.Length; i++) {
			e = projectiles [i];
			if(!e.hasDestination){
				e.AddDestination (e.target.e.position.value + e.target.e.view.go.GetColliderCenterOffset());
			}
			if (e.position.value == e.destination.value) { //projectile reaches its target
				
				if(e.target.e.hasEnemy){
					e.target.e.AddDamage (ProjectileHelper.GetDamage(
						e.attackDamage.maxDamage,
						e.attackDamage.minDamage,
						e.attack.attackType,
						e.target.e.armor.armorList
					));
				}

				e.IsReachedEnd (true);
				continue;
			}
			if(e.target.e.hasEnemy){
				e.ReplaceDestination (e.target.e.position.value + e.target.e.view.go.GetColliderCenterOffset());
			}
			e.ReplacePosition (Vector3.MoveTowards (e.position.value, e.destination.value, e.projectileHoming.travelSpeed * Time.deltaTime));
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
	
}
