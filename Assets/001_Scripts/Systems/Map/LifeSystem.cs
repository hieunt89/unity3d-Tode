using UnityEngine;
using System.Collections;
using Entitas;

public class LifeSystem : ISetPool, IInitializeSystem, IReactiveSystem {
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
		_pool.SetLifePlayer (ConstantData.INIT_LIFE);
		Messenger.Broadcast<int> (Events.Game.LIFE_CHANGE, _pool.lifePlayer.value);
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		Messenger.Broadcast<int> (Events.Game.LIFE_CHANGE, _pool.lifePlayer.value);
	}
	#endregion
	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.LifePlayer.OnEntityAdded ();
		}
	}
	#endregion
}
