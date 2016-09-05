using UnityEngine;
using System.Collections;
using Entitas;
using Entitas.Unity.VisualDebugging;

public class GameController : MonoBehaviour {

	public Vector3[] towerPoints;
	public Vector3[] wayPoints;

	public bool debug = true;
	Systems _systems;

	void Start() {
		_systems = createSystems(Pools.pool);
		_systems.Initialize();
	}

	void Update() {
		_systems.Execute();
	}

	Systems createSystems(Pool pool) {
		Systems systems;
		if(debug){
			systems = new DebugSystems ();
		}else{
			systems = new Systems ();
		}
		return systems
			.Add(pool.CreateSystem<UpdateTickSystem>())
			.Add(pool.CreateSystem<InitTowerSystem>())
			.Add(pool.CreateSystem<InitEnemyPathSystem>())

			.Add(pool.CreateSystem<UpdateTowerViewSystem>())
			.Add(pool.CreateSystem<WaveSystem>())

			.Add(pool.CreateSystem<ProcessTapInputSystem>())
			;
	}
}
