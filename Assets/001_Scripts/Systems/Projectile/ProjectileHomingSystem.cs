using UnityEngine;
using System.Collections;
using Entitas;

public class ProjectileHomingSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Pool _pool;
	Group _groupPrjHoming;
	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupPrjHoming = _pool.GetGroup (Matcher.AllOf (Matcher.ProjectileHoming, Matcher.Target).NoneOf (Matcher.ReachedEnd));
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if(_groupPrjHoming.count <= 0){
			return;
		}

		Entity prj;
		var ens = _groupPrjHoming.GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			prj = ens [i];
			if (!prj.hasDestination) {
				prj.AddDestination (prj.target.e.position.value + prj.target.e.viewOffset.pivotToCenter);
			} 
			if (prj.target.e.hasEnemy) {
				prj.ReplaceDestination (prj.target.e.position.value + prj.target.e.viewOffset.pivotToCenter);
				if (prj.target.e.view.ColliderBound.Contains(prj.position.value)) {
					prj.IsReachedEnd (true);
					continue;
				}
			} else if (prj.position.value == prj.destination.value) { //projectile reaches its target
				prj.IsReachedEnd (true);
				continue;
			}
			prj.ReplacePosition (Vector3.MoveTowards (prj.position.value, prj.destination.value, prj.movable.speed * Time.deltaTime));
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
