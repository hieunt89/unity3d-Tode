using UnityEngine;
using System.Collections;
using Entitas;

public class ProjectileLaserSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Pool _pool;
	Group _groupPrjLaser;
	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupPrjLaser = _pool.GetGroup (Matcher.AllOf(Matcher.ProjectileMark, Matcher.Target, Matcher.ProjectileLaser).NoneOf(Matcher.ReachedEnd));
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if(_groupPrjLaser.count <= 0){
			return;
		}
			
		Entity e;
		var ens = _groupPrjLaser.GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			e = ens [i];
			if(!e.hasDestination){
				e.AddDestination(e.target.e.position.value)
					.AddProjectileTime (0f);
			}
			if(e.projectileTime.time >= e.projectileLaser.travelTime || !e.target.e.hasEnemy){
				e.IsReachedEnd (true);
				continue;
			}
			if(e.target.e.hasEnemy){
				e.ReplaceDestination (
					((e.target.e.position.value + e.target.e.view.go.GetColliderCenterOffset()) - e.position.value) * Mathf.Clamp01(e.projectileTime.time*e.projectileLaser.travelSpeed) + e.position.value
				);
			}
			e.ReplaceProjectileTime (e.projectileTime.time + Time.deltaTime);
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
