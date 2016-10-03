﻿using UnityEngine;
using Entitas;

public class TimeSystem : IInitializeSystem, IExecuteSystem, ISetPool {
	Pool _pool;
	#region ISetPool implementation

	public void SetPool (Pool pool)
	{
		_pool = pool;
	}

	#endregion

	#region IExecuteSystem implementation

	public void Execute ()
	{
		if(!_pool.isGamePause){
			_pool.ReplaceTick (_pool.tick.time + Time.deltaTime);
		}
	}

	#endregion

	#region IInitializeSystem implementation

	public void Initialize ()
	{
		_pool.ReplaceTick (0f);
	}

	#endregion


}
