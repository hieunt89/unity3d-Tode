using UnityEngine;
using System.Collections;
using Entitas;

public class ProjectileHomingSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Pool _pool;
	Group _groupProjectiles;
	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupProjectiles = _pool.GetGroup (Matcher.AllOf (Matcher.Projectile, Matcher.Target).NoneOf (Matcher.Tower, Matcher.ReachedEnd));
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if(_groupProjectiles.count <= 0){
			return;
		}

		var projectiles = _groupProjectiles.GetEntities ();
		for (int i = 0; i < projectiles.Length; i++) {
			var e = projectiles [i];
			if (e.position.value == e.destination.value) {
				e.IsReachedEnd (true);
			}
			if(e.target.e.hasEnemy){
				e.ReplaceDestination (e.target.e.position.value);
			}
			e.ReplacePosition (Vector3.MoveTowards (e.position.value, e.destination.value, e.movable.speed * Time.deltaTime));
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
