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

	public static Entity CreateProjectile(this Pool pool, ProjectileType prjType, Vector3 pos, AttackType atkType, int minDmg, int maxDmg, float travelSpeed, Entity target){
		return pool.CreateEntity ()
			.AddProjectile(prjType)
			.AddPosition(pos)
			.AddAttack (atkType)
			.AddAttackDamage (minDmg, maxDmg)
			.AddTarget (target)
			.AddMovable (travelSpeed);
	}
}
