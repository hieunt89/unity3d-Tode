using UnityEngine;
using System.Collections;
using Entitas;

public class UpdateLookDirectionSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Pool _pool;
	public void SetPool (Pool pool)
	{
		_pool = pool;
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];
			Vector3 targetDir = e.destination.value - e.position.value;
			if (e.hasTurnSpeed) {
				float step = e.turnSpeed.value * _pool.tick.change;
				targetDir = Vector3.RotateTowards (e.view.go.transform.forward, targetDir, step, 0f);
				if (GameManager.debug) {
					Debug.DrawRay (e.position.value, targetDir, Color.red);
				}
			}
				
			if (targetDir != Vector3.zero) {
				e.view.go.transform.rotation = Quaternion.LookRotation (targetDir);
			}
		}
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.AllOf (Matcher.Position, Matcher.Destination, Matcher.View).OnEntityAdded ();
		}
	}

	#endregion
}
