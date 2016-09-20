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
		var towerData = DataManager.Instance.GetTowerData ("tower0");
		var tps = DataManager.Instance.GetMapData ("map0").TowerPoints;
		if(towerData == null || tps == null){
			return;
		}

		for (int i = 0; i < tps.Count; i++) {
			_pool.CreateEntity ()
				.AddTower (towerData.Id)
				.AddId ("tower" + i)
				.IsInteractable (true)
				.AddPosition (tps[i].TowerPointPos)
				;
		}
	}
	#endregion
}
