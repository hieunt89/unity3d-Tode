using UnityEngine;
using System.Collections;
using Entitas;

public class InitMapViewSystem : IInitializeSystem, ISetPool {
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
		var tiles = _pool.GetEntities (Matcher.Tile);
		for (int i = 0; i < tiles.Length; i++) {
			tiles [i].AddTileView (null);
		}
	}

	#endregion


}
