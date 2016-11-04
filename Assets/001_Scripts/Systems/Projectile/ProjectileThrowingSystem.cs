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
		_groupPrjThrowing = _pool.GetGroup (Matcher.AllOf (Matcher.ProjectileThrowing, Matcher.Target).NoneOf(Matcher.ReachedEnd));
	}
	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if(_groupPrjThrowing.count <= 0){
			return;
		}
			
		var tickEn = entities.SingleEntity ();
		Entity prj;
		var ens = _groupPrjThrowing.GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			prj = ens [i];
			#region calculate trajectory data
			if(!prj.hasProjectileThrowingParams){ 
				var startPos = Vector3.ProjectOnPlane (prj.position.value, Vector3.up);
				var finalPos = GetEnemyFuturePosition (prj.target.e, prj.projectileThrowing.travelTime);
				var initHeight = Vector3.Project (prj.position.value, Vector3.down).magnitude;
				float initVelocity;
				float initAngle;
				GetInitVelocityAndAngle(startPos, finalPos, prj.projectileThrowing.travelTime, initHeight, out initVelocity, out initAngle);

				prj.AddProjectileThrowingParams (startPos, finalPos, initHeight, initVelocity, initAngle).AddDuration(0f).AddDestination(prj.position.value);
				if (GameManager.debug) {
					Debug.DrawLine (prj.target.e.position.value, prj.projectileThrowingParams.end, Color.blue, Mathf.Infinity);
					Debug.DrawLine (startPos, finalPos, Color.yellow, Mathf.Infinity);
				}
			}
			#endregion
			if ((prj.target.e.isTargetable && prj.target.e.view.Collider.bounds.Contains(prj.position.value) ) || prj.position.value.y <= 0f) {
				prj.IsReachedEnd (true);
			}else {
				prj.ReplaceDuration (prj.duration.value + tickEn.tick.change);
				if (prj.position.value == prj.destination.value) {
					prj.ReplaceDestination (
						GetNextDestination (
							prj.duration.value,
							prj.projectileThrowingParams.initVelocity,
							prj.projectileThrowingParams.height,
							prj.projectileThrowingParams.initAngle,
							prj.projectileThrowingParams.start,
							prj.projectileThrowingParams.end
						)
					);
				} else {
					if (GameManager.debug) {
						Debug.DrawLine (prj.position.value, prj.destination.value, Color.gray, Mathf.Infinity);
					}
					prj.ReplacePosition (prj.destination.value);
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
		var distanceToTravel = enemyDistanceFromStart + e.moveSpeed.speed * timeToContact;

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
		var resultVector = startPoint.ToEndFragment (endPoint, scaleOnVector);

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
		var vec = start.ToEndFragment (end, dX / d);

		float x = vec.x;
		float y = h + (v * t * Mathf.Sin (a * Mathf.Deg2Rad)) - (ConstantData.G * (Mathf.Pow (t, 2f)) / 2);
		float z = vec.z;

		return new Vector3(x, y, z);
	}
}
