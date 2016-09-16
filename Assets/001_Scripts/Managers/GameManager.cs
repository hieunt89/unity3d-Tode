﻿using UnityEngine;
using Entitas;
using Entitas.Unity.VisualDebugging;

public class GameManager : MonoBehaviour {
	public bool showDebug = true;
	public static bool debug;
	Systems _systems;

	void Start() {
		debug = showDebug;
		_systems = CreateSystems(Pools.pool);
		_systems.Initialize();
	}

	void Update() {
		_systems.Execute();
	}

	Systems CreateSystems(Pool pool) {
		Systems systems;
		if(debug){
			systems = new DebugSystems ();
		}else{
			systems = new Systems ();
		}
		return systems
			//Map
			.Add(pool.CreateSystem<CoroutineSystem>())
			.Add(pool.CreateSystem<TimeSystem>())
			.Add(pool.CreateSystem<LifeSystem>())
			.Add(pool.CreateSystem<GoldSystem>())
			.Add(pool.CreateSystem<PathSystem>())
			.Add(pool.CreateSystem<WaveSystem>())

			//Tower
			.Add(pool.CreateSystem<TowerInitSystem>())
			.Add(pool.CreateSystem<TowerUpgradeSystem>())
			.Add(pool.CreateSystem<TowerBuildSystem>())
			.Add(pool.CreateSystem<TowerStatsUpdateSystem>())
			.Add(pool.CreateSystem<TowerCheckTargetSystem>())
			.Add(pool.CreateSystem<TowerFindTargetSystem>())
			.Add(pool.CreateSystem<TowerAttackSystem>())
			.Add(pool.CreateSystem<TowerAttackCooldownSystem>())

			//Enemy
			.Add(pool.CreateSystem<EnemyInitSystem>())
			.Add(pool.CreateSystem<EnemyActiveSystem>())
			.Add(pool.CreateSystem<EnemyMoveSystem>())
			.Add(pool.CreateSystem<EnemyReachEndSystem>())
			.Add(pool.CreateSystem<EnemyDeadSystem>())

			//Projectile
			.Add(pool.CreateSystem<ProjectileHomingSystem>())
			.Add(pool.CreateSystem<ProjectileThrowingSystem>())
			.Add(pool.CreateSystem<ProjectileReachEndSystem>())

			//View
			.Add(pool.CreateSystem<TowerCreateViewSystem>())
			.Add(pool.CreateSystem<EnemyCreateViewSystem>())
			.Add(pool.CreateSystem<ProjectileCreateViewSystem>())
			.Add(pool.CreateSystem<UpdateViewPositionSystem>())
			.Add(pool.CreateSystem<UpdateLookDirectionSystem>())
			.Add(pool.CreateSystem<HeathBarViewSystem>())
			.Add(pool.CreateSystem<TowerProgressBarViewSystem>())

			//Destroy things
			.Add(pool.CreateSystem<DestroyEntitySystem>())
			;
	}
}