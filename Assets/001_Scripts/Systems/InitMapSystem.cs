using UnityEngine;
using System.Collections;
using Entitas;

public class InitMapSystem : IInitializeSystem, ISetPool {
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
		_pool.CreateEntity ().AddMap (ConstantData.MAP_WIDTH, ConstantData.MAP_HEIGHT, new GameObject("Map"));
		for (int h = 0; h < _pool.map.height; h++) {
			for (int w = 0; w < _pool.map.width; w++) {
				_pool.CreateEntity ().AddPosition (w, h).AddTile(TileType.movable).AddName(h + "_" + w);
			}
		}
	}

	#endregion
}
