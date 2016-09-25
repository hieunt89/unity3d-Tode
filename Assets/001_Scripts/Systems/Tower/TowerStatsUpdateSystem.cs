using UnityEngine;
using System.Collections;
using Entitas;

public class TowerStatsUpdateSystem : IReactiveSystem, IEnsureComponents {
	#region IEnsureComponents implementation

	public IMatcher ensureComponents {
		get {
			return Matcher.Tower;
		}
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		TowerData towerData;
		for (int i = 0; i < entities.Count; i++) {
			var tower = entities [i];
			towerData = DataManager.Instance.GetTowerData (tower.tower.towerId);
			if (towerData != null) {
				tower
					.ReplaceProjectile (towerData.PrjType)
					.ReplaceAttack (towerData.AtkType)
					.ReplaceAttackRange (towerData.AtkRange)
					.ReplaceAttackDamage (towerData.MinDmg, towerData.MaxDmg)
					.ReplaceAttackSpeed (towerData.AtkSpeed)
					.ReplaceGold(towerData.GoldRequired)
					.IsInteractable(true)
					.IsActive (true);
			} else {
				tower.IsActive (false);
			}
		}

	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.Tower.OnEntityAdded ();
		}
	}

	#endregion


}
