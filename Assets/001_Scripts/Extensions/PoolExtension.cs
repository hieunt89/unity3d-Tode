using UnityEngine;
using System.Collections;
using Entitas;

public static class PoolExtension {
	public static bool ReselectEntity(this Pool pool, Entity e){
		if (pool.currentSelected.e != null && pool.currentSelected.e == e) {
			pool.ReplaceCurrentSelected (pool.currentSelected.e);
			return true;
		}else{
			return false;
		}
	}

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
			.AddPosition(origin.position.value + origin.pointAttack.offset)
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
		case ProjectileType.instant:
			e.IsProjectileInstant (true);
			break;
		default:
			break;
		}
		return e;
	}

	public static Entity CreateCharacter(this Pool pool, CharacterData charData){		
		var e = pool.CreateEntity ()
			.AddMoveSpeed (charData.MoveSpeed)
			.AddMoveSpeedBase (charData.MoveSpeed)
			.AddTurnSpeed (charData.TurnSpeed)
			.AddLifeCount (charData.LifeCount)
			.AddGold (charData.GoldWorth)
			.AddAttack (charData.AtkType)
			.AddAttackSpeed (charData.AtkSpeed)
			.AddAttackTime (charData.AtkTime)
			.AddAttackDamageRange (charData.MinAtkDmg, charData.MaxAtkDmg)
			.AddAttackRange (charData.AtkRange)
			.AddArmor (charData.Armors)
			.AddHp (charData.Hp)
			.AddHpTotal (charData.Hp)
			.AddDyingTime (charData.DyingTime)
			.AddPointTarget (charData.AtkPoint)
			.AddEngageRange (4f) // psuedo data
			;
		if (charData.HpRegenRate > 0 && charData.HpRegenInterval > 0) {
			e.AddHpRegen (charData.HpRegenRate, charData.HpRegenInterval, charData.HpRegenInterval);
		}
		return e;
	}
}
