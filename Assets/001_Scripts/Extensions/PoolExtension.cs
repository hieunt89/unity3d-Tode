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

	public static Entity FindEngagingAlly(this Pool pool, Entity _e){
		var ens = pool.GetGroup(Matcher.AllOf(Matcher.Engage, Matcher.Ally, Matcher.Active)).GetEntities ();

		for (int i = 0; i < ens.Length; i++) {
			var e = ens[i];
			if (e.engage.target == _e) {
				return e;
			}
		}

		return null;
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
		Entity e = pool.CreateEntity ();
		
		if (string.IsNullOrEmpty(prjId)) {
			e.IsProjectileInstant (true);
		} else {
			ProjectileData prj = DataManager.Instance.GetProjectileData (prjId);
			e.AddProjectile (prjId);
			switch (prj.Type) {
			case ProjectileType.homing:
				e.IsProjectileHoming (true)
				.AddMoveSpeed (prj.TravelSpeed);
				break;
			case ProjectileType.throwing:
				e.AddProjectileThrowing (prj.Duration);
				break;
			case ProjectileType.laser:
				e.AddProjectileLaser (prj.MaxDmgBuildTime, prj.Duration)
				.AddMoveSpeed (prj.TravelSpeed)
				.AddInterval (prj.TickInterval);
				break;
			case ProjectileType.instant:
				e.IsProjectileInstant (true);
				break;
			default:
				e.IsProjectileInstant (true);
				break;
			}
		}

		return e.IsProjectileMark(true)
			.AddPosition(origin.position.value + origin.pointAttack.offset)
			.AddOrigin(origin)
			.AddTarget (target);
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
			.AddPointAttack (charData.AtkPoint)
			.AddEngageRange (4f) // psuedo data
			;
		if (charData.HpRegenRate > 0 && charData.HpRegenInterval > 0) {
			e.AddHpRegen (charData.HpRegenRate, charData.HpRegenInterval, charData.HpRegenInterval);
		}
		return e;
	}
}
