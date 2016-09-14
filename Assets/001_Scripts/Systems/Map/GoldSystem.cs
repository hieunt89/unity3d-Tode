using UnityEngine;
using System.Collections;
using Entitas;

public class GoldSystem : IInitializeSystem, IReactiveSystem, ISetPool {
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
		_pool.SetGoldPlayer (ConstantData.INIT_GOLD);
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		Messenger.Broadcast<int> (Events.Game.GOLD_CHANGE, _pool.goldPlayer.value);
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.GoldPlayer.OnEntityAdded ();
		}
	}

	#endregion
}
