using UnityEngine;
using System.Collections;
using Entitas;

public class CheckEngageDistanceSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Group _groupEngaging;
	public void SetPool (Pool pool)
	{
		_groupEngaging = pool.GetGroup (Matcher.AllOf(Matcher.Active, Matcher.Engage).NoneOf(Matcher.CloseCombat));
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if (_groupEngaging.count <= 0) {
			return;
		}

		var ens = _groupEngaging.GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			var e = ens [i];

			if (Vector3.Distance (e.position.value, e.engage.target.position.value) <= ConstantData.CLOSE_COMBAT_RANGE) {
				//reach its target in close combat
				e.AddCloseCombat (e.engage.target);

				e.IsMovable (false);
				continue;
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
