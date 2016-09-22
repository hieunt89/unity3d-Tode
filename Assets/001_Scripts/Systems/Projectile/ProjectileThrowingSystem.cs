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
			#region calculate trajectory data
			if(!e.hasProjectileThrowingParams){ 
				var startPos = Vector3.ProjectOnPlane (e.position.value, Vector3.up);
				var finalPos = GetEnemyFuturePosition (e.target.e, e.projectileThrowing.travelTime);
				var initHeight = Vector3.Project (e.position.value, Vector3.down).magnitude;
				var initAngle = GetInitAngle (e.projectileThrowing.travelTime, e.projectileThrowing.initSpeed, startPos, finalPos);
				var gravity = GetGravity (e.projectileThrowing.travelTime, e.projectileThrowing.initSpeed, initHeight, initAngle);

				e.AddProjectileThrowingParams (startPos, finalPos, gravity, initAngle, initHeight).AddProjectileThrowingTime(0f).AddDestination(e.position.value);
				if (GameManager.debug) {
					Debug.DrawLine (e.target.e.position.value, e.projectileThrowingParams.end, Color.blue, Mathf.Infinity);
					Debug.DrawLine (startPos, finalPos, Color.yellow, Mathf.Infinity);
				}
			}
			#endregion

			if (e.position.value.y <= 0f) {
				e.IsReachedEnd (true);
			} else {
				e.ReplaceProjectileThrowingTime (e.projectileThrowingTime.time + Time.deltaTime);
				if (e.position.value == e.destination.value) {
					e.ReplaceDestination (
						GetNextDestination (
							e.projectileThrowingTime.time,
							e.projectileThrowing.initSpeed,
							e.projectileThrowing.travelTime,
							e.projectileThrowingParams.height,
							e.projectileThrowingParams.angle,
							e.projectileThrowingParams.start,
							e.projectileThrowingParams.end,
							e.projectileThrowingParams.gravity
						)
					);
				} else {
					if (GameManager.debug) {
						Debug.DrawLine (e.position.value, e.destination.value, Color.gray, Mathf.Infinity);
					}
					e.ReplacePosition (e.destination.value);
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

	float GetInitAngle(float t, float v, Vector3 start, Vector3 end){
		var distanceX = Mathf.Clamp((end - start).magnitude, -1f, 1f);
		float angleX = Mathf.Acos (distanceX / (v * t));
		return angleX;
	}

	float GetGravity(float t, float v, float h, float a){
		return (h + v * t * Mathf.Sin (a)) * 2 / Mathf.Pow (t, 2f);
	}

	Vector3 GetNextDestination(float t, float v, float totalTime, float h, float a, Vector3 start, Vector3 end, float g){
		var sToE = (((end - start) * (t / totalTime)) + start);

		float x = sToE.x;
		float y = h + (v * t * Mathf.Sin (a)) - ((Mathf.Pow (t, 2f) * g) / 2);
		float z = sToE.z;
		return new Vector3(x, y, z);
	}
}
