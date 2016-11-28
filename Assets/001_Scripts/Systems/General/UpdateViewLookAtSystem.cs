using UnityEngine;
using System.Collections;
using Entitas;

public class UpdateViewLookAtSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Group _groupViewLookAt;
	public void SetPool (Pool pool)
	{
		_groupViewLookAt = pool.GetGroup (Matcher.AllOf(Matcher.ViewLookAt, Matcher.Target, Matcher.Active));
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if (_groupViewLookAt.count <= 0) {
			return;
		}

		var tickEn = entities.SingleEntity ();
		var ens = _groupViewLookAt.GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			var e = ens[i];

			Vector3 targetDir = e.target.e.position.value - e.position.value;
			var turnSpeed = (!e.hasTurnSpeed || e.turnSpeed.value <= 0) ? ConstantData.DEFAULT_TURN_SPEED : e.turnSpeed.value;
			var step = turnSpeed * tickEn.tick.change;

			var elsToLookAt = e.viewLookAt.elsToLookAt;
			for (int j = 0; j < elsToLookAt.Count; j++) {
				targetDir = Vector3.RotateTowards (elsToLookAt[j].forward, targetDir, step, 0f);
				if (targetDir != Vector3.zero) {
					elsToLookAt[j].rotation = Quaternion.LookRotation (targetDir);
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
	
}
