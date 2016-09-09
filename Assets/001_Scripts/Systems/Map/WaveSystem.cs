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
		WaveGroup group1 = new WaveGroup ("enemy1", 10, 1.0f, 0f, "path_0");
		WaveGroup group2 =  new WaveGroup ("enemy2", 10, 1.0f, 3.0f, "path_1");

		List<WaveGroup> groups = new List<WaveGroup>();
		groups.Add(group1);
		groups.Add(group2);

		_pool.CreateEntity().AddWave(groups).AddId("wave_1");
	}

	#endregion
}
