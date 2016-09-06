using System.Collections.Generic;
using Entitas;

public class InitEnemySystem : IReactiveSystem, ISetPool
{
	Pool _pool;
	Group _groupPathId;

	public void SetPool(Pool pool)
	{
		_pool = pool;
		_groupPathId = _pool.GetGroup (Matcher.AllOf (Matcher.Path, Matcher.Id).NoneOf(Matcher.Enemy));
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

				var ePath = GetPathEntityById(waveGroup.PathId);
				if(ePath != null){
					_pool.CreateEntity ()
						.AddEnemy (waveGroup.EClass, waveGroup.Type)
						.AddId (e.id.value + "_" + i + "_" + j)
						.AddActivable (activeTime)
						.IsIntractable (true)
						.AddPath (ePath.path.wayPoints);
						;
				}
			}
		} 

		_pool.DestroyEntity (e);
    }

	Entity GetPathEntityById(string id){
		var ens = _groupPathId.GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			if (ens [i].id.value.Equals (id)) {
				return ens [i];
			}
		}
		return null;
	}
}
