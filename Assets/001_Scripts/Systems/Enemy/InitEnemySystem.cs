using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class InitEnemySystem : IReactiveSystem, ISetPool
{
	Pool _pool;

	public void SetPool(Pool pool)
	{
		_pool = pool;
	}

    public TriggerOnEvent trigger
    {
        get
        {
            return Matcher.Wave.OnEntityAdded ();
        }
    }

	void IReactiveExecuteSystem.Execute(List<Entity> entities)
    {
		var e = entities.SingleEntity ();
		float activeTime = _pool.tick.time;
		WaveGroup waveGroup;

		for (int i = 0; i < e.wave.groups.Count; i++) { //loop throu all datas in wave
			waveGroup = e.wave.groups[i];
			activeTime = activeTime + waveGroup.WaveDelay;
			for (int j = 0; j < waveGroup.Amount; j++) { //loop throu all enemies is wave data
				if (j != 0) {
					activeTime = activeTime + waveGroup.SpawnInterval;
				}

				var ePath = _pool.GetPathEntityById(waveGroup.PathId);
				if(ePath != null){
					_pool.CreateEntity ()
						.AddEnemy (waveGroup.EClass, waveGroup.Type, waveGroup.PathId)
						.AddId (e.id.value + "_g" + i + "_e" + j)
						.AddActivable (activeTime)
						.IsIntractable (true)
						.AddMovable (1.0f)
						.AddDestination (ePath.path.wayPoints[0])
						.AddPosition (ePath.path.wayPoints[0])
						;
				}
			}
		} 

		_pool.DestroyEntity (e);
    }


}
