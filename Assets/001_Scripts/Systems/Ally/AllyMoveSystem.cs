using UnityEngine;
using System.Collections;
using Entitas;

public class AllyMoveSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Group _groupAllyMoving;
	public void SetPool (Pool pool)
	{
		_groupAllyMoving = pool.GetGroup (Matcher.AllOf(Matcher.Ally, Matcher.Active, Matcher.Destination, Matcher.Movable, Matcher.PathQueue));
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

			if (!ally.hasMoveTo) {
				ally.AddMoveTo (ally.pathQueue.queue.Dequeue());
			}
				
			if (ally.position.value == ally.moveTo.position) {
				if (ally.pathQueue.queue.Count > 0) {
					ally.ReplaceMoveTo (ally.pathQueue.queue.Dequeue ());
				} else {
					ally.RemoveDestination ();
					ally.RemoveMoveTo ();
					ally.IsMovable (false);
				}
			} else if (Physics.Raycast(ally.position.value, ally.moveTo.position, Vector3.Distance(ally.position.value, ally.moveTo.position))) {
				ally.ReplaceDestination (ally.destination.value);
			} else {
				ally.ReplacePosition (Vector3.MoveTowards (ally.position.value, ally.moveTo.position, ally.moveSpeed.speed * tickEn.tick.change));
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
