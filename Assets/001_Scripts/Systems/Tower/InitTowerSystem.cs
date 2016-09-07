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
			_pool.CreateEntity ()
				.AddTower (TowerType.type1)
				.AddRange (2.0f)
				.AddDamage (1, 2)
				.IsActive(true)
				.AddId("t_" + i)
				.IsInteractable(true)
				.AddPosition(tps[i].transform.position)
				;
		}
	}
	#endregion
}
