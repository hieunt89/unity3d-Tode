﻿using UnityEngine;
using System.Collections;
using Entitas;

public class ProjectileLaserSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Pool _pool;
	Group _groupPrjLaser;
	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupPrjLaser = _pool.GetGroup (Matcher.AllOf(Matcher.ProjectileLaser, Matcher.Target).NoneOf(Matcher.ReachedEnd));
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if(_groupPrjLaser.count <= 0){
			return;
		}
			
		Entity prj;
		var ens = _groupPrjLaser.GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			prj = ens [i];
			if(!prj.hasDestination){
				prj.AddDestination (prj.position.value);
				prj.origin.e.IsChanneling (true);
			}

			if (prj.origin.e.isActive && prj.origin.e.hasTarget && prj.origin.e.target.e == prj.target.e && prj.target.e.hasEnemy) {
				var scaleFromPos = Vector3.Distance (prj.destination.value, prj.position.value) / Vector3.Distance (prj.position.value, prj.target.e.position.value + prj.target.e.viewOffset.pivotToCenter);
				prj.ReplaceDestination (
					prj.position.value.ToEndFragment (prj.target.e.position.value + prj.target.e.viewOffset.pivotToCenter, Mathf.Clamp01 (prj.movable.speed * Time.deltaTime + scaleFromPos))
				);
			} else {
				prj.IsMarkedForDestroy (true).origin.e.IsChanneling (false);
				continue;
			}

			if (prj.hasProjectileTime) {
				if (prj.projectileTime.time >= prj.duration.value) {
					prj.IsMarkedForDestroy (true).origin.e.IsChanneling (false);
					continue;
				} else {
					float dmgScale = Mathf.Clamp01(prj.projectileTime.time / prj.projectileLaser.maxDmgBuildTime);
					int damage = CombatUtility.GetDamage (
						prj.origin.e.attackDamageRange.maxDmg, 
						prj.origin.e.attackDamageRange.minDmg,
						dmgScale,
						prj.attack.attackType,
						prj.target.e.armor.armorList
					);
					prj.ReplaceAttackOverTime (damage, prj.interval.value)
						.ReplaceProjectileTime (prj.projectileTime.time + Time.deltaTime);
				}
			} else if (prj.destination.value == (prj.target.e.position.value + prj.target.e.viewOffset.pivotToCenter)) {
				prj.AddProjectileTime (0f);
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
}
