using UnityEngine;
using System.Collections;
using Entitas;

public class DestroyEnemySystem : IReactiveSystem, ISetPool {
	Pool _pool;
	#region ISetPool implementation

	public void SetPool (Pool pool)
	{
		_pool = pool;
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];
			GameObject.Destroy (e.view.go);
			_pool.DestroyEntity (e);
		}
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.MarkedForDestroy.OnEntityAdded ();
		}
	}

	#endregion


}
