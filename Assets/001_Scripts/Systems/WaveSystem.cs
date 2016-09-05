using Entitas;
using System.Collections.Generic;
public class WaveSystem : IReactiveSystem, ISetPool {
	Pool _pool;
	#region ISetPool implementation

	public void SetPool (Pool pool)
	{
		_pool = pool;
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		var e = entities.SingleEntity ();

		// TODO: hard code data, can sua lai cho nay!
		WaveData data = new WaveData (EnemyType.type1, 10, 1.0f);
		List<WaveData> datas = new List<WaveData>();
		datas.Add(data);

		_pool.CreateEntity().AddWave(datas);
		
		_pool.DestroyEntity (e);
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.StartInput.OnEntityAdded ();
		}
	}

	#endregion


}
