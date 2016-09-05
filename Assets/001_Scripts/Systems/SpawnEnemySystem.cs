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
		float delayTime = _pool.tick.time;
		WaveData data;

		for (int i = 0; i < e.wave.datas.Count; i++) { //loop throu all datas in wave
			data = e.wave.datas[i];
			delayTime = delayTime + data.Delay;
			for (int j = 0; j < data.Amount; j++) { //loop throu all enemies is wave data
				if (j != 0) {
					delayTime = delayTime + data.Interval;
				}
				_pool.CreateEntity().AddEnemy(data.Type, delayTime).AddId(e.id.value+"_e_"+i+"_"+j).IsActivable(true);
			}
		} 

		_pool.DestroyEntity (e);
    }
}
