using Entitas;
using System.Collections.Generic;
public class WaveSystem : ISetPool, IInitializeSystem {
	Pool _pool;
	#region ISetPool implementation

	public void SetPool (Pool pool)
	{
		_pool = pool;
	}

	#endregion

	#region IInitializeSystem implementation

	public void Initialize ()
	{
		
		MapData map = DataManager.Instance.GetMapData ("map0");

		_pool.CreateEntity().AddWave(map.Waves[0].Groups).AddId(map.Waves[0].Id);
	}

	#endregion
}
