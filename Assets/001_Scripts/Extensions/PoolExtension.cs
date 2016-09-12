using UnityEngine;
using System.Collections;
using Entitas;

public static class PoolExtension {

	public static Entity GetPathEntityById(this Pool pool, string id){
		var ens = pool.GetGroup (Matcher.AllOf (Matcher.Path, Matcher.Id)).GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			if (ens [i].id.value.Equals (id)) {
				return ens [i];
			}
		}
		return null;
	}

	public static Entity CreateProjectile(this Pool pool, string projectileId, Vector3 pos, AttackType atkType, int minDmg, int maxDmg, Entity target){
		ProjectileData prj = DataManager.Instance.GetProjectileData (projectileId);
		Entity e = pool.CreateEntity ()
			.AddProjectile(projectileId)
			.AddPosition(pos)
			.AddAttack (atkType)
			.AddAttackDamage (minDmg, maxDmg)
			.AddTarget (target)
			.AddDestination (target.position.value)
			.AddMovable (prj.travelSpeed)
			;
		if(prj.range > 0){
			e.AddAttackRange (prj.range);
		}
		return e;
	}
}
