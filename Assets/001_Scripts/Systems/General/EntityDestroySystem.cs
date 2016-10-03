using UnityEngine;
using System.Collections;
using Entitas;

public class EntityDestroySystem : IReactiveSystem, ISetPool {
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
			if(e.hasView){
				Lean.LeanPool.Despawn (e.view.go);
			}
			if(e.hasViewSlider){
				Lean.LeanPool.Despawn (e.viewSlider.bar.gameObject);
			}
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
