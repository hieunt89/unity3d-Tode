using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class ProjectileThrowingSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Pool _pool;
	Group _groupPrjThrowing;
	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupPrjThrowing = _pool.GetGroup (Matcher.AllOf (Matcher.Projectile, Matcher.Target, Matcher.ProjectileThrowing).NoneOf(Matcher.Tower, Matcher.ReachedEnd));
	}
	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if(_groupPrjThrowing.count <= 0){
			return;
		}

		Entity e;
		var ens = _groupPrjThrowing.GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			
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

	Vector3 GetEnemyFuturePosition(Entity e, float timeToContact){
		Vector3 result = Vector3.zero;

		var points = e.pathReference.e.path.wayPoints;
		var pointsMag = new List<float> ();
		for (int i = 0; i < points.Count; i++) {
			pointsMag.Add (points [i].magnitude);
		}

		var distanceTravel = e.movable.speed * timeToContact;
		var distanceReached = 0f;
		var index = 0;
		for (int i = 0; i < pointsMag.Count; i++) {
			if(distanceTravel > distanceReached){
				index = i;
				distanceReached = distanceReached + pointsMag [i];
			}
		}

		var distanceRelative = distanceTravel - distanceReached;
		var scaleToMag = distanceRelative / pointsMag [index];

		return result;
	}
}
