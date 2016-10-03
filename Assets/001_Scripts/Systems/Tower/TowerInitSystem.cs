using UnityEngine;
using System.Collections.Generic;
using Entitas;

public class TowerInitSystem : IInitializeSystem, ISetPool {
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
		var tps = _pool.map.data.TowerPoints;
		if(tps == null){
			return;
		}

		for (int i = 0; i < tps.Count; i++) {
			_pool.CreateEntity ()
				.IsTowerBase (true)
				.AddId ("tower" + i)
				.IsInteractable (true)
				.AddPosition (tps[i].TowerPointPos)
				.AddGold(0)
				;
		}
	}
	#endregion
}
