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

	public static Entity CreateProjectile(this Pool pool, string prjId, Entity origin, Entity target){
		ProjectileData prj = DataManager.Instance.GetProjectileData (prjId);
		if (prj == null) {
			return null;
		}

		Entity e = pool.CreateEntity ()
			.AddProjectile(prjId)
			.IsProjectileMark(true)
			.AddPosition(origin.position.value + Vector3.up)
			.AddOrigin(origin)
			.AddTarget (target);
		switch (prj.Type) {
		case ProjectileType.homing:
			e.IsProjectileHoming(true)
				.AddMoveSpeed(prj.TravelSpeed);
			break;
		case ProjectileType.throwing:
			e.IsProjectileThrowing (true)
				.AddDuration(prj.Duration);
			break;
		case ProjectileType.laser:
			e.AddProjectileLaser (prj.MaxDmgBuildTime)
				.AddDuration(prj.Duration)
				.AddMoveSpeed(prj.TravelSpeed)
				.AddInterval(prj.TickInterval);
			break;
		default:
			break;
		}
		return e;
	}
}
