using UnityEngine;
using System.Collections;
using Entitas;

public static class PoolExtension {

	public static Entity GetEntityById(this Pool pool, string id){
		var entities = pool.GetGroup (Matcher.AllOf(Matcher.Id)).GetEntities ();
		for (int i = 0; i < entities.Length; i++) {
			if(entities[i].id.value.Equals(id)){
				return entities [i];
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
