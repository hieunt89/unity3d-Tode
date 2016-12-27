using UnityEngine;
using System.Collections;
using Entitas;

public class UpdateLookDirectionSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Group _groupLookAt;
	public void SetPool (Pool pool)
	{
		_groupLookAt = pool.GetGroup (Matcher.AllOf (Matcher.View, Matcher.Position, Matcher.Active).AnyOf(Matcher.Engage, Matcher.Destination, Matcher.CloseCombat, Matcher.MoveTo));
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if (_groupLookAt.count <= 0) {
			return;
		}

		var tickEn = entities.SingleEntity ();
		var ens = _groupLookAt.GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			var e = ens [i];
			Vector3 targetDir = Vector3.zero;
			if(e.hasMoveTo){
				targetDir = e.moveTo.position - e.position.value;
			}else if (e.hasCloseCombat) {
				targetDir = e.closeCombat.opponent.position.value - e.position.value;
			}else if (e.hasEngage) {
				targetDir = e.engage.target.position.value - e.position.value;
			}else if (e.hasDestination) {
				targetDir = e.destination.value - e.position.value;
			}
			if (e.hasTurnSpeed) {
				float step = e.turnSpeed.value * tickEn.tick.change;
				targetDir = Vector3.RotateTowards (e.view.go.transform.forward, targetDir, step, 0f);
				if (GameManager.ShowDebug) {
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
			return Matcher.Tick.OnEntityAdded ();
		}
	}

	#endregion
}
