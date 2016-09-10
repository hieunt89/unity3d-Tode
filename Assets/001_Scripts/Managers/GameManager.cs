using UnityEngine;
using Entitas;
using Entitas.Unity.VisualDebugging;

public class GameManager : MonoBehaviour {
	public bool debug = true;
	Systems _systems;

	void Start() {
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
			.Add(pool.CreateSystem<TimeSystem>())
			.Add(pool.CreateSystem<LifeSystem>())
			.Add(pool.CreateSystem<PathSystem>())
			.Add(pool.CreateSystem<WaveSystem>())
			.Add(pool.CreateSystem<HpWatchSystem>())

			//Tower
			.Add(pool.CreateSystem<TowerInitSystem>())
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

			//Projectile
			.Add(pool.CreateSystem<ProjectileHomingSystem>())
			.Add(pool.CreateSystem<ProjectileReachEndSystem>())

			//view
			.Add(pool.CreateSystem<TowerUpdateViewSystem>())
			.Add(pool.CreateSystem<EnemyCreateViewSystem>())
			.Add(pool.CreateSystem<ProjectileCreateViewSystem>())
			.Add(pool.CreateSystem<UpdateViewPositionSystem>())

			//destroy things
			.Add(pool.CreateSystem<DestroyEntitySystem>())

			//Input
			.Add(pool.CreateSystem<ProcessTapInputSystem>())
			;
	}
}
