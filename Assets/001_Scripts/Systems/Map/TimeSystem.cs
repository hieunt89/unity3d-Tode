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
		var e = _pool.tickEntity;
		e.ReplaceTick (Time.deltaTime);
		e.ReplaceTimeTotal (e.timeTotal.value + e.tick.change);
	}

	#endregion

	#region IInitializeSystem implementation

	public void Initialize ()
	{
		_pool.ReplaceTick (0f)
			.AddTimeTotal (0f);
	}

	#endregion


}
