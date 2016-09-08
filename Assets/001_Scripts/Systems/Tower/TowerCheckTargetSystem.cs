using UnityEngine;
using System.Collections;
using Entitas;
public class TowerCheckTargetSystem : IReactiveSystem, ISetPool {
	Pool _pool;
	Group _groupTowerWithTarget;
	#region ISetPool implementation

	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupTowerWithTarget = _pool.GetGroup (Matcher.AllOf (Matcher.Tower, Matcher.Active, Matcher.Target));
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if(_groupTowerWithTarget.count <= 0){
			return;
		}

		var ens = _groupTowerWithTarget.GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			var tower = ens [i];
			var enemy = ens [i].target.e;
			if(!enemy.hasEnemy || !enemy.position.value.IsInRange(tower.position.value, tower.attackRange.value)){
				tower.RemoveTarget ();
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
