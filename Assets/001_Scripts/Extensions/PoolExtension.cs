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
			.AddPosition(origin.position.value + origin.view.go.transform.FindChild("out").transform.localPosition)
//			.AddPosition(origin.position.value + origin.view.go.transform.InverseTransformPoint(new Vector3(3f,3f,3f)))

			.AddOrigin(origin)
			.AddTarget (target);
		switch (prj.Type) {
		case ProjectileType.homing:
			e.IsProjectileHoming(true)
				.AddMoveSpeed(prj.TravelSpeed);
			break;
		case ProjectileType.throwing:
			e.AddProjectileThrowing (prj.Duration);
			break;
		case ProjectileType.laser:
			e.AddProjectileLaser (prj.MaxDmgBuildTime, prj.Duration)
				.AddMoveSpeed(prj.TravelSpeed)
				.AddInterval(prj.TickInterval);
			break;
		default:
			break;
		}
		return e;
	}
}
