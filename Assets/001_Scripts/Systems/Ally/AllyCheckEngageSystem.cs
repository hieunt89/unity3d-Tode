using UnityEngine;
using System.Collections;
using Entitas;

public class AllyCheckEngageSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Group _groupAllyEngaging;
	public void SetPool (Pool pool)
	{
		_groupAllyEngaging = pool.GetGroup (Matcher.AllOf(Matcher.Ally, Matcher.Active, Matcher.Engage));
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if (_groupAllyEngaging.count <= 0) {
			return;
		}

		var tickEn = entities.SingleEntity ();
		var allies = _groupAllyEngaging.GetEntities ();
		for (int i = 0; i < allies.Length; i++) {
			var ally = allies [i];

			if (ally.engage.target.isTargetable) {
				if (Vector3.Distance (ally.position.value, ally.engage.target.position.value) <= ConstantData.MIN_ATK_RANGE) {
					//reach its target in close combat
					ally.AddTarget (ally.engage.target);
					ally.RemoveEngage ();
					ally.RemoveDestination ();
					ally.IsMovable (false);
					continue;
				}
			} else {
				ally.RemoveEngage ();
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
