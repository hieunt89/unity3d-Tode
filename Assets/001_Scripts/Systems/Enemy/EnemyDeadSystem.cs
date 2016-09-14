using UnityEngine;
using System.Collections;
using Entitas;

public class EnemyDeadSystem : ISetPool, IReactiveSystem, IEnsureComponents {
	#region ISetPool implementation
	Pool _pool;
	public void SetPool (Pool pool)
	{
		_pool = pool;
	}
	#endregion

	#region IEnsureComponents implementation

	public IMatcher ensureComponents {
		get {
			return Matcher.Hp;
		}
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];
			if(e.hp.value <= 0){
				if(e.hasGold){
					_pool.ReplaceGoldPlayer (_pool.goldPlayer.value + e.gold.value);
				}
				e.IsMarkedForDestroy (true);;
			}
		}
	}
	#endregion
	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.AllOf(Matcher.Enemy, Matcher.Hp).OnEntityAdded ();
		}
	}
	#endregion
	
}
