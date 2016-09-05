using System.Collections.Generic;
using Entitas;

public class SpawnEnemySystem : IReactiveSystem, ISetPool
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
				_pool.CreateEntity ().AddEnemy (waveGroup.Type, activeTime).AddId (e.id.value + "_e_" + i + "_" + j).IsActivable (true).IsIntractable (true);
			}
		} 

		_pool.DestroyEntity (e);
    }
}
