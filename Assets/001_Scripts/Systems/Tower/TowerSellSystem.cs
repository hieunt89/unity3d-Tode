using UnityEngine;
using System.Collections;
using Entitas;
public class TowerSellSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Pool _pool;
	public void SetPool (Pool pool)
	{
		_pool = pool;
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		Entity e;
		for (int i = 0; i < entities.Count; i++) {
			e = entities [i];

			_pool.ReplaceGoldPlayer (_pool.goldPlayer.value + e.gold.value);
			e.IsTowerReset(true).IsActive(false).IsInteractable(false).RemoveTower ().IsTowerBase (true).ReplaceGold(0).IsMarkedForSell(false);

		}
	}
	#endregion
	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.AllOf(Matcher.Gold, Matcher.Tower, Matcher.MarkedForSell).OnEntityAdded ();
		}
	}
	#endregion
}
