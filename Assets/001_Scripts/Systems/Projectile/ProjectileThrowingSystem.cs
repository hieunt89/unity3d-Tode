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
			e = ens [i];
			if(!e.hasDestination){
				e.AddDestination (GetEnemyFuturePosition(e.target.e, e.projectileThrowing.travelTime));
				if (GameManager.debug) {
					Debug.DrawLine (e.target.e.position.value, e.destination.value, Color.blue, Mathf.Infinity);
				}
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

	Vector3 GetEnemyFuturePosition(Entity e, float timeToContact){
		var points = e.pathReference.e.path.wayPoints;
		var distanceBtwPoints = e.pathReference.e.pathLength.distances;
		var enemyDistanceFromStart = (e.position.value - points [0]).magnitude;
		var distanceToTravel = enemyDistanceFromStart + e.movable.speed * timeToContact;

		var totalDistance = 0f;
		for (int i = 0; i < distanceBtwPoints.Count; i++) {
			totalDistance += distanceBtwPoints [i];
		}
		if (distanceToTravel > totalDistance) {
			return points [points.Count-1];
		}

		var index = 0;
		var distanceToPoint = 0f;

		for (int i = 0; i < distanceBtwPoints.Count; i++) {
			distanceToPoint += distanceBtwPoints [i];
			if(distanceToTravel <= distanceToPoint){
				index = i;
				distanceToPoint -= distanceBtwPoints [i];
				break;
			}
		}

		var distanceDiff = distanceToTravel - distanceToPoint;
		var scaleOnVector = distanceDiff / distanceBtwPoints [index];

		var startPoint = points [index];
		var endPoint = points [index + 1];
		var resultVector = ((endPoint - startPoint) * scaleOnVector) + startPoint;

		return resultVector;

	}

	float GetInitAngle(float travelTime, float initSpeed, float distanceX){
		return 0f;
	}
}
