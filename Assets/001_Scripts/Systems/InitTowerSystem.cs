using UnityEngine;
using System.Collections.Generic;
using Entitas;

public class InitTowerSystem : IInitializeSystem, ISetPool {
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
		var tps = GameObject.FindGameObjectsWithTag ("TowerPoint");
		for (int i = 0; i < tps.Length; i++) {
			_pool.CreateEntity ().AddTower (TowerType.type1).AddPosition(tps[i].transform.position.x, tps[i].transform.position.y , tps[i].transform.position.z).AddId("t" + i);
		}
	}
	#endregion
}
