using UnityEngine;
using System.Collections;
using Entitas;

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


		}
	}
	#endregion
}
