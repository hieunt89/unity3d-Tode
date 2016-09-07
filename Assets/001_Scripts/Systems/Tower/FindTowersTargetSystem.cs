using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class FindTowersTargetSystem : IExecuteSystem, ISetPool {
	Pool _pool;
	Group _groupActiveTower;
	Group _groupActiveEnemy;
	List<Entity> targetableEnemies;

	public FindTowersTargetSystem(){
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

	#region IExecuteSystem implementation

	public void Execute ()
	{
		if(_groupActiveTower.count <= 0 || _groupActiveEnemy.count <= 0){
			return;
		}

		var towerEns = _groupActiveTower.GetEntities ();
		var enemyEns = _groupActiveEnemy.GetEntities ();
		Entity target;
		for (int i = 0; i < towerEns.Length; i++) {
			target = FindTarget (towerEns[i], enemyEns);
			if(target != null){
				towerEns [i].AddTarget (target.id.value);
			}
		}
	}

	#endregion
	Entity FindTarget(Entity tower, Entity[] enemies ){
		targetableEnemies.Clear ();
		for (int i = 0; i < enemies.Length; i++) {
			if(Vector3.Distance(tower.position.value, enemies[i].position.value) < tower.range.value){
				targetableEnemies.Add (enemies [i]);
			}
		}
		return RandomTarget (targetableEnemies);
	}

	Entity RandomTarget(List<Entity> enemies){
		if (enemies.Count > 0) {
			return enemies[Random.Range (0, enemies.Count)];
		} else {
			return null;
		}
	}
}
