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

		Entity prj;
		var ens = _groupPrjHoming.GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			prj = ens [i];
			if(!prj.hasDestination){
				prj.AddDestination (prj.target.e.position.value + prj.target.e.viewOffset.pivotToCenter);
			}
			if (prj.position.value == prj.destination.value) { //projectile reaches its target
				
				if(prj.target.e.hasEnemy){
					prj.target.e.AddDamage (CombatUtility.RandomDamage(
						prj.attackDamage.maxDamage,
						prj.attackDamage.minDamage,
						prj.attack.attackType,
						prj.target.e.armor.armorList
					));
				}

				prj.IsReachedEnd (true);
				continue;
			}
			if(prj.target.e.hasEnemy){
				prj.ReplaceDestination (prj.target.e.position.value + prj.target.e.viewOffset.pivotToCenter);
			}
			prj.ReplacePosition (Vector3.MoveTowards (prj.position.value, prj.destination.value, prj.projectileHoming.travelSpeed * Time.deltaTime));
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
