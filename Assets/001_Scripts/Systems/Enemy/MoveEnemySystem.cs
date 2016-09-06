using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class MoveEnemySystem : IExecuteSystem, ISetPool {
	Pool _pool;
	Group _groupEnemyMovable;
	#region ISetPool implementation

	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupEnemyMovable = _pool.GetGroup(Matcher.AllOf(Matcher.Enemy, Matcher.Movable).NoneOf(Matcher.Activable));
	}

	#endregion

	#region IExecuteSystem implementation
	public void Execute ()
	{
		var ens = _groupEnemyMovable.GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			var e = ens [i];
			if (e.position.value == e.destination.value) {
				var target = GetNextDestination (e.position.value, e.enemy.ePathId);
				if (e.position.value == target) {
					e.RemoveMovable ();
					continue;
				} else {
					e.ReplaceDestination (target);
				}
			}
			e.ReplacePosition (Vector3.MoveTowards (e.position.value, e.destination.value, e.movable.speed * Time.deltaTime));
		}
	}
	#endregion

	Vector3 GetNextDestination(Vector3 currentPos, string pathId){
		Vector3 result = currentPos;
		List<Vector3> wayPoints = _pool.GetPathEntityById (pathId).path.wayPoints;
		for (int i = 0; i < wayPoints.Count; i++) {
			if(currentPos == wayPoints[i]){
				result = (i+1) < wayPoints.Count ? wayPoints[i+1] : currentPos;
				break;
			}
		}
		return result;
	}
}
