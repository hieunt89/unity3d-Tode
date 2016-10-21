using UnityEngine;
using System.Collections;
using Entitas;

public class TowerBuildSystem : IReactiveSystem, ISetPool {
	Group _groupTowerUpgrading;
	#region ISetPool implementation
	Pool _pool;
	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupTowerUpgrading = _pool.GetGroup (Matcher.AllOf(Matcher.TowerUpgrade, Matcher.Duration));
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if(_groupTowerUpgrading.count <= 0){
			return;
		}

		var upgrades = _groupTowerUpgrading.GetEntities ();
		for (int i = 0; i < upgrades.Length; i++) {
			var e = upgrades [i];

			if (e.duration.value < e.towerUpgrade.upgradeTime) {
				e.ReplaceDuration (e.duration.value + Time.deltaTime);
			} else {
				e.IsTowerUpgrading(false)
					.ReplaceTower (e.towerUpgrade.upgradeNode)
					.RemoveTowerUpgrade ()
					.RemoveDuration ()
				;
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