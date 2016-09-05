using UnityEngine;
using System.Collections;
using Entitas;

public class ActiveEnemySystem : IReactiveSystem, ISetPool {
	Pool _pool;
	Group _groupActivable;
	#region ISetPool implementation

	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupActivable = _pool.GetGroup (Matcher.Activable);
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if(_groupActivable.count <= 0){
			return;
		}

		var e = entities.SingleEntity ();
		var eActivable = _groupActivable.GetEntities ();
		for (int i = 0; i < eActivable.Length; i++) {
			if(e.tick.time >= eActivable[i].enemy.activeTime){
				eActivable [i].IsActivable (false).AddMovable (1.0f);
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
