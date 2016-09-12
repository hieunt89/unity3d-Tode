using UnityEngine;
using System.Collections;
using Entitas;

public class MapInitSystem : ISetPool, IInitializeSystem {
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
		_pool.SetLifePlayer (ConstantData.INIT_LIFE);
		_pool.SetGoldPlayer (ConstantData.INIT_GOLD);
	}

	#endregion

	
}
