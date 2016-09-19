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
			.AddTurnable (prj.TurnSpeed)
			;
		if(prj.Range > 0){
			e.AddAttackRange (prj.Range);
		}
		switch (prj.Type) {
		case ProjectileType.homing:
			e.AddProjectileHoming (prj.TravelSpeed);
			break;
		case ProjectileType.throwing:
			e.AddProjectileThrowing (prj.TravelTime, prj.TravelSpeed);
			break;
		default:
			break;
		}
		return e;
	}
}
