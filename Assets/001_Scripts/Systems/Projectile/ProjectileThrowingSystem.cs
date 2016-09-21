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
			if(!e.hasProjectileThrowingDestination){
				var startPos = Vector3.ProjectOnPlane (e.position.value, Vector3.up);
				var finalPos = GetEnemyFuturePosition (e.target.e, e.projectileThrowing.travelTime);
				var initAngle = GetInitAngle (e.projectileThrowing.travelTime, e.projectileThrowing.initSpeed, startPos, finalPos);
				var initHeight = Vector3.Project (e.position.value, Vector3.down).magnitude;
				e.AddProjectileThrowingDestination (startPos, finalPos, initAngle, initHeight).AddProjectileThrowingTime(0f);
				if (GameManager.debug) {
					Debug.DrawLine (e.target.e.position.value, e.projectileThrowingDestination.destination, Color.blue, Mathf.Infinity);
					Debug.DrawLine (startPos, finalPos, Color.yellow, Mathf.Infinity);
				}
			}

			if (e.position.value == e.projectileThrowingDestination.destination) {
				e.IsReachedEnd (true);
				continue;
			} else {
				e.ReplaceProjectileThrowingTime (e.projectileThrowingTime.time + Time.deltaTime)
					.ReplaceDestination(
						GetNextDestination(
							e.projectileThrowingTime.time,
							e.projectileThrowing.initSpeed,
							e.projectileThrowingDestination.initHeight,
							e.projectileThrowingDestination.initAngle,
							e.projectileThrowingDestination.start,
							e.projectileThrowingDestination.destination,
							e.projectileThrowing.travelTime
						)
					);
				e.ReplacePosition (e.destination.value);
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
	float GetEnemyTraveledDistance(Entity e){
		var points = e.pathReference.e.path.wayPoints;
		var distanceBtwPoints = e.pathReference.e.pathLength.distances;
		var destination = e.destination.value;

		var prevDesIndex = 0;
		for (int i = 0; i < points.Count; i++) {
			if(destination == points[i]){
				prevDesIndex = i - 1;
			}
		}

		var distanceToIndexPoint = 0f;
		for (int i = 0; i < prevDesIndex; i++) {
			distanceToIndexPoint += distanceBtwPoints [i];
		}

		return (e.position.value - points [prevDesIndex]).magnitude + distanceToIndexPoint;
	}

	Vector3 GetEnemyFuturePosition(Entity e, float timeToContact){
		var points = e.pathReference.e.path.wayPoints;
		var distanceBtwPoints = e.pathReference.e.pathLength.distances;
		var enemyDistanceFromStart = GetEnemyTraveledDistance(e);
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

	float GetInitAngle(float travelTime, float initSpeed, Vector3 startPos, Vector3 finalPos){
		var distanceX = (finalPos - startPos).magnitude;
		float angle = Mathf.Acos (distanceX / (initSpeed * travelTime)) * Mathf.Rad2Deg;
		return angle;
	}

	Vector3 GetNextDestination(float time, float initSpeed, float initHeight, float initAngle, Vector3 start, Vector3 end, float totalTime){
		var v = (((end - start) * (time / totalTime)) + start);
		Debug.DrawLine (start, v, Color.gray, 1f);

		float x = v.x;
		float y = initHeight + initSpeed * time * Mathf.Sin (initAngle) - ((Mathf.Pow (time, 2f) * ConstantData.G) / 2);
		float z = v.z;

		return new Vector3(x, y, z);
	}
}
