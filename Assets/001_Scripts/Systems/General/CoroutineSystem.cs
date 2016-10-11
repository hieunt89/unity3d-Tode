using UnityEngine;
using System.Collections;
using Entitas;

public class CoroutineSystem : IExecuteSystem, ISetPool, IReactiveSystem {
	#region ISetPool implementation
	Pool _pool;
	Group _groupCoroutine;
	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupCoroutine = _pool.GetGroup (Matcher.Coroutine);
	}

	#endregion

	#region IExecuteSystem implementation

	public void Execute ()
	{
		
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if(_groupCoroutine.count <= 0){
			return;
		}

		var ens = _groupCoroutine.GetEntities ();
		IEnumerator coroutine;
		for (int i = 0; i < ens.Length; i++) {
			coroutine = ens [i].coroutine.task;
			if (!coroutine.MoveNext())
			{
				ens[i].RemoveCoroutine();
			}
		}
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.Tick.OnEntityAdded ();
		}
	}

	#endregion
}
