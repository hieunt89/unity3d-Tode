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
				float initVelocity;
				float initAngle;
				GetInitVelocityAndAngle(startPos, finalPos, e.projectileThrowing.travelTime, initHeight, out initVelocity, out initAngle);

				e.AddProjectileThrowingParams (startPos, finalPos, initHeight, initVelocity, initAngle).AddProjectileThrowingTime(0f).AddDestination(e.position.value);
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
							e.projectileThrowingParams.initVelocity,
							e.projectileThrowingParams.height,
							e.projectileThrowingParams.initAngle,
							e.projectileThrowingParams.start,
							e.projectileThrowingParams.end
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

	void GetInitVelocityAndAngle(Vector3 start, Vector3 end, float t, float h, out float velocity, out float angle){
		var d = (end - start).magnitude;
		var vX = d / t;
		var vY = ((ConstantData.G * Mathf.Pow (t, 2f) / 2) - h) / t;
		var delta = 1f;
		float v0FromX;
		float v0FromY;
		velocity = 0f;
		angle = 0f;

		for (int i = 0; i < 90; i++) {
			v0FromX = vX / (Mathf.Cos (i * Mathf.Deg2Rad));
			v0FromY = vY / (Mathf.Sin (i * Mathf.Deg2Rad));

			if (Mathf.Abs (v0FromX - v0FromY) < delta) {
				delta = Mathf.Abs (v0FromX - v0FromY);
				velocity = v0FromX;
				angle = (float)i;
			}
		}
	}

	Vector3 GetNextDestination(float t, float v, float h, float a, Vector3 start, Vector3 end){
		var d = (end - start).magnitude;
		var dX = v * t * Mathf.Cos (a * Mathf.Deg2Rad);
		var vec = (end - start) * (dX / d) + start;

		float x = vec.x;
		float y = h + (v * t * Mathf.Sin (a * Mathf.Deg2Rad)) - (ConstantData.G * (Mathf.Pow (t, 2f)) / 2);
		float z = vec.z;

		return new Vector3(x, y, z);
	}
}
