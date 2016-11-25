using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class EffectMovementUpdateSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Group _groupMovementSlowed;
	public void SetPool (Pool pool)
	{
		_groupMovementSlowed = pool.GetGroup (Matcher.AllOf(Matcher.EffectMovementList, Matcher.Active));
	}

	#endregion

	
	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if (_groupMovementSlowed.count <= 0) {
			return;
		}

		var tick = entities.SingleEntity ().tick.change;
		var ens = _groupMovementSlowed.GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			var efToDur = ens [i].effectMovementList.efToDuration;

			if (efToDur.Count <= 0) {
				ens [i].RemoveEffectMovementList ();
				continue;
			}

			foreach(KeyValuePair<SkillEffect, float> entry in efToDur)
			{
				if (entry.Value > 0) {
					efToDur [entry.Key] -= tick;
				} else {
					DeApplySlow (entry.Key.value, ens[i]);
					efToDur.Remove (entry.Key);
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

	void DeApplySlow(float slow, Entity target){
		var newSpeed = target.moveSpeed.speed / slow.ReverseAndClamp01 ();
		target.ReplaceMoveSpeed (newSpeed);
	}
}
