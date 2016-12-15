using UnityEngine;
using System.Collections;
using Entitas;

public class AllyMoveSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Group _groupAllyMoving;
	public void SetPool (Pool pool)
	{
		_groupAllyMoving = pool.GetGroup (Matcher.AllOf(Matcher.Ally, Matcher.Active, Matcher.Destination));
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if (_groupAllyMoving.count <= 0) {
			return;
		}

		var tickEn = entities.SingleEntity ();
		var allies = _groupAllyMoving.GetEntities ();
		for (int i = 0; i < allies.Length; i++) {
			var ally = allies [i];

			if (ally.position.value == ally.destination.value) {
				ally.RemoveDestination ();
				ally.IsMovable (false);
			} else {
				ally.ReplacePosition (Vector3.MoveTowards (ally.position.value, ally.destination.value, ally.moveSpeed.speed * tickEn.tick.change));
				if (!ally.isMovable) {
					ally.IsMovable (true);
				}
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
