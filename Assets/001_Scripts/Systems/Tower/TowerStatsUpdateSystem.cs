using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

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
		for (int i = 0; i < entities.Count; i++) {
			var tower = entities [i];
			tower.ReplaceTowerStats (DataManager.Instance.GetTowerData (tower.tower.towerNode.data));
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
