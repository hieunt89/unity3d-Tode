﻿using UnityEngine;
using Entitas;
using Entitas.Unity.VisualDebugging;

public class GameManager : MonoBehaviour {
	public bool showDebug = true;
	public static bool ShowDebug;
	public InputType inputType = InputType.Unity;

	Systems _systems;

	void Awake() {
		ShowDebug = showDebug;

		UserManager.Init ();
		DataManager.Init ();

		var pools = Pools.sharedInstance;
		pools.SetAllPools ();
		_systems = CreateSystems (pools);

		_systems.Initialize ();
	}

	void Update() {
		_systems.Execute ();
	}

	void OnDestroy(){
		_systems.TearDown ();
	}

	Systems CreateSystems(Pools pools) {
		Systems systems;
		if(ShowDebug){
			systems = new DebugSystems ();
		}else{
			systems = new Systems ();
		}
		return systems
				//Map
				.Add(pools.pool.CreateSystem ( new TimeSystem () ))
				.Add(pools.pool.CreateSystem ( new CoroutineSystem () ))
				.Add(pools.pool.CreateSystem ( new CoroutineQueueSystem () ))
				.Add(pools.pool.CreateSystem ( new MapSystem () ))
				.Add(pools.pool.CreateSystem ( new LifeSystem () ))
				.Add(pools.pool.CreateSystem ( new GoldSystem () ))
				.Add(pools.pool.CreateSystem ( new PathSystem () ))
				.Add(pools.pool.CreateSystem ( new WaveSystem () ))

				//Input
				.Add(pools.pool.CreateSystem ( new InputSystem (inputType) ))
				.Add(pools.pool.CreateSystem ( new EntitySelectSystem () ))
				.Add(pools.pool.CreateSystem ( new CameraControlSystem () ))

				//Combat
				.Add(pools.pool.CreateSystem ( new CheckTargetSystem () ))
				.Add(pools.pool.CreateSystem ( new FindTargetSystem () ))
				.Add(pools.pool.CreateSystem ( new AttackOverTimeSystem () ))
				.Add(pools.pool.CreateSystem ( new AttackCooldownSystem () ))
				.Add(pools.pool.CreateSystem ( new HpRegenSystem () ))

				//Tower
				.Add(pools.pool.CreateSystem ( new TowerInitSystem () ))
				.Add(pools.pool.CreateSystem ( new TowerUpgradeSystem () ))
				.Add(pools.pool.CreateSystem ( new TowerBuildSystem () ))
				.Add(pools.pool.CreateSystem ( new TowerStatsUpdateSystem () ))
				.Add(pools.pool.CreateSystem ( new TowerAttackSystem () ))
				.Add(pools.pool.CreateSystem ( new TowerSellSystem () ))
				.Add(pools.pool.CreateSystem ( new TowerResetSystem () ))

				//Enemy
				.Add(pools.pool.CreateSystem ( new EnemyInitSystem () ))
				.Add(pools.pool.CreateSystem ( new EnemyMoveSystem () ))
				.Add(pools.pool.CreateSystem ( new EnemyReachEndSystem () ))
				.Add(pools.pool.CreateSystem ( new EnemyWatchHpSystem () ))

				//Ally
				.Add(pools.pool.CreateSystem ( new AllyInitSystem () ))
				.Add(pools.pool.CreateSystem ( new AllyEngageSystem () ))

				//Projectile
				.Add(pools.pool.CreateSystem ( new ProjectileInstantSystem() ))
				.Add(pools.pool.CreateSystem ( new ProjectileHomingSystem () ))
				.Add(pools.pool.CreateSystem ( new ProjectileThrowingSystem () ))
				.Add(pools.pool.CreateSystem ( new ProjectileLaserSystem () ))
				.Add(pools.pool.CreateSystem ( new ProjectileCollideSystem () ))
				.Add(pools.pool.CreateSystem ( new ProjectileReachEndSystem () ))

				//Skill
				.Add(pools.pool.CreateSystem ( new SkillInitSystem () ))
				.Add(pools.pool.CreateSystem ( new SkillUpgradeSystem () ))
				//Skill Combat
				.Add(pools.pool.CreateSystem ( new SkillCombatCastSystem () ))
				//Effect
				.Add(pools.pool.CreateSystem ( new EffectMovementApplySystem () ))
				.Add(pools.pool.CreateSystem ( new EffectMovementUpdateSystem () ))

				//View
				.Add(pools.pool.CreateSystem ( new TowerCreateViewSystem () ))
				.Add(pools.pool.CreateSystem ( new CharacterCreateViewSystem () ))
				.Add(pools.pool.CreateSystem ( new ProjectileCreateViewSystem () ))
				.Add(pools.pool.CreateSystem ( new ProjectileLaserViewSystem () ))
				.Add(pools.pool.CreateSystem ( new UpdateViewPositionSystem () ))
				.Add(pools.pool.CreateSystem ( new UpdateLookDirectionSystem () ))
				.Add(pools.pool.CreateSystem ( new UpdateViewLookAtSystem () ))

				//View mecanim
				.Add(pools.pool.CreateSystem ( new MecanimAttackingSystem () ))
				.Add(pools.pool.CreateSystem ( new MecanimMoveSystem () ))
				.Add(pools.pool.CreateSystem ( new MecanimDyingSystem () ))

				//Indicators
				.Add(pools.pool.CreateSystem ( new HealthBarToggleSystem () ))
				.Add(pools.pool.CreateSystem ( new HealthBarUpdateSystem() ))
				.Add(pools.pool.CreateSystem ( new TowerProgressBarViewSystem () ))
				.Add(pools.pool.CreateSystem ( new SelectedViewToggleSystem () ))
				.Add(pools.pool.CreateSystem ( new SelectedViewUpdateSystem () ))
				
				//General
				.Add(pools.pool.CreateSystem ( new EntityActiveSystem () ))
				.Add(pools.pool.CreateSystem ( new EntityDestroySystem () ))
			;
	}
}