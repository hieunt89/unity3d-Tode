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
		if(!_pool.isGamePause){
			var e = _pool.tickEntity;
			e.ReplaceTick (Time.deltaTime * e.timeSpeed.value);
			e.ReplaceTimeTotal (e.timeTotal.value + e.tick.change);
		}
	}

	#endregion

	#region IInitializeSystem implementation

	public void Initialize ()
	{
		_pool.ReplaceTick (0f);
		_pool.tickEntity.AddTimeSpeed (1.0f);
		_pool.tickEntity.AddTimeTotal (0f);
	}

	#endregion


}
