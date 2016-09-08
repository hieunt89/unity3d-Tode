using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class TowerFindTargetSystem : IReactiveSystem, ISetPool {
	Pool _pool;
	Group _groupActiveTower;
	Group _groupActiveEnemy;
	List<Entity> targetableEnemies;

	public TowerFindTargetSystem(){
		targetableEnemies = new List<Entity> ();
	}
	#region ISetPool implementation

	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupActiveTower = _pool.GetGroup (Matcher.AllOf (Matcher.Tower, Matcher.Active).NoneOf(Matcher.Target));
		_groupActiveEnemy = _pool.GetGroup (Matcher.AllOf (Matcher.Enemy, Matcher.Active));
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (List<Entity> entities)
	{
		if(_groupActiveTower.count <= 0 || _groupActiveEnemy.count <= 0){
			return;
		}

		var towerEns = _groupActiveTower.GetEntities ();
		var enemyEns = _groupActiveEnemy.GetEntities ();
		Entity target;
		for (int i = 0; i < towerEns.Length; i++) {
			target = FindTarget (towerEns[i], enemyEns);
			if (target != null) {
				towerEns [i].AddTarget (target);
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

	Entity FindTarget(Entity tower, Entity[] enemies ){
		targetableEnemies.Clear ();
		for (int i = 0; i < enemies.Length; i++) {
			if( enemies[i].position.value.IsInRange(tower.position.value, tower.attackRange.value) ){
				targetableEnemies.Add (enemies [i]);
			}
		}
		return ChooseTarget (targetableEnemies);
	}
		
	Entity ChooseTarget(List<Entity> enemies){
		if (enemies.Count > 0) {
			return enemies[Random.Range (0, enemies.Count)];
		} else {
			return null;
		}
	}
}
