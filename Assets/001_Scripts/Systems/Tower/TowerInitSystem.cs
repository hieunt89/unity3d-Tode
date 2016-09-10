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
		var tps = GameObject.FindGameObjectsWithTag ("TowerPoint");
		for (int i = 0; i < tps.Length; i++) {
			_pool.CreateEntity ()
				.AddTower ("arrow1")
				.AddId ("tower_" + i + "_" + "arrow1")
				.IsInteractable (true)
				.AddPosition (tps[i].transform.position)
				;
		}
	}
	#endregion
}
