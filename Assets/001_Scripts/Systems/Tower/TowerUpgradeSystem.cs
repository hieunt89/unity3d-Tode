using UnityEngine;
using System.Collections;
using Entitas;

public class TowerUpgradeSystem : IReactiveSystem, ISetPool{
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
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];
			var cost = DataManager.Instance.GetTowerData (e.towerUpgrade.upgradeNode.data).GoldRequired;
			if (_pool.goldPlayer.value < cost) {
				e.RemoveTowerUpgrade ();
			} else {
				e.IsTowerReset(true).IsActive (false);
				if (e.isTowerBase) {
					e.IsTowerBase (false);
				}
				if (e.hasTower) {
					e.RemoveTower ();
				}
				_pool.ReplaceGoldPlayer (_pool.goldPlayer.value - cost);
				e.ReplaceGold(e.gold.value + cost).AddDuration (0f).IsTowerUpgrading(true);

				_pool.ReselectEntity (e);
			}
		}
	}
	#endregion

	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.TowerUpgrade.OnEntityAdded ();
		}
	}
	#endregion


}
