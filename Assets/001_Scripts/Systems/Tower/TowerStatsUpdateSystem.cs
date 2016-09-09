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
		TowerData data;
		for (int i = 0; i < entities.Count; i++) {
			var tower = entities [i];
			data = DataManager.Instance.GetTowerData (tower.tower.towerId);
			if (data != null) {
				tower
					.ReplaceProjectile (data.prjType)
					.ReplaceAttack (data.atkType)
					.ReplaceAttackRange (data.atkRange)
					.ReplaceAttackDamage (data.minDmg, data.maxDmg)
					.ReplaceAttackSpeed (data.atkSpeed)
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
