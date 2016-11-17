using UnityEngine;
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
		_pool.ReplaceTick (Time.deltaTime)
			.ReplaceTimeTotal (_pool.timeTotal.value + _pool.tick.change);
	}

	#endregion

	#region IInitializeSystem implementation

	public void Initialize ()
	{
		_pool.ReplaceTick (0f)
			.ReplaceTimeTotal (0f);
	}

	#endregion


}
