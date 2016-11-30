using UnityEngine;
using System.Collections;
using Entitas;

public class EnemyReachEndSystem : IReactiveSystem, ISetPool, IEnsureComponents {
	Pool _pool;
	#region ISetPool implementation

	public void SetPool (Pool pool)
	{
		_pool = pool;
	}

	#endregion

	#region IEnsureComponents implementation

	public IMatcher ensureComponents {
		get {
			return Matcher.LifeCount;
		}
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];
			_pool.ReplaceLifePlayer (_pool.lifePlayer.value - e.lifeCount.value);
			e.IsMarkedForDestroy (true);
		}
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.AllOf(Matcher.Enemy, Matcher.ReachedEnd).OnEntityAdded ();
		}
	}

	#endregion


}
