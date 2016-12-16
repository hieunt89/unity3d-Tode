using UnityEngine;
using System.Collections;
using Entitas;

public class AllyEngageSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Pool _pool;
	Group _groupActiveAlly;
	Group _groupActiveEnemy;
	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupActiveAlly = _pool.GetGroup (Matcher.AllOf (Matcher.Active, Matcher.Ally).NoneOf(Matcher.Engage, Matcher.Target));
		_groupActiveEnemy = _pool.GetGroup (Matcher.AllOf (Matcher.Enemy, Matcher.Active, Matcher.Targetable));
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if(_groupActiveAlly.count <= 0 || _groupActiveEnemy.count <= 0){
			return;
		}

		var allyEns = _groupActiveAlly.GetEntities ();
		var enemyEns = _groupActiveEnemy.GetEntities ();

		for (int i = 0; i < allyEns.Length; i++) {
			var ally = allyEns [i];

			var target = CombatUtility.FindTargetInRange (ally.rallyPoint.position, enemyEns, ally.engageRange.value);
			if (target != null) {
				ally.AddEngage (target);
				ally.ReplaceDestination (ally.engage.target.position.value);

				ally.engage.target.AddEngaged (ally).IsMovable (false);
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
