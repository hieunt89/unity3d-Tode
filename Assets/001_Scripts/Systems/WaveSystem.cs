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
		WaveData data = new WaveData (EnemyType.type1, 10, 1.0f, 0f);
		WaveData data2 =  new WaveData (EnemyType.type2, 10, 1.0f, 3.0f);
		List<WaveData> datas = new List<WaveData>();
		datas.Add(data);
		datas.Add(data2);

		_pool.CreateEntity().AddWave(datas).AddId("wave1");
	}

	#endregion
}
