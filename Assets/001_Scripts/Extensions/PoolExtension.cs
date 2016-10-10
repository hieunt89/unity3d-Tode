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

	public static Entity CreateProjectile(this Pool pool, string projectileId, Vector3 pos, AttackType atkType, int minDmg, int maxDmg, float aoe, Entity target){
		ProjectileData prj = DataManager.Instance.GetProjectileData (projectileId);
		if (prj == null) {
			return null;
		}

		Entity e = pool.CreateEntity ()
			.IsProjectileMark(true)
			.AddProjectile(projectileId)
			.AddPosition(pos)
			.AddAttack (atkType)
			.AddAttackDamage (minDmg, maxDmg)
			.AddTarget (target)
			;
		if(aoe > 0){
			e.AddAoe (aoe);
		}
		switch (prj.Type) {
		case ProjectileType.homing:
			e.IsProjectileHoming(true)
				.AddMovable(prj.TravelSpeed);
			break;
		case ProjectileType.throwing:
			e.IsProjectileThrowing (true)
				.AddDuration(prj.Duration);
			break;
		case ProjectileType.laser:
			e.AddProjectileLaser (prj.MaxDmgBuildTime)
				.AddDuration(prj.Duration)
				.AddMovable(prj.TravelSpeed)
				.AddInterval(prj.TickInterval);
			break;
		default:
			break;
		}
		return e;
	}

	public static Entity CreateSkill(this Pool pool, string skillId, Entity origin){
		Entity e = pool.CreateEntity ();
		return e;
	}
}
