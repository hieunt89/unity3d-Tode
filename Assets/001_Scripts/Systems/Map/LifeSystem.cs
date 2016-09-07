﻿using UnityEngine;
using System.Collections;
using Entitas;

public class LifeSystem : ISetPool, IInitializeSystem, IReactiveSystem, IEnsureComponents {
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
		_pool.ReplaceLife (ConstantData.INIT_LIFE);
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
			_pool.ReplaceLife (_pool.life.value + e.lifeCount.value);
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
