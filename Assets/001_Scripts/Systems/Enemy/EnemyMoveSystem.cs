using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class EnemyMoveSystem : IReactiveSystem, ISetPool {
	Pool _pool;
	Group _groupEnemyMovable;
	#region ISetPool implementation

	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupEnemyMovable = _pool.GetGroup(Matcher.AllOf(Matcher.Enemy, Matcher.Movable, Matcher.PathReference, Matcher.Active));
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (List<Entity> entities)
	{
		if (_groupEnemyMovable.count <= 0) {
			return;
		}

		var tickEn = entities.SingleEntity ();
		var ens = _groupEnemyMovable.GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			var enemy = ens [i];
			if (enemy.position.value == enemy.destination.value) {
				var target = GetNextDestination (enemy.position.value, enemy.pathReference.e.path.wayPoints);
				if (enemy.position.value == target) {
					enemy.IsMovable (false).IsReachedEnd(true);
					continue;
				} else {
					enemy.ReplaceDestination (target);
				}
			}

			enemy.ReplacePosition (Vector3.MoveTowards (enemy.position.value, enemy.destination.value, enemy.moveSpeed.speed * tickEn.tick.change));
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

	Vector3 GetNextDestination(Vector3 currentPos, List<Vector3> wayPoints){
		Vector3 result = currentPos;
		for (int i = 0; i < wayPoints.Count; i++) {
			if(currentPos == wayPoints[i]){
				result = (i+1) < wayPoints.Count ? wayPoints[i+1] : currentPos;
				break;
			}
		}
		return result;
	}
}
