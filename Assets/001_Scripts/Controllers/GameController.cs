using UnityEngine;
using Entitas;
using Entitas.Unity.VisualDebugging;

public class GameController : MonoBehaviour {
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
			.Add(pool.CreateSystem<UpdateTickSystem>())

			.Add(pool.CreateSystem<InitTowerSystem>())
			.Add(pool.CreateSystem<InitPathSystem>())

			.Add(pool.CreateSystem<WaveSystem>())
			.Add(pool.CreateSystem<SpawnEnemySystem>())
			.Add(pool.CreateSystem<ActiveEnemySystem>())

			.Add(pool.CreateSystem<UpdateTowerViewSystem>())

			.Add(pool.CreateSystem<ProcessTapInputSystem>())
			;
	}
}
