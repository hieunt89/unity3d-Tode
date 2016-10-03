using Entitas;
using System.Collections.Generic;
using UnityEngine;


public class WaveSystem : ISetPool, IInitializeSystem, IReactiveSystem {
	List<Entity> waveList;

	#region ISetPool implementation
	Pool _pool;
	public void SetPool (Pool pool)
	{
		_pool = pool;
	}

	#endregion

	#region IInitializeSystem implementation

	public void Initialize ()
	{
		MapData map = _pool.map.data;
		waveList = new List<Entity> ();
		float activeTime = _pool.tick.time;
		for (int i = 0; i < map.Waves.Count; i++) {
			activeTime += map.Waves [i].waveDelay;
			var e = _pool.CreateEntity()
				.AddWave(map.Waves [i].Groups)
				.AddId(map.Waves [i].Id)
				.AddMarkedForActive(activeTime);
			if(i == 0){
				e.IsNextWave (true);
			}
			waveList.Add (e);
		}
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];
			waveList.Remove (e);
		}
		if(waveList.Count > 0){
			waveList [0].IsNextWave (true);
		}
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.Wave.OnEntityRemoved ();
		}
	}

	#endregion
}
