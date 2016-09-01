using UnityEngine;
using System.Collections;
using Entitas;
using Entitas.Unity.VisualDebugging;

public class GameController : MonoBehaviour {

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
			.Add(pool.CreateSystem<InitMapSystem>())
			.Add(pool.CreateSystem<InitMapViewSystem>())
			.Add(pool.CreateSystem<UpdateMapViewSystem>())
			;
	}
}
